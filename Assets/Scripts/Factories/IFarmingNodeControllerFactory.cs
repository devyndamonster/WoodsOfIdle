using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WoodsOfIdle
{
    public interface IFarmingNodeControllerFactory
    {
        public FarmingNodeController CreateFarmingNodeController(FarmingNodeType nodeType, Vector2Int position);

        public FarmingNodeController CreateFarmingNodeController(FarmingNodeState state);
    }
}
