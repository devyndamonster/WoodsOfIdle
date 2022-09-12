using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace WoodsOfIdle
{
    public class WorldClickListenerComponent : MonoBehaviour
    {
        public Camera playerCamera;
        public float clickMaxReleaseDistance;

        private PlayerInputActions playerInput;
        private IPointerInfoService pointerInfoService;
        private Vector2 screenPositionStartClick;

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
        }

        private void OnPointerPressed(InputAction.CallbackContext context)
        {
            screenPositionStartClick = pointerInfoService.GetPointerPosition();
        }

        private void OnPointerReleased(InputAction.CallbackContext context)
        {
            Vector2 screenPositionEndClick = pointerInfoService.GetPointerPosition();
            float clickDistance = Vector2.Distance(screenPositionStartClick, screenPositionEndClick);
            if (clickDistance <= clickMaxReleaseDistance)
            {
                Ray ray = playerCamera.ScreenPointToRay(screenPositionEndClick);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit))
                {
                    FarmingNodeComponent clickableComponent = hit.collider.gameObject.GetComponent<FarmingNodeComponent>();
                    if (clickableComponent != null)
                    {
                        clickableComponent.Click();
                    }
                }
            }

            
        }
    }
}
