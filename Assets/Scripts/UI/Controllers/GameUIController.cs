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

        public GameUIController(UIDocument gameUIPanel, GameRelay gameRelay)
        {
            _uiDocument = gameUIPanel;
            _gameRelay = gameRelay;

            _gameRelay.OnFarmingNodeClicked += OnNodeClicked;
        }
        
        public void OnNodeClicked(FarmingNodeController farmingNode)
        {
            //TODO replace with factory
            HarvestOptionElement harvestOption = CreateHarvestOptionElement(farmingNode, null);
            SetTopPopoutMenuContent(harvestOption);
        }

        public void OnNothingClicked(Vector2 screenPosition)
        {
            ClearTopPopoutMenuContent();
        }

        private HarvestOptionElement CreateHarvestOptionElement(FarmingNodeController farmingNode, Dictionary<ItemType, ItemData> itemData)
        {
            HarvestOptionElement harvestOption = new HarvestOptionElement();

            IEnumerable<Texture> itemIcons = farmingNode.Data.HarvestableItems
                .OrderByDescending(item => item.ChanceToFarm)
                .Select(item => itemData[item.ItemType].ItemIcon.texture);

            harvestOption.SetItemIcons(itemIcons);
            harvestOption.SetHarvestText(farmingNode.Data.HarvestText);
            harvestOption.HarvestButton.clicked += farmingNode.ToggleActive;
            farmingNode.HarvestProgressChanged += harvestOption.ProgressBar.SetProgress;

            return harvestOption;
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
