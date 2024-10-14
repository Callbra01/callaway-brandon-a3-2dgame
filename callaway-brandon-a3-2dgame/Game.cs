// Include code libraries you need below (use the namespace).
using System;
using System.Numerics;

// The namespace your code is in.
namespace Game10003
{
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


        /// <summary>
        ///     Setup runs once before the game loop begins.
        /// </summary>
        public void Setup()
        {
            Window.SetSize(windowWidth, windowHeight);
            tileRowCount = windowHeight / tileSize;
            tileColCount = windowWidth / tileSize;


            tileArray = new Tile[tileRowCount * tileColCount];



           // 
            

        }

        /// <summary>
        ///     Update runs every frame.
        /// </summary>
        public void Update()
        {
            Window.ClearBackground(Color.Black);
            /*
            for (int i = 0; i < tileArray.Length; i++)
            {
                tileArray[i].Render();
                
            }
            */

            // Loop through every index in the tile array
            for (int i = 0; i < tileArray.Length; i++)
            {
                // for every row
                for (int row = 0; row < tileRowCount; row++)
                {
                    // for every column
                    for (int col = 0; col < tileColCount; col++)
                    {
                        //Populate a given index 
                        //tileArray[i] = new Tile(col * tileSize, row * tileSize, tileSize);
                        Draw.FillColor = Color.White;
                        Draw.Square(col * tileSize, row * tileSize, tileSize);
                    }
                }
            }

        }
    }
}
