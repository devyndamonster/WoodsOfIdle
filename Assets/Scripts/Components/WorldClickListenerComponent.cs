using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

namespace WoodsOfIdle
{
    public class WorldClickListenerComponent : MonoBehaviour
    {
        public static event Action<Vector2> WorldClicked;
        public static event Action<Vector2, IClickable> ClickableClicked;

        public Camera playerCamera;
        public float clickMaxReleaseDistance;
        public UIDocument uiContainer;
        
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
            TryClick();
        }

        private void TryClick()
        {
            Vector2 screenPositionEndClick = pointerInfoService.GetPointerPosition();
            Vector2 pointerUIPosition = new Vector2(screenPositionEndClick.x, Screen.height - screenPositionEndClick.y);
            float clickDistance = Vector2.Distance(screenPositionStartClick, screenPositionEndClick);

            if (clickDistance <= clickMaxReleaseDistance)
            {
                VisualElement clickedUI = pointerInfoService.GetVisualElementAtScreenPosition(uiContainer.rootVisualElement, pointerUIPosition);
                IClickable clickable = TryGetClickableComponent(screenPositionEndClick);
                
                if (clickedUI != null)
                {
                    Debug.Log($"Clicked UI: {clickedUI.name}");
                }
                else if (clickable != null)
                {
                    ClickableClicked?.Invoke(screenPositionEndClick, clickable);
                    clickable.Click();
                }
                else
                {
                    WorldClicked?.Invoke(screenPositionEndClick);
                }
            }
        }

        private IClickable TryGetClickableComponent(Vector2 clickPosition)
        {
            Ray ray = playerCamera.ScreenPointToRay(clickPosition);
            RaycastHit hit;
            
            if (Physics.Raycast(ray, out hit))
            {
                return hit.collider.gameObject.GetComponent<IClickable>();
            }

            return null;
        }
    }
}
