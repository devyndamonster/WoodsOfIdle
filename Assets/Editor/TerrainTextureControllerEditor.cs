using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace WoodsOfIdle
{
    [CustomEditor(typeof(TerrainTextureController))]
    public class TerrainTextureControllerEditor : Editor
    {
        public override VisualElement CreateInspectorGUI()
        {
            TerrainTextureController terrainTextureController = (TerrainTextureController)target;
            
            var container = new VisualElement();
            InspectorElement.FillDefaultInspector(container, serializedObject, this);

            Button generateButton = new Button { text = "Generate Terrain"};
        generateButton.clicked += terrainTextureController.GenerateTextureToMesh;
            container.Add(generateButton);

            return container;
        }
    }
}
