using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Handgun : Weapon
{

    public override void Use(ShooterController owner)
    {
        base.Use(owner);

        var hits = ShootRaycast(owner.CameraContainer.forward, owner.Hitablemask);

        foreach (var hit in hits)
        {
            switch (hit.collider.gameObject.layer)
            {
                // Hit player body
                case 3: case 6:
                    /*var ennemy = hit.collider.GetComponent<Player>() ? hit.collider.GetComponent<Player>() : hit.collider.GetComponentInParent<Player>();
                    Debug.Log("ShooterController, Shoot : #" + owner.NetworkObjectId + " shot #" + ennemy.NetworkObjectId);
                    owner.SummitGetHitServerRpc(ennemy.NetworkObjectId, damage);*/
                    break;
                default:
                    break;
            }
        }
    }
}
