using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WoodsOfIdle;

public class ObservableTerrainReceiver : MonoBehaviour, ITerrainReceiver
{
    public CellData[,] CellData;
    public Dictionary<Vector2Int, GameObject> FarmingNodePrefabs;
    public TerrainGenerationSettings TerrainSettings;
    
    public void ApplyTerrain(TerrainGenerationData terrainData, TerrainGenerationSettings settings)
    {
        CellData = terrainData.CellData;
        FarmingNodePrefabs = terrainData.FarmingNodePrefabs;
        TerrainSettings = settings;
    }

    public static ObservableTerrainReceiver Create()
    {
        GameObject gameObject = new GameObject();
        return gameObject.AddComponent<ObservableTerrainReceiver>();
    }
}
