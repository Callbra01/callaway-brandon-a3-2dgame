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
    public bool isPowerUpActive = false;

    public int spriteIndex = -1;
    //public int nextspriteIndex = 0;

    
    public Tile()
    {
        position = new Vector2(0, 0);
        color = Color.Clear;
        size = 50;
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

        // Air tile
        if (spriteIndex == -1 || optionalspriteIndex == -1)
        {
            color = Color.Clear;
            canCollide = false;
        }
        // Brick tile
        else if (spriteIndex == 0 || optionalspriteIndex == 0)
        {
            sprite = SceneHandler.caveTexture;

            canCollide = true;
        }
        // Exit tile
        else if (spriteIndex == 1 || optionalspriteIndex == 1)
        {
            color = Color.Red;
            canCollide = true;
        }
        // Jump boost tile
        else if (spriteIndex == 2 || optionalspriteIndex == 2)
        {
            isPowerUpActive = true;
            sprite = SceneHandler.jumpBoostTexture;
        }
        // Speed boost tile
        else if (spriteIndex == 3 || optionalspriteIndex == 3)
        {
            isPowerUpActive = true;
            sprite = SceneHandler.speedBoostTexture;
        }
        // Spike tile
        else if (spriteIndex == 4 || optionalspriteIndex == 4)
        {
            sprite = SceneHandler.topSpikeTexture;
            position.Y -= 15;
        }
        // Bottom Spike tile
        else if (spriteIndex == 5 || optionalspriteIndex == 5)
        {
            sprite = SceneHandler.bottomSpikeTexture;
            position.Y += 15;
        }
        else if (spriteIndex > 5 || optionalspriteIndex > 5)
        {
            color = Color.Clear;
            spriteIndex = -1;
        }
    }

    public void Render(bool Outline)
    {
        if (spriteIndex == 0 || spriteIndex == 4 || spriteIndex == 5)
        {
            Graphics.Draw(sprite, position);
        }
        else if (spriteIndex == 3 || spriteIndex == 2)
        {
            if (isPowerUpActive)
            {
                Graphics.Draw(sprite, position);
            }
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
