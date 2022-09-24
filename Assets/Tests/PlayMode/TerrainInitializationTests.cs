using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;
using UnityEngine.UIElements;
using WoodsOfIdle;

[TestFixture]
public class TerrainInitializationTests
{
    [UnityTest]
    public IEnumerator AssetsWillBeLoaded()
    {
        yield return GameSetupTestUtils.SetNewSceneWithNewSave("AssetsWillBeLoaded");
        yield return GameSetupTestUtils.WaitUntilSceneLoaded();
        
        SceneManagerComponent sceneComp = GameObject.FindObjectOfType<SceneManagerComponent>();

        Assert.That(sceneComp.AssetCollection.Value.LoadedFarmingNodeData.Count(), Is.EqualTo(3));
        Assert.That(sceneComp.AssetCollection.Value.LoadedItemData.Count(), Is.EqualTo(3));
        Assert.That(sceneComp.AssetCollection.Value.LoadedFarmingNodePrefabs.Count(), Is.EqualTo(3));
    }

    [UnityTest]
    public IEnumerator TerrainWillBeLoadedFromSave()
    {
        yield return GameSetupTestUtils.SetNewSceneWithNewSave("TerrainWillBeLoadedFromSave", 0);
        yield return GameSetupTestUtils.WaitUntilSceneLoaded();

        ObservableTerrainReceiver terrainReceiverA = GameObject.FindObjectOfType<ObservableTerrainReceiver>();
        CellData[,] cellsA = terrainReceiverA.CellData;

        yield return GameSetupTestUtils.SetNewSceneWithCurrentSave(1);
        yield return GameSetupTestUtils.WaitUntilSceneLoaded();

        ObservableTerrainReceiver terrainReceiverB = GameObject.FindObjectOfType<ObservableTerrainReceiver>();
        CellData[,] cellsB = terrainReceiverB.CellData;

        for(int row = 0; row < cellsB.GetLength(0); row++)
        {
            for(int col = 0; col < cellsB.GetLength(1); col++)
            {
                Assert.That(cellsA[row, col].Height, Is.EqualTo(cellsB[row, col].Height));
            }
        }
    }

    [UnityTest]
    public IEnumerator TerrainWillBeGenerated()
    {
        yield return GameSetupTestUtils.SetNewSceneWithNewSave("TerrainWillBeGenerated");
        yield return GameSetupTestUtils.WaitUntilSceneLoaded();

        ObservableTerrainReceiver terrainReceiverA = GameObject.FindObjectOfType<ObservableTerrainReceiver>();

        Assert.That(terrainReceiverA.CellData.GetLength(0), Is.EqualTo(50));
        Assert.That(terrainReceiverA.CellData.GetLength(1), Is.EqualTo(100));
    }
}
