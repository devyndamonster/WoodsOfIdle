using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace WoodsOfIdle
{
    public class CameraController : MonoBehaviour
    {
        private PlayerInput playerInputComponent;
        private PlayerInputActions playerInput;

        private void Awake()
        {
            playerInputComponent = FindObjectOfType<PlayerInput>();


            playerInput = new PlayerInputActions();
        }

        private void OnEnable()
        {
            playerInput.Player.Enable();
        }

        private void OnDisable()
        {
            playerInput.Player.Disable();
        }

        private void Start()
        {
            playerInput.Player.PointerPressed.performed += OnPointerPressed;
            playerInput.Player.PointerPressed.canceled += OnPointerReleased;
            //playerInput.Player.PointerMoved.performed += OnPointerMoved;

        }

        private void OnPointerPressed(InputAction.CallbackContext context)
        {
            Debug.Log("Started press!");
        }

        private void OnPointerReleased(InputAction.CallbackContext context)
        {
            Debug.Log("Stopped press!");
        }

        private void OnPointerMoved(InputAction.CallbackContext context)
        {
            Debug.Log("Mouse Moved");
        }
    }
}

