using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

namespace WoodsOfIdle
{
    public class MainMenuComponent : MonoBehaviour
    {
        public VisualTreeAsset SaveSelectOption;

        protected UIDocument rootDocument;
        protected VisualElement rootElement;
        protected ListView saveContainer;
        protected VisualElement popupContainer;
        protected List<string> loadedSaveNames = new List<string>();

        protected ISaveService saveService;

        private void Awake()
        {
            InitializeServices();
            InitializeRoot();
            InitializeSaveContainer();
            InitializeSaveCreation();
        }

        private void Start()
        {
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
            saveContainer = rootElement.Q<ListView>("SaveSelectContainer");
            saveContainer.makeItem = MakeSaveListItem;
            saveContainer.bindItem = BindSaveListItem;
            saveContainer.itemsSource = loadedSaveNames;
        }

        private void InitializeSaveCreation()
        {
            popupContainer = rootElement.Q<VisualElement>("ScreenCenterPopups");
            HideSaveCreationPopup();

            Button saveCreationButton = rootElement.Q<Button>("CreateNewSaveButton");
            saveCreationButton.clicked += DisplaySaveCreationPopup;
        }

        private VisualElement MakeSaveListItem()
        {
            return SaveSelectOption.CloneTree();
        }

        private void BindSaveListItem(VisualElement saveSelectOption, int index)
        {
            string saveName = loadedSaveNames[index];
            Button deleteSaveButton = saveSelectOption.Q<Button>("DeleteSaveButton");
            Button selectSaveButton = saveSelectOption.Q<Button>("SelectSaveButton");

            selectSaveButton.text = saveName;

            deleteSaveButton.clicked += () =>
            {
                saveService.DeleteSave(saveName);
                PopulateSaveContainer();
            };

            selectSaveButton.clicked += () =>
            {
                SceneManagerComponent.SetNextSaveToOpen(saveName);
                SceneManager.LoadScene("EmptyIdleScene", LoadSceneMode.Single);
            };
        }

        private void PopulateSaveContainer()
        {
            loadedSaveNames = saveService.GetSaveNames().ToList();
            saveContainer.itemsSource = loadedSaveNames;

            Debug.Log(string.Join(',', loadedSaveNames));

            saveContainer.Rebuild();
        }

        private void DisplaySaveCreationPopup()
        {
            popupContainer.visible = true;

            TextField saveNameField = popupContainer.Q<TextField>("NewSaveNameField");
            Button createNewSaveButton = popupContainer.Q<Button>("CreateNewSaveButton");
            Button cancelNewSaveButton = popupContainer.Q<Button>("CancelSaveCreationButton");

            cancelNewSaveButton.clicked += HideSaveCreationPopup;
            createNewSaveButton.clicked += () => { CreateNewSave(saveNameField.text); };
        }

        private void HideSaveCreationPopup()
        {
            popupContainer.visible = false;
        }

        private void CreateNewSave(string saveName)
        {
            SaveState newSave = saveService.LoadOrCreate(saveName);
            saveService.SaveGame(newSave);
            HideSaveCreationPopup();
            PopulateSaveContainer();
        }
    }
}


