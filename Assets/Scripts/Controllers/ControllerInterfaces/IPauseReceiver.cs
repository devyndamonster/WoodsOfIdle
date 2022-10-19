using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WoodsOfIdle
{
    public interface IPauseReceiver
    {
        public void Pause();

        public void Unpause();
    }
}
