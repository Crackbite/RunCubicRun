//------------------------------------------------------------------------------
// <auto-generated>
//     This code was auto-generated by com.unity.inputsystem:InputActionCodeGenerator
//     version 1.4.4
//     from Assets/InputActions/PlayerInput.inputactions
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

public partial class @PlayerInput : IInputActionCollection2, IDisposable
{
    public InputActionAsset asset { get; }
    public @PlayerInput()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""PlayerInput"",
    ""maps"": [
        {
            ""name"": ""Cubic"",
            ""id"": ""b0bc01ae-27f7-40b3-8f4b-f7d8421db9dd"",
            ""actions"": [
                {
                    ""name"": ""KeyboardArrow"",
                    ""type"": ""Value"",
                    ""id"": ""0816c746-d3f8-4b91-9a56-2e4dea4b5070"",
                    ""expectedControlType"": ""Vector3"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""KeyboardWASD"",
                    ""type"": ""Value"",
                    ""id"": ""1a1eb9c6-0826-4a45-8d39-7c4291c824f3"",
                    ""expectedControlType"": ""Vector3"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""PointerPosition"",
                    ""type"": ""Value"",
                    ""id"": ""bb4fc578-2d86-4bdf-8425-bd44edb42a9c"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""PointerPress"",
                    ""type"": ""Button"",
                    ""id"": ""b602f47a-75bc-4cf2-a8e6-7b1f257f6e11"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""PressSpeedReduce"",
                    ""type"": ""Button"",
                    ""id"": ""af889383-7be0-48cc-998e-c4e4a521498c"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                }
            ],
            ""bindings"": [
                {
                    ""name"": ""3D Vector"",
                    ""id"": ""c607644f-3470-408e-ba79-60eff5d94623"",
                    ""path"": ""3DVector(mode=1)"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""KeyboardArrow"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""Up"",
                    ""id"": ""db7bd348-53d9-40a8-af16-15e91378d8d3"",
                    ""path"": ""<Keyboard>/upArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""KeyboardArrow"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""Forward"",
                    ""id"": ""50ba34ef-4487-4029-8609-2ea202066b12"",
                    ""path"": ""<Keyboard>/leftArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""KeyboardArrow"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""Backward"",
                    ""id"": ""2e06a90e-0630-4e54-b379-c9011f5e8afd"",
                    ""path"": ""<Keyboard>/rightArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""KeyboardArrow"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""f00c6f80-e164-4aed-b724-7788e67c3da0"",
                    ""path"": ""<Pointer>/position"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Mouse;Touchscreen"",
                    ""action"": ""PointerPosition"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""9c38e8ca-3b3c-4bb2-a18a-7d5aebe7e54b"",
                    ""path"": ""<Pointer>/press"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Mouse;Touchscreen"",
                    ""action"": ""PointerPress"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""1d9ab85d-bb8c-4166-98b9-82f4501e5264"",
                    ""path"": ""<Keyboard>/space"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""PressSpeedReduce"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""1d9bbda7-93dc-4a09-a986-e273d8353ea5"",
                    ""path"": ""<Pointer>/press"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Mouse;Touchscreen"",
                    ""action"": ""PressSpeedReduce"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""3D Vector"",
                    ""id"": ""334472b9-c835-4489-ad6b-b5056043e404"",
                    ""path"": ""3DVector(mode=1)"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""KeyboardWASD"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""Up"",
                    ""id"": ""bbf5428a-7990-43c1-a2ea-6e9366573d4f"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""KeyboardWASD"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""Forward"",
                    ""id"": ""1c6db332-3aac-4086-9ce9-742046a1f56c"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""KeyboardWASD"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""Backward"",
                    ""id"": ""b02f4b7c-0e1a-459c-9b58-771b6056d694"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""KeyboardWASD"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                }
            ]
        }
    ],
    ""controlSchemes"": [
        {
            ""name"": ""Mouse"",
            ""bindingGroup"": ""Mouse"",
            ""devices"": [
                {
                    ""devicePath"": ""<Mouse>"",
                    ""isOptional"": false,
                    ""isOR"": false
                }
            ]
        },
        {
            ""name"": ""Keyboard"",
            ""bindingGroup"": ""Keyboard"",
            ""devices"": [
                {
                    ""devicePath"": ""<Keyboard>"",
                    ""isOptional"": false,
                    ""isOR"": false
                }
            ]
        },
        {
            ""name"": ""Touchscreen"",
            ""bindingGroup"": ""Touchscreen"",
            ""devices"": [
                {
                    ""devicePath"": ""<Touchscreen>"",
                    ""isOptional"": false,
                    ""isOR"": false
                }
            ]
        }
    ]
}");
        // Cubic
        m_Cubic = asset.FindActionMap("Cubic", throwIfNotFound: true);
        m_Cubic_KeyboardArrow = m_Cubic.FindAction("KeyboardArrow", throwIfNotFound: true);
        m_Cubic_KeyboardWASD = m_Cubic.FindAction("KeyboardWASD", throwIfNotFound: true);
        m_Cubic_PointerPosition = m_Cubic.FindAction("PointerPosition", throwIfNotFound: true);
        m_Cubic_PointerPress = m_Cubic.FindAction("PointerPress", throwIfNotFound: true);
        m_Cubic_PressSpeedReduce = m_Cubic.FindAction("PressSpeedReduce", throwIfNotFound: true);
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

