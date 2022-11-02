using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using WoodsOfIdle;

public class TerrainServiceTests
{
    private ITerrainService terrainService = new TerrainService(new FarmingNodeService());

    [Test]
    public void CellsWillBeCorrectDimensions()
    {
        TerrainGenerationSettings settings = GetSettingsSplitType(CellType.Water, CellType.Grass, 0);
        settings.Size = new Vector2Int(50, 100);
        
        CellData [,] cells = terrainService.GenerateTerrainData(settings);
        
        Assert.That(cells.GetLength(0) == 50);
        Assert.That(cells.GetLength(1) == 100);
    }

    [Test]
    public void CellsWillBeSameOnSameSeed()
    {
        TerrainGenerationSettings settings = GetSettingsSplitType(CellType.Water, CellType.Grass, 0);
        
        CellData[,] firstCells = terrainService.GenerateTerrainData(settings);
        CellData[,] secondCells = terrainService.GenerateTerrainData(settings);
        
        for (int x = 0; x < firstCells.GetLength(0); x++)
        {
            for (int y = 0; y < firstCells.GetLength(1); y++)
            {
                Assert.That(firstCells[x, y].Height == secondCells[x, y].Height);
                Assert.That(firstCells[x, y].Type == secondCells[x, y].Type);
            }
        }
    }

    [Test]
    public void MultipleCellTypesWillSpawn()
    {
        TerrainGenerationSettings settings = GetSettingsSplitType(CellType.Water, CellType.Grass, 0);

        CellData[,] cells = terrainService.GenerateTerrainData(settings);

        Assert.That(cells.Cast<CellData>().Any(cell => cell.Type == CellType.Water));
        Assert.That(cells.Cast<CellData>().Any(cell => cell.Type == CellType.Grass));
    }

    //[Test]
    //[Ignore("Need to come up with a better way of verifying 2D array has some differences")]
    public void CellsWillBeDifferentOnDifferentSeed()
    {
        TerrainGenerationSettings firstSettings = GetSettingsSplitType(CellType.Water, CellType.Grass, 0);
        TerrainGenerationSettings secondSettings = GetSettingsSplitType(CellType.Water, CellType.Grass, 1);
        
        CellData[,] firstCells = terrainService.GenerateTerrainData(firstSettings);
        CellData[,] secondCells = terrainService.GenerateTerrainData(secondSettings);

        for (int x = 0; x < firstCells.GetLength(0); x++)
        {
            for (int y = 0; y < firstCells.GetLength(1); y++)
            {
                Assert.That(firstCells[x, y].Height != secondCells[x, y].Height);
                Assert.That(firstCells[x, y].Type != secondCells[x, y].Type);
            }
        }
    }

    [Test]
    public void NodesWontSpawnWhenNoValidCells()
    {
        TerrainGenerationSettings terrainSettings = GetSettingsAllSameType(CellType.Water);
        CellData[,] cells = terrainService.GenerateTerrainData(terrainSettings);

        FarmingNodeData nodeData = new FarmingNodeData
        {
            SpawnChance = 1f,
            AllowedCellTypes = new List<CellType>() { CellType.Grass }
        };

        List<FarmingNodeController> nodes = terrainService.GenerateFarmingNodeControllers(terrainSettings, cells, new FarmingNodeData[] { nodeData });

        Assert.That(nodes, Is.Empty);
    }


    [Test]
    public void NodesWillOnlySpawnOnValidCells()
    {
        TerrainGenerationSettings terrainSettings = GetSettingsSplitType(CellType.Water, CellType.Grass);
        CellData[,] cells = terrainService.GenerateTerrainData(terrainSettings);

        FarmingNodeData nodeData = new FarmingNodeData
        {
            SpawnChance = 1f,
            AllowedCellTypes = new List<CellType>() { CellType.Grass }
        };

        List<FarmingNodeController> nodes = terrainService.GenerateFarmingNodeControllers(terrainSettings, cells, new FarmingNodeData[] { nodeData });

        Assert.That(nodes.All(node => cells[node.State.Position.x, node.State.Position.y].Type == CellType.Grass));
    }


    [Test]
    public void NodesWillNotOverlap()
    {
        TerrainGenerationSettings terrainSettings = GetSettingsAllSameType(CellType.Grass);
        terrainSettings.Size = new Vector2Int(10, 10);
        CellData[,] cells = terrainService.GenerateTerrainData(terrainSettings);
        
        FarmingNodeData firstNodeData = new FarmingNodeData
        {
            SpawnChance = 1f,
            AllowedCellTypes = new List<CellType>() { CellType.Grass },
            NodeType = FarmingNodeType.Forest
        };

        FarmingNodeData secondNodeData = new FarmingNodeData
        {
            SpawnChance = 1f,
            AllowedCellTypes = new List<CellType>() { CellType.Grass },
            NodeType = FarmingNodeType.Boulder
        };

        List<FarmingNodeController> nodes = terrainService.GenerateFarmingNodeControllers(terrainSettings, cells, new FarmingNodeData[] { firstNodeData, secondNodeData });
        Vector2Int[] positions = nodes.Select(node => node.State.Position).ToArray();
        
        Assert.That(positions, Is.Unique);
    }


