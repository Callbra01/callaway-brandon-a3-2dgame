// Include code libraries you need below (use the namespace).
using System;
using System.Numerics;

// The namespace your code is in.
namespace Game10003;

/// <summary>
///     Your game code goes inside this class!
/// </summary>
public class Game
{
    // Place your variables here:
    int windowWidth = 800;
    int windowHeight = 600;
    int tileSize = 50;
    int tileRowCount;
    int tileColCount;
    Tile[] tileArray;
    Vector2[] tilePositions;


    /// <summary>
    ///     Setup runs once before the game loop begins.
    /// </summary>
    public void Setup()
    {
        Window.SetTitle("bGame");
        Window.SetSize(windowWidth, windowHeight);
        Window.TargetFPS = 60;
        

        // Populate tile array with a given amount of tiles
        tileRowCount = windowHeight / tileSize;
        tileColCount = windowWidth / tileSize;
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

    /// <summary>
    ///     Update runs every frame.
    /// </summary>
    public void Update()
    {
        Window.ClearBackground(Color.Black);
        
        for (int i = 0; i < tileArray.Length; i++)
        {
            tileArray[i].Render();
        }
        
        CollisionCheck();
        DEBUGINPUT();
    }

    void DEBUGINPUT()
    {
        if (Input.IsKeyboardKeyDown(KeyboardInput.A))
        {
            for (int tile = 0; tile < tileArray.Length; tile++)
            {
                tileArray[tile].position.X -= 50 * Time.DeltaTime;
            }
        }
        else if (Input.IsKeyboardKeyDown(KeyboardInput.D))
        {
            for (int tile = 0; tile < tileArray.Length; tile++)
            {
                tileArray[tile].position.X += 50 * Time.DeltaTime;
            }
        }
    }

    // If user clicks within the boundaries of a given tile, cycle colors
    void CollisionCheck()
    {
        
        for (int tile = 0; tile < tileArray.Length; tile++)
        {   
            // Mouse/Tile collision check
            if (Input.GetMousePosition().X >= tileArray[tile].position.X + 2 && Input.GetMousePosition().X <= tileArray[tile].position.X + tileSize - 2)
            {
                if (Input.GetMousePosition().Y >= tileArray[tile].position.Y + 2 && Input.GetMousePosition().Y <= tileArray[tile].position.Y + tileSize - 2)
                {
                    if (Input.IsMouseButtonPressed(MouseInput.Left))
                    {
                        tileArray[tile].UpdateColorIndex();
                    }
                }
            }

            // Player Collision check with Tile tags

        }
    }
}
