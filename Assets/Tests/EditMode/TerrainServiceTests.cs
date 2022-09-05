using System;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using WoodsOfIdle;

public class TerrainServiceTests
{
    private ITerrainService terrainService = new TerrainService();

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
}
