using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace WoodsOfIdle
{
    public class InventorySlotState
    {
        public string SlotId;

        public ItemType ItemType;

        public int Quantity;

        public bool CanAutoInsert;
    }
}


