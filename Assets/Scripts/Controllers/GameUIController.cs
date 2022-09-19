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
        protected Dictionary<Vector2Int, FarmingNodeController> farmingNodes;

        public GameUIController(UIDocument gameUIPanel, Dictionary<ItemType, ItemData> itemData, IEnumerable<FarmingNodeController> farmingNodes)
        {
            this.gameUIPanel = gameUIPanel;
            this.itemData = itemData;
            this.farmingNodes = farmingNodes.ToDictionary(node => node.State.Position);

            SetupEvents();
        }
        
        private void SetupEvents()
        {
            FarmingNodeComponent.NodeClicked += OnNodeClicked;
            WorldClickListenerComponent.WorldClicked += OnNothingClicked;
        }
        
        private void OnNodeClicked(Vector2Int nodePosition)
        {
            HarvestOptionElement harvestOption = CreateHarvestOptionElement(farmingNodes[nodePosition]);
            SetTopPopoutMenuContent(harvestOption);
        }

        private void OnNothingClicked(Vector2 screenPosition)
        {
            ClearTopPopoutMenuContent();
        }

        private HarvestOptionElement CreateHarvestOptionElement(FarmingNodeController farmingNode)
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
        
        public void ClearTopPopoutMenuContent()
        {
            VisualElement popoutContainer = gameUIPanel.rootVisualElement.Q<VisualElement>("TopMenuPopoutContainer");
            popoutContainer.Clear();
        }
    }
}
