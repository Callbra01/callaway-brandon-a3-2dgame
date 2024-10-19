using System;
using System.Numerics;
using System.IO;

namespace Game10003;

public class LevelHandler
{
    // TODO: CHECK WITH RAPH IF THIS IS OKAY
    public int[] LoadLevel(string filePath, int[] tileArrayVar)
    {
        // Read string from file path
        StreamReader streamReader = new StreamReader(filePath);

        string[] tempStringArray = streamReader.ReadToEnd().Split('_');

        int[] tempIntArray = new int[tempStringArray.Length];

        for (int colorIndex = 0; colorIndex < tempIntArray.Length; colorIndex++)
        {
            int currentValue;
            string currentChar = tempStringArray[colorIndex];
            if (int.TryParse(currentChar, out currentValue))
            {
                tempIntArray[colorIndex] = currentValue;
            }
        }
        streamReader.Close();
        
        return tempIntArray;
    }

    // 
    public void SaveLevel(string levelFilePath, Tile[] tileArrayVar)
    {
        StreamWriter streamWriter = new StreamWriter(levelFilePath);
        
        for (int tile = 0; tile < tileArrayVar.Length; tile++)
        {
            streamWriter.Write($"{tileArrayVar[tile].colorIndex}{'_'}");
        }
        streamWriter.Close();
    }
}
