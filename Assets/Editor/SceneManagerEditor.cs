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

            IFarmingNodeService farmingNodeService = new FarmingNodeService();
            ITerrainService terrainService = new TerrainService(farmingNodeService);
            ISaveService saveService = new SaveService();
            SaveController saveController = new SaveController(saveService);
            TerrainGenerationController terrainController = new TerrainGenerationController(terrainService, saveController, sceneManager.AssetCollection);
            
            terrainController.GenerateTerrain(sceneManager.TerrainSettings);
        }
    }
}
