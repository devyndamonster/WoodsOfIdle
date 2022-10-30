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
            
            for (int x = 0; x < terrainData.CellData.GetLength(0); x++)
            {
                for (int y = 0; y < terrainData.CellData.GetLength(1); y++)
                {
                    var tileVertices = GetVerticesForTile(x, y);
                    
                    foreach(var vertex in tileVertices)
                    {
                        vertices.Add(vertex);
                        triangles.Add(triangles.Count);
                    }
                }
            }

            terrainMesh.triangles = triangles.ToArray();
            terrainMesh.vertices = vertices.ToArray();
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
    }
}
