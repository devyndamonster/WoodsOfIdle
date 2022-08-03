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

        GameObject controllerObject = new GameObject();
        GameController controllerComp = controllerObject.AddComponent<GameController>();
        controllerComp.SetSave(saveName);
        GameObject farmingNodeObject = new GameObject();

        FarmingNodeComponent farmingNodeComponent = farmingNodeObject.AddComponent<FarmingNodeComponent>();
        farmingNodeComponent.state.NodeId = 6;
        farmingNodeComponent.state.NodeType = NodeType.Dirt;

        yield return null;

        Assert.AreEqual(controllerComp.currentSaveState.FarmingNodes[6], farmingNodeComponent.state);

        controllerComp.currentSaveState.FarmingNodes[6].NodeType = NodeType.Stone;

        Assert.AreEqual(controllerComp.currentSaveState.FarmingNodes[6].NodeType, farmingNodeComponent.state.NodeType);
    }

    [UnityTest]
    public IEnumerator SaveIsCreatedOnDestroy()
    {
        string saveName = "SaveIsCreated";
        saveService.DeleteSave(saveName);

        GameObject controllerObject = new GameObject();
        GameController controllerComp = controllerObject.AddComponent<GameController>();
        controllerComp.SetSave(saveName);

        GameObject farmingNodeObject = new GameObject();
        FarmingNodeComponent farmingNodeComponent = farmingNodeObject.AddComponent<FarmingNodeComponent>();
        

        Assert.That(saveService.DoesSaveExist(saveName), Is.False);

        yield return null;
        GameObject.Destroy(controllerObject);
        yield return null;

        Assert.That(saveService.DoesSaveExist(saveName), Is.True);
    }
}
