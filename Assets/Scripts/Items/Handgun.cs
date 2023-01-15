using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Handgun : Weapon
{

    public override void Use(ShooterController owner)
    {
        if (Time.time < nextShot) return;
        nextShot = Time.time + coolDown;

        owner.CameraFollow.RotationOffset = rndRecoil;

        owner.SubmitShootServerRpc();
        var hits = Physics.RaycastAll(canonEnd.position, owner.CameraContainer.forward, 50f, owner.Hitablemask);
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
