using System;
using System.Numerics;
using System.IO;

namespace Game10003;

public class LevelHandler
{
    // Read string from file path
    public int[] LoadLevel(string filePath, int[] tileArrayVar)
    {
        StreamReader streamReader = new StreamReader(filePath);

        // Split level file by '_'s to separate individual tile sprites
        string[] tempInputStringArray = streamReader.ReadToEnd().Split('_');
        int[] tempIntArray = new int[tempInputStringArray.Length];

        for (int spriteIndex = 0; spriteIndex < tempIntArray.Length; spriteIndex++)
        {
            int currentValue;
            string currentChar = tempInputStringArray[spriteIndex];
            // If parse is successful, populate tile array with the given tile sprite values
            if (int.TryParse(currentChar, out currentValue))
            {
                tempIntArray[spriteIndex] = currentValue;
            }
        }
        streamReader.Close();
        
        return tempIntArray;
    }

    // Write an array of ints to a given file path
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
