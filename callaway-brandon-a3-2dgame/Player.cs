﻿using System;
using System.ComponentModel.Design;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Threading;

namespace Game10003;

public class Player
{
    // Player variables
    public int playerSpeed = 500;
    public Vector2 position = new Vector2(400, 250);
    bool isGrounded = false;
    int playerMaxSpeed = 1000;
    int playerSize = 100;
    int gravityForce = 18;
    float friction = 50f;
    float collisionOffset = 5;
    float jumpForce = 6f;
    Vector2 velocity = new Vector2(0, 0);
    Vector2 playerTop;
    Vector2 playerBottom;
    Vector2 playerLeft;
    Vector2 playerRight;

    // Level variables
    public bool playerHasExited = false;
    public int playerScore = 0;
    bool isPlayerInBounds = true;
    Vector2[] playerBounds = [new Vector2(0, 0), new Vector2(530, 0)];


    // Animation variables
    public bool canPlayerMove = true;
    public bool isPlayerDancing = false;
    bool isPlayerJumping = false;
    int currentFrame = 0;
    float animationSpeed = 11.85f;
    float currentFloatFrame = 0;
    Texture2D[] idleFrames;
    Texture2D[] runRightFrames;
    Texture2D[] runLeftFrames;
    Texture2D[] danceFrames;
    Texture2D[] jumpFrames;

    // Audio variables
    Sound caveFootstep;
    public Sound grindTrack;
    Sound speedBoostSound;
    Sound jumpBoostSound;

