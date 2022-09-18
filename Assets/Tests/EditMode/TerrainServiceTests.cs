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
        TerrainGenerationSettings settings = new TerrainGenerationSettings
        {
            Seed = 0,
            Size = new Vector2Int(50, 100)
        };

        CellData [,] cells = terrainService.GenerateTerrainData(settings);
        
        Assert.That(cells.GetLength(0) == 50);
        Assert.That(cells.GetLength(1) == 100);
    }

    [Test]
    public void CellsWillBeSameOnSameSeed()
    {
        TerrainGenerationSettings settings = new TerrainGenerationSettings
        {
            Seed = 0,
            Size = new Vector2Int(50, 50)
        };

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
    public void CellsWillBeDifferentOnDifferentSeed()
    {
        TerrainGenerationSettings firstSettings = new TerrainGenerationSettings
        {
            Seed = 0,
            Size = new Vector2Int(50, 50)
        };

        TerrainGenerationSettings secondSettings = new TerrainGenerationSettings
        {
            Seed = 1,
            Size = new Vector2Int(50, 50)
        };

        CellData[,] firstCells = terrainService.GenerateTerrainData(firstSettings);
        CellData[,] secondCells = terrainService.GenerateTerrainData(secondSettings);

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
    public void NodesWontSpawnWhenNoValidCells()
    {
        TerrainGenerationSettings terrainSettings = new TerrainGenerationSettings
        {
            Seed = 0,
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
                    CellType = CellType.Water,
                    HeightRange = new Vector2(0, 1)
                }
            }
        };

        CellData[,] cells = terrainService.GenerateTerrainData(terrainSettings);
        FarmingNodeData nodeData = ScriptableObject.CreateInstance<FarmingNodeData>();
        nodeData.SpawnChance = 1f;
        nodeData.AllowedCellTypes = new List<CellType>() { CellType.Grass };

        List<Vector2Int> spawnPositions = terrainService.GetSpawnPositionsForFarmingNode(terrainSettings, nodeData, cells);

        Assert.That(spawnPositions, Is.Empty);
    }


    [Test]
    public void NodesWillOnlySpawnOnValidCells()
    {
        TerrainGenerationSettings terrainSettings = new TerrainGenerationSettings
        {
            Seed = 0,
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
                    CellType = CellType.Water,
                    HeightRange = new Vector2(0, .5f)
                },
                new TileMapSettings
                {
                    CellType = CellType.Grass,
                    HeightRange = new Vector2(0.5f, 1)
                }
            }
        };

        CellData[,] cells = terrainService.GenerateTerrainData(terrainSettings);
        FarmingNodeData nodeData = ScriptableObject.CreateInstance<FarmingNodeData>();
        nodeData.SpawnChance = 1f;
        nodeData.AllowedCellTypes = new List<CellType>() { CellType.Grass };

        List<Vector2Int> spawnPositions = terrainService.GetSpawnPositionsForFarmingNode(terrainSettings, nodeData, cells);

        Assert.That(spawnPositions.All(spawnPosition => cells[spawnPosition.x, spawnPosition.y].Type == CellType.Grass));
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
}
