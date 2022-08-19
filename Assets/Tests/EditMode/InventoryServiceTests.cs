using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using WoodsOfIdle;

public class InventoryServiceTests
{
    private IInventoryService inventoryService = new InventoryService();

    [Test]
    public void RemovingFromEmptyInventoryHasNoChanges()
    {
        int itemsChange = -100;
        List<InventorySlotState> inventoryState = GetEmptyInventory();

        List<InventoryChangeRequest> changes = inventoryService.GetInventoryChanges(inventoryState, ItemType.Wood, itemsChange);

        Assert.That(changes.Count() == 0);
    }

    [Test]
    public void RemovingNothingHasNoChanges()
    {
        int itemsChange = 0;
        List<InventorySlotState> inventoryState = GetFullDiverseInventory();

        List<InventoryChangeRequest> changes = inventoryService.GetInventoryChanges(inventoryState, ItemType.Dirt, itemsChange);

        Assert.That(changes.Count() == 0);
    }

    [Test]
    public void ChangesRemoveFromCorrectSlot()
    {
        int itemsChange = -50;
        List<InventorySlotState> inventoryState = GetFullDiverseInventory();

        List<InventoryChangeRequest> changes = inventoryService.GetInventoryChanges(inventoryState, ItemType.Dirt, itemsChange);

        Assert.That(changes.Count() == 1);
        Assert.That(changes.First().ItemType == ItemType.Dirt);
        Assert.That(changes.First().NewQuantity == 50);
    }

    [Test]
    public void ChangesRemoveFromOnlyOneSlot()
    {
        int itemsChange = -25;
        List<InventorySlotState> inventoryState = GetFullSimilarInventory();

        List<InventoryChangeRequest> changes = inventoryService.GetInventoryChanges(inventoryState, ItemType.Dirt, itemsChange);

        InventorySlotState targetedSlot = inventoryState.First(o => o.SlotId == changes.First().SlotId);

        Assert.That(changes.Count() == 1);
        Assert.That(changes.First().ItemType == ItemType.Dirt);
        Assert.That(changes.First().NewQuantity == targetedSlot.Quantity + itemsChange);
    }


    [Test]
    public void ChangesRemoveFromMultipleSlots()
    {
        int itemsChange = -250;
        List<InventorySlotState> inventoryState = GetFullSimilarInventory();

        List<InventoryChangeRequest> changes = inventoryService.GetInventoryChanges(inventoryState, ItemType.Dirt, itemsChange);

        Assert.That(changes.Count() > 1);
    }

    [Test]
    public void ChangesAddToOnlyOneSlot()
    {
        int itemsChange = 100;
        List<InventorySlotState> inventoryState = GetEmptyInventory();

        List<InventoryChangeRequest> changes = inventoryService.GetInventoryChanges(inventoryState, ItemType.Dirt, itemsChange);

        InventorySlotState targetedSlot = inventoryState.First(o => o.SlotId == changes.First().SlotId);

        Assert.That(changes.Count() == 1);
        Assert.That(changes.First().ItemType == ItemType.Dirt);
        Assert.That(changes.First().NewQuantity == itemsChange);
    }


    [Test]
    public void ChangesAddToSlotThatAlreadyHasItems()
    {
        int itemsChange = 100;
        List<InventorySlotState> inventoryState = GetDiverseQuantitySameTypeInventory();

        List<InventoryChangeRequest> changes = inventoryService.GetInventoryChanges(inventoryState, ItemType.Wood, itemsChange);

        InventorySlotState targetedSlot = inventoryState.First(o => o.SlotId == changes.First().SlotId);

        Assert.That(changes.Count() == 1);
        Assert.That(targetedSlot.Quantity > 0);
        Assert.That(changes.First().ItemType == ItemType.Wood);
        Assert.That(changes.First().NewQuantity == targetedSlot.Quantity + itemsChange);
    }

    [Test]
    public void ApplyingChangesEffectsCorrectSlot()
    {
        int itemsChange = 100;
        List<InventorySlotState> inventoryState = GetEmptyInventory();

        List<InventoryChangeRequest> changes = inventoryService.GetInventoryChanges(inventoryState, ItemType.Dirt, itemsChange);
        inventoryService.ApplyInventoryChange(changes.First(), inventoryState);

        InventorySlotState targetedSlot = inventoryState.First(o => o.SlotId == changes.First().SlotId);
        Assert.That(targetedSlot.Quantity == itemsChange);
        Assert.That(targetedSlot.ItemType == ItemType.Dirt);
    }


    private List<InventorySlotState> GetEmptyInventory()
    {
        return new List<InventorySlotState>
        {
            new InventorySlotState
            {
                SlotId = "First",
                Quantity = 0,
                CanAutoInsert = true,
                ItemType = ItemType.Wood
            },
            new InventorySlotState
            {
                SlotId = "Second",
                Quantity = 0,
                CanAutoInsert = true,
                ItemType = ItemType.Wood
            },
            new InventorySlotState
            {
                SlotId = "Third",
                Quantity = 0,
                CanAutoInsert = true,
                ItemType = ItemType.Stone
            }
        };
    }

    private List<InventorySlotState> GetDiverseQuantitySameTypeInventory()
    {
        return new List<InventorySlotState>
        {
            new InventorySlotState
            {
                SlotId = "First",
                Quantity = 0,
                CanAutoInsert = true,
                ItemType = ItemType.Wood
            },
            new InventorySlotState
            {
                SlotId = "Second",
                Quantity = 50,
                CanAutoInsert = true,
                ItemType = ItemType.Wood
            },
            new InventorySlotState
            {
                SlotId = "Third",
                Quantity = 0,
                CanAutoInsert = true,
                ItemType = ItemType.Wood
            }
        };
    }

    private List<InventorySlotState> GetFullDiverseInventory()
    {
        return new List<InventorySlotState>
        {
            new InventorySlotState
            {
                SlotId = "First",
                Quantity = 100,
                CanAutoInsert = true,
                ItemType = ItemType.Wood
            },
            new InventorySlotState
            {
                SlotId = "Second",
                Quantity = 100,
                CanAutoInsert = true,
                ItemType = ItemType.Dirt
            },
            new InventorySlotState
            {
                SlotId = "Third",
                Quantity = 100,
                CanAutoInsert = true,
                ItemType = ItemType.Stone
            }
        };
    }

    private List<InventorySlotState> GetFullSimilarInventory()
    {
        return new List<InventorySlotState>
        {
            new InventorySlotState
            {
                SlotId = "First",
                Quantity = 50,
                CanAutoInsert = true,
                ItemType = ItemType.Dirt
            },
            new InventorySlotState
            {
                SlotId = "Second",
                Quantity = 100,
                CanAutoInsert = true,
                ItemType = ItemType.Dirt
            },
            new InventorySlotState
            {
                SlotId = "Third",
                Quantity = 200,
                CanAutoInsert = true,
                ItemType = ItemType.Dirt
            }
        };
    }
}
