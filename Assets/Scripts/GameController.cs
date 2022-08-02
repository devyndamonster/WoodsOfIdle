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
            saveService = new SaveService();
            currentSaveState = saveService.LoadOrCreate("test");
        }

        private void OnDestroy()
        {
            saveService.SaveGame(currentSaveState);
        }


    }
}
