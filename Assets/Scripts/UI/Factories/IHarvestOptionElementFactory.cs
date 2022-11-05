using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace WoodsOfIdle
{
    public interface IHarvestOptionElementFactory
    {
        public VisualElement CreateElement(FarmingNodeController farmingNode);
    }
}
