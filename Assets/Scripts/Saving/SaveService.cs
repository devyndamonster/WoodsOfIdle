using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace WoodsOfIdle
{
    public class SaveService : ISaveService
    {

        public SaveState LoadOrCreate(string saveName)
        {
            string savePath = GetSavePath(saveName);

            Debug.Log(savePath);

            if (!File.Exists(savePath))
            {
                return new SaveState()
                {
                    SaveName = saveName
                };
            }

            return LoadSave(saveName);
        }

        public SaveState LoadSave(string saveName)
        {
            string savePath = GetSavePath(saveName);

            if (!File.Exists(savePath))
            {
                throw new System.Exception($"Save '{saveName}' could not be found");
            }

            string saveJson = File.ReadAllText(savePath);
            return JsonConvert.DeserializeObject<SaveState>(saveJson);
        }

        public void SaveGame(SaveState saveState)
        {
            string savePath = GetSavePath(saveState.SaveName);
            string saveJson = JsonConvert.SerializeObject(saveState);
            File.WriteAllText(savePath, saveJson);
        }

        private string GetSavePath(string saveName)
        {
            return $"{Application.persistentDataPath}/{saveName}.save";
        }
    }
}
