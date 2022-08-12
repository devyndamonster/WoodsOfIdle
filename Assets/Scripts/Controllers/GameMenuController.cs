using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

namespace WoodsOfIdle
{
    public class GameMenuController : MonoBehaviour
    {
        [SerializeField]
        private List<TextMeshProUGUI> storedItemsTexts = new List<TextMeshProUGUI>();

        public void UpdateDisplayFromState(SaveState state)
        {
            foreach (ItemType nodeType in Enum.GetValues(typeof(ItemType)))
            {
                storedItemsTexts[(int)nodeType].text = state.StoredItems[nodeType].ToString();
            }
                
        }
    }
}

