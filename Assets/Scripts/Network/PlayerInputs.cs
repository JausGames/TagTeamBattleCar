
using System;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputAction;

namespace Inputs
{
    public class PlayerInputs : NetworkBehaviour
    {
        [SerializeField] ClientAutoritative.ShipController motor = null;
        [SerializeField] ShooterController shooter = null;

        public void Start()
        {
            Debug.Log("Network Informations : IsOwner " + IsOwner);
            if (!IsOwner) return;

            OnlineInputManager.Controls.DrivingActions.Torque.performed += ctx => OnTorque(ctx.ReadValue<float>());
            OnlineInputManager.Controls.DrivingActions.Torque.canceled += _ => OnTorque(0f);

            OnlineInputManager.Controls.DrivingActions.Rotate.performed += ctx => OnRotate(ctx.ReadValue<float>());
            OnlineInputManager.Controls.DrivingActions.Rotate.canceled += _ => OnRotate(0f);

            OnlineInputManager.Controls.DrivingActions.ChangeRotationAxe.performed += _ => OnChangeRotation(false);
            OnlineInputManager.Controls.DrivingActions.ChangeRotationAxe.canceled += _ => OnChangeRotation(true);

            OnlineInputManager.Controls.ShootingActions.Shoot.performed += _ => OnShoot(true);
            OnlineInputManager.Controls.ShootingActions.Shoot.canceled += _ => OnShoot(false);

            OnlineInputManager.Controls.ShootingActions.Look.performed += ctx => OnLook(ctx.ReadValue<Vector2>());
            OnlineInputManager.Controls.ShootingActions.Look.canceled += _ => OnLook(Vector2.zero);

            OnlineInputManager.Controls.DrivingActions.ParkingBreak.performed += _ => OnParkingBreaking(true);
            OnlineInputManager.Controls.DrivingActions.ParkingBreak.canceled += _ => OnParkingBreaking(false);
        }

        private void OnLook(Vector2 vector2)
        {
            if (shooter == null || !IsOwner) return;
            shooter.Look(vector2);
        }

        public void OnRotate(float context)
        {
            //Debug.Log("Network Informations : IsLocalPlayer " + IsLocalPlayer);
            if (motor == null || !IsOwner) return;
            motor.SetRotation(context);
        }
        public void OnShoot(bool context)
        {
            Debug.Log("Network Informations : IsLocalPlayer " + IsLocalPlayer);
            if (shooter == null || !IsOwner) return;
            shooter.Shoot(context);
        }
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
