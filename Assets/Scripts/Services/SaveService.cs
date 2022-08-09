using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
                SaveState saveState = new SaveState()
                {
                    SaveName = saveName
                };

                InitializeState(saveState);
                return saveState;
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
            SaveState saveState = JsonConvert.DeserializeObject<SaveState>(saveJson);
            InitializeState(saveState);
            return saveState;
        }

        public void SaveGame(SaveState saveState)
        {
            string savePath = GetSavePath(saveState.SaveName);
            string saveJson = JsonConvert.SerializeObject(saveState);
            File.WriteAllText(savePath, saveJson);
        }

        public void DeleteSave(SaveState saveState)
        {
            DeleteSave(saveState.SaveName);
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

        private void InitializeState(SaveState state)
        {
            foreach(NodeType nodeType in Enum.GetValues(typeof(NodeType)))
            {
                if (!state.StoredItems.ContainsKey(nodeType))
                {
                    state.StoredItems[nodeType] = 0;
                }
            }
        }

        public IEnumerable<string> GetSaveNames()
        {
            return Directory
                .GetFiles(Application.persistentDataPath, "*.save")
                .Select(path => Path.GetFileNameWithoutExtension(path));
        }
    }
}
