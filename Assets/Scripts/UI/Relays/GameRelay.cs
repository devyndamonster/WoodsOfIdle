using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WoodsOfIdle
{
    public class GameRelay
    {
        public event Action<FarmingNodeController> OnFarmingNodeClicked;

        private GameController _gameController;

        public GameRelay(GameController gameController)
        {
            _gameController = gameController;

            _gameController.OnFarmingNodeClicked += (farmingNode) => OnFarmingNodeClicked?.Invoke(farmingNode);
        }
        

        
    }
}
