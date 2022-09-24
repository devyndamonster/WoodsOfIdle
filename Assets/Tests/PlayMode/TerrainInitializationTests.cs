using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.UIElements;
using WoodsOfIdle;

[TestFixture]
public class TerrainInitializationTests
{
    [SetUp]
    public void SetUp()
    {
        GameObject uiDocumentObject = new GameObject();
        UIDocument uiDocumentComponent = uiDocumentObject.AddComponent<UIDocument>();

        TerrainGenerationSettings terrainSettings = new TerrainGenerationSettings
        {
            CellSize = 1,
            Size = new Vector2Int(10, 10),
        };

        AssetReferenceCollectionScriptable assetCollection = ScriptableObject.CreateInstance<AssetReferenceCollectionScriptable>();
        
        GameObject sceneManagerObject = new GameObject();
        SceneManagerComponent sceneManagerComponent = sceneManagerObject.AddComponent<SceneManagerComponent>();
        sceneManagerComponent.InventoryPanel = uiDocumentComponent;
        sceneManagerComponent.TerrainSettings = terrainSettings;
        sceneManagerComponent.AssetCollection = assetCollection;
    }

    [UnityTest]
    public IEnumerator TerrainWillBeLoadedFromSave()
    {
        yield return null;

        Debug.Log("Test 1");

        Assert.That(false);
    }

    [UnityTest]
    public IEnumerator TerrainWillBeGenerated()
    {
        yield return null;

        Debug.Log("Test 2");

        Assert.That(false);
    }

    private Dictionary<ItemType, ItemData> GetTestItemData()
    {
        return null;
    }
}
