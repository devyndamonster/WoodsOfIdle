using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WoodsOfIdle
{
    public class SaveController : MonoBehaviour
    {
        
        [HideInInspector]
        public SaveState CurrentSaveState { get; protected set; }

        protected ISaveService saveService = new SaveService();

        private static string nextSaveToOpen;

        public static void SetNextSaveToOpen(string saveName)
        {
            nextSaveToOpen = saveName;
        }

        public void OpenSave()
        {
            OpenSave(nextSaveToOpen);
        }

        public void OpenSave(string saveName)
        {
            if (CurrentSaveState is not null)
            {
                saveService.SaveGame(CurrentSaveState);
            }

            CurrentSaveState = saveService.LoadOrCreate(saveName);
            ConnectNodesToCurrentSaveState();
        }

        private void ConnectNodesToCurrentSaveState()
        {
            foreach (FarmingNodeController node in FindObjectsOfType<FarmingNodeController>())
            {
                node.ConnectToSaveState(CurrentSaveState);
            }
        }

        private void OnDestroy()
        {
            saveService.SaveGame(CurrentSaveState);
        }

        private void OnApplicationPause(bool pause)
        {
            if (!Application.isEditor && pause)
            {
                saveService.SaveGame(CurrentSaveState);
            }
        }

        private void OnApplicationFocus(bool focus)
        {
            if (!Application.isEditor && !focus)
            {
                saveService.SaveGame(CurrentSaveState);
            }
        }

    }
}