    // Cubic
    private readonly InputActionMap m_Cubic;
    private ICubicActions m_CubicActionsCallbackInterface;
    private readonly InputAction m_Cubic_KeyboardArrow;
    private readonly InputAction m_Cubic_KeyboardWASD;
    private readonly InputAction m_Cubic_PointerPosition;
    private readonly InputAction m_Cubic_PointerPress;
    private readonly InputAction m_Cubic_PressSpeedReduce;
    public struct CubicActions
    {
        private @PlayerInput m_Wrapper;
        public CubicActions(@PlayerInput wrapper) { m_Wrapper = wrapper; }
        public InputAction @KeyboardArrow => m_Wrapper.m_Cubic_KeyboardArrow;
        public InputAction @KeyboardWASD => m_Wrapper.m_Cubic_KeyboardWASD;
        public InputAction @PointerPosition => m_Wrapper.m_Cubic_PointerPosition;
        public InputAction @PointerPress => m_Wrapper.m_Cubic_PointerPress;
        public InputAction @PressSpeedReduce => m_Wrapper.m_Cubic_PressSpeedReduce;
        public InputActionMap Get() { return m_Wrapper.m_Cubic; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(CubicActions set) { return set.Get(); }
        public void SetCallbacks(ICubicActions instance)
        {
            if (m_Wrapper.m_CubicActionsCallbackInterface != null)
            {
                @KeyboardArrow.started -= m_Wrapper.m_CubicActionsCallbackInterface.OnKeyboardArrow;
                @KeyboardArrow.performed -= m_Wrapper.m_CubicActionsCallbackInterface.OnKeyboardArrow;
                @KeyboardArrow.canceled -= m_Wrapper.m_CubicActionsCallbackInterface.OnKeyboardArrow;
                @KeyboardWASD.started -= m_Wrapper.m_CubicActionsCallbackInterface.OnKeyboardWASD;
                @KeyboardWASD.performed -= m_Wrapper.m_CubicActionsCallbackInterface.OnKeyboardWASD;
                @KeyboardWASD.canceled -= m_Wrapper.m_CubicActionsCallbackInterface.OnKeyboardWASD;
                @PointerPosition.started -= m_Wrapper.m_CubicActionsCallbackInterface.OnPointerPosition;
                @PointerPosition.performed -= m_Wrapper.m_CubicActionsCallbackInterface.OnPointerPosition;
                @PointerPosition.canceled -= m_Wrapper.m_CubicActionsCallbackInterface.OnPointerPosition;
                @PointerPress.started -= m_Wrapper.m_CubicActionsCallbackInterface.OnPointerPress;
                @PointerPress.performed -= m_Wrapper.m_CubicActionsCallbackInterface.OnPointerPress;
                @PointerPress.canceled -= m_Wrapper.m_CubicActionsCallbackInterface.OnPointerPress;
                @PressSpeedReduce.started -= m_Wrapper.m_CubicActionsCallbackInterface.OnPressSpeedReduce;
                @PressSpeedReduce.performed -= m_Wrapper.m_CubicActionsCallbackInterface.OnPressSpeedReduce;
                @PressSpeedReduce.canceled -= m_Wrapper.m_CubicActionsCallbackInterface.OnPressSpeedReduce;
            }
            m_Wrapper.m_CubicActionsCallbackInterface = instance;
            if (instance != null)
            {
                @KeyboardArrow.started += instance.OnKeyboardArrow;
                @KeyboardArrow.performed += instance.OnKeyboardArrow;
                @KeyboardArrow.canceled += instance.OnKeyboardArrow;
                @KeyboardWASD.started += instance.OnKeyboardWASD;
                @KeyboardWASD.performed += instance.OnKeyboardWASD;
                @KeyboardWASD.canceled += instance.OnKeyboardWASD;
                @PointerPosition.started += instance.OnPointerPosition;
                @PointerPosition.performed += instance.OnPointerPosition;
                @PointerPosition.canceled += instance.OnPointerPosition;
                @PointerPress.started += instance.OnPointerPress;
                @PointerPress.performed += instance.OnPointerPress;
                @PointerPress.canceled += instance.OnPointerPress;
                @PressSpeedReduce.started += instance.OnPressSpeedReduce;
                @PressSpeedReduce.performed += instance.OnPressSpeedReduce;
                @PressSpeedReduce.canceled += instance.OnPressSpeedReduce;
            }
        }
    }
    public CubicActions @Cubic => new CubicActions(this);
    private int m_MouseSchemeIndex = -1;
    public InputControlScheme MouseScheme
    {
        get
        {
            if (m_MouseSchemeIndex == -1) m_MouseSchemeIndex = asset.FindControlSchemeIndex("Mouse");
            return asset.controlSchemes[m_MouseSchemeIndex];
        }
    }
    private int m_KeyboardSchemeIndex = -1;
    public InputControlScheme KeyboardScheme
    {
        get
        {
            if (m_KeyboardSchemeIndex == -1) m_KeyboardSchemeIndex = asset.FindControlSchemeIndex("Keyboard");
            return asset.controlSchemes[m_KeyboardSchemeIndex];
        }
    }
    private int m_TouchscreenSchemeIndex = -1;
    public InputControlScheme TouchscreenScheme
    {
        get
        {
            if (m_TouchscreenSchemeIndex == -1) m_TouchscreenSchemeIndex = asset.FindControlSchemeIndex("Touchscreen");
            return asset.controlSchemes[m_TouchscreenSchemeIndex];
        }
    }
    public interface ICubicActions
    {
        void OnKeyboardArrow(InputAction.CallbackContext context);
        void OnKeyboardWASD(InputAction.CallbackContext context);
        void OnPointerPosition(InputAction.CallbackContext context);
        void OnPointerPress(InputAction.CallbackContext context);
        void OnPressSpeedReduce(InputAction.CallbackContext context);
    }
}
