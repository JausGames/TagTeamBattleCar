//------------------------------------------------------------------------------
// <auto-generated>
//     This code was auto-generated by com.unity.inputsystem:InputActionCodeGenerator
//     version 1.4.2
//     from Assets/Controls/Controls.inputactions
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public partial class @Controls : IInputActionCollection2, IDisposable
{
    public InputActionAsset asset { get; }
    public @Controls()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""Controls"",
    ""maps"": [
        {
            ""name"": ""DrivingActions"",
            ""id"": ""a04d92d6-f200-4f4f-8ec0-32ef0d099861"",
            ""actions"": [
                {
                    ""name"": ""Rotate"",
                    ""type"": ""Value"",
                    ""id"": ""54beb160-9519-462e-8b4d-1a5108c8ba45"",
                    ""expectedControlType"": ""Axis"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""Torque"",
                    ""type"": ""Value"",
                    ""id"": ""d8071b42-e196-4a04-ac04-280440f12c75"",
                    ""expectedControlType"": ""Axis"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""ChangeRotationAxe"",
                    ""type"": ""Button"",
                    ""id"": ""5e5bb05a-a971-41d2-b47a-89e38514e0d9"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""ParkingBreak"",
                    ""type"": ""Button"",
                    ""id"": ""b7967166-8ca2-41e5-917e-d70cad8bdc02"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                }
            ],
            ""bindings"": [
                {
                    ""name"": ""1D Axis"",
                    ""id"": ""b5fbb121-3a64-44d0-a600-e4ddf79081cb"",
                    ""path"": ""1DAxis"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Torque"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""negative"",
                    ""id"": ""a8609f52-7ba3-49c6-87c0-a42887799a81"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Torque"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""positive"",
                    ""id"": ""32e86c0f-9678-4587-8164-722aef24b13d"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Torque"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""1D Axis"",
                    ""id"": ""9a1ada2e-ad63-40e4-9f98-4d0dd8540fd6"",
                    ""path"": ""1DAxis"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Torque"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""negative"",
                    ""id"": ""1b4dd337-112d-46f0-9231-4a437f367733"",
                    ""path"": ""<Gamepad>/leftTrigger"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Torque"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""positive"",
                    ""id"": ""9848821e-33c3-4d8f-af6f-5dbb5b25bd85"",
                    ""path"": ""<Gamepad>/rightTrigger"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Torque"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""1D Axis"",
                    ""id"": ""2ccd93cb-2788-4aaf-acdd-f4d8117247be"",
                    ""path"": ""1DAxis"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Rotate"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""negative"",
                    ""id"": ""1a2e675f-c442-4f38-8075-dfdbe751a165"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Rotate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""positive"",
                    ""id"": ""95379bf1-933f-4dde-9981-b9d762e78db2"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Rotate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""1D Axis"",
                    ""id"": ""709c2870-5cdb-4206-815b-ba8261e47b3b"",
                    ""path"": ""1DAxis"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Rotate"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""negative"",
                    ""id"": ""c3fe57a3-9537-4a08-91d3-4f0fb2e74647"",
                    ""path"": ""<Gamepad>/leftStick/left"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Rotate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""positive"",
                    ""id"": ""1ba29d4e-4667-4815-86ec-c2fc5816d274"",
                    ""path"": ""<Gamepad>/leftStick/right"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Rotate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""da721dd0-7268-4496-a11f-1bc4c245e8db"",
                    ""path"": ""<Gamepad>/leftShoulder"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""ChangeRotationAxe"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""16c1c729-7064-43d4-a7a8-2688e72e728c"",
                    ""path"": ""<Keyboard>/shift"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""ChangeRotationAxe"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""c9971234-64f3-4d65-acb0-cded42a14a83"",
                    ""path"": ""<Gamepad>/buttonWest"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""ParkingBreak"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""ce846cc2-7338-410b-a702-ca9a4eb58f99"",
                    ""path"": ""<Keyboard>/ctrl"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""ParkingBreak"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        },
        {
            ""name"": ""ShootingActions"",
            ""id"": ""eb34750f-6567-4173-b97a-cfbe26cfdcce"",
            ""actions"": [
                {
                    ""name"": ""Shoot"",
                    ""type"": ""Button"",
                    ""id"": ""826f04dc-f5fd-4ede-8c16-8f06e4a45cfd"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Look"",
                    ""type"": ""Value"",
                    ""id"": ""4aee7aab-093b-4745-90b3-097b677b5096"",
                    ""expectedControlType"": ""Stick"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""18c3b314-aa8f-4e7a-bf33-00ce63949ecb"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Shoot"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""cea6f4dc-3673-4012-8b44-e9405044af5b"",
                    ""path"": ""<Gamepad>/rightShoulder"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Shoot"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""72ca4160-936e-407b-8d58-919416633754"",
                    ""path"": ""<Gamepad>/rightStick"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Look"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": [
        {
            ""name"": ""Keyboard"",
            ""bindingGroup"": ""Keyboard"",
            ""devices"": [
                {
                    ""devicePath"": ""<Keyboard>"",
                    ""isOptional"": false,
                    ""isOR"": false
                },
                {
                    ""devicePath"": ""<Mouse>"",
                    ""isOptional"": false,
                    ""isOR"": false
                }
            ]
        },
        {
            ""name"": ""Gamepad"",
            ""bindingGroup"": ""Gamepad"",
            ""devices"": [
                {
                    ""devicePath"": ""<Gamepad>"",
                    ""isOptional"": false,
                    ""isOR"": false
                }
            ]
        }
    ]
}");
        // DrivingActions
        m_DrivingActions = asset.FindActionMap("DrivingActions", throwIfNotFound: true);
        m_DrivingActions_Rotate = m_DrivingActions.FindAction("Rotate", throwIfNotFound: true);
        m_DrivingActions_Torque = m_DrivingActions.FindAction("Torque", throwIfNotFound: true);
        m_DrivingActions_ChangeRotationAxe = m_DrivingActions.FindAction("ChangeRotationAxe", throwIfNotFound: true);
        m_DrivingActions_ParkingBreak = m_DrivingActions.FindAction("ParkingBreak", throwIfNotFound: true);
        // ShootingActions
        m_ShootingActions = asset.FindActionMap("ShootingActions", throwIfNotFound: true);
        m_ShootingActions_Shoot = m_ShootingActions.FindAction("Shoot", throwIfNotFound: true);
        m_ShootingActions_Look = m_ShootingActions.FindAction("Look", throwIfNotFound: true);
    }

    public void Dispose()
    {
        UnityEngine.Object.Destroy(asset);
    }

    public InputBinding? bindingMask
    {
        get => asset.bindingMask;
        set => asset.bindingMask = value;
    }

    public ReadOnlyArray<InputDevice>? devices
    {
        get => asset.devices;
        set => asset.devices = value;
    }

    public ReadOnlyArray<InputControlScheme> controlSchemes => asset.controlSchemes;

    public bool Contains(InputAction action)
    {
        return asset.Contains(action);
    }

    public IEnumerator<InputAction> GetEnumerator()
    {
        return asset.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public void Enable()
    {
        asset.Enable();
    }

    public void Disable()
    {
        asset.Disable();
    }
    public IEnumerable<InputBinding> bindings => asset.bindings;

    public InputAction FindAction(string actionNameOrId, bool throwIfNotFound = false)
    {
        return asset.FindAction(actionNameOrId, throwIfNotFound);
    }
    public int FindBinding(InputBinding bindingMask, out InputAction action)
    {
        return asset.FindBinding(bindingMask, out action);
    }

    // DrivingActions
    private readonly InputActionMap m_DrivingActions;
    private IDrivingActionsActions m_DrivingActionsActionsCallbackInterface;
    private readonly InputAction m_DrivingActions_Rotate;
    private readonly InputAction m_DrivingActions_Torque;
    private readonly InputAction m_DrivingActions_ChangeRotationAxe;
    private readonly InputAction m_DrivingActions_ParkingBreak;
    public struct DrivingActionsActions
    {
        private @Controls m_Wrapper;
        public DrivingActionsActions(@Controls wrapper) { m_Wrapper = wrapper; }
        public InputAction @Rotate => m_Wrapper.m_DrivingActions_Rotate;
        public InputAction @Torque => m_Wrapper.m_DrivingActions_Torque;
        public InputAction @ChangeRotationAxe => m_Wrapper.m_DrivingActions_ChangeRotationAxe;
        public InputAction @ParkingBreak => m_Wrapper.m_DrivingActions_ParkingBreak;
        public InputActionMap Get() { return m_Wrapper.m_DrivingActions; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(DrivingActionsActions set) { return set.Get(); }
        public void SetCallbacks(IDrivingActionsActions instance)
        {
            if (m_Wrapper.m_DrivingActionsActionsCallbackInterface != null)
            {
                @Rotate.started -= m_Wrapper.m_DrivingActionsActionsCallbackInterface.OnRotate;
                @Rotate.performed -= m_Wrapper.m_DrivingActionsActionsCallbackInterface.OnRotate;
                @Rotate.canceled -= m_Wrapper.m_DrivingActionsActionsCallbackInterface.OnRotate;
                @Torque.started -= m_Wrapper.m_DrivingActionsActionsCallbackInterface.OnTorque;
                @Torque.performed -= m_Wrapper.m_DrivingActionsActionsCallbackInterface.OnTorque;
                @Torque.canceled -= m_Wrapper.m_DrivingActionsActionsCallbackInterface.OnTorque;
                @ChangeRotationAxe.started -= m_Wrapper.m_DrivingActionsActionsCallbackInterface.OnChangeRotationAxe;
                @ChangeRotationAxe.performed -= m_Wrapper.m_DrivingActionsActionsCallbackInterface.OnChangeRotationAxe;
                @ChangeRotationAxe.canceled -= m_Wrapper.m_DrivingActionsActionsCallbackInterface.OnChangeRotationAxe;
                @ParkingBreak.started -= m_Wrapper.m_DrivingActionsActionsCallbackInterface.OnParkingBreak;
                @ParkingBreak.performed -= m_Wrapper.m_DrivingActionsActionsCallbackInterface.OnParkingBreak;
                @ParkingBreak.canceled -= m_Wrapper.m_DrivingActionsActionsCallbackInterface.OnParkingBreak;
            }
            m_Wrapper.m_DrivingActionsActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Rotate.started += instance.OnRotate;
                @Rotate.performed += instance.OnRotate;
                @Rotate.canceled += instance.OnRotate;
                @Torque.started += instance.OnTorque;
                @Torque.performed += instance.OnTorque;
                @Torque.canceled += instance.OnTorque;
                @ChangeRotationAxe.started += instance.OnChangeRotationAxe;
                @ChangeRotationAxe.performed += instance.OnChangeRotationAxe;
                @ChangeRotationAxe.canceled += instance.OnChangeRotationAxe;
                @ParkingBreak.started += instance.OnParkingBreak;
                @ParkingBreak.performed += instance.OnParkingBreak;
                @ParkingBreak.canceled += instance.OnParkingBreak;
            }
        }
    }
    public DrivingActionsActions @DrivingActions => new DrivingActionsActions(this);

    // ShootingActions
    private readonly InputActionMap m_ShootingActions;
    private IShootingActionsActions m_ShootingActionsActionsCallbackInterface;
    private readonly InputAction m_ShootingActions_Shoot;
    private readonly InputAction m_ShootingActions_Look;
    public struct ShootingActionsActions
    {
        private @Controls m_Wrapper;
        public ShootingActionsActions(@Controls wrapper) { m_Wrapper = wrapper; }
        public InputAction @Shoot => m_Wrapper.m_ShootingActions_Shoot;
        public InputAction @Look => m_Wrapper.m_ShootingActions_Look;
        public InputActionMap Get() { return m_Wrapper.m_ShootingActions; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(ShootingActionsActions set) { return set.Get(); }
        public void SetCallbacks(IShootingActionsActions instance)
        {
            if (m_Wrapper.m_ShootingActionsActionsCallbackInterface != null)
            {
                @Shoot.started -= m_Wrapper.m_ShootingActionsActionsCallbackInterface.OnShoot;
                @Shoot.performed -= m_Wrapper.m_ShootingActionsActionsCallbackInterface.OnShoot;
                @Shoot.canceled -= m_Wrapper.m_ShootingActionsActionsCallbackInterface.OnShoot;
                @Look.started -= m_Wrapper.m_ShootingActionsActionsCallbackInterface.OnLook;
                @Look.performed -= m_Wrapper.m_ShootingActionsActionsCallbackInterface.OnLook;
                @Look.canceled -= m_Wrapper.m_ShootingActionsActionsCallbackInterface.OnLook;
            }
            m_Wrapper.m_ShootingActionsActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Shoot.started += instance.OnShoot;
                @Shoot.performed += instance.OnShoot;
                @Shoot.canceled += instance.OnShoot;
                @Look.started += instance.OnLook;
                @Look.performed += instance.OnLook;
                @Look.canceled += instance.OnLook;
            }
        }
    }
    public ShootingActionsActions @ShootingActions => new ShootingActionsActions(this);
    private int m_KeyboardSchemeIndex = -1;
    public InputControlScheme KeyboardScheme
    {
        get
        {
            if (m_KeyboardSchemeIndex == -1) m_KeyboardSchemeIndex = asset.FindControlSchemeIndex("Keyboard");
            return asset.controlSchemes[m_KeyboardSchemeIndex];
        }
    }
    private int m_GamepadSchemeIndex = -1;
    public InputControlScheme GamepadScheme
    {
        get
        {
            if (m_GamepadSchemeIndex == -1) m_GamepadSchemeIndex = asset.FindControlSchemeIndex("Gamepad");
            return asset.controlSchemes[m_GamepadSchemeIndex];
        }
    }
    public interface IDrivingActionsActions
    {
        void OnRotate(InputAction.CallbackContext context);
        void OnTorque(InputAction.CallbackContext context);
        void OnChangeRotationAxe(InputAction.CallbackContext context);
        void OnParkingBreak(InputAction.CallbackContext context);
    }
    public interface IShootingActionsActions
    {
        void OnShoot(InputAction.CallbackContext context);
        void OnLook(InputAction.CallbackContext context);
    }
}
