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
    public string levelName = "";

    int[] tileSpriteIndexVals;
    LevelHandler levelHandler;

    // When level is constructed, get width height and name of text file
    public Level(int newLevelWidth, int newLevelHeight, string newLevelName)
    {
        levelWidth = newLevelWidth;
        levelHeight = newLevelHeight;
        levelName = newLevelName;
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

        tileSpriteIndexVals = new int[tileArray.Length];
        tileSpriteIndexVals = levelHandler.LoadLevel($"../../../assets/levels/{levelName}.txt", tileSpriteIndexVals);

        for (int tile = 0; tile < tileArray.Length; tile++)
        {
            tileArray[tile] = new Tile();
            tileArray[tile].position = tilePositions[tile];
            tileArray[tile].UpdateColorIndex(tileSpriteIndexVals[tile]);
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
