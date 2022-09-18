using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace WoodsOfIdle
{
    public class FarmingNodeComponent : MonoBehaviour, IClickable
    {
        public static event Action<Vector2Int> NodeClicked;

        public FarmingNodeType NodeType;
        [HideInInspector] public Vector2Int Position;
        
        public void Click()
        {
            NodeClicked?.Invoke(Position);
        }
    }
}
