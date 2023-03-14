using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Unity.Netcode;
using RootMotion.FinalIK;
using UnityEngine.Events;

public class ShooterController : PlayerController
{
    [Header("Inputs")][SerializeField] NetworkVariable<Vector3> look = new NetworkVariable<Vector3>(Vector3.zero, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    [SerializeField] Seat seat;
    [SerializeField] GameObject ui;

    [Space]
    [Header("Item")]
    [SerializeField] Weapon currentWeapon;
    [SerializeField] Tool currentTool;
    [SerializeField] Item heldItem;

    [Space]
    [Header("Components")]
    private LayerMask hitablemask;
    [SerializeField] Player player;
    [SerializeField] Transform cameraContainer;
    [SerializeField] ShooterAnimatorController animatorController;
    [SerializeField] float cameraSpeed;
    [SerializeField] RadialMenuController weaponWheel;
    [SerializeField] RadialMenuController toolWheel;
    [SerializeField] private CameraFollow cameraFollow;
    [SerializeField] private ClientAutoritative.ShipController ship;

    [HideInInspector] UnityEvent<float> getHitCreditsEarnEvent = new UnityEvent<float>();

    [SerializeField] Transform rightHand;
    [SerializeField] AudioListener audioListener;
    [SerializeField] Transform target;
    [SerializeField] Quaternion rotationOffset;
    private bool reloading;
    [SerializeField] private AimIK ik;
    private bool alive = true;

    public Transform CameraContainer { get => cameraContainer; set => cameraContainer = value; }


    public LayerMask Hitablemask { get => hitablemask; set => hitablemask = value; }
    public CameraFollow CameraFollow { get => cameraFollow; set => cameraFollow = value; }
    public Seat Seat { get => seat; set
        {
            seat = value;
            //animatorController.Animator = seat.animator;
        }
    }
    public bool IsOnMenu { 
        get => toolWheel.gameObject.activeSelf || weaponWheel.gameObject.activeSelf;
    }
    public Tool CurrentTool { 
        get => currentTool;
        set 
        {
            if (currentTool == null || value.Id != currentTool.Id)
            {
                currentTool = value; 
                HeldItem = currentTool;
            }

            if (heldItem is Weapon) SwitchItem();
        }
    }
    public Weapon CurrentWeapon
    {
        get => currentWeapon;
        set
        {

            if (currentWeapon == null  || value.Id != currentWeapon.Id)
            {
                currentWeapon = value;
                HeldItem = currentWeapon;
            }
            else if(value.Id == currentWeapon.Id)
            {
                if (heldItem is Tool) SwitchItem();
                ((Weapon)HeldItem).ResetAmmo();
            }


        }
    }
    public Item HeldItem 
    { 
        get => heldItem;
        set
        {
            if (!IsOwner) return;
            SetItemUp(value);

            SubmitChangeItemServerRpc(heldItem.Id, heldItem is Weapon);
        }
    }

    public ClientAutoritative.ShipController Ship { get => ship; set => ship = value; }
    public UnityEvent<float> GetHitCreditsEarnEvent { get => getHitCreditsEarnEvent; set => getHitCreditsEarnEvent = value; }

    // Update is called once per frame
    private void Start()
    {
        hitablemask = (1 << 3) | (1 << 6) | (1 << 0) | (1 << 8);
        if (IsOwner && IsLocalPlayer)
        {
            Cursor.lockState = CursorLockMode.Locked;
            //body.isKinematic = false;
            cameraContainer.GetComponent<CameraFollow>().GetComponentInChildren<Camera>().enabled = true;
            ui.SetActive(true);
            audioListener.enabled = true;
        }

        if (!weaponWheel) return;
        for (int i = 0; i < weaponWheel.items.Count; i++)
        {
            weaponWheel.items[i].Prefab.GetComponent<Item>().Prefab = weaponWheel.items[i].Prefab;
            weaponWheel.items[i].Prefab.GetComponent<Item>().Id = weaponWheel.items[i].Id;
        }
        for (int i = 0; i < toolWheel.items.Count; i++)
        {
            toolWheel.items[i].Prefab.GetComponent<Item>().Prefab = toolWheel.items[i].Prefab;
            toolWheel.items[i].Prefab.GetComponent<Item>().Id = toolWheel.items[i].Id;
        }
        
    }


    private void Update()
    {
        //body.AddForce(Vector3.down, ForceMode.VelocityChange);
        if (Seat && heldItem is Weapon)
        {
            foreach (var prtcl in ((Weapon)heldItem).shootParticles)
            {
                prtcl.transform.position = ((Weapon)heldItem).canonEnd.position;
                prtcl.transform.LookAt(target);
            }
        }


    }
    private void LateUpdate()
    {
        if (Seat)
        {
            transform.position = Seat.transform.position;
            transform.rotation = Seat.transform.parent.rotation * rotationOffset;
        }
        if (IsOwner && IsLocalPlayer)
        {

            if (IsOnMenu) return;
            cameraFollow.RotateCamera(look.Value, cameraSpeed, Seat.transform, out float angle, out Vector3 target);

            animatorController.Angle = angle;

            PlayAnimationServerRpc(angle, target);
        }
    }


    [ServerRpc]
    internal void SubmitShotContactParticleServerRpc(int layer, Vector3 origin, Vector3 direction)
    {
        InstantiateShotContactParticlesClientRpc(layer, origin, direction);
    }

    [ClientRpc]
    private void InstantiateShotContactParticlesClientRpc(int layer, Vector3 origin, Vector3 direction)
    {
        ((Weapon)heldItem).InstantiateContactParticles(layer, origin, direction);
    }

    private void SetItemUp(Item value)
    {

        if (heldItem != null)
            Destroy(heldItem.gameObject); ;
        heldItem = Instantiate(value, rightHand);
        //if (item.GetComponent<Weapon>()) bulletStart = item.GetComponent<Weapon>().canonEnd;
        //else bulletStart = null;
        heldItem.Id = value.GetComponent<Item>().Id;
        heldItem.Prefab = value.GetComponent<Item>().Prefab;

        /*if (heldItem.GetComponent<Weapon>()) bulletStart = heldItem.GetComponent<Weapon>().canonEnd;
        else bulletStart = null;*/
        //seat.Item = heldItem.gameObject;

        //bulletStart = value.GetComponent<Weapon>().canonEnd;

        //seat.Item = value.Prefab;
        //heldItem = seat.Item.GetComponent<Item>();

        if (heldItem is Weapon)
        {
            ((Weapon)heldItem).shootParticles[0].transform.parent.parent = seat.transform;
        }
    }

    [ServerRpc]
    private void SubmitChangeItemServerRpc(int id, bool isWeapon)
    {
        if(IsServer && !IsOwner)
        {
            SetItemUp(FindItemById(id, isWeapon).Prefab.GetComponent<Item>());
        }
        ChangeItemClientRpc(id, isWeapon);
    }
    [ClientRpc]
    private void ChangeItemClientRpc(int id, bool isWeapon)
    {
        if (IsServer || IsOwner) return;
        SetItemUp(FindItemById(id, isWeapon).Prefab.GetComponent<Item>());
    }

    ItemHolder FindItemById(int id, bool isWeapon)
    {
        if (isWeapon)
        {
            for(int i = 0; i < weaponWheel.items.Count; i++)
            {
                if (weaponWheel.items[i].Id == id) return weaponWheel.items[i];
            }
        }else{
            for(int i = 0; i < toolWheel.items.Count; i++)
            {
                if (toolWheel.items[i].Id == id) return toolWheel.items[i];
            }
        }
        throw new Exception("No item found for switch");
    }

    internal void ActivateWeaponWheel(bool v)
    {
        if (v)
        {
            if(toolWheel.gameObject.activeSelf) toolWheel.gameObject.SetActive(false);
            Cursor.lockState = CursorLockMode.Confined;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
        }
        weaponWheel.gameObject.SetActive(v);
    }

    internal void BuyItem()
    {
        if (toolWheel.gameObject.activeSelf && player.Team.ChangeCreditAmount(-toolWheel.PickedItem.Cost))
        {
            var tool = toolWheel.PickedItem.Prefab.GetComponent<Tool>();
            CurrentTool = tool;
            toolWheel.gameObject.SetActive(false);
        }
        else if (weaponWheel.gameObject.activeSelf && player.Team.ChangeCreditAmount(-weaponWheel.PickedItem.Cost))
        {
            var weap = weaponWheel.PickedItem.Prefab.GetComponent<Weapon>();
            CurrentWeapon = weap;
            weaponWheel.gameObject.SetActive(false);
        }
    }
    internal void SwitchItem(bool destroyCurrent = false)
    {
        if (destroyCurrent)
        {
            if (heldItem is Weapon) currentWeapon = null;
            else currentTool = null;

            Destroy(heldItem.gameObject);
        }

        if ((heldItem is Tool || heldItem == null) && currentWeapon != null) HeldItem = currentWeapon;
        else if ((heldItem is Weapon || heldItem == null) && currentTool != null) HeldItem = currentTool;
    }

    internal void ActivateToolWheel(bool v)
    {
        if (v)
        {
            if (weaponWheel.gameObject.activeSelf) weaponWheel.gameObject.SetActive(false);
            Cursor.lockState = CursorLockMode.Confined;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
        }
        if (weaponWheel.gameObject.activeSelf && v) weaponWheel.gameObject.SetActive(false);
        toolWheel.gameObject.SetActive(v);
    }

    [ServerRpc]
    void PlayAnimationServerRpc(float angle, Vector3 target)
    {
        PlayAnimationClientRpc(angle, target);
    }

    [ClientRpc]
    void PlayAnimationClientRpc(float angle, Vector3 target)
    {
        if(!IsOwner)
        {
            animatorController.Angle = angle;
            this.target.position = target;
        }
        //animatorController.Direction = (x * Vector2.right + y * Vector2.up).normalized;
    }
    internal void StartReloading()
    {
        if (reloading || ((Weapon)heldItem).RemainingAmmo == 0) return;
        reloading = true;
        SubmitReloadServerRpc();
    }
    internal void Reload()
    {
        ((Weapon)heldItem).Reload();
        ik.enabled = true;
        reloading = false;
    }
    [ServerRpc]
    public void SubmitReloadServerRpc()
    {
        PlayReloadClientRpc();
    }

    [ClientRpc]
    void PlayReloadClientRpc()
    {
        ik.enabled = false;
        animatorController.Reload();
    }
    internal void Shoot(bool context)
    {
        if (!IsOwner || !context || IsOnMenu || heldItem == null) return;

        heldItem.Use(this);
    }

    internal void ShootBullet()
    {
        foreach (var prtcl in ((Weapon)heldItem).shootParticles)
        {
            prtcl.Play();
        }
        SubmitShootServerRpc(((Weapon)heldItem).canonEnd.position, ((Weapon)heldItem).canonEnd.rotation);
    }
    [ServerRpc]
    public void SubmitShootServerRpc(Vector3 position, Quaternion rotation)
    {
        PlayShootParticleClientRpc(position, rotation);
    }

    [ClientRpc]
    void PlayShootParticleClientRpc(Vector3 position, Quaternion rotation)
    {
        if (IsOwner) return;
        foreach (var prtcl in ((Weapon)heldItem).shootParticles)
        {
            prtcl.transform.position = position;
            prtcl.transform.rotation = rotation;
            prtcl.Play();
        }
    }


    [ServerRpc]
    public void SummitGetHitServerRpc(ulong playerid, float damage, ulong originId)
    {
        Debug.Log("ShooterController, SummitGetHitServerRpc : touched player = #" + playerid);
        GetNetworkObject(playerid).GetComponentInChildren<Player>().GetHit(damage, originId);
    }

    [ClientRpc]
    internal void FindSeatClientRpc(ulong ownerObjId, int seatId)
    {
        Debug.Log("ShooterController, SummitGetHitServerRpc : cart owner = #" + ownerObjId);
        var ship = GetNetworkObject(ownerObjId).GetComponentInChildren<ClientAutoritative.ShipController>();
        Seat = ship.Seats[seatId];
        Ship = ship;
    }


    internal void Look(Vector2 vector2)
    {
        look.Value = vector2;
    }
    //Called on client rpc
    public override void Die()
    {
        if (!alive) return;
        alive = false;
        animatorController.Die();
    }
    public override void Respawn()
    {
        if(IsOwner)
            SubmitRespawnServerRpc();
    }

    [ServerRpc]
    private void SubmitRespawnServerRpc()
    {
        player.Health.Value = player.MaxHealth;
        alive = true;
        SetAliveClientRpc();
    }
    [ClientRpc]
    private void SetAliveClientRpc()
    {
        alive = true;
    }
}