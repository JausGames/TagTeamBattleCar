//------------------------------------------------------------------------------
// <auto-generated>
//     This code was auto-generated by com.unity.inputsystem:InputActionCodeGenerator
//     version 1.2.0
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
                    ""expectedControlType"": ""Stick"",
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
                    ""name"": """",
                    ""id"": ""31a619d6-6424-44d5-a91e-cabf201c5bcb"",
                    ""path"": ""<Gamepad>/leftStick"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Rotate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""2D Vector"",
                    ""id"": ""bd876cc3-c6c5-4e05-89f0-38abb73cb5c1"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Rotate"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""6d410052-0d4a-435c-beef-61d6eea7023c"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Rotate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""fcfaab17-8994-48bf-8c71-3973cc20e72b"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Rotate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""70136a09-d74b-4370-94f5-b438e2b87ab5"",
                    ""path"": ""<Keyboard>/q"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Rotate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""3bda088d-8a2b-46ef-b7a2-bdaa70ba2dc8"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
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
                    ""name"": ""New action"",
                    ""type"": ""Button"",
                    ""id"": ""826f04dc-f5fd-4ede-8c16-8f06e4a45cfd"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""18c3b314-aa8f-4e7a-bf33-00ce63949ecb"",
                    ""path"": """",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""New action"",
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
        m_ShootingActions_Newaction = m_ShootingActions.FindAction("New action", throwIfNotFound: true);
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
    private readonly InputAction m_ShootingActions_Newaction;
    public struct ShootingActionsActions
    {
        private @Controls m_Wrapper;
        public ShootingActionsActions(@Controls wrapper) { m_Wrapper = wrapper; }
        public InputAction @Newaction => m_Wrapper.m_ShootingActions_Newaction;
        public InputActionMap Get() { return m_Wrapper.m_ShootingActions; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(ShootingActionsActions set) { return set.Get(); }
        public void SetCallbacks(IShootingActionsActions instance)
        {
            if (m_Wrapper.m_ShootingActionsActionsCallbackInterface != null)
            {
                @Newaction.started -= m_Wrapper.m_ShootingActionsActionsCallbackInterface.OnNewaction;
                @Newaction.performed -= m_Wrapper.m_ShootingActionsActionsCallbackInterface.OnNewaction;
                @Newaction.canceled -= m_Wrapper.m_ShootingActionsActionsCallbackInterface.OnNewaction;
            }
            m_Wrapper.m_ShootingActionsActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Newaction.started += instance.OnNewaction;
                @Newaction.performed += instance.OnNewaction;
                @Newaction.canceled += instance.OnNewaction;
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
        void OnNewaction(InputAction.CallbackContext context);
    }
}
