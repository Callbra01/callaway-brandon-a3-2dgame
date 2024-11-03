﻿using System;
using System.Drawing;
using System.Numerics;

namespace Game10003;

public class Player
{
    // Player variables
    string playerName = "";
    int playerClass;
    int playerSize = 50;

    // Motion variables
    public Vector2 position = new Vector2(350, 250);
    Vector2 velocity = new Vector2(0, 0);
    int playerSpeed = 500;
    int playerMaxSpeed = 1000;
    int gravityForce = 10;
    float friction = 50f;


    // Animation variables
    Texture2D[] idleFrames;
    Texture2D[] runFrames;
    int currentFrame = 0;
    float animationSpeed = 5f;
    float currentFloatFrame = 0;

    public Player()
    {
        // Initialize animation frames
        idleFrames = new Texture2D[5];
        runFrames = new Texture2D[5];

        for (int frame = 0; frame < idleFrames.Length; frame++)
        {
            //idleFrames[frame] = new Texture2D();
            idleFrames[frame] = Graphics.LoadTexture($"../../../assets/textures/playerSprites/idleFrame{frame + 1}.png");
            runFrames[frame] = Graphics.LoadTexture($"../../../assets/textures/playerSprites/runFrame{frame + 1}.png");
        }
    }

    public void Handle()
    {
        HandleInput();
        Render();
    }

    void HandleInput()
    {

        if (Input.IsKeyboardKeyDown(KeyboardInput.A))
        {
            velocity.X = -playerSpeed;

        }
        else if (Input.IsKeyboardKeyDown(KeyboardInput.D))
        {
            velocity.X = playerSpeed;
        }

        if (Input.IsKeyboardKeyPressed(KeyboardInput.Space))
        {
            velocity.Y -= playerSpeed;
        }
        //velocity.Y += gravityForce * Time.DeltaTime * 100;
        
        position += velocity * Time.DeltaTime;

        // Apply friction to x velocity to slow player down
        if (velocity.X > 0)
        {
            velocity.X -= friction;

        }
        else if (velocity.X < 0)
        {
            velocity.X += friction;
        }
    }

    // TODO: FIX COLLISION
    public void HandleCollision(Tile tile)
    {
        //Vector2 lastPosition = position;
        /*
        if (tile.position.X >= position.X && tile.position.X <= position.X + playerSize && tile.position.Y >= position.Y && tile.position.Y <= position.Y + playerSize)
        {
            velocity = new Vector2(0, 0);
        }
        else if (tile.position.X >= position.X && tile.position.X <= position.X + playerSize && tile.position.Y + tile.size >= position.Y && tile.position.Y + tile.size <= position.Y + playerSize)
        {
            velocity = new Vector2(0, 0);
        }
        else if (tile.position.X + tile.size >= position.X && tile.position.X + tile.size <= position.X + playerSize && tile.position.Y >= position.Y && tile.position.Y <= position.Y + playerSize)
        {
            velocity = new Vector2(0, 0);
        }
        else if (tile.position.X + tile.size >= position.X && tile.position.X + tile.size <= position.X + playerSize && tile.position.Y + tile.size >= position.Y && tile.position.Y + tile.size <= position.Y + playerSize)
        {
            velocity = new Vector2(0, 0);
        }
        */
        
        Vector2 tilePosition = tile.position;
        float tileSize = tile.size;

        float tileEdgeLeft = tilePosition.X;
        float tileEdgeRight = tilePosition.X + tileSize;
        float tileEdgeTop = tilePosition.Y;
        float tileEdgeBottom = tilePosition.Y + tileSize;

        float playerEdgeLeft = position.X;
        float playerEdgeRight = position.X + playerSize;
        float playerEdgeTop = position.Y;
        float playerEdgeBottom = position.Y + playerSize;

        bool doesOverlapLeft = playerEdgeLeft < tileEdgeRight;
        bool doesOverlapRight = playerEdgeRight > tileEdgeLeft;
        bool doesOverlapTop = playerEdgeTop < tileEdgeBottom;
        bool doesOverlapBottom = playerEdgeBottom > tileEdgeTop;

        bool isColliding = doesOverlapLeft && doesOverlapRight && doesOverlapTop && doesOverlapBottom && tile.canCollide;

        //if (isColliding)
        //{
        //   velocity = new Vector2(0, 0);
        //}
    }

    void Render()
    {
        // Update currentFloatFrame by delta time * scalar to allow for frame gaps
        currentFloatFrame += Time.DeltaTime * animationSpeed;

        if (currentFloatFrame < 1)
        {
            currentFrame = 0;
        }
        else if (currentFloatFrame > 1 && currentFloatFrame < 2)
        {
            currentFrame = 1;
        }
        else if (currentFloatFrame > 2 && currentFloatFrame < 3)
        {
            currentFrame = 2;
        }
        else if (currentFloatFrame > 3 && currentFloatFrame < 4)
        {
            currentFrame = 3;
        }
        else if (currentFloatFrame > 4 && currentFloatFrame < 5)
        {
            currentFrame = 4;
        }

        if (currentFloatFrame > 5 && currentFloatFrame < 6)
        {
            currentFloatFrame = 0;
        }

        //Console.WriteLine(currentFloatFrame);

        // If the player is moving, display run animation. Otherwise, display idle animation
        if (velocity.X == 0)
        {
            Graphics.Draw(idleFrames[currentFrame], position);
        }
        else
        {
            Graphics.Draw(runFrames[currentFrame], position);
        }


        //Draw.Square(position, playerSize);
    }
}
