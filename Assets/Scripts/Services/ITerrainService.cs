using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WoodsOfIdle
{
    public interface ITerrainService
    {
        public CellData[,] GenerateTerrainData(TerrainGenerationSettings settings);

        public Texture2D GetTextureFromTerrainData(CellData[,] cells);

        public List<Vector2Int> GetSpawnPositionsForFarmingNode(int seed, FarmingNodeData nodeData, CellData[,] cells);

        public Vector3 GetSpawnPositionOffset(TerrainGenerationSettings settings);

        public Vector3 GetWorldPositionFromCellPosition(TerrainGenerationSettings settings, Vector2Int cellPosition);
    }
}

