using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WoodsOfIdle
{
    public interface ISaveService
    {
        public SaveState LoadOrCreate(string saveName);

        public SaveState LoadSave(string saveName);

        public void SaveGame(SaveState saveState);

        public void DeleteSave(string saveName);

        public bool DoesSaveExist(string saveName);

    }
}
