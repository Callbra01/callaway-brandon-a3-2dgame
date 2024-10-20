using System;
using System.Numerics;
using System.Runtime.InteropServices;

namespace Game10003;

public class Tile
{
    public Vector2 position;
    public int size;
    Color color;
    Texture2D sprite;
    public bool canCollide;

    public int spriteIndex = -1;
    //public int nextspriteIndex = 0;

    
    public Tile()
    {
        position = new Vector2(0, 0);
        color = Color.Clear;
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

    public void ResetColorIndices()
    {
        this.spriteIndex = -1;
    }

    public void UpdateColorIndex(int optionalspriteIndex = -2)
    {
        if (optionalspriteIndex != -2)
        {
            spriteIndex = optionalspriteIndex;
        }
        else
        {
            spriteIndex += 1;
        }

        if (spriteIndex == -1 || optionalspriteIndex == -1)
        {
            color = Color.Clear;
            canCollide = false;
        }
        else if (spriteIndex == 0 || optionalspriteIndex == 0)
        {
            color = Color.Blue;
            sprite = Game.caveTexture;

            canCollide = true;
        }
        else if (spriteIndex == 1 || optionalspriteIndex == 1)
        {
            color = Color.Red;
            canCollide = false;
        }
        else if (spriteIndex == 2 || optionalspriteIndex == 2)
        {
            color = Color.Yellow;
            canCollide = false;
        }
        else if (spriteIndex == 3 || optionalspriteIndex == 3)
        {
            color = Color.Green;
            canCollide = false;
        }
        else if (spriteIndex > 3 || optionalspriteIndex > 3)
        {
            color = Color.Clear;
            spriteIndex = -1;
        }
    }

    // TODO SETUP COLLISION CHECK
    public void CollisionCheck(Tile tile)
    {

    }

    public void UpdateSprite(Texture2D newSprite)
    {
        sprite = newSprite;
    }

    public void Render()
    {
        if (spriteIndex == 0)
        {
            Graphics.Draw(sprite, position);
        }
        else
        {
            Draw.FillColor = color;
            Draw.Square(position, size);
        }
    }
}
