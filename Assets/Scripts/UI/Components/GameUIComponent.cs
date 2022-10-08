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
        
        private VisualElement _popoutContainer;

        private void Awake()
        {
            UIDocument.visualTreeAsset = GameViewAsset;
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

        /*
        private void ApplySlotStates(IDictionary<string, InventorySlotState> inventoryInSlots)
        {
            foreach (var slotPair in dragAndDropSlots)
            {
                if (!inventoryInSlots.ContainsKey(slotPair.Value.SlotId))
                {
                    inventoryInSlots[slotPair.Value.SlotId] = new InventorySlotState
                    {
                        SlotId = slotPair.Value.SlotId,
                        CanAutoInsert = slotPair.Value.CanAutoInsert
                    };
                }

                InventorySlotState slotState = inventoryInSlots[slotPair.Value.SlotId];
                slotPair.Value.SetSlotState(itemData[slotState.ItemType], slotState.Quantity);
            }
        }
        */

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
