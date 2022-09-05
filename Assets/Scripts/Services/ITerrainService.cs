using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WoodsOfIdle
{
    public interface ITerrainService
    {
        public CellData[,] GenerateTerrainData(TerrainGenerationSettings settings);

        public Texture2D GetTextureFromTerrainData(CellData[,] cells);

        public List<Vector2> GetSpawnPositionsForFarmingNode(TerrainGenerationSettings settings, FarmingNodeData nodeData, CellData[,] cells);
    }
}

