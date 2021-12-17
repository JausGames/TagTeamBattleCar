// GENERATED AUTOMATICALLY FROM 'Assets/Controls/Controls.inputactions'

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class @Controls : IInputActionCollection, IDisposable
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
                    ""name"": ""Steer"",
                    ""type"": ""Value"",
                    ""id"": ""54beb160-9519-462e-8b4d-1a5108c8ba45"",
                    ""expectedControlType"": ""Axis"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Torque"",
                    ""type"": ""Value"",
                    ""id"": ""d8071b42-e196-4a04-ac04-280440f12c75"",
                    ""expectedControlType"": ""Axis"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": ""1D Axis"",
                    ""id"": ""77634013-a622-438f-86eb-35adbbea08fe"",
                    ""path"": ""1DAxis"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Steer"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""negative"",
                    ""id"": ""95ac09ab-3be7-4a66-99bf-b23d26c3c561"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Steer"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""positive"",
                    ""id"": ""fa61eed1-c8f8-4110-862c-b22950889683"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Steer"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
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
                    ""interactions"": """"
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
        }
    ]
}");
        // DrivingActions
        m_DrivingActions = asset.FindActionMap("DrivingActions", throwIfNotFound: true);
        m_DrivingActions_Steer = m_DrivingActions.FindAction("Steer", throwIfNotFound: true);
        m_DrivingActions_Torque = m_DrivingActions.FindAction("Torque", throwIfNotFound: true);
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

    // DrivingActions
    private readonly InputActionMap m_DrivingActions;
    private IDrivingActionsActions m_DrivingActionsActionsCallbackInterface;
    private readonly InputAction m_DrivingActions_Steer;
    private readonly InputAction m_DrivingActions_Torque;
    public struct DrivingActionsActions
    {
        private @Controls m_Wrapper;
        public DrivingActionsActions(@Controls wrapper) { m_Wrapper = wrapper; }
        public InputAction @Steer => m_Wrapper.m_DrivingActions_Steer;
        public InputAction @Torque => m_Wrapper.m_DrivingActions_Torque;
        public InputActionMap Get() { return m_Wrapper.m_DrivingActions; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(DrivingActionsActions set) { return set.Get(); }
        public void SetCallbacks(IDrivingActionsActions instance)
        {
            if (m_Wrapper.m_DrivingActionsActionsCallbackInterface != null)
            {
                @Steer.started -= m_Wrapper.m_DrivingActionsActionsCallbackInterface.OnSteer;
                @Steer.performed -= m_Wrapper.m_DrivingActionsActionsCallbackInterface.OnSteer;
                @Steer.canceled -= m_Wrapper.m_DrivingActionsActionsCallbackInterface.OnSteer;
                @Torque.started -= m_Wrapper.m_DrivingActionsActionsCallbackInterface.OnTorque;
                @Torque.performed -= m_Wrapper.m_DrivingActionsActionsCallbackInterface.OnTorque;
                @Torque.canceled -= m_Wrapper.m_DrivingActionsActionsCallbackInterface.OnTorque;
            }
            m_Wrapper.m_DrivingActionsActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Steer.started += instance.OnSteer;
                @Steer.performed += instance.OnSteer;
                @Steer.canceled += instance.OnSteer;
                @Torque.started += instance.OnTorque;
                @Torque.performed += instance.OnTorque;
                @Torque.canceled += instance.OnTorque;
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
    public interface IDrivingActionsActions
    {
        void OnSteer(InputAction.CallbackContext context);
        void OnTorque(InputAction.CallbackContext context);
    }
    public interface IShootingActionsActions
    {
        void OnNewaction(InputAction.CallbackContext context);
    }
}