    public Player(Vector2 newPosition)
    {

        position = newPosition;

        InitializeAudio();

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

    void InitializeAudio()
    {
        // Initialize audio variables
        caveFootstep = Audio.LoadSound("../../../assets/audio/caveFootstep.wav");
        Audio.SetVolume(caveFootstep, 0.3f);

        grindTrack = Audio.LoadSound("../../../assets/audio/music/grindTrack.wav");
        Audio.SetVolume(grindTrack, 0.5f);

        jumpBoostSound = Audio.LoadSound("../../../assets/audio/JumpUpCollected.wav");
        Audio.SetVolume(jumpBoostSound, 0.5f);

        speedBoostSound = Audio.LoadSound("../../../assets/audio/powerUpCollected.wav");
        Audio.SetVolume(speedBoostSound, 0.5f);
    }

    public void Handle(Tile[] tileArray, int sceneCount)
    {
        MoveLevel(tileArray);
        HandleInput();
        Render();
    }

    void MoveLevel(Tile[] tileArray)
    {
        Vector2 LeftBoundTopleft = new Vector2(playerBounds[0].X - (collisionOffset / 2), playerBounds[0].Y - (collisionOffset / 2));
        Vector2 LeftBoundTopRight = new Vector2(playerBounds[0].X + 50 + (collisionOffset / 2), playerBounds[0].Y - (collisionOffset / 2));
        Vector2 LeftBoundBottomleft = new Vector2(playerBounds[0].X - (collisionOffset / 2), playerBounds[0].Y + Window.Height + (collisionOffset / 2));
        Vector2 LeftBoundBottomRight = new Vector2(playerBounds[0].X + 50 + (collisionOffset / 2), playerBounds[0].Y + Window.Height + (collisionOffset / 2));

        Vector2 RightBoundTopleft = new Vector2(playerBounds[1].X - (collisionOffset / 2), playerBounds[1].Y - (collisionOffset / 2));
        Vector2 RightBoundTopRight = new Vector2(playerBounds[1].X + 265 + (collisionOffset / 2), playerBounds[1].Y - (collisionOffset / 2));
        Vector2 RightBoundBottomleft = new Vector2(playerBounds[1].X - (collisionOffset / 2), playerBounds[1].Y + Window.Height + (collisionOffset / 2));
        Vector2 RightBoundBottomRight = new Vector2(playerBounds[1].X + 265 + (collisionOffset / 2), playerBounds[1].Y + Window.Height + (collisionOffset / 2));

        // Prevent player from scrolling backwards
        if (playerLeft.X < LeftBoundTopRight.X && playerLeft.X > LeftBoundTopleft.X)
        {
            if (playerLeft.Y < LeftBoundBottomRight.Y && playerLeft.Y > LeftBoundTopRight.Y)
            {
                velocity.X = 0;
                position.X = LeftBoundTopRight.X;
            }
        }
        // Move level on X position when player hits right boundary
        else if (playerRight.X > RightBoundTopleft.X && playerRight.X < RightBoundTopRight.X)
        {
            if (playerRight.Y < RightBoundBottomleft.Y && playerRight.Y > RightBoundTopleft.Y)
            {
                velocity.X = 0;
                position.X = RightBoundTopleft.X - playerSize;
                for (int i = 0; i < tileArray.Length; i++)
                {
                    tileArray[i].position.X -= playerSpeed / 50;
                }
            }
        }
    }

    void HandleInput()
    {
        if (Input.IsKeyboardKeyDown(KeyboardInput.F))
        {
            isPlayerDancing = true;

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
        }

        if (Input.IsKeyboardKeyDown(KeyboardInput.A) && canPlayerMove)
        {
            velocity.X = -playerSpeed;
            if (!Audio.IsPlaying(caveFootstep) && isGrounded && canPlayerMove)
            {
                Audio.Play(caveFootstep);
            }
        }
        else if (Input.IsKeyboardKeyDown(KeyboardInput.D) && canPlayerMove)
        {
            velocity.X = playerSpeed;
            if (!Audio.IsPlaying(caveFootstep) && isGrounded)
            {
                Audio.Play(caveFootstep);
            }
        }
        else
        {
            if (velocity.X == 0 || !isGrounded)
            {
                Audio.Stop(caveFootstep);
            }
        }

        // If player can jump, allow jump. If player is already jumping, dont allow another jump
        if (Input.IsKeyboardKeyPressed(KeyboardInput.Space) && !isPlayerJumping && isGrounded && canPlayerMove)
        {
            currentFloatFrame = 0;
            Audio.Stop(caveFootstep);
            velocity.Y -= velocity.Y + (jumpForce * 100);
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
        // Get mid point of player edges
        playerTop = new Vector2(position.X + (playerSize / 2), position.Y - collisionOffset);
        playerBottom = new Vector2(position.X + (playerSize / 2), position.Y + playerSize - collisionOffset);
        playerLeft = new Vector2(position.X, position.Y + (playerSize / 2) - collisionOffset);
        playerRight = new Vector2(position.X + playerSize, position.Y + (playerSize / 2) - collisionOffset);

        // Get corners of given tile
        Vector2 tileTopLeft = new Vector2(tile.position.X - (collisionOffset / 2), tile.position.Y - (collisionOffset / 2));
        Vector2 tileTopRight = new Vector2(tile.position.X + tile.size + (collisionOffset / 2), tile.position.Y - (collisionOffset / 2));
        Vector2 tileBottomLeft = new Vector2(tile.position.X - (collisionOffset / 2), tile.position.Y + tile.size + (collisionOffset / 2));
        Vector2 tileBottomRight = new Vector2(tile.position.X + tile.size + (collisionOffset / 2), tile.position.Y + tile.size + (collisionOffset / 2));

        // Handle player left and right collision
        if (playerLeft.X < tileTopRight.X && playerLeft.X > tileTopLeft.X)
        {
            if (playerLeft.Y < tileBottomRight.Y && playerLeft.Y > tileTopRight.Y)
            {
                // Handle Collision for bricks and power ups
                if (tile.spriteIndex == 0)
                {
                    velocity.X = 0;
                    position.X = tileTopRight.X;
                }
                // Jump powerup
                else if (tile.spriteIndex == 2 && tile.isPowerUpActive)
                {
                    jumpForce += 1f;
                    if (!Audio.IsPlaying(speedBoostSound))
                    {
                        Audio.Play(speedBoostSound);
                    }
                    tile.isPowerUpActive = false;
                }
                // Speed powerup
                else if (tile.spriteIndex == 3 && tile.isPowerUpActive)
                {
                    playerSpeed += 50;
                    if (!Audio.IsPlaying(speedBoostSound))
                    {
                        Audio.Play(speedBoostSound);
                    }
                    tile.isPowerUpActive = false;
                }
                // Level Exit
                else if (tile.spriteIndex == 1)
                {
                    playerHasExited = true;
                }
            }
        }
        else if (playerRight.X > tileTopLeft.X && playerRight.X < tileTopRight.X)
        {
            if (playerRight.Y < tileBottomLeft.Y && playerRight.Y > tileTopLeft.Y)
            {

                if (tile.spriteIndex == 0)
                {
                    velocity.X = 0;
                    position.X = tileTopLeft.X - playerSize;
                }
                else if (tile.spriteIndex == 2 && tile.isPowerUpActive)
                {
                    jumpForce += 1f;
                    if (!Audio.IsPlaying(jumpBoostSound))
                    {
                        Audio.Play(jumpBoostSound);
                    }
                    tile.isPowerUpActive = false;
                }
                else if (tile.spriteIndex == 3 && tile.isPowerUpActive)
                {
                    playerSpeed += 50;
                    if (!Audio.IsPlaying(speedBoostSound))
                    {
                        Audio.Play(speedBoostSound);
                    }
                    tile.isPowerUpActive = false;
                }
            }
        }

        // Handle player ceiling collision
        if (playerTop.Y < tileBottomLeft.Y && playerTop.Y > tileTopLeft.Y)
        {
            if (playerTop.X > tileTopLeft.X && playerTop.X < tileTopRight.X)
            {
                if (tile.spriteIndex == 0)
                {
                    velocity.Y = 0;
                    position.Y = tileBottomLeft.Y + 5;
                }
                if (tile.spriteIndex == 4)
                {
                    position.Y = 850;
                }
            }
        }

        // Handle player floor collision
        else if (playerBottom.Y > tileTopLeft.Y && playerBottom.Y < tileBottomLeft.Y)
        {
            if (playerBottom.X > tileTopLeft.X && playerBottom.X < tileTopRight.X)
            {
                if (tile.spriteIndex == 0)
                {
                    velocity.Y = 0;
                    isGrounded = true;
                    position.Y = tileTopLeft.Y - playerSize + 5;
                }
                if (tile.spriteIndex == 5)
                {
                    position.Y = 850;
                }
            }
        }
    }

    void UpdateFrames()
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
    }

    void Render()
    {
        // Update frame counter every second * animation speed scalar
        UpdateFrames();

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
