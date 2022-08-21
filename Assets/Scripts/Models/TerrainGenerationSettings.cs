using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TerrainGenerationSettings
{
    public int MinCoordX { get; set; }
    public int MaxCoordX { get; set; }
    public int MinCoordY { get; set; }
    public int MaxCoordY { get; set; }
    public int Seed { get; set; }
    
}
