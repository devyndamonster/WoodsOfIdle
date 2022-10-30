using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WoodsOfIdle
{
    public interface IFarmingNodeComponentFactory
    {
        public FarmingNodeComponent Create(FarmingNodeController farmingNodeController);
    }
}
