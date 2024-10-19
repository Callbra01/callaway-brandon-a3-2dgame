using System;
using System.Numerics;
using System.Runtime.InteropServices;

namespace Game10003;

public class Tile
{
    public Vector2 position;
    public int size;
    Color color;
    public bool canCollide;

    public int colorIndex = -1;
    //public int nextColorIndex = 0;
    
    public Tile()
    {
        position = new Vector2(0, 0);
        color = Color.White;
        size = 50;
    }
   
    public void SetupPosition(Vector2 newPosition)
    {
        position = newPosition;
        //nextColorIndex = colorIndex + 1;
    }

    public void UpdatePosition(Vector2 newPosition, int newSize)
    {
        position = newPosition;
        size = newSize;
    }

    public void ResetColorIndices()
    {
        this.colorIndex = -1;
    }

    public void UpdateColorIndex(int optionalColorIndex = -2)
    {
        if (optionalColorIndex != -2)
        {
            colorIndex = optionalColorIndex;
        }
        else
        {
            colorIndex += 1;
        }

        if (colorIndex == -1 || optionalColorIndex == -1)
        {
            color = Color.OffWhite;
            canCollide = false;
        }
        else if (colorIndex == 0 || optionalColorIndex == 0)
        {
            color = Color.Blue;
            canCollide = true;
        }
        else if (colorIndex == 1 || optionalColorIndex == 1)
        {
            color = Color.Red;
            canCollide = false;
        }
        else if (colorIndex == 2 || optionalColorIndex == 2)
        {
            color = Color.Yellow;
            canCollide = false;
        }
        else if (colorIndex == 3 || optionalColorIndex == 3)
        {
            color = Color.Green;
            canCollide = false;
        }
        else if (colorIndex > 3 || optionalColorIndex > 3)
        {
            color = Color.OffWhite;
            colorIndex = -1;
        }
    }

    // TODO SETUP COLLISION CHECK
    public void CollisionCheck(Tile tile)
    {

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
