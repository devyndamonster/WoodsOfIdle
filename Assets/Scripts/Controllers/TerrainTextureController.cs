using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainTextureController : MonoBehaviour
{
    public MeshRenderer meshRenderer;
    public TerrainGenerationSettings terrainSettings = new TerrainGenerationSettings();

    private ITerrainService terrainService = new TerrainService();

    public void GenerateTextureToMesh()
    {
        TerrainGenerationSettings settings = new TerrainGenerationSettings
        {
            MinCoordX = 0,
            MinCoordY = 0,
            MaxCoordX = 100,
            MaxCoordY = 100,
            Seed = 1
        };

        TerrainBuilder terrainBuilder = new TerrainBuilder(settings);
        CellData[,] cells = terrainService.GenerateTerrainData(terrainBuilder, settings);
        Texture2D texture = terrainService.GetTextureFromTerrainData(cells);
        SetMeshTexture(texture);
    }
    
    private void SetMeshTexture(Texture2D texture)
    {
        meshRenderer.sharedMaterial.mainTexture = texture;
    }
}
