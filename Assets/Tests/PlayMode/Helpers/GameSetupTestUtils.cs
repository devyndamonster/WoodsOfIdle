using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using WoodsOfIdle;

public static class GameSetupTestUtils
{
    public static IEnumerator SetNewSceneWithNewSave(string saveName, int seed = 0)
    {
        SetNewSave(saveName);
        yield return SetNewSceneWithCurrentSave(seed);
    }

    public static IEnumerator SetNewSceneWithCurrentSave(int seed = 0)
    {
        yield return ClearScene();

        ObservableTerrainReceiver.Create();

        GameObject sceneManagerObject = new GameObject();
        SceneManagerComponent sceneComp = sceneManagerObject.AddComponent<SceneManagerComponent>();
        sceneComp.InventoryPanel = GetTestUIDocument();
        sceneComp.TerrainSettings = GetTestTerrainGenerationSettings(seed);
        sceneComp.AssetCollection = GetTestAssetCollection();
    }
    
    public static IEnumerator WaitUntilSceneLoaded()
    {
        SceneManagerComponent sceneComp = GameObject.FindObjectOfType<SceneManagerComponent>();
        
        yield return new WaitUntil(() =>
        {
            return sceneComp.IsSceneInitialized();
        });
    }

    private static UIDocument GetTestUIDocument()
    {
        GameObject uiDocumentObject = new GameObject();
        UIDocument uiDocumentComponent = uiDocumentObject.AddComponent<UIDocument>();
        return uiDocumentComponent;
    }

    private static IEnumerator ClearScene()
    {
        foreach (GameObject o in Object.FindObjectsOfType<GameObject>())
        {
            GameObject.Destroy(o);
        }

        yield return new WaitForEndOfFrame();
    }

    private static AssetReferenceCollectionScriptable GetTestAssetCollection()
    {
        return AssetDatabase.LoadAssetAtPath<AssetReferenceCollectionScriptable>("Assets/Tests/PlayMode/TestAssets/TestAssetReferences.asset");
    }

    private static void SetNewSave(string saveName)
    {
        ISaveService saveService = new SaveService();
        saveService.DeleteSave(saveName);
        SceneManagerComponent.SetNextSaveToOpen(saveName);
    }

    private static TerrainGenerationSettings GetTestTerrainGenerationSettings(int seed = 0)
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
}
