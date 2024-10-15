using System;
using System.Numerics;
using System.Runtime.InteropServices;

namespace Game10003;

public class Tile
{
    public Vector2 position;
    int size;
    Color color;
    int collisionTag;

    int colorIndex = -1;
    
    public Tile()
    {
        position = new Vector2(0, 0);
        color = Color.White;
        size = 50;
    }
   
    public void SetupPosition(Vector2 newPosition)
    {
        position = newPosition;
    }

    public void UpdatePosition(Vector2 newPosition, int newSize)
    {
        position = newPosition;
        size = newSize;
    }

    public void UpdateColorIndex()
    {
        colorIndex += 1;

        if (colorIndex == -1)
        {
            color = Color.OffWhite;
        }
        else if (colorIndex == 0)
        {
            color = Color.Blue;
        }
        else if (colorIndex == 1)
        {
            color = Color.Red;
        }
        else if (colorIndex == 2)
        {
            color = Color.Yellow;
        }
        else if (colorIndex == 3)
        {
            color = Color.Green;
        }
        else if (colorIndex > 3)
        {
            color = Color.OffWhite;
            colorIndex = -1;
        }
    }
    public void UpdateColor(Color newColor)
    {
        color = newColor;
    }

    public void Render()
    {
        Draw.FillColor = color;
        Draw.Square(position, size);
    }
}
