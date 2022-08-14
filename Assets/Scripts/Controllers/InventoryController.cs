using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace WoodsOfIdle
{
    public class InventoryController : MonoBehaviour
    {
        public UIDocument InventoryPanel;

        private List<DragAndDropSlot> dragAndDropSlots;

        private void Awake()
        {
            ItemData[] itemData = Resources.LoadAll<ItemData>("Items/Data"); ;

            
            dragAndDropSlots = InventoryPanel.rootVisualElement.Query<DragAndDropSlot>().ToList();

            dragAndDropSlots[0].AddItemToSlot(itemData[0], 1);
        }
    }
}
