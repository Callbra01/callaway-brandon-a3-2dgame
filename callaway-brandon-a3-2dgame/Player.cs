using System;
using System.Drawing;
using System.Numerics;

namespace Game10003;

public class Player
{
    public Vector2 position = new Vector2(350, 250);
    Vector2 velocity = new Vector2(0, 0);
    int playerSize = 50;
    int playerSpeed = 50;
    int gravityForce = 10;

    Texture2D texture;

    public Player()
    {
        texture = Graphics.LoadTexture("../../../assets/wretchTexture.png");
    }
    public void Update()
    {
        HandleInput();
        Render();
    }

    void HandleInput()
    {
        if (Input.IsKeyboardKeyDown(KeyboardInput.A))
        {
            velocity.X -= playerSpeed;
        }
        else if (Input.IsKeyboardKeyDown(KeyboardInput.D))
        {
            velocity.X += playerSpeed;
        }

        if (Input.IsKeyboardKeyPressed(KeyboardInput.Space))
        {
            velocity.Y -= playerSpeed;
        }
        //velocity.Y += gravityForce * Time.DeltaTime * 100;
        position += velocity * Time.DeltaTime;
    }

    public void HandleCollision(Tile tile)
    {
        if (tile.position.X >= position.X && tile.position.X <= position.X + playerSize && tile.position.Y >= position.Y && tile.position.Y <= position.Y + playerSize)
        {
            velocity = -velocity;
        }
        else if (tile.position.X >= position.X && tile.position.X <= position.X + playerSize && tile.position.Y + tile.size >= position.Y && tile.position.Y + tile.size <= position.Y + playerSize)
        {
            velocity = -velocity;
        }
        else if (tile.position.X + tile.size >= position.X && tile.position.X + tile.size <= position.X + playerSize && tile.position.Y >= position.Y && tile.position.Y <= position.Y + playerSize)
        {
            velocity = -velocity;
        }
        else if (tile.position.X + tile.size >= position.X && tile.position.X + tile.size <= position.X + playerSize && tile.position.Y + tile.size >= position.Y && tile.position.Y + tile.size <= position.Y + playerSize)
        {
            velocity = -velocity;
        }
    }

    void Render()
    {
        Draw.FillColor = Color.White;
        Draw.Square(position, playerSize);
    }
}
