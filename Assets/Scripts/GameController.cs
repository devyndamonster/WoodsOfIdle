using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WoodsOfIdle
{
    public class GameController : MonoBehaviour
    {
        protected ISaveService saveService;

        public SaveState currentSaveState;

        private void Awake()
        {
            InitializeServices();
            SetSave("test");
        }

        private void InitializeServices()
        {
            saveService = new SaveService();
        }

        public void SetSave(string levelName)
        {
            if(currentSaveState is not null)
            {
                saveService.SaveGame(currentSaveState);
            }

            currentSaveState = saveService.LoadOrCreate(levelName);
        }

        private void OnDestroy()
        {
            saveService.SaveGame(currentSaveState);
        }


    }
}
