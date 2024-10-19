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


    public void Setup()
    {
        // Populate tile array with a given amount of tiles
        tileRowCount = Game.windowHeight / tileSize;
        tileColCount = Game.windowWidth / tileSize;
        tileArray = new Tile[tileRowCount * tileColCount];
        tilePositions = new Vector2[tileRowCount * tileColCount];

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
        // Render all tiles in tile array
        for (int i = 0; i < tileArray.Length; i++)
        {
            tileArray[i].Render();
        }

        MouseCollisionCheck();
    }

    void MouseCollisionCheck()
    {
        for (int tile = 0; tile < tileArray.Length; tile++)
        {
            int currentColorIndex = tileArray[tile].colorIndex;
            // int newColorIndex = tileArray[tile].UpdateColorIndex();

            // Left alt wipes the screen
            if (Input.IsKeyboardKeyDown(KeyboardInput.LeftAlt))
            {
                tileArray[tile].UpdateColorIndex(-1);
               
            }

            // Set collision check bool true if 
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

            // Mouse/Tile collision check


            if (Input.IsKeyboardKeyDown(KeyboardInput.LeftControl))
            {

                if (mouseIntersectsY)
                {
                    if (Input.IsMouseButtonPressed(MouseInput.Right))
                    {
                        tileArray[tile].UpdateColorIndex();
                    }
                }
                else if (mouseIntersectsX)
                {
                    if (Input.IsMouseButtonPressed(MouseInput.Left))
                    {
                        tileArray[tile].UpdateColorIndex();
                    }
                }
            }



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
