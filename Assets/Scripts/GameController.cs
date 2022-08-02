using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WoodsOfIdle
{
    public class GameController : MonoBehaviour
    {
        protected ISaveService saveService;

        [HideInInspector]
        public SaveState currentSaveState;

        private void Awake()
        {
            saveService = new SaveService();
        }

        private void Start()
        {
            currentSaveState = saveService.LoadOrCreate("test");
        }

        private void OnDestroy()
        {
            saveService.SaveGame(currentSaveState);
        }


    }
}
