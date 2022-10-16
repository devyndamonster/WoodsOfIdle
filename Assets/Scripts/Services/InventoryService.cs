using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace WoodsOfIdle
{
    public class InventoryService : IInventoryService
    {
        public List<InventoryChangeRequest> GetInventoryChanges(IEnumerable<InventorySlotState> slotStates, ItemType itemType, int quantityChange)
        {
            if(quantityChange > 0)
            {
                return GetInventoryChangesFromAddition(slotStates, itemType, quantityChange);
            }

            else if(quantityChange < 0)
            {
                return GetInventoryChangesFromRemoval(slotStates, itemType, -quantityChange);
            }

            return new List<InventoryChangeRequest>();
        }


        private List<InventoryChangeRequest> GetInventoryChangesFromAddition(IEnumerable<InventorySlotState> slotStates, ItemType itemType, int quantityToAdd)
        {
            List<InventoryChangeRequest> changes = new List<InventoryChangeRequest>();

            List<InventorySlotState> slotsWithItem = slotStates
                .Where(slot => slot.ItemType == itemType && slot.CanAutoInsert)
                .OrderByDescending(slot => slot.Quantity)
                .ToList();

            List<InventorySlotState> emptySlots = slotStates
                .Where(slot => slot.Quantity == 0 && slot.CanAutoInsert)
                .ToList();

            if (slotsWithItem.Count > 0)
            {
                changes.Add(new InventoryChangeRequest
                {
                    SlotId = slotsWithItem.First().SlotId,
                    ItemType = itemType,
                    NewQuantity = slotsWithItem.First().Quantity + quantityToAdd
                });
            }
            else if (emptySlots.Count > 0)
            {
                changes.Add(new InventoryChangeRequest
                {
                    SlotId = emptySlots.First().SlotId,
                    ItemType = itemType,
                    NewQuantity = quantityToAdd
                });
            }

            return changes;
        }


        private List<InventoryChangeRequest> GetInventoryChangesFromRemoval(IEnumerable<InventorySlotState> slotStates, ItemType itemType, int quantityToRemove)
        {
            List<InventoryChangeRequest> changes = new List<InventoryChangeRequest>();
            List<InventorySlotState> slotsWithItem = slotStates.Where(slot => slot.ItemType == itemType).ToList();
            foreach (InventorySlotState slot in slotsWithItem)
            {
                if (quantityToRemove > 0 && slot.Quantity > 0 && slot.Quantity <= quantityToRemove)
                {
                    changes.Add(new InventoryChangeRequest
                    {
                        SlotId = slot.SlotId,
                        ItemType = itemType,
                        NewQuantity = 0
                    });

                    quantityToRemove -= slot.Quantity;
                }
                else if(quantityToRemove > 0 && slot.Quantity > 0)
                {
                    changes.Add(new InventoryChangeRequest
                    {
                        SlotId = slot.SlotId,
                        ItemType = itemType,
                        NewQuantity = slot.Quantity - quantityToRemove
                    });

                    quantityToRemove = 0;
                }
            }

            return changes;
        }

        public void ApplyInventoryChange(InventoryChangeRequest changeRequest, IEnumerable<InventorySlotState> slotStates)
        {
            InventorySlotState targetSlotState = slotStates.First(slot => slot.SlotId == changeRequest.SlotId);
            targetSlotState.Quantity = changeRequest.NewQuantity;
            targetSlotState.ItemType = changeRequest.ItemType;
        }

        public void SwapInventoryContents(InventorySlotState slotFrom, InventorySlotState slotTo)
        {
            int tempQuantity = slotFrom.Quantity;
            ItemType tempType = slotFrom.ItemType;

            slotFrom.Quantity = slotTo.Quantity;
            slotFrom.ItemType = slotTo.ItemType;

            slotTo.Quantity = tempQuantity;
            slotTo.ItemType = tempType;
        }

        public IEnumerable<InventorySlotState> GetInventorySlotStatesFromQuantityChange(IDictionary<string, InventorySlotState> slotStates, ItemType itemType, int quantityChange)
        {
            var changes = GetInventoryChanges(slotStates.Values, itemType, quantityChange);
            var updatedStates = new List<InventorySlotState>();

            foreach (InventoryChangeRequest change in changes)
            {
                ApplyInventoryChange(change, slotStates.Values);
                updatedStates.Add(slotStates[change.SlotId]);
            }

            return updatedStates;
        }
    }

}

