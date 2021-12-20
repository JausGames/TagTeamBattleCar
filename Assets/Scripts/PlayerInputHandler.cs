using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputAction;
using System.Linq;

namespace Inputs
{
    public class PlayerInputHandler : MonoBehaviour
    {
        [SerializeField] PlayerInput playerInput;
        //[SerializeField] PlayerCombat combat;
        [SerializeField] CarController motor = null;
        [SerializeField] private Vector2 move;
        [SerializeField] private Vector2 look;
        [SerializeField] private float index;
        [SerializeField] private bool attack = false;

        private Controls controls;
        private Controls Controls
        {
            get
            {
                if (controls != null) { return controls; }
                return controls = new Controls();
            }
        }
        private void Awake()
        {
            playerInput = GetComponent<PlayerInput>();
        }

        // Update is called once per frame
        private void Start()
        {
            /*index = playerInput.playerIndex;
            var motors = FindObjectsOfType<PlayerController>();
            motor = motors.FirstOrDefault(m => m.GetPlayerIndex() == index);

            var combats = FindObjectsOfType<PlayerCombat>();
            combat = combats.FirstOrDefault(m => m.GetPlayerIndex() == index);*/

        }
        public void FindPlayer()
        {
            /*index = playerInput.playerIndex;
            var motors = FindObjectsOfType<PlayerController>();
            motor = motors.FirstOrDefault(m => m.GetPlayerIndex() == index);

            var combats = FindObjectsOfType<PlayerCombat>();
            combat = combats.FirstOrDefault(m => m.GetPlayerIndex() == index);*/
        }
        public void OnRotate(CallbackContext context)
        {
            if (motor == null) return;
            motor.Rotation = context.ReadValue<Vector2>();
        }
        public void OnTorque(CallbackContext context)
        {
            if (motor == null) return;
            motor.TorqueApply = context.ReadValue<float>();
        }
        public void OnChangeRotation(CallbackContext context)
        {
            if (motor == null) return;
            motor.RotationAxeIsY = context.canceled;
        }
        public void OnParkingBreaking(CallbackContext context)
        {
            if (motor == null) return;
            if (context.performed)
                motor.ParkingBreaking = true;
            else if(context.canceled)
                motor.ParkingBreaking = false;
        }
    }
}
