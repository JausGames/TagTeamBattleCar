
using System;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputAction;

namespace Inputs
{
    public class AiInputs : NetworkBehaviour
    {
        [SerializeField] ClientAutoritative.ShipController motor = null;

        AiPath path;
        int segmentId = 0;

        public void Update()
        {
            if (path == null)
            {
                path = FindObjectOfType<AiPath>();
            }
            else
            {
                var nextCheckpoint = segmentId == path.Checkpoints.Count - 1 ? 0 : segmentId + 1;
                var destinationV3 = FindNearestPointOnLine(path.Checkpoints[segmentId].position, path.Checkpoints[segmentId].position, transform.position + (path.Checkpoints[nextCheckpoint].position - transform.position) * .01f);
                //var destinationV3 = path.Checkpoints[segmentId].position;
   
                var move = (Vector3.Dot(destinationV3 - transform.position, transform.forward) * Vector2.up + Vector3.Dot(destinationV3 - transform.position, transform.right) * Vector2.right).normalized * .2f;

                OnRotate(move.x);
                OnTorque(move.y);

                Debug.Log("AiInputs, Update : length to end = " + (path.Segments[segmentId][1] - transform.position).magnitude);

                if ((destinationV3 - transform.position).magnitude < 30f) 
                    segmentId = nextCheckpoint;

            }
        }
        public Vector3 FindNearestPointOnLine(Vector3 origin, Vector3 end, Vector3 point)
        {
            //Get heading
            Vector3 heading = (end - origin);
            float magnitudeMax = heading.magnitude;
            heading.Normalize();

            //Do projection from the point but clamp it
            Vector2 lhs = point - origin;
            float dotP = Vector2.Dot(lhs, heading);
            dotP = Mathf.Clamp(dotP, 0f, magnitudeMax);
            return origin + heading * dotP;
        }
        /*
                private void OnSwitchItem()
                {
                    if (shooter == null || !IsOwner) return;
                    shooter.SwitchItem();
                }

                private void OnToolShop(bool v)
                {
                    if (shooter == null || !IsOwner) return;
                    shooter.ActivateToolWheel(v);
                }

                private void OnWeaponShop(bool v)
                {
                    if (shooter == null || !IsOwner) return;
                    shooter.ActivateWeaponWheel(v);
                }

                private void OnLook(Vector2 vector2)
                {
                    if (shooter == null || !IsOwner) return;
                    shooter.Look(vector2);
                }*/

        public void OnRotate(float context)
        {
            //Debug.Log("Network Informations : IsLocalPlayer " + IsLocalPlayer);
            if (motor == null || !IsOwner) return;
            motor.SetRotation(context);
        }
       /* public void OnShoot(bool context)
        {
            Debug.Log("Network Informations : IsLocalPlayer " + IsLocalPlayer);
            if (shooter == null || !IsOwner) return;

            if (shooter.IsOnMenu && context) shooter.BuyItem();
            else shooter.Shoot(context);
        }*/
        public void OnTorque(float context)
        {
            Debug.Log(gameObject.ToString() + ", Network Informations : IsLocalPlayer " + IsLocalPlayer);
            //Debug.Log("Network Informations : IsLocalPlayer " + IsLocalPlayer);
            if (motor == null || !IsOwner) return;
            motor.SetMotorTorque(context);
        }
        public void OnChangeRotation(bool context)
        {
            //Debug.Log(gameObject.ToString() + ", Network Informations : IsLocalPlayer " + IsLocalPlayer);
            if (motor == null || !IsOwner) return;
            motor.SetRotationAxeY(context);
        }
        public void OnParkingBreaking(bool context)
        {
            //Debug.Log("Network Informations : IsLocalPlayer " + IsLocalPlayer);
            if (motor == null || !IsOwner) return;
                motor.SetParkingBreak(context);
        }
    }
}