    [Test]
    public void NodesWilLoadFromStateCorrectly()
    {
        FarmingNodeData nodeData = new FarmingNodeData { NodeType = FarmingNodeType.Dirt };
        Dictionary<FarmingNodeType, FarmingNodeData> dataDict = (new FarmingNodeData[] { nodeData }).ToDictionary(node => node.NodeType);

        List<FarmingNodeState> states = new List<FarmingNodeState>()
        {
            new FarmingNodeState()
            {
                Position = new Vector2Int(0, 0),
                TimeToHarvest = 7f,
                NodeType = FarmingNodeType.Dirt
            }
        };

        List<FarmingNodeController> nodes = terrainService.GetFarmingNodeControllersFromState(states, dataDict);

        Assert.That(nodes.Count() == 1);
        Assert.That(nodes[0].State.Equals(states[0]));
        Assert.That(nodes[0].Data.Equals(nodeData));
    }


    [Test]
    public void SpawnPositionCorrectOnCellSizeOne()
    {
        TerrainGenerationSettings terrainSettings = new TerrainGenerationSettings
        {
            Seed = 0,
            Size = new Vector2Int(50, 50),
            CellSize = 1
        };
        
        Assert.That(terrainService.GetWorldPositionFromCellPosition(terrainSettings, Vector2Int.zero), Is.EqualTo(new Vector3(0, 0, 0)));
        Assert.That(terrainService.GetWorldPositionFromCellPosition(terrainSettings, Vector2Int.up), Is.EqualTo(new Vector3(0, 0, 1)));
        Assert.That(terrainService.GetWorldPositionFromCellPosition(terrainSettings, Vector2Int.right), Is.EqualTo(new Vector3(1, 0, 0)));
        Assert.That(terrainService.GetWorldPositionFromCellPosition(terrainSettings, Vector2Int.down), Is.EqualTo(new Vector3(0, 0, -1)));
        Assert.That(terrainService.GetWorldPositionFromCellPosition(terrainSettings, Vector2Int.left), Is.EqualTo(new Vector3(-1, 0, 0)));
    }

    
    [Test]
    public void SpawnPositionCorrectOnCellSizeHalf()
    {
        TerrainGenerationSettings terrainSettings = new TerrainGenerationSettings
        {
            Seed = 0,
            Size = new Vector2Int(50, 50),
            CellSize = 0.5f
        };

        Assert.That(terrainService.GetWorldPositionFromCellPosition(terrainSettings, Vector2Int.zero), Is.EqualTo(new Vector3(0, 0, 0)));
        Assert.That(terrainService.GetWorldPositionFromCellPosition(terrainSettings, Vector2Int.up), Is.EqualTo(new Vector3(0, 0, 0.5f)));
        Assert.That(terrainService.GetWorldPositionFromCellPosition(terrainSettings, Vector2Int.right), Is.EqualTo(new Vector3(0.5f, 0, 0)));
        Assert.That(terrainService.GetWorldPositionFromCellPosition(terrainSettings, Vector2Int.down), Is.EqualTo(new Vector3(0, 0, -0.5f)));
        Assert.That(terrainService.GetWorldPositionFromCellPosition(terrainSettings, Vector2Int.left), Is.EqualTo(new Vector3(-0.5f, 0, 0)));
    }

    [Test]
    public void SpawnPositionCorrectOnCellSizeTwo()
    {
        TerrainGenerationSettings terrainSettings = new TerrainGenerationSettings
        {
            Seed = 0,
            Size = new Vector2Int(50, 50),
            CellSize = 2
        };

        Assert.That(terrainService.GetWorldPositionFromCellPosition(terrainSettings, Vector2Int.zero), Is.EqualTo(new Vector3(0, 0, 0)));
        Assert.That(terrainService.GetWorldPositionFromCellPosition(terrainSettings, Vector2Int.up), Is.EqualTo(new Vector3(0, 0, 2)));
        Assert.That(terrainService.GetWorldPositionFromCellPosition(terrainSettings, Vector2Int.right), Is.EqualTo(new Vector3(2, 0, 0)));
        Assert.That(terrainService.GetWorldPositionFromCellPosition(terrainSettings, Vector2Int.down), Is.EqualTo(new Vector3(0, 0, -2)));
        Assert.That(terrainService.GetWorldPositionFromCellPosition(terrainSettings, Vector2Int.left), Is.EqualTo(new Vector3(-2, 0, 0)));
    }


    private TerrainGenerationSettings GetSettingsAllSameType(CellType cellType, int seed = 0)
    {
        return new TerrainGenerationSettings
        {
            Seed = seed,
            Size = new Vector2Int(50, 50),
            HeightMapSettings = new List<PerlinNoiseSettings>
            {
                new PerlinNoiseSettings
                {
                    Scale = 0.1f,
                    Strength = 1f,
                    Offset = new Vector2(0, 0)
                }
            },
            TileMapSettings = new List<TileMapSettings>
            {
                new TileMapSettings
                {
                    CellType = cellType,
                    HeightRange = new Vector2(0, 1)
                }
            }
        };
    }

    private TerrainGenerationSettings GetSettingsSplitType(CellType firstCellType, CellType secondCellType, int seed = 0)
    {
        return new TerrainGenerationSettings
        {
            Seed = seed,
            Size = new Vector2Int(50, 50),
            HeightMapSettings = new List<PerlinNoiseSettings>
            {
                new PerlinNoiseSettings
                {
                    Scale = 0.1f,
                    Strength = 1f,
                    Offset = new Vector2(0, 0)
                }
            },
            TileMapSettings = new List<TileMapSettings>
            {
                new TileMapSettings
                {
                    CellType = firstCellType,
                    HeightRange = new Vector2(0, 0.5f)
                },
                new TileMapSettings
                {
                    CellType = secondCellType,
                    HeightRange = new Vector2(0.5f, 1)
                }
            }
        };
    }
}
