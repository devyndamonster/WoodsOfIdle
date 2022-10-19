using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WoodsOfIdle
{
    public interface IFocusReceiver
    {
        public void Focus();
        public void Unfocus();
    }
}
