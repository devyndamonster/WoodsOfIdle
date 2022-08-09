using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace WoodsOfIdle
{
    public class MainMenuController : MonoBehaviour
    {
        public VisualTreeAsset SaveSelectOption;

        protected UIDocument rootDocument;
        protected VisualElement rootElement;
        protected VisualElement saveContainer;

        protected ISaveService saveService;

        private void Awake()
        {
            InitializeServices();
            InitializeRoot();
            InitializeSaveContainer();
            PopulateSaveContainer();
        }

        private void InitializeServices()
        {
            saveService = new SaveService();
        }

        private void InitializeRoot()
        {
            rootDocument = GetComponent<UIDocument>();
            rootElement = rootDocument.rootVisualElement;
        }

        private void InitializeSaveContainer()
        {
            saveContainer = rootElement.Q<VisualElement>("SaveSelectContainer");
            saveContainer.RemoveAllChildren();
        }

        private void PopulateSaveContainer()
        {
            foreach (string saveName in saveService.GetSaveNames())
            {
                VisualElement saveSelectOption = SaveSelectOption.CloneTree();
                Button deleteSaveButton = saveSelectOption.Q<Button>("DeleteSaveButton");
                Button selectSaveButton = saveSelectOption.Q<Button>("SelectSaveButton");

                deleteSaveButton.clicked += () =>
                {
                    Debug.Log("Lol deleted!");
                };

                selectSaveButton.text = saveName;

                saveContainer.Add(saveSelectOption);
            }
        }


    }
}


