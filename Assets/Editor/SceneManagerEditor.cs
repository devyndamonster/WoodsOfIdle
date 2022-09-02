using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace WoodsOfIdle
{
    [CustomEditor(typeof(SceneManagerComponent))]
    public class SceneStartupControllerEditor : Editor
    {
        public override VisualElement CreateInspectorGUI()
        {
            SceneManagerComponent sceneManager = (SceneManagerComponent)target;
            
            var container = new VisualElement();
            InspectorElement.FillDefaultInspector(container, serializedObject, this);

            Button generateButton = new Button { text = "Generate Terrain"};
            generateButton.clicked += OnGenerateButtonClicked;
            container.Add(generateButton);

            return container;
        }

        private void OnGenerateButtonClicked()
        {
            SceneManagerComponent sceneManager = (SceneManagerComponent)target;
            
            ITerrainService terrainService = new TerrainService();
            ISaveService saveService = new SaveService();
            SaveController saveController = new SaveController(saveService);
            TerrainTextureController terrainController = new TerrainTextureController(terrainService, saveController);
            
            terrainController.GenerateTerrain(sceneManager.TerrainSettings, sceneManager.TerrainMeshRenderer);
        }
    }
}
