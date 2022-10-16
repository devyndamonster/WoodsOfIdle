using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WoodsOfIdle
{
    public class InventorySlotData
    {
        public string SlotId { get; set; }
        public bool BelongsToPlayer { get; set; }
        public bool CanAutoInsert { get; set; }
    }
}
