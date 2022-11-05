using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace WoodsOfIdle
{
    public class HarvestOptionElementFactory : IHarvestOptionElementFactory
    {
        private AssetReferenceCollection _assetReferences;
        private VisualTreeAsset _harvestOptionElement;

        private const string _harvestItemIconClass = "HarvestOptionItemIcon";
        
        public HarvestOptionElementFactory(AssetReferenceCollection assetReferences)
        {
            _assetReferences = assetReferences;
            _harvestOptionElement = assetReferences.FarmingNodeMenuAsset;
        }

        public VisualElement CreateElement(FarmingNodeController farmingNode)
        {
            var harvestOptionElement = _harvestOptionElement.CloneTree();
            var harvestItemContainer = GetHarvestItemContainer(harvestOptionElement);
            var harvestProgressBar = GetHarvestProgressBar(harvestOptionElement);
            var harvestButton = GetHarvestButton(harvestOptionElement);

            PopulateHarvestItemContainer(harvestItemContainer, farmingNode);
            BindHarvestProgressBar(harvestProgressBar, farmingNode);
            BindHarvestButton(harvestButton, farmingNode);

            return harvestOptionElement;
        }

        private VisualElement GetHarvestItemContainer(VisualElement baseElement)
        {
            return baseElement.Q<VisualElement>("HarvestItemContainer");
        }

        private SimpleProgressBar GetHarvestProgressBar(VisualElement baseElement)
        {
            return baseElement.Q<SimpleProgressBar>("HarvestProgressBar");
        }

        private Button GetHarvestButton(VisualElement baseElement)
        {
            return baseElement.Q<Button>("HarvestButton");
        }
        
        private void PopulateHarvestItemContainer(VisualElement harvestItemContainer, FarmingNodeController farmingNode)
        {
            harvestItemContainer.Clear();
            var itemOptions = farmingNode.Data.HarvestableItems;

            foreach (var itemOption in itemOptions)
            {
                var itemIcon = CreateItemIcon(itemOption.ItemType);
                harvestItemContainer.Add(itemIcon);
            }
        }

        private void BindHarvestProgressBar(SimpleProgressBar progressBar, FarmingNodeController farmingNode)
        {
            progressBar.SetProgress(0);
            farmingNode.HarvestProgressChanged += progressBar.SetProgress;
        }

        private void BindHarvestButton(Button harvestButton, FarmingNodeController farmingNode)
        {
            harvestButton.clicked += farmingNode.ToggleActive;
            harvestButton.text = farmingNode.Data.HarvestText;
        }

        private VisualElement CreateItemIcon(ItemType itemType)
        {
            Sprite itemSprite = _assetReferences.LoadedItemData[itemType].ItemIcon;

            VisualElement itemIconElement = new VisualElement();
            itemIconElement.name = "FarmingLocationIcon";
            itemIconElement.AddToClassList(_harvestItemIconClass);
            itemIconElement.style.backgroundImage = new StyleBackground(itemSprite);

            return itemIconElement;
        }
        
    }
}
