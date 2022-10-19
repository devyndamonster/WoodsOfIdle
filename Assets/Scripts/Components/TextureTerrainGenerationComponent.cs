using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WoodsOfIdle
{
    public class TextureTerrainGenerationComponent : MonoBehaviour, ITerrainReceiver
    {
        public MeshRenderer terrainMesh;

        private ITerrainService terrainService = new TerrainService(new FarmingNodeService());

        public void ApplyTerrain(TerrainGenerationData terrainData, TerrainGenerationSettings settings)
        {
            GenerateTerrain(terrainData.CellData, settings);
            SpawnFarmingNodes(terrainData.FarmingNodePrefabs, settings);
        }
        
        private void GenerateTerrain(CellData[,] cells, TerrainGenerationSettings settings)
        {
            Texture2D texture = GetTextureFromTerrainData(cells);
            SetMeshTexture(texture, terrainMesh);
            SetMeshScale(settings, terrainMesh);
            SetMeshPosition(settings, terrainMesh);
        }

        private void SpawnFarmingNodes(Dictionary<Vector2Int, GameObject> farmingNodes, TerrainGenerationSettings settings)
        {
            foreach (var farmingNode in farmingNodes)
            {
                GameObject node = Instantiate(farmingNode.Value, transform);
                node.transform.position = terrainService.GetWorldPositionFromCellPosition(settings, farmingNode.Key) + terrainService.GetSpawnPositionOffset(settings);
                node.GetComponent<FarmingNodeComponent>().Position = farmingNode.Key;
            }
        }

        private Texture2D GetTextureFromTerrainData(CellData[,] cells)
        {
            int width = cells.GetLength(0);
            int height = cells.GetLength(1);

            Color[] colorMap = new Color[width * height];
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    colorMap[y * width + x] = cells[x, y].Color;
                }
            }

            Texture2D texture = new Texture2D(width, height);
            texture.SetPixels(colorMap);
            texture.wrapMode = TextureWrapMode.Clamp;
            texture.filterMode = FilterMode.Point;
            texture.Apply();

            return texture;
        }

        private void SetMeshTexture(Texture2D texture, MeshRenderer targetMesh)
        {
            targetMesh.sharedMaterial.mainTexture = texture;
        }

        private void SetMeshScale(TerrainGenerationSettings settings, MeshRenderer targetMesh)
        {
            targetMesh.transform.localScale = new Vector3(settings.Size.x, 1, settings.Size.y) * settings.CellSize / 10;
        }

        private void SetMeshPosition(TerrainGenerationSettings settings, MeshRenderer targetMesh)
        {
            targetMesh.gameObject.transform.position = new Vector3(settings.Origin.x + settings.Size.x / 2, 0, settings.Origin.y + settings.Size.y / 2);
        }


    }
}
