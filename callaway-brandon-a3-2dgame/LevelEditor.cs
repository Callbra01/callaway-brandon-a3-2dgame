using System;
using System.IO;
using System.Numerics;

namespace Game10003;

public class LevelEditor
{
    public int tileSize = 50;
    public int tileRowCount;
    public int tileColCount;
    public int levelSize = 3;
    public string levelName = "levelEditor";

    public Tile[] tileArray;
    public Vector2[] tilePositions;
    bool mouseIntersectsX = false;
    bool mouseIntersectsY = false;

    int[] tileColorIndexVals;

    public void Setup()
    {
        // Populate tile array with a given amount of tiles
        tileRowCount = Game.windowHeight / tileSize;
        tileColCount = (Game.windowWidth / tileSize) * levelSize;
        tileArray = new Tile[tileRowCount * tileColCount];
        tilePositions = new Vector2[tileArray.Length];

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

    public void UpdateLevel()
    {
        if (levelName == "levelEditor")
        {

        }
    }

    public void Update()
    {

        UpdateLevel();
        Render();
        MouseCollisionCheck();
        HandleEditorInput();
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
    void HandleEditorInput()
    {
        if(Input.IsKeyboardKeyPressed(KeyboardInput.Q))
        {
            SaveLevel("../../../assets/levels/mainLevel0.txt", tileArray);
        }
        else if (Input.IsKeyboardKeyPressed(KeyboardInput.E))
        {
            tileColorIndexVals = new int[tileArray.Length];

            tileColorIndexVals = LoadLevel("../../../assets/levels/mainLevel0.txt", tileColorIndexVals);

            for (int i = 0; i < tileArray.Length; i++)
            {
                tileArray[i].UpdateColorIndex(tileColorIndexVals[i]);
            }
        }

        ///* DEBUG INPUT
        if (Input.IsKeyboardKeyDown(KeyboardInput.A))
        {
            for (int tile = 0; tile < tileArray.Length; tile++)
            {
                tileArray[tile].position.X -= 1000 * Time.DeltaTime;
            }
        }
        else if (Input.IsKeyboardKeyDown(KeyboardInput.D))
        {
            for (int tile = 0; tile < tileArray.Length; tile++)
            {
                tileArray[tile].position.X += 1000 * Time.DeltaTime;
            }
        }
        //*/

    }

    void MouseCollisionCheck()
    {
        for (int tile = 0; tile < tileArray.Length; tile++)
        {
            int currentColorIndex;

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

    public int[] LoadLevel(string filePath, int[] tileArrayVar)
    {
        // Read string from file path
        StreamReader streamReader = new StreamReader(filePath);

        string[] tempStringArray = streamReader.ReadToEnd().Split('_');

        int[] tempIntArray = new int[tempStringArray.Length];

        for (int spriteIndex = 0; spriteIndex < tempIntArray.Length; spriteIndex++)
        {
            int currentValue;
            string currentChar = tempStringArray[spriteIndex];
            if (int.TryParse(currentChar, out currentValue))
            {
                tempIntArray[spriteIndex] = currentValue;
            }
        }
        streamReader.Close();

        return tempIntArray;
    }

    public void SaveLevel(string levelFilePath, Tile[] tileArrayVar)
    {
        // Write int array to string
        StreamWriter streamWriter = new StreamWriter(levelFilePath);

        for (int tile = 0; tile < tileArrayVar.Length; tile++)
        {
            streamWriter.Write($"{tileArrayVar[tile].spriteIndex}{'_'}");
        }
        streamWriter.Close();
    }
}
