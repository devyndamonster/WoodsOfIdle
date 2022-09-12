using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace WoodsOfIdle
{
    public class CameraControllerComponent : MonoBehaviour
    {
        public Camera playerCamera;
        public Vector2 moveScale = new Vector2(0.00085f, 0.0018f);
        public float scrollZoomScale = .001f;
        public float touchZoomScale = .05f;

        private PlayerInputActions playerInput;
        private IPointerInfoService pointerInfoService;
        private Vector3 cameraPositionStart;
        private Vector2 pointerPositionStart;
        private float touchDistanceStart;
        private float cameraZoomStart;
        private bool isPointerActive;
        private bool isTouchZoomActive;
        

        private void Awake()
        {
            playerInput = new PlayerInputActions();
            pointerInfoService = new PointerInfoService();
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
            playerInput.Player.MouseScrolled.performed += OnMouseScrolled;
            playerInput.Player.SecondTouchContact.performed += OnSecondTouchPressed;
            playerInput.Player.SecondTouchContact.canceled += OnSecondTouchReleased;
            playerInput.Player.SecondTouchMoved.performed += OnSecondTouchMoved;
        }

        private void OnPointerPressed(InputAction.CallbackContext context)
        {
            isPointerActive = true;
            pointerPositionStart = pointerInfoService.GetPointerPosition();
            cameraPositionStart = playerCamera.transform.position;
        }

        private void OnPointerReleased(InputAction.CallbackContext context)
        {
            isPointerActive = false;
            isTouchZoomActive = false;
        }

        private void OnPointerMoved(InputAction.CallbackContext context)
        {
            if (isTouchZoomActive)
            {
                UpdateTouchZoom();
            }

            else if(isPointerActive)
            {
                Vector2 mouseDelta = pointerPositionStart - pointerInfoService.GetPointerPosition();
                Vector3 cameraDeltaX = Vector3.ProjectOnPlane(playerCamera.transform.right, Vector3.up).normalized * mouseDelta.x * moveScale.x * playerCamera.orthographicSize;
                Vector3 cameraDeltaZ = Vector3.ProjectOnPlane(playerCamera.transform.forward, Vector3.up).normalized * mouseDelta.y * moveScale.y * playerCamera.orthographicSize;
                playerCamera.transform.position = cameraPositionStart + cameraDeltaX + cameraDeltaZ;
            }
        }

        private void OnMouseScrolled(InputAction.CallbackContext context)
        {
            float zoomDelta = -context.ReadValue<Vector2>().y * scrollZoomScale;
            Debug.Log(zoomDelta);
            playerCamera.orthographicSize = Mathf.Clamp(playerCamera.orthographicSize + zoomDelta, 0.5f, 10);
        }

        private void OnSecondTouchPressed(InputAction.CallbackContext context)
        {
            isTouchZoomActive = true;
            touchDistanceStart = GetDistanceBetweenTouchPointers();
            cameraZoomStart = playerCamera.orthographicSize;
        }

        private void OnSecondTouchReleased(InputAction.CallbackContext context)
        {
            isTouchZoomActive = false;
        }

        private void OnSecondTouchMoved(InputAction.CallbackContext context)
        {
            if (isTouchZoomActive)
            {
                UpdateTouchZoom();
            }
        }

        private void UpdateTouchZoom()
        {
            float touchDelta = (touchDistanceStart - GetDistanceBetweenTouchPointers()) * touchZoomScale;
            playerCamera.orthographicSize = Mathf.Clamp(cameraZoomStart + touchDelta, 0.5f, 10);
        }

        private float GetDistanceBetweenTouchPointers()
        {
            return Vector2.Distance(pointerInfoService.GetPointerPosition(0), pointerInfoService.GetPointerPosition(1));
        }
        
    }
}

