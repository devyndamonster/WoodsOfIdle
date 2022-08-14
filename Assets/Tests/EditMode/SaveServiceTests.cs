using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using WoodsOfIdle;

public class SaveServiceTests
{
    private ISaveService saveService = new SaveService();

    [Test]
    public void SaveDeletesProperly()
    {
        string saveName = "SaveDeletesProperly";
        SaveState save = saveService.LoadOrCreate(saveName);
        saveService.SaveGame(save);

        Assert.That(saveService.DoesSaveExist(saveName), Is.True);
        saveService.DeleteSave(saveName);
        Assert.That(saveService.DoesSaveExist(saveName), Is.False);
    }


    [Test]
    public void SaveExists()
    {
        string saveName = "SaveThatShouldAlwaysExist";
        SaveState save = saveService.LoadOrCreate(saveName);

        saveService.SaveGame(save);

        Assert.That(saveService.DoesSaveExist(saveName), Is.True);
    }

    [Test]
    public void SaveDoesNotExist()
    {
        string saveName = "SaveThatNeverShouldExist";

        Assert.That(saveService.DoesSaveExist(saveName), Is.False);
    }

    [Test]
    public void DataIsSavedProperly()
    {
        SaveState originalSave = GetSampleSaveState();

        saveService.DeleteSave(originalSave.SaveName);
        saveService.SaveGame(originalSave);
        SaveState loadedSave = saveService.LoadOrCreate(originalSave.SaveName);

        Assert.AreEqual(originalSave.SaveName, loadedSave.SaveName);
        Assert.AreEqual(originalSave.FarmingNodes[4].IsActive, loadedSave.FarmingNodes[4].IsActive);
        Assert.AreEqual(originalSave.FarmingNodes[4].NodeId, loadedSave.FarmingNodes[4].NodeId);
        Assert.AreEqual(originalSave.FarmingNodes[4].NodeType, loadedSave.FarmingNodes[4].NodeType);
        Assert.AreEqual(originalSave.InventoryInSlots["InventorySlot1"].Quantity, loadedSave.InventoryInSlots["InventorySlot1"].Quantity);
        Assert.AreEqual(originalSave.InventoryInSlots["InventorySlot1"].ItemType, loadedSave.InventoryInSlots["InventorySlot1"].ItemType);
    }

    [Test]
    public void SavesAreInSaveList()
    {
        List<string> saveNamesToTest = new List<string>()
        {
            "ACoolSave", "AnotherCoolSave", "WowWhatACoolSave", "IsThisACoolSave"
        };

        List<SaveState> saveStatesToTest = saveNamesToTest
            .Select(saveName => new SaveState { SaveName = saveName })
            .ToList();

        saveStatesToTest.ForEach(saveState => saveService.DeleteSave(saveState));
        IEnumerable<string> returnedSaveNames = saveService.GetSaveNames();
        Assert.That(!saveNamesToTest.Any(saveName => returnedSaveNames.Contains(saveName)));

        saveStatesToTest.ForEach(saveState => saveService.SaveGame(saveState));
        returnedSaveNames = saveService.GetSaveNames();
        Assert.That(saveNamesToTest.All(saveName => returnedSaveNames.Contains(saveName)));
    }

    private SaveState GetSampleSaveState()
    {
        FarmingNodeState nodeState = new FarmingNodeState
        {
            IsActive = true,
            NodeId = 4,
            NodeType = ItemType.Dirt
        };

        InventorySlotState slotState = new InventorySlotState
        {
            ItemType = ItemType.Stone,
            Quantity = 5
        };

        SaveState save = new SaveState
        {
            SaveName = "DataIsSavedProperly",
            InventoryInSlots = new Dictionary<string, InventorySlotState>
            {
                { "InventorySlot1", slotState }
            },
            FarmingNodes = new Dictionary<int, FarmingNodeState>
            {
                { 4, nodeState }
            }
        };

        return save;
    }

}
