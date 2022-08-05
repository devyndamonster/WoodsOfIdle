using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace WoodsOfIdle
{
    public class FarmingNodeController : MonoBehaviour
    {
        public Button activateButton;

        public FarmingNodeState state = new FarmingNodeState();

        public void ConnectToSaveState(SaveState saveState)
        {
            if (saveState.FarmingNodes.ContainsKey(state.NodeId))
            {
                state = saveState.FarmingNodes[state.NodeId];
            }
            else
            {
                saveState.FarmingNodes[state.NodeId] = state;
            }
        }

        public void ToggleActive()
        {
            SetActive(!state.IsActive);
        }

        public void UpdateState()
        {

        }

        private void SetActive(bool isActive)
        {
            state.IsActive = isActive;
        }
    }
}
