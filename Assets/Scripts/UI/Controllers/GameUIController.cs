using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

namespace WoodsOfIdle
{
    public class GameUIController
    {
        private UIDocument _uiDocument;
        private GameRelay _gameRelay;
        private IHarvestOptionElementFactory _harvestOptionElementFactory;

        public GameUIController(UIDocument gameUIPanel, GameRelay gameRelay, IHarvestOptionElementFactory harvestElementFactory)
        {
            _uiDocument = gameUIPanel;
            _gameRelay = gameRelay;
            _harvestOptionElementFactory = harvestElementFactory;

            _gameRelay.OnFarmingNodeClicked += OnNodeClicked;
        }
        
        public void OnNodeClicked(FarmingNodeController farmingNode)
        {
            VisualElement harvestOption = _harvestOptionElementFactory.CreateElement(farmingNode);
            SetTopPopoutMenuContent(harvestOption);
        }

        public void OnNothingClicked(Vector2 screenPosition)
        {
            ClearTopPopoutMenuContent();
        }

        public void SetTopPopoutMenuContent(VisualElement element)
        {
            VisualElement popoutContainer = _uiDocument.rootVisualElement.Q<VisualElement>("TopMenuPopoutContainer");
            popoutContainer.Clear();
            popoutContainer.Add(element);
        }

        public void ClearTopPopoutMenuContent()
        {
            VisualElement popoutContainer = _uiDocument.rootVisualElement.Q<VisualElement>("TopMenuPopoutContainer");
            popoutContainer.Clear();
        }

    }
}
