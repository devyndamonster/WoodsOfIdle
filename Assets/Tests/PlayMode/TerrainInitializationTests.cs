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
    public ObservableTerrainReceiver TerrainReceiver;
    public SceneManagerComponent SceneManager;

    /* TODO
     * - Need to start loading assets from scriptable objects because addressable loader is throwing errors (plus its probably good to test asset loading)
     */

    [SetUp]
    public void SetUp()
    {
        TerrainReceiver = ObservableTerrainReceiver.Create();

        GameObject sceneManagerObject = new GameObject();
        SceneManager = sceneManagerObject.AddComponent<SceneManagerComponent>();
        SceneManager.InventoryPanel = GetTestUIDocument();
        SceneManager.TerrainSettings = GetTestTerrainGenerationSettings();
        SceneManager.AssetCollection = GetTestAssetCollection();
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
        Debug.Log("Test 2");

        yield return new WaitForSecondsRealtime(10);

        Debug.Log("Test 2 End");

        Assert.That(false);
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
            Size = new Vector2Int(50, 50),
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
        AssetReferenceCollectionScriptable assetCollection = ScriptableObject.CreateInstance<AssetReferenceCollectionScriptable>();
        assetCollection.Value = new AssetReferenceCollection
        {
            LoadedItemData = GetTestItemData(),
            LoadedFarmingNodeData = GetTestFarmingNodeData(),
            LoadedFarmingNodePrefabs = GetTestFarmingNodePrefabs(GetTestFarmingNodeData())
        };
        return assetCollection;
    }

    private Dictionary<ItemType, ItemData> GetTestItemData()
    {
        return new Dictionary<ItemType, ItemData>
        {
            { ItemType.Dirt, new ItemData { ItemType = ItemType.Dirt, ItemIcon = null } },
            { ItemType.Wood, new ItemData { ItemType = ItemType.Wood, ItemIcon = null } },
            { ItemType.Stone, new ItemData { ItemType = ItemType.Stone, ItemIcon  = null } },
        };
    }

    private Dictionary<FarmingNodeType, FarmingNodeData> GetTestFarmingNodeData()
    {
        return new Dictionary<FarmingNodeType, FarmingNodeData>
        {
            {
                FarmingNodeType.Dirt,
                new FarmingNodeData
                {
                    NodeType = FarmingNodeType.Dirt,
                    SpawnChance = 0.01f,
                    AllowedCellTypes = new List<CellType>{ CellType.Forest },
                    DefaultTimeToHarvest = 1f,
                    HarvestableItems = new List<FarmingNodeItemOption> { new FarmingNodeItemOption { ItemType = ItemType.Dirt, ChanceToFarm = 1 } },
                }
            },
            {
                FarmingNodeType.Forest,
                new FarmingNodeData
                {
                    NodeType = FarmingNodeType.Forest,
                    SpawnChance = 0.01f,
                    AllowedCellTypes = new List<CellType>{ CellType.Forest },
                    DefaultTimeToHarvest = 1f,
                    HarvestableItems = new List<FarmingNodeItemOption> { new FarmingNodeItemOption { ItemType = ItemType.Wood, ChanceToFarm = 1 } },
                }
            },
            {
                FarmingNodeType.Boulder,
                new FarmingNodeData
                {
                    NodeType = FarmingNodeType.Boulder,
                    SpawnChance = 0.01f,
                    AllowedCellTypes = new List<CellType>{ CellType.Forest },
                    DefaultTimeToHarvest = 1f,
                    HarvestableItems = new List<FarmingNodeItemOption> { new FarmingNodeItemOption { ItemType = ItemType.Stone, ChanceToFarm = 1 } },
                }
            },
        };
    }

    private Dictionary<FarmingNodeType, GameObject> GetTestFarmingNodePrefabs(Dictionary<FarmingNodeType, FarmingNodeData> farmingNodeData)
    {
        var farmingNodePrefabs = new Dictionary<FarmingNodeType, GameObject>();

        foreach (FarmingNodeType type in farmingNodeData.Keys)
        {
            GameObject nodeObject = new GameObject();
            FarmingNodeComponent nodeComponent = nodeObject.AddComponent<FarmingNodeComponent>();
            nodeComponent.NodeType = type;
            farmingNodePrefabs[type] = nodeObject;
        }

        return farmingNodePrefabs;
    }
}
