
using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputAction;

namespace Inputs
{
    public class PlayerInputs : NetworkBehaviour
    {
        [SerializeField] ClientAutoritative.ShipController motor = null;
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

            OnlineInputManager.Controls.DrivingActions.ParkingBreak.performed += _ => OnParkingBreaking(true);
            OnlineInputManager.Controls.DrivingActions.ParkingBreak.canceled += _ => OnParkingBreaking(false);
        }
        public void OnRotate(float context)
        {
            //Debug.Log("Network Informations : IsLocalPlayer " + IsLocalPlayer);
            if (motor == null || !IsOwner) return;
            motor.SetRotation(context);
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
