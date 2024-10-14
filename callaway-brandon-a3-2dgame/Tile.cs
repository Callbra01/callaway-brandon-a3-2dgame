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
