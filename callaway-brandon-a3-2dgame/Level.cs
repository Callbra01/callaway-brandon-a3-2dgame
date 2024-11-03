using Raylib_cs;
using System;
using System.Numerics;

namespace Game10003;

public class Level
{
    public int tileSize = 50;
    int tileRowCount;
    int tileColumnCount;
    int levelHeight;
    int levelWidth;

    public Tile[] tileArray;
    public Vector2[] tilePositions;

    int[] tileSpriteIndexVals;
    LevelHandler levelHandler;

    public Level(int newLevelWidth, int newLevelHeight)
    {
        levelWidth = newLevelWidth;
        levelHeight = newLevelHeight;
    }

    public void Setup()
    {

        levelHandler = new LevelHandler();

        tileRowCount = (Window.Height / tileSize) * levelHeight;
        tileColumnCount = (Window.Width / tileSize) * levelWidth;

        tileArray = new Tile[tileRowCount * tileColumnCount];
        tilePositions = new Vector2[tileArray.Length];

        int tilePositionIndex = 0;
        for (int row = 0; row < tileRowCount; row++)
        {
            for (int col = 0; col < tileColumnCount; col++)
            {
                Vector2 newPosition = new Vector2(col * tileSize, row * tileSize);
                tilePositions[tilePositionIndex] = newPosition;
                tilePositionIndex++;
            }
        }

        for (int tile = 0; tile < tileArray.Length; tile++)
        {
            tileArray[tile] = new Tile();
            tileArray[tile].position = tilePositions[tile];
        }

        tileSpriteIndexVals = new int[tileArray.Length];
        tileSpriteIndexVals = levelHandler.LoadLevel("../../../assets/levels/testLevelOne.txt", tileSpriteIndexVals);
    
        for (int i = 0; i < tileArray.Length; i++)
        {
            tileArray[i].UpdateColorIndex(tileSpriteIndexVals[i]);
        }
    }

    public void Render()
    {
        // Render level tiles, as tiles are not editable, outlines should not be displayed
        for (int i = 0; i < tileArray.Length; i++)
        {
            tileArray[i].Render(false);
        }
    }
}
