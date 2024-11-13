using System;
using System.ComponentModel.Design;
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
    int gravityForce = 18;
    float friction = 50f;
    bool canPlayerLeft = true;
    bool canPlayerRight = true;
    bool isGrounded = false;
    float collisionOffset = 5;
    float jumpForce = 6f;
    Vector2 playerTop;
    Vector2 playerBottom;
    Vector2 playerLeft;
    Vector2 playerRight;

    // Level variables
    Vector2[] playerBounds = [new Vector2(0, 0), new Vector2(530, 0), new Vector2(265, 0), new Vector2(265, 400)];
    bool isPlayerInBounds = true;


    // Animation variables
    Texture2D[] idleFrames;
    Texture2D[] runRightFrames;
    Texture2D[] runLeftFrames;
    Texture2D[] danceFrames;
    Texture2D[] jumpFrames;
    int currentFrame = 0;
    float animationSpeed = 20f;
    float currentFloatFrame = 0;
    bool isPlayerDancing = false;
    bool isPlayerJumping = false;

    // Audio variables
    Sound caveFootstep;
    Sound grindTrack;
    // Music variables
    

    public Player()
    {
        // Initialize audio variables
        caveFootstep = Audio.LoadSound("../../../assets/audio/caveFootstep.wav");
        Audio.SetVolume(caveFootstep, 0.1f);

        grindTrack = Audio.LoadSound("../../../assets/audio/music/grindTrack.wav");
        Audio.SetVolume(grindTrack, 0.5f);

        // Initialize animation frame arrays
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

    public void Handle(Tile[] tileArray)
    {
        MoveLevel(tileArray);
        HandleInput();
        Render();
    }

    void MoveLevel(Tile[] tileArray)
    {
        Vector2 LeftBoundTopleft = new Vector2(playerBounds[0].X - (collisionOffset / 2), playerBounds[0].Y - (collisionOffset / 2));
        Vector2 LeftBoundTopRight = new Vector2(playerBounds[0].X + 265 + (collisionOffset / 2), playerBounds[0].Y - (collisionOffset / 2));
        Vector2 LeftBoundBottomleft = new Vector2(playerBounds[0].X - (collisionOffset / 2), playerBounds[0].Y + Window.Height + (collisionOffset / 2));
        Vector2 LeftBoundBottomRight = new Vector2(playerBounds[0].X + 265 + (collisionOffset / 2), playerBounds[0].Y + Window.Height + (collisionOffset / 2));

        Vector2 RightBoundTopleft = new Vector2(playerBounds[1].X - (collisionOffset / 2), playerBounds[1].Y - (collisionOffset / 2));
        Vector2 RightBoundTopRight = new Vector2(playerBounds[1].X + 265 + (collisionOffset / 2), playerBounds[1].Y - (collisionOffset / 2));
        Vector2 RightBoundBottomleft = new Vector2(playerBounds[1].X - (collisionOffset / 2), playerBounds[1].Y + Window.Height + (collisionOffset / 2));
        Vector2 RightBoundBottomRight = new Vector2(playerBounds[1].X + 265 + (collisionOffset / 2), playerBounds[1].Y + Window.Height + (collisionOffset / 2));

        if (playerLeft.X < LeftBoundTopRight.X && playerLeft.X > LeftBoundTopleft.X)
        {
            if (playerLeft.Y < LeftBoundBottomRight.Y && playerLeft.Y > LeftBoundTopRight.Y)
            {
                velocity.X = 0;
                position.X = LeftBoundTopRight.X;
                for (int i = 0; i < tileArray.Length; i++)
                {
                    tileArray[i].position.X += 10;
                }
            }
        }
        else if (playerRight.X > RightBoundTopleft.X && playerRight.X < RightBoundTopRight.X)
        {
            if (playerRight.Y < RightBoundBottomleft.Y && playerRight.Y > RightBoundTopleft.Y)
            {
                velocity.X = 0;
                position.X = RightBoundTopleft.X - playerSize;
                for (int i = 0; i < tileArray.Length; i++)
                {
                    tileArray[i].position.X -= 10;
                    
                }
            }
        }

    }

    void HandleInput()
    {
        if (Input.IsKeyboardKeyDown(KeyboardInput.F))
        {
            isPlayerDancing = true;
            canPlayerLeft = false;
            canPlayerRight = false;

            // Reset the audio
            if (!Audio.IsPlaying(grindTrack))
            {
                Audio.Play(grindTrack);
            }
        }
        else
        {
            Audio.Stop(grindTrack);
            isPlayerDancing = false;
            canPlayerLeft = true;
            canPlayerRight = true;
        }

        if (Input.IsKeyboardKeyDown(KeyboardInput.A) && canPlayerLeft)
        {
            velocity.X = -playerSpeed;
            if (!Audio.IsPlaying(caveFootstep))
            {
                Audio.Play(caveFootstep);
            }
        }
        else if (Input.IsKeyboardKeyDown(KeyboardInput.D) && canPlayerRight)
        {
            velocity.X = playerSpeed;
            if (!Audio.IsPlaying(caveFootstep))
            {
                Audio.Play(caveFootstep);
            }
        }
        else
        {
            if (velocity.X == 0)
            {
                Audio.Stop(caveFootstep);
            }
        }

        // If player can jump, allow jump. If player is already jumping, dont allow another jump
        if (Input.IsKeyboardKeyPressed(KeyboardInput.Space) && !isPlayerJumping && isGrounded)
        {
            velocity.Y -= velocity.Y + (jumpForce * 100);
            currentFloatFrame = 0;
            Audio.Stop(caveFootstep);
            isPlayerJumping = true;
            isGrounded = false;
        }

        // Apply gravity to the players velocity, and apply velocity to the players position.
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

    public void HandleCollision(Tile tile)
    {
        playerTop = new Vector2(position.X + (playerSize / 2), position.Y - collisionOffset);
        playerBottom = new Vector2(position.X + (playerSize / 2), position.Y + playerSize - collisionOffset);
        playerLeft = new Vector2(position.X, position.Y + (playerSize / 2) - collisionOffset);
        playerRight = new Vector2(position.X + playerSize, position.Y + (playerSize / 2) - collisionOffset);

        Vector2 tileTopLeft = new Vector2(tile.position.X - (collisionOffset / 2), tile.position.Y - (collisionOffset / 2));
        Vector2 tileTopRight = new Vector2(tile.position.X + tile.size + (collisionOffset / 2), tile.position.Y - (collisionOffset / 2));
        Vector2 tileBottomLeft = new Vector2(tile.position.X - (collisionOffset / 2), tile.position.Y + tile.size + (collisionOffset / 2));
        Vector2 tileBottomRight = new Vector2(tile.position.X + tile.size + (collisionOffset / 2), tile.position.Y + tile.size + (collisionOffset / 2));

        /*
        Draw.FillColor = Color.Red;
        Draw.Circle(tileTopLeft, 10);
        Draw.Circle(tileTopRight, 10);
        Draw.Circle(tileBottomLeft, 10);
        Draw.Circle(tileBottomRight, 10);

        Draw.Circle(playerTop, 10);
        Draw.Circle(playerBottom, 10);
        Draw.Circle(playerLeft, 10);
        Draw.Circle(playerRight, 10);
        */

        // Handle player left and right collision
        if (playerLeft.X < tileTopRight.X && playerLeft.X > tileTopLeft.X)
        {
            if (playerLeft.Y < tileBottomRight.Y && playerLeft.Y > tileTopRight.Y)
            {
                velocity.X = 0;
                position.X = tileTopRight.X;
            }
        }
        else if (playerRight.X > tileTopLeft.X && playerRight.X < tileTopRight.X)
        {
            if (playerRight.Y < tileBottomLeft.Y && playerRight.Y > tileTopLeft.Y)
            {
                velocity.X = 0;
                position.X = tileTopLeft.X - playerSize;
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
        else if (playerBottom.Y > tileTopLeft.Y && playerBottom.Y < tileBottomLeft.Y)
        {
            if (playerBottom.X > tileTopLeft.X && playerBottom.X < tileTopRight.X)
            {
                velocity.Y = 0;
                isGrounded = true;
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
        /*
        //DEBUG BOUND VIEW
        Draw.FillColor = new Color(255, 255, 255, 50);
        Draw.Rectangle(playerBounds[0][0], playerBounds[0][1], 265, Window.Height);
        Draw.Rectangle(playerBounds[1][0], playerBounds[1][1], 265, Window.Height);
        */
    }
}
