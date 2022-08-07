using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace WoodsOfIdle
{
    public class GameMenuController : MonoBehaviour
    {
        [SerializeField]
        private List<Text> storedItemsTexts = new List<Text>();

        public void UpdateDisplayFromState(SaveState state)
        {

        }
    }
}

