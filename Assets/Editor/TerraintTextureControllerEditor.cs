using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace WoodsOfIdle
{
    [CustomEditor(typeof(TerrainTextureController))]
    public class TerraintTextureControllerEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            TerrainTextureController terrainTextureController = (TerrainTextureController)target;
            DrawDefaultInspector();

            if (GUILayout.Button("Generate Textures To Mesh"))
            {
                terrainTextureController.GenerateTextureToMesh();
            }
        }
    }
}
