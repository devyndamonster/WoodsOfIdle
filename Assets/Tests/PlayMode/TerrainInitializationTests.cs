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
    public ObservableTerrainReceiver TerrainReceiver;
    public SceneManagerComponent SceneManagerComponent;

    private IEnumerator SetupNewScene(string saveName)
    {
        yield return ClearScene();
        SetTestSave(saveName);
        SceneManagerComponent.SetNextSaveToOpen(saveName);

        TerrainReceiver = ObservableTerrainReceiver.Create();

        GameObject sceneManagerObject = new GameObject();
        SceneManagerComponent = sceneManagerObject.AddComponent<SceneManagerComponent>();
        SceneManagerComponent.InventoryPanel = GetTestUIDocument();
        SceneManagerComponent.TerrainSettings = GetTestTerrainGenerationSettings();
        SceneManagerComponent.AssetCollection = GetTestAssetCollection();
    }

    private IEnumerator ClearScene()
    {
        foreach (GameObject o in Object.FindObjectsOfType<GameObject>())
        {
            GameObject.Destroy(o);
        }

        yield return new WaitForEndOfFrame();
    }

    [UnityTest]
    public IEnumerator AssetsWillBeLoaded()
    {
        yield return SetupNewScene("AssetsWillBeLoaded");

        yield return new WaitForSecondsRealtime(0.5f);

        Assert.That(SceneManagerComponent.AssetCollection.Value.LoadedFarmingNodeData.Count(), Is.EqualTo(3));
        Assert.That(SceneManagerComponent.AssetCollection.Value.LoadedItemData.Count(), Is.EqualTo(3));
        Assert.That(SceneManagerComponent.AssetCollection.Value.LoadedFarmingNodePrefabs.Count(), Is.EqualTo(3));
    }

    [UnityTest]
    public IEnumerator TerrainWillBeLoadedFromSave()
    {
        Debug.Log("Test 1");

        yield return new WaitForSecondsRealtime(10);

        Debug.Log("Test 1 End");

        Assert.That(false);
    }

    [UnityTest]
    public IEnumerator TerrainWillBeGenerated()
    {
        yield return SetupNewScene("TerrainWillBeGenerated");

        yield return new WaitForSecondsRealtime(0.5f);

        Assert.That(TerrainReceiver.CellData.GetLength(0), Is.EqualTo(50));
        Assert.That(TerrainReceiver.CellData.GetLength(1), Is.EqualTo(100));
    }

    private UIDocument GetTestUIDocument()
    {
        GameObject uiDocumentObject = new GameObject();
        UIDocument uiDocumentComponent = uiDocumentObject.AddComponent<UIDocument>();
        return uiDocumentComponent;
    }

    private TerrainGenerationSettings GetTestTerrainGenerationSettings(int seed = 0)
    {
        return new TerrainGenerationSettings
        {
            Size = new Vector2Int(50, 100),
            CellSize = seed,
            HeightMapSettings = new List<PerlinNoiseSettings> 
            {
                new PerlinNoiseSettings 
                {
                    Scale = 12, 
                    Strength = 1 
                } 
            },
            TileMapSettings = new List<TileMapSettings> 
            { 
                new TileMapSettings
                {
                    CellType = CellType.Water,
                    HeightRange = new Vector2(0, 0.5f)
                },
                new TileMapSettings
                {
                    CellType = CellType.Grass,
                    HeightRange = new Vector2(0.5f, 1)
                },
            },
            TileColorSettings = new List<TileColorSettings>
            {
                new TileColorSettings
                {
                    CellType = CellType.Water,
                    Color = Color.blue
                },
                new TileColorSettings
                {
                    CellType = CellType.Grass,
                    Color = Color.green
                }
            }
        };
    }

    private AssetReferenceCollectionScriptable GetTestAssetCollection()
    {
        return AssetDatabase.LoadAssetAtPath<AssetReferenceCollectionScriptable>("Assets/Tests/PlayMode/TestAssets/TestAssetReferences.asset");
    }

    private void SetTestSave(string saveName)
    {
        ISaveService saveService = new SaveService();
        saveService.DeleteSave(saveName);
        SceneManagerComponent.SetNextSaveToOpen(saveName);
    }
}
