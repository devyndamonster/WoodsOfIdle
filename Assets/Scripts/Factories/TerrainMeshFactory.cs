using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WoodsOfIdle
{
    public class TerrainMeshFactory : ITerrainMeshFactory
    {
        public Mesh CreateTerrainMesh(TerrainGenerationData terrainData)
        {
            Mesh terrainMesh = new Mesh();
            List<Vector3> vertices = new List<Vector3>();
            List<int> triangles = new List<int>();
            List<Vector2> uvs = new List<Vector2>();
            
            for (int x = 0; x < terrainData.CellData.GetLength(0); x++)
            {
                for (int y = 0; y < terrainData.CellData.GetLength(1); y++)
                {
                    var tileVertices = GetVerticesForTile(x, y);
                    var tileUVs = GetUVsForTile(x, y, terrainData.CellData.GetLength(0), terrainData.CellData.GetLength(1));

                    foreach (var vertex in tileVertices)
                    {
                        vertices.Add(vertex);
                        triangles.Add(triangles.Count);
                    }
                    
                    foreach (var uv in tileUVs)
                    {
                        uvs.Add(uv);
                    }
                }
            }

            terrainMesh.vertices = vertices.ToArray();
            terrainMesh.triangles = triangles.ToArray();
            terrainMesh.uv = uvs.ToArray();
            terrainMesh.RecalculateNormals();

            return terrainMesh;
        }
        
        private Vector3[] GetVerticesForTile(int x, int y)
        {
            Vector3 vertexA = new Vector3(x - 0.5f, 0, y + 0.5f);
            Vector3 vertexB = new Vector3(x + 0.5f, 0, y + 0.5f);
            Vector3 vertexC = new Vector3(x - 0.5f, 0, y - 0.5f);
            Vector3 vertexD = new Vector3(x + 0.5f, 0, y - 0.5f);
            return new Vector3[] { vertexA, vertexB, vertexC, vertexB, vertexD, vertexC };
        }

        private Vector2[] GetUVsForTile(int x, int y, int sizeX, int sizeY)
        {
            Vector2 uvA = new Vector2(x / (float)sizeX, y / (float)sizeY);
            Vector2 uvB = new Vector2((x + 1) / (float)sizeX, y / (float)sizeY);
            Vector2 uvC = new Vector2(x / (float)sizeX, (y + 1) / (float)sizeY);
            Vector2 uvD = new Vector2((x + 1) / (float)sizeX, (y + 1) / (float)sizeY);
            return new Vector2[] { uvA, uvB, uvC, uvB, uvD, uvC };
        }
    }
}
