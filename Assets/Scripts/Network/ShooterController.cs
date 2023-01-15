using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Unity.Netcode;
using RootMotion.FinalIK;

public class ShooterController : NetworkBehaviour
{
    [Header("Inputs")]
    [SerializeField] NetworkVariable<float> health = new NetworkVariable<float>(50f, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);
    [SerializeField] NetworkVariable<Vector3> look = new NetworkVariable<Vector3>(Vector3.zero, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    [SerializeField] Seat seat;
    [SerializeField] GameObject aimeUi;

    [Space]
    [Header("Item")]
    [SerializeField] Weapon currentWeapon;
    [SerializeField] Tool currentTool;
    [SerializeField] Item heldItem;

    [Space]
    [Header("Components")]
    public LayerMask hitablemask;
    [SerializeField] Transform cameraContainer;
    [SerializeField] ShooterAnimatorController animatorController;
    [SerializeField] float cameraSpeed;
    [SerializeField] RadialMenuController weaponWheel;
    [SerializeField] RadialMenuController toolWheel;



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
            currentTool = value;
            currentTool.Prefab = toolWheel.PickedItem.Prefab;
            if (heldItem == null) HeldItem = currentTool;
        }
    }
    public Weapon CurrentWeapon
    {
        get => currentWeapon;
        set
        {
            currentWeapon = value;
            currentWeapon.Prefab = weaponWheel.PickedItem.Prefab;
            if (heldItem == null) HeldItem = currentWeapon;
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

    // Update is called once per frame
    private void Start ()
    {
        hitablemask = (1 << 3) | (1 << 6) | (1 << 0);
        if (IsOwner)
        {
            Cursor.lockState = CursorLockMode.Locked;
            //body.isKinematic = false;
            cameraContainer.GetComponent<CameraFollow>().camera.enabled = true;
            aimeUi.SetActive(true);
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

            var eulerRot = cameraContainer.rotation.eulerAngles + new Vector3(look.Value.normalized.y * Time.deltaTime * cameraSpeed * look.Value.magnitude, look.Value.normalized.x * Time.deltaTime * cameraSpeed * look.Value.magnitude, 0f);
            eulerRot.x = eulerRot.x < 0 ? eulerRot.x + 360f : eulerRot.x;
            var newX = eulerRot.x > 0 && eulerRot.x <= 150f ? Mathf.Clamp(eulerRot.x, 0f, 50f) : Mathf.Clamp(eulerRot.x, 310f, 360f);
            eulerRot = newX * Vector3.right + eulerRot.y * Vector3.up + eulerRot.z * Vector3.forward;

            cameraContainer.rotation = Quaternion.Euler(eulerRot);
            
            var forward = (cameraContainer.forward.x * Vector2.right + cameraContainer.forward.z * Vector2.up).normalized;
            var seatForward = (Seat.transform.forward.x * Vector2.right + Seat.transform.forward.z * Vector2.up).normalized;

            var angle = Vector2.SignedAngle(seatForward, forward);
            var target = cameraContainer.position + cameraContainer.forward * 10f;


            PlayAnimationServerRpc(angle, target);
        }
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
        if (toolWheel.gameObject.activeSelf)
            CurrentTool = toolWheel.PickedItem.Prefab.GetComponent<Tool>();
        else if (weaponWheel.gameObject.activeSelf)
            CurrentWeapon = weaponWheel.PickedItem.Prefab.GetComponent<Weapon>();
    }
    internal void SwitchItem()
    {
        if ((heldItem == currentWeapon || heldItem == null) && currentTool != null) HeldItem = currentTool;
        else if ((heldItem == currentTool || heldItem == null) && currentWeapon != null) HeldItem = currentWeapon;
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

    #region Network var : Health
    public void SetHealth(float health)
    {
        this.health.Value = health;
    }
    #endregion
    internal void Shoot(bool context)
    {
        if (!IsOwner) return;
        if (!context || IsOnMenu || heldItem == null) return;
        Debug.DrawRay(transform.position + 0.35f * Vector3.up, cameraContainer.forward * 50f, Color.red, .5f);
        var hits = Physics.RaycastAll(transform.position + 0.35f * Vector3.up, cameraContainer.forward, 50f, 1 << 3);


        //if(hits.Length > 0) shootParticles.transform.Lok
        SubmitShootServerRpc();

        foreach (var hit in hits)
        {
            switch (hit.collider.gameObject.layer)
            {
                // Hit player body
                case 3:
                    //var ennemy = hit.collider.GetComponent<ShooterController>() ? hit.collider.GetComponent<ShooterController>() : hit.collider.GetComponentInParent<ShooterController>();
                    //Debug.Log("ShooterController, Shoot : #" + NetworkObjectId + " shot #" + ennemy.NetworkObjectId);
                    //SummitGetHitServerRpc(ennemy.NetworkObjectId, 50f);
                    break;
                // Hit cart body
                case 6:
                    // code block
                    break;
                default:
                    break;
            }
        }
    }

    [ServerRpc]
    void SubmitShootServerRpc()
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
    void SummitGetHitServerRpc(ulong playerid, float damage)
    {

        Debug.Log("ShooterController, SummitGetHitServerRpc : touched player = #" + playerid);
        var id = GetNetworkObject(playerid);
        GetNetworkObject(playerid).GetComponentInChildren<ShooterController>().GetHit(damage);
    }

    [ClientRpc]
    internal void FindSeatClientRpc(ulong ownerObjId, int seatId)
    {
        Debug.Log("ShooterController, SummitGetHitServerRpc : cart owner = #" + ownerObjId);
        var ship = GetNetworkObject(ownerObjId).GetComponentInChildren<ClientAutoritative.ShipController>();
        Seat = ship.Seats[seatId];
    }

    private void GetHit(float damage)
    {
        Debug.Log("ShooterController, GetHit : damage = " + damage);
        SetHealth(Mathf.Max(0f, health.Value - damage));
        if (health.Value == 0) SubmitDieServerRpc(NetworkObjectId);
    }

    [ServerRpc]
    private void SubmitDieServerRpc(ulong playerid)
    {
        Debug.Log("ShooterController, SubmitDieServerRpc : playerid = " + playerid);
        KillPlayerClientRpc(playerid);
    }

    [ClientRpc]
    private void KillPlayerClientRpc(ulong playerid)
    {
        Debug.Log("ShooterController, KillPlayerClientRpc : playerid = " + playerid);
        GetNetworkObject(playerid).gameObject.GetComponentInChildren<ShooterController>().Die();
    }
    private void Die()
    {
        Debug.Log("ShooterController, Die : Die = " + NetworkObjectId);
        Destroy(gameObject);
    }

    internal void Look(Vector2 vector2)
    {
        look.Value = vector2;
    }
}
