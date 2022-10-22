using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace WoodsOfIdle
{
    public class GameEventComponent : MonoBehaviour
    {
        private IUpdateReceiver[] _updateReceivers = { };
        private IDestroyReceiver[] _destoryReceivers = { };
        private IPauseReceiver[] _pauseReceivers = { };
        private IFocusReceiver[] _focusReceivers = { };

        public static GameEventComponent Create()
        {
            var go = new GameObject("GameEventComponent");
            var component = go.AddComponent<GameEventComponent>();
            return component;
        }

        public GameEventComponent WithReceivers(IEnumerable<IUpdateReceiver> receivers)
        {
            _updateReceivers = receivers.ToArray();
            return this;
        }

        public GameEventComponent WithReceivers(IEnumerable<IDestroyReceiver> receivers)
        {
            _destoryReceivers = receivers.ToArray();
            return this;
        }

        public GameEventComponent WithReceivers(IEnumerable<IPauseReceiver> receivers)
        {
            _pauseReceivers = receivers.ToArray();
            return this;
        }

        public GameEventComponent WithReceivers(IEnumerable<IFocusReceiver> receivers)
        {
            _focusReceivers = receivers.ToArray();
            return this;
        }
        
        private void Update()
        {
            foreach (var receiver in _updateReceivers)
            {
                receiver.Update();
            }
        }

        private void OnDestroy()
        {
            foreach (var receiver in _destoryReceivers)
            {
                receiver.Destroy();
            }
        }

        private void OnApplicationPause(bool pauseStatus)
        {
            foreach (var receiver in _pauseReceivers)
            {
                if (pauseStatus)
                {
                    receiver.Pause();
                }
                else
                {
                    receiver.Unpause();
                }
            }
        }

        private void OnApplicationFocus(bool focusStatus)
        {
            foreach (var receiver in _focusReceivers)
            {
                if (focusStatus)
                {
                    receiver.Focus();
                }
                else
                {
                    receiver.Unfocus();
                }
            }
        }
    }
}
