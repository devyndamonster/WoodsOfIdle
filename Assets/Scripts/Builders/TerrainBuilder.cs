using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainBuilder
{
    private TerrainGenerationSettings settings;
    private CellData[,] cells;

    public TerrainBuilder(TerrainGenerationSettings settings)
    {
        this.settings = settings;
    }
}
