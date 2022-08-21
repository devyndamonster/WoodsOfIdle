using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ITerrainService
{
    public CellData[,] GenerateTerrainData(TerrainBuilder builder, TerrainGenerationSettings settings);

    public Texture2D GetTextureFromTerrainData(CellData[,] cells);
}
