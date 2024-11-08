using System;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Threading;

namespace Game10003;

public class Player
{
    // Player variables
    string playerName = "";
    int playerClass;
    int playerSize = 100;

    // Motion variables
    public Vector2 position = new Vector2(400, 250);
    Vector2 velocity = new Vector2(0, 0);
    int playerSpeed = 500;
    int playerMaxSpeed = 1000;
    int gravityForce = 10;
    float friction = 50f;
    bool canPlayerLeft = true;
    bool canPlayerRight = true;
    float collisionOffset = 5;
    float jumpForce = 500;


    // Animation variables
    Texture2D[] idleFrames;
    Texture2D[] runRightFrames;
    Texture2D[] runLeftFrames;
    Texture2D[] danceFrames;
    Texture2D[] jumpFrames;
    int currentFrame = 0;
    float animationSpeed = 10f;
    float currentFloatFrame = 0;
    bool isPlayerDancing = false;
    bool isPlayerJumping = false;

    public Player()
    {
        // Declare animation frame arrays
        idleFrames = new Texture2D[5];
        runRightFrames = new Texture2D[5];
        runLeftFrames = new Texture2D[5];
        danceFrames = new Texture2D[5];
        jumpFrames = new Texture2D[5];

        // Load individual frames into their respective arrays
        for (int frame = 0; frame < idleFrames.Length; frame++)
        {
            idleFrames[frame] = Graphics.LoadTexture($"../../../assets/textures/playerSprites/idle/idleFrame{frame + 1}.png");
            runRightFrames[frame] = Graphics.LoadTexture($"../../../assets/textures/playerSprites/runRight/runFrame{frame + 1}.png");
            runLeftFrames[frame] = Graphics.LoadTexture($"../../../assets/textures/playerSprites/runLeft/runFrame{frame + 1}.png");
            danceFrames[frame] = Graphics.LoadTexture($"../../../assets/textures/playerSprites/dance/danceFrame{frame + 1}.png");
            jumpFrames[frame] = Graphics.LoadTexture($"../../../assets/textures/playerSprites/jump/jumpFrame{frame + 1}.png");
        }
    }

    public void Handle()
    {
        HandleInput();
        Render();
    }

    void HandleInput()
    {

        if (Input.IsKeyboardKeyDown(KeyboardInput.F))
        {
            isPlayerDancing = true;
        }
        else
        {
            isPlayerDancing = false;
        }

        if (Input.IsKeyboardKeyDown(KeyboardInput.A) && canPlayerLeft)
        {
            velocity.X = -playerSpeed;

        }
        else if (Input.IsKeyboardKeyDown(KeyboardInput.D) && canPlayerRight)
        {
            velocity.X = playerSpeed;
        }

        // If player can jump, allow jump. If player is already jumping, dont allow another jump
        if (Input.IsKeyboardKeyPressed(KeyboardInput.Space) && !isPlayerJumping)
        {
            velocity.Y -= velocity.Y + jumpForce;
            currentFloatFrame = 0;
            isPlayerJumping = true;
        }

        velocity.Y += gravityForce * Time.DeltaTime * 100;
        
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

    // Handle player collision via ch
    public void HandleCollision(Tile tile)
    {
        Vector2 playerTop = new Vector2(position.X + (playerSize / 2), position.Y - collisionOffset);
        Vector2 playerBottom = new Vector2(position.X + (playerSize / 2), position.Y + playerSize - collisionOffset);
        Vector2 playerLeft = new Vector2(position.X, position.Y + (playerSize / 2) - 10);
        Vector2 playerRight = new Vector2(position.X + playerSize, position.Y + (playerSize / 2) - collisionOffset);
        
        Vector2 tileTopLeft = new Vector2(tile.position.X - (collisionOffset / 2), tile.position.Y - (collisionOffset/2));
        Vector2 tileTopRight = new Vector2(tile.position.X + tile.size + (collisionOffset / 2), tile.position.Y - (collisionOffset / 2));
        Vector2 tileBottomLeft = new Vector2(tile.position.X - (collisionOffset / 2), tile.position.Y + tile.size + (collisionOffset / 2));
        Vector2 tileBottomRight = new Vector2(tile.position.X + tile.size + (collisionOffset / 2), tile.position.Y + tile.size + (collisionOffset / 2));

        // Handle player left collision
        if (playerLeft.X < tileTopRight.X && playerLeft.X > tileTopLeft.X)
        {
            if (playerLeft.Y < tileBottomRight.Y && playerLeft.Y > tileTopRight.Y)
            {
                velocity.X = 0;
                position.X = tileTopRight.X;
                Console.WriteLine("CHECK");
            }
        }

        // Handle player right collision
        if (playerRight.X > tileTopLeft.X && playerRight.X < tileTopRight.X)
        {
            if (playerRight.Y < tileBottomLeft.Y && playerRight.Y > tileTopLeft.Y)
            {
                velocity.X = 0;
                position.X = tileTopLeft.X - playerSize;
                Console.WriteLine("CHECK");
            }
        }

        // Handle player ceiling collision
        if (playerTop.Y < tileBottomLeft.Y && playerTop.Y > tileTopLeft.Y)
        {
            if (playerTop.X > tileTopLeft.X && playerTop.X < tileTopRight.X)
            {
                velocity.Y = 0;
                position.Y = tileBottomLeft.Y + 5;
            }
        }

        // Handle player floor collision
        if (playerBottom.Y > tileTopLeft.Y && playerBottom.Y < tileBottomLeft.Y)
        {
            if (playerBottom.X > tileTopLeft.X && playerBottom.X < tileTopRight.X)
            {
                velocity.Y = 0;
                position.Y = tileTopLeft.Y - playerSize + 5;
            }
        }

    }

    // Render and Handle all animation frames
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
        if (velocity.X == 0 && !isPlayerDancing && !isPlayerJumping)
        {
            Graphics.Draw(idleFrames[currentFrame], position);
        }
        else if (velocity.X > 0 && !isPlayerDancing && !isPlayerJumping)
        {
            Graphics.Draw(runRightFrames[currentFrame], position);
        }
        else if (velocity.X < 0 && !isPlayerDancing && !isPlayerJumping)
        {
            Graphics.Draw(runLeftFrames[currentFrame], position);
        }


        if (isPlayerJumping && !isPlayerDancing)
        {
            Graphics.Draw(jumpFrames[currentFrame], position);
            if (currentFrame == 4)
            {
                isPlayerJumping = false;
            }
        }

        if (isPlayerDancing && !isPlayerJumping)
        {
            Graphics.Draw(danceFrames[currentFrame], position);
        }
    }
}
