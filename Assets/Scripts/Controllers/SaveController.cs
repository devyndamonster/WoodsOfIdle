using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace WoodsOfIdle
{
    public class SaveController : IDestroyReceiver, IPauseReceiver, IFocusReceiver
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

        public void UpdateSaveStateFromTerrainData(TerrainGenerationData terrainData)
        {
            CurrentSaveState.Cells = terrainData.CellData;
            CurrentSaveState.FarmingNodes = terrainData.FarmingNodes.ToDictionary(node => node.State.Position, node => node.State);
        }
        
        public void Destroy()
        {
            saveService.SaveGame(CurrentSaveState);
        }

        public void Pause()
        {
            if (!Application.isEditor)
            {
                saveService.SaveGame(CurrentSaveState);
            }
        }

        public void Unfocus()
        {
            if (!Application.isEditor)
            {
                saveService.SaveGame(CurrentSaveState);
            }
        }
        
        public void Unpause() { }

        public void Focus() { }
    }
}
