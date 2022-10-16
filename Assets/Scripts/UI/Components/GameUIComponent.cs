using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

namespace WoodsOfIdle
{
    public class GameUIComponent : MonoBehaviour
    {
        public UIDocument UIDocument;
        public VisualTreeAsset GameViewAsset;
        public VisualTreeAsset FarmingNodeMenuAsset;
        public VisualTreeAsset InventoryElementAsset;

        private InventoryUIController _inventoryUIController;
        private VisualElement _popoutContainer;

        private void Awake()
        {
            UIDocument.visualTreeAsset = GameViewAsset;
        }

        private void Initialize(InventoryRelay inventoryRelay, InventoryElementFactory inventoryElementFactory)
        {
            //TODO: should this object have a dependancy injection container? How do we link this to the base game container?
            // Probably should come up with dependancy injection map on design doc
            
            _inventoryUIController = new InventoryUIController(inventoryElementFactory, UIDocument, inventoryRelay);
        }

        public void OnNodeClicked(FarmingNodeController farmingNode, Dictionary<ItemType, ItemData> itemData)
        {
            HarvestOptionElement harvestOption = CreateHarvestOptionElement(farmingNode, itemData);
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
            VisualElement popoutContainer = UIDocument.rootVisualElement.Q<VisualElement>("TopMenuPopoutContainer");
            popoutContainer.Clear();
            popoutContainer.Add(element);
        }

        public void ClearTopPopoutMenuContent()
        {
            VisualElement popoutContainer = UIDocument.rootVisualElement.Q<VisualElement>("TopMenuPopoutContainer");
            popoutContainer.Clear();
        }
    }
}
