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
    public bool canCollide = false;
    public bool isEnemy = false;

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
            sprite = SceneHandler.caveTexture;

            canCollide = true;
        }
        else if (spriteIndex == 1 || optionalspriteIndex == 1)
        {
            color = Color.Red;
            isEnemy = true;
            canCollide = true;
        }
        else if (spriteIndex == 2 || optionalspriteIndex == 2)
        {
            color = Color.Yellow;
        }
        else if (spriteIndex == 3 || optionalspriteIndex == 3)
        {
            color = Color.Green;
        }
        else if (spriteIndex > 3 || optionalspriteIndex > 3)
        {
            color = Color.Clear;
            spriteIndex = -1;
        }
    }

    public void UpdateSprite(Texture2D newSprite)
    {
        sprite = newSprite;
    }

    public void Render(bool Outline)
    {
        if (spriteIndex == 0)
        {
            Graphics.Draw(sprite, position);
        }
        else if (spriteIndex == 1)
        {
            Enemy newEnemy = new Enemy("spider", position);
        }
        else
        {
            Draw.FillColor = color;
            if (!Outline)
            {
                Draw.LineColor = Color.Clear;
            }
            Draw.Square(position, size);
        }
    }
}
