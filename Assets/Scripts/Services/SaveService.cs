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

        public void DeleteSave(string saveName)
        {
            string savePath = GetSavePath(saveName);

            if (File.Exists(savePath))
            {
                File.Delete(savePath);
            }
        }

        public bool DoesSaveExist(string saveName)
        {
            return File.Exists(GetSavePath(saveName));
        }

        private string GetSavePath(string saveName)
        {
            return $"{Application.persistentDataPath}/{saveName}.save";
        }
    }
}
