using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WoodsOfIdle
{
    [System.Serializable]
    public class FarmingNodeState
    {
        public int NodeId;

        public FarmingNodeType NodeType;

        public bool IsActive;

        [HideInInspector]
        public DateTime TimeLastHarvested = DateTime.Now;

        public float TimeToHarvest;
    }
}
