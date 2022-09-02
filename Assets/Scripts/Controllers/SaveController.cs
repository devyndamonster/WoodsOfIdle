using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WoodsOfIdle
{
    public class SaveController
    {
        [HideInInspector] public SaveState CurrentSaveState { get; protected set; }
        
        protected ISaveService saveService;
        
        public SaveController(ISaveService saveService)
        {
            this.saveService = saveService;
        }

        public void OpenSave(string saveName)
        {
            if (CurrentSaveState is not null)
            {
                saveService.SaveGame(CurrentSaveState);
            }

            CurrentSaveState = saveService.LoadOrCreate(saveName);
        }
        
        public void OnDestroy()
        {
            saveService.SaveGame(CurrentSaveState);
        }

        public void OnApplicationPause(bool pause)
        {
            if (!Application.isEditor && pause)
            {
                saveService.SaveGame(CurrentSaveState);
            }
        }

        public void OnApplicationFocus(bool focus)
        {
            if (!Application.isEditor && !focus)
            {
                saveService.SaveGame(CurrentSaveState);
            }
        }

    }
}
