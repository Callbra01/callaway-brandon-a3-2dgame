using System;
using System.Numerics;

namespace Game10003;

public class LevelEditor
{
    public int tileSize = 50;
    public int tileRowCount;
    public int tileColCount;
    public Tile[] tileArray;
    public Vector2[] tilePositions;
    bool mouseIntersectsX = false;
    bool mouseIntersectsY = false;

    int[] tileColorIndexVals;
    LevelHandler levelHandler;

    public void Setup()
    {
        // Populate tile array with a given amount of tiles
        tileRowCount = Game.windowHeight / tileSize;
        tileColCount = Game.windowWidth / tileSize;
        tileArray = new Tile[tileRowCount * tileColCount];
        tilePositions = new Vector2[tileRowCount * tileColCount];

        levelHandler = new LevelHandler();

        // index starts at 0, loop through rows and columns to get new position via tile size. 
        int tilePositionIndex = 0;
        for (int row = 0; row < tileRowCount; row++)
        {
            for (int col = 0; col < tileColCount; col++)
            {
                Vector2 newPosition = new Vector2(col * tileSize, row * tileSize);
                tilePositions[tilePositionIndex] = newPosition;
                tilePositionIndex++;
            }
        }

        // For every tile, update position
        for (int tile = 0; tile < tileArray.Length; tile++)
        {
            tileArray[tile] = new Tile();
            tileArray[tile].position = tilePositions[tile];
        }
    }

    public void Update()
    {
        Render();
        MouseCollisionCheck();
        HandleInput();
    }

    // Draw all tiles
    void Render()
    {
        // Render all tiles in tile array
        for (int i = 0; i < tileArray.Length; i++)
        {
            tileArray[i].Render();
        }
    }

    // Save or Load depending on input
    void HandleInput()
    {
        if(Input.IsKeyboardKeyPressed(KeyboardInput.Q))
        {
            levelHandler.SaveLevel("../../../assets/levels/levelEditor0.txt", tileArray);
        }
        else if (Input.IsKeyboardKeyPressed(KeyboardInput.E))
        {
            tileColorIndexVals = new int[tileArray.Length];

            tileColorIndexVals = levelHandler.LoadLevel("../../../assets/levels/levelEditor0.txt", tileColorIndexVals);

            for (int i = 0; i < tileArray.Length; i++)
            {
                tileArray[i].UpdateColorIndex(tileColorIndexVals[i]);
            }
        }
    }

    void MouseCollisionCheck()
    {
        for (int tile = 0; tile < tileArray.Length; tile++)
        {
            int currentColorIndex = tileArray[tile].colorIndex;
            int newColorIndex = currentColorIndex + 1;

            // Left alt wipes the screen
            if (Input.IsKeyboardKeyDown(KeyboardInput.LeftAlt))
            {
                tileArray[tile].UpdateColorIndex(-1);
            }

            // Keep track to tiles that intersect mouseX and mouseY
            if (Input.GetMousePosition().X >= tileArray[tile].position.X + 2 && Input.GetMousePosition().X <= tileArray[tile].position.X + tileSize - 2)
            {
                mouseIntersectsX = true;
            }
            else
            {
                mouseIntersectsX = false;
            }

            if (Input.GetMousePosition().Y >= tileArray[tile].position.Y + 2 && Input.GetMousePosition().Y <= tileArray[tile].position.Y + tileSize - 2)
            {
                mouseIntersectsY = true;
            }
            else
            {
                mouseIntersectsY = false;
            }

            // If control is down, left click for columns, right click for rows
            if (Input.IsKeyboardKeyDown(KeyboardInput.LeftControl))
            {
                // Create rows
                if (mouseIntersectsY)
                {
                    if (Input.IsMouseButtonPressed(MouseInput.Right))
                    { 
                        tileArray[tile].UpdateColorIndex();
                    }
                }

                // Create collumns
                else if (mouseIntersectsX)
                {
                    if (Input.IsMouseButtonPressed(MouseInput.Left))
                    {
                        tileArray[tile].UpdateColorIndex();
                    }
                }
            }

            // If mouse is clicked over a tile, change color
            if (mouseIntersectsX && mouseIntersectsY)
            {
                if (Input.IsMouseButtonPressed(MouseInput.Left))
                {
                    tileArray[tile].UpdateColorIndex();
                }
            }
        }
    }
}
