
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

        [SerializeField] float stickLookSensibility = 10f;
        [SerializeField] float mouseLookSensibility = 1f;

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

            OnlineInputManager.Controls.ShootingActions.Look.performed += ctx => OnLook(ctx.ReadValue<Vector2>() * mouseLookSensibility);
            OnlineInputManager.Controls.ShootingActions.Look.canceled += _ => OnLook(Vector2.zero);

            OnlineInputManager.Controls.ShootingActions.LookStick.performed += ctx => OnLook((ctx.ReadValue<Vector2>().x * Vector2.right - ctx.ReadValue<Vector2>().y * Vector2.up) * stickLookSensibility);
            OnlineInputManager.Controls.ShootingActions.LookStick.canceled += _ => OnLook(Vector2.zero);

            OnlineInputManager.Controls.DrivingActions.ParkingBreak.performed += _ => OnParkingBreaking(true);
            OnlineInputManager.Controls.DrivingActions.ParkingBreak.canceled += _ => OnParkingBreaking(false);

            OnlineInputManager.Controls.ShootingActions.WeaponShop.performed += _ => OnWeaponShop(true);
            OnlineInputManager.Controls.ShootingActions.WeaponShop.canceled += _ => OnWeaponShop(false);

            OnlineInputManager.Controls.ShootingActions.ToolShop.performed += _ => OnToolShop(true);
            OnlineInputManager.Controls.ShootingActions.ToolShop.canceled += _ => OnToolShop(false);

            OnlineInputManager.Controls.ShootingActions.SwitchItem.performed += _ => OnSwitchItem();
        }

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

            if (shooter.IsOnMenu && context) shooter.BuyItem();
            else shooter.Shoot(context);
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
