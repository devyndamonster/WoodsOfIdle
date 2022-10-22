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
        
        GameObject gameContainer = new GameObject();
        GameContainerComponent sceneComp = gameContainer.AddComponent<GameContainerComponent>();
        sceneComp.terrainSettings = GetTestTerrainGenerationSettings(seed);
        sceneComp.assetReferences = GetTestAssetCollection();
        sceneComp.uiDocument = GetTestUIDocument();
    }
    
    public static IEnumerator WaitUntilSceneLoaded()
    {
        GameContainerComponent gameContainer = GameObject.FindObjectOfType<GameContainerComponent>();
        
        yield return new WaitUntil(() =>
        {
            return gameContainer.IsInitialized;
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
        GameContainerComponent.SetNextSaveToOpen(saveName);
    }

    private static TerrainGenerationSettings GetTestTerrainGenerationSettings(int seed = 0)
    {
        return new TerrainGenerationSettings
        {
            Seed = seed,
            Size = new Vector2Int(50, 100),
            CellSize = 1,
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
