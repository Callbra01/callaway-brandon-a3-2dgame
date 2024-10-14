using System;
using System.Numerics;
using System.Runtime.InteropServices;

namespace Game10003;

public class Tile
{
    public int x;
    public int y;
    int size;

    int collisionTag;
    
    public Tile(int xCoord, int yCoord, int tileSize)
    {
        x = xCoord;
        y = yCoord;
        size = tileSize;

    }

    void Setup()
    {
        
    }

    public void Render()
    {
        Draw.FillColor = Color.White;
        Draw.Square(x, y, size);
    }
}
