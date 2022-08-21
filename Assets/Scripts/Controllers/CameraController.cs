using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace WoodsOfIdle
{
    public class CameraController : MonoBehaviour
    {
        public Camera playerCamera;
        public Vector2 moveScale = new Vector2(0.00085f, 0.0018f);

        private PlayerInputActions playerInput;
        private bool isPointerPressed;
        private Vector2 mousePositionStart;
        private Vector3 cameraPositionStart;

        private void Awake()
        {
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
            playerInput.Player.PointerMoved.performed += OnPointerMoved;
        }

        private void OnPointerPressed(InputAction.CallbackContext context)
        {
            isPointerPressed = true;
            mousePositionStart = GetPointPosition();
            cameraPositionStart = playerCamera.transform.position;
        }

        private void OnPointerReleased(InputAction.CallbackContext context)
        {
            isPointerPressed = false;
        }

        private void OnPointerMoved(InputAction.CallbackContext context)
        {
            if (isPointerPressed)
            {
                Vector2 mouseDelta = mousePositionStart - GetPointPosition();
                Vector3 cameraDeltaX = Vector3.ProjectOnPlane(playerCamera.transform.right, Vector3.up).normalized * mouseDelta.x * moveScale.x * playerCamera.orthographicSize;
                Vector3 cameraDeltaZ = Vector3.ProjectOnPlane(playerCamera.transform.forward, Vector3.up).normalized * mouseDelta.y * moveScale.y * playerCamera.orthographicSize;
                playerCamera.transform.position = cameraPositionStart + cameraDeltaX + cameraDeltaZ;
            }
        }

        private Vector2 GetPointPosition()
        {
            #if UNITY_EDITOR
            return Mouse.current.position.ReadValue();
            #elif UNITY_ANDROID || UNITY_IOS
            return Touchscreen.current.primaryTouch.position.ReadValue();
            #endif
        }
    }
}

