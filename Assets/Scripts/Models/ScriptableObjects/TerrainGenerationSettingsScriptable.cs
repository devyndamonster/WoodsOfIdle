using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WoodsOfIdle
{
    [CreateAssetMenu(fileName = "TerrainSettings", menuName = "Settings/TerrainSettings")]
    public class TerrainGenerationSettingsScriptable : ScriptableObject
    {
        public TerrainGenerationSettings Value;
    }
}
