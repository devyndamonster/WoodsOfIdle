using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WoodsOfIdle
{
    [CreateAssetMenu(fileName = "FarmingNodeData", menuName = "Data/FarmingNodeData")]
    public class FarmingNodeDataScriptable : ScriptableObject
    {
        public FarmingNodeData Value;
    }
}
