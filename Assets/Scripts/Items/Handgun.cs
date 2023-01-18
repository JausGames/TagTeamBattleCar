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
                case 3:
                    var ennemy = hit.collider.GetComponent<ShooterController>() ? hit.collider.GetComponent<ShooterController>() : hit.collider.GetComponentInParent<ShooterController>();
                    Debug.Log("ShooterController, Shoot : #" + owner.NetworkObjectId + " shot #" + ennemy.NetworkObjectId);
                    owner.SummitGetHitServerRpc(ennemy.NetworkObjectId, damage);
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
}
