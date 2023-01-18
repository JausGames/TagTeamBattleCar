using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Unity.Netcode;
using RootMotion.FinalIK;

[RequireComponent(typeof(Player))]
public class ShooterController : NetworkBehaviour
{
    [Header("Inputs")][SerializeField] NetworkVariable<Vector3> look = new NetworkVariable<Vector3>(Vector3.zero, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    [SerializeField] Seat seat;
    [SerializeField] GameObject aimeUi;

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

    public Transform CameraContainer { get => cameraContainer; set => cameraContainer = value; }
    public LayerMask Hitablemask { get => hitablemask; set => hitablemask = value; }
    public CameraFollow CameraFollow { get => cameraFollow; set => cameraFollow = value; }
    public Seat Seat { get => seat; set
        {
            seat = value;
            animatorController.Animator = seat.animator;
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

    // Update is called once per frame
    private void Start()
    {
        hitablemask = (1 << 3) | (1 << 6) | (1 << 0);
        if (IsOwner)
        {
            Cursor.lockState = CursorLockMode.Locked;
            //body.isKinematic = false;
            cameraContainer.GetComponent<CameraFollow>().camera.enabled = true;
            aimeUi.SetActive(true);
        }


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
                prtcl.transform.position = Seat.BulletStart.position;
                prtcl.transform.LookAt(seat.target);
            }
        }

    }
    private void LateUpdate()
    {
        if (IsOwner)
        {
            if (Seat)
            {
                transform.position = Seat.transform.position;
            }

            if (IsOnMenu) return;
            cameraFollow.RotateCamera(look.Value, cameraSpeed, Seat.transform, out float angle, out Vector3 target);

            PlayAnimationServerRpc(angle, target);
        }
    }



    private void SetItemUp(Item value)
    {
        seat.Item = value.Prefab;
        heldItem = seat.Item.GetComponent<Item>();
        if (heldItem is Weapon)
        {
            ((Weapon)heldItem).shootParticles[0].transform.parent.parent = seat.transform;
        }
    }

    [ServerRpc]
    private void SubmitChangeItemServerRpc(int id, bool isWeapon)
    {
        if(IsServer)
        {
            SetItemUp(FindItemById(id, isWeapon).Prefab.GetComponent<Item>());
        }
        ChangeItemClientRpc(id, isWeapon);
    }
    [ClientRpc]
    private void ChangeItemClientRpc(int id, bool isWeapon)
    {
        if (IsServer) return;
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
        }
        else if (weaponWheel.gameObject.activeSelf && player.Team.ChangeCreditAmount(-weaponWheel.PickedItem.Cost))
        {
            var weap = weaponWheel.PickedItem.Prefab.GetComponent<Weapon>();
            CurrentWeapon = weap;
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
        animatorController.Angle = angle;
        seat.target.position = target;
        //animatorController.Direction = (x * Vector2.right + y * Vector2.up).normalized;
    }
    internal void Shoot(bool context)
    {
        if (!IsOwner || !context || IsOnMenu || heldItem == null) return;

        heldItem.Use(this);
    }

    [ServerRpc]
    public void SubmitShootServerRpc()
    {
        PlayShootParticleClientRpc();
    }

    [ClientRpc]
    void PlayShootParticleClientRpc()
    {
        foreach (var prtcl in ((Weapon)heldItem).shootParticles)
        {
            prtcl.Play();
        }
    }


    [ServerRpc]
    public void SummitGetHitServerRpc(ulong playerid, float damage)
    {

        Debug.Log("ShooterController, SummitGetHitServerRpc : touched player = #" + playerid);
        var id = GetNetworkObject(playerid);
        GetNetworkObject(playerid).GetComponentInChildren<Player>().GetHit(damage);
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
}
