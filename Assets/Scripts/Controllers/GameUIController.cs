using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

namespace WoodsOfIdle
{
    public class GameUIController
    {
        protected UIDocument gameUIPanel;
        protected Dictionary<ItemType, ItemData> itemData;

        public GameUIController(UIDocument gameUIPanel, IEnumerable<ItemData> itemData)
        {
            this.gameUIPanel = gameUIPanel;
            this.itemData = itemData.ToDictionary(item => item.ItemType);
            
            SetupEvents();
        }
        
        private void SetupEvents()
        {
            FarmingNodeComponent.NodeClicked += OnNodeClicked;
        }

        private void OnNodeClicked(FarmingNodeComponent farmingNode)
        {
            HarvestOptionElement harvestOption = CreateHarvestOptionElement(farmingNode);
            SetTopPopoutMenuContent(harvestOption);
        }

        private HarvestOptionElement CreateHarvestOptionElement(FarmingNodeComponent farmingNode)
        {
            HarvestOptionElement harvestOption = new HarvestOptionElement();

            IEnumerable<Texture> itemIcons = farmingNode.Data.HarvestableItems
                .OrderByDescending(item => item.ChanceToFarm)
                .Select(item => itemData[item.ItemType].ItemIcon.texture);

            harvestOption.SetItemIcons(itemIcons);
            harvestOption.HarvestButton.clicked += farmingNode.ToggleActive;
            farmingNode.HarvestProgressChanged += harvestOption.ProgressBar.SetProgress;

            return harvestOption;
        }

        public void SetTopPopoutMenuContent(VisualElement element)
        {
            VisualElement popoutContainer = gameUIPanel.rootVisualElement.Q<VisualElement>("TopMenuPopoutContainer");
            popoutContainer.Clear();
            popoutContainer.Add(element);
        }
    }
}
