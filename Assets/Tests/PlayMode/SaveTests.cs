using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using WoodsOfIdle;

public class SaveTests
{
    private ISaveService saveService = new SaveService();

    [UnityTest]
    public IEnumerator FarmingNodeLinksSaveState()
    {
        string saveName = "FarmingNodeLinksSaveState";
        saveService.DeleteSave(saveName);

        GameObject farmingNodeObject = new GameObject();
        FarmingNodeController farmingNodeComponent = farmingNodeObject.AddComponent<FarmingNodeController>();
        farmingNodeComponent.State.NodeId = 6;
        farmingNodeComponent.State.NodeType = NodeType.Dirt;

        GameObject controllerObject = new GameObject();
        GameController controllerComp = controllerObject.AddComponent<GameController>();
        controllerComp.SetSave(saveName);

        yield return null;

        Assert.AreEqual(controllerComp.CurrentSaveState.FarmingNodes[6], farmingNodeComponent.State);

        controllerComp.CurrentSaveState.FarmingNodes[6].NodeType = NodeType.Stone;

        Assert.AreEqual(controllerComp.CurrentSaveState.FarmingNodes[6].NodeType, farmingNodeComponent.State.NodeType);
    }

    [UnityTest]
    public IEnumerator SaveIsCreatedOnDestroy()
    {
        string saveName = "SaveIsCreated";
        saveService.DeleteSave(saveName);

        GameObject controllerObject = new GameObject();
        GameController controllerComp = controllerObject.AddComponent<GameController>();
        controllerComp.SetSave(saveName);

        Assert.That(saveService.DoesSaveExist(saveName), Is.False);

        yield return null;
        GameObject.Destroy(controllerObject);
        yield return null;

        Assert.That(saveService.DoesSaveExist(saveName), Is.True);
    }
}
