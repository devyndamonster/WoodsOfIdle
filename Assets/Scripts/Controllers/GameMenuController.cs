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
            storedItemsTexts[0].text = state.StoredItems.First().Value.ToString();
        }
    }
}

