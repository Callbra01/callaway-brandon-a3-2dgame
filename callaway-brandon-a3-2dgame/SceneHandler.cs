﻿using Raylib_cs;
using System;
using System.Linq;
using System.Numerics;

namespace Game10003;

/*
 * All levels and scenes within the game should be added and modified within this class.
 */
public class SceneHandler
{
    public int currentScene = 0;

    // Splash screen variables
    static Texture2D avarusSplashTexture;
    Color blackSplashColor = new Color(0, 0, 0, 254);
    Sound splashSound;

    static Texture2D backgroundTexture;
    public static Texture2D caveTexture;
    public static Texture2D speedBoostTexture;
    public static Texture2D jumpBoostTexture;
    public static Texture2D bottomSpikeTexture;
    public static Texture2D topSpikeTexture;
    public static Texture2D cameraTexture;

    // Start screen variables
    static Texture2D startScreenTexture;
    static Texture2D startButton;
    static Texture2D levelEditorButton;
    static Texture2D exitButton;
    int buttonTracker = 0;
    float timeTracker = 0;
    float backgroundXOffset = 0;
    float backgroundYOffset = 0;

    LevelEditor Editor;
    LevelHandler levelHandler;
    Level levelOne;
    Level levelTwo;
    Level levelThree;
    Level levelFour;
    Player player;

    Music levelMusic;
    Sound playerScream;
    Sound confettiSound;
    float levelMusicOffset = 0.8f;
    bool hasEndMusicPlayed = false;
    bool canLevelMusicPlay = true;

    public void Setup()
    {
        InitializeTextures();

        splashSound = Audio.LoadSound("../../../assets/audio/music/splashScreenMusic.wav");
        Audio.SetVolume(splashSound, 0.5f);

        levelMusic = Audio.LoadMusic("../../../assets/audio/music/levelMusic.wav");
        Audio.SetVolume(levelMusic, levelMusicOffset);

        playerScream = Audio.LoadSound("../../../assets/audio/playerScream.wav");
        Audio.SetVolume(playerScream, 2.5f);

        confettiSound = Audio.LoadSound("../../../assets/audio/confettiSound.wav");
        Audio.SetVolume(confettiSound, 1f);

        Editor = new LevelEditor(8, 1);
        levelHandler = new LevelHandler();
        levelOne = new Level(8, 1, "LevelOne");
        levelTwo = new Level(8, 1, "LevelTwo");
        levelThree = new Level(8, 1, "LevelThree");
        levelFour = new Level(8, 1, "LevelFour");

        player = new Player(new Vector2(400, 250));

        Editor.Setup();
        levelOne.Setup();
        levelTwo.Setup();
        levelThree.Setup();
        levelFour.Setup();
    }

    public void Update()
    {
        HandleScenes();
    }

    // Handle scene transitions: 0 beginning splash screen, 1 start screen, 2 leveleditor, 3 game fail
    void HandleScenes()
    {
        if (currentScene == 0)
        {
            SplashScreen();
        }
        else if (currentScene == 1)
        {
            StartScreen();
        }
        else if (currentScene == 2)
        {
            LevelEditor();
        }
        else if (currentScene == 3)
        {
            // Upon player death, play player scream
            Audio.SetVolume(levelMusic, 0.2f);
            if (!Audio.IsPlaying(playerScream))
            {
                Audio.Play(playerScream);
            }

            Text.Size = 25;
            Text.Color = Color.White;
            Text.Draw($"SCORE: {player.playerScore}", 300, 300);
        }
        // If current scene is above 4 (game has started), play music constantly
        // Level one
        else if (currentScene >= 4)
        {
            Draw.FillColor = Color.Green;
            Draw.Rectangle(0, 0, 800, 600);
            Graphics.Draw(backgroundTexture, backgroundXOffset, backgroundYOffset);
            if (canLevelMusicPlay)
            {
                Audio.Play(levelMusic);
            }

            if (currentScene == 4)
            {
                LevelLogic(levelOne);

                if (player.playerHasExited)
                {
                    ResetLevel();
                    player.playerScore += 100;
                    currentScene = 5;
                }
            }
            // Level two
            else if (currentScene == 5)
            {
                player.playerHasExited = false;
                LevelLogic(levelTwo);

                if (player.playerHasExited)
                {
                    ResetLevel();
                    player.playerScore += 100;
                    currentScene = 6;
                }
            }
            // Level three
            else if (currentScene == 6)
            {
                player.playerHasExited = false;
                LevelLogic(levelThree);

                if (player.playerHasExited)
                {
                    ResetLevel();
                    player.playerScore += 100;
                    currentScene = 7;
                }
            }
            // Level four
            else if (currentScene == 7)
            {
                WinScreen();
            }

            if (currentScene != 3 || currentScene != 7)
            {
                Text.Size = 25;
                Text.Color = Color.White;
                Text.Draw($"SCORE: {player.playerScore}", 50, 550);
            }
        }
    }

    // Resets player position and tile positions back to origin
    void ResetLevel()
    {
        player.position.X = 400;
        player.position.Y = 250;

        if (currentScene != 7)
        {
            for (int i = 0; i < levelOne.tileArray.Length; i++)
            {
                levelOne.tileArray[i].position.X = levelOne.tilePositions[i].X;
            }
        }

    }

    // Render level, handle player collision
    void LevelLogic(Level level)
    {
        level.Render();
        player.Handle(level.tileArray, currentScene);

        backgroundXOffset = level.tileArray[0].position.X * 0.5f;

        for (int i = 0; i < level.tileArray.Length; i++)
        {
            player.HandleCollision(level.tileArray[i]);
        }

        if (player.position.Y > Window.Height)
        {
            currentScene = 3;
        }
    }

    void WinScreen()
    {
        player.playerHasExited = false;
        LevelLogic(levelFour);

        player.canPlayerMove = false;

        if (backgroundYOffset > -525)
        {
            backgroundYOffset -= 3;
        }
        else
        {
            Audio.Stop(levelMusic);
            canLevelMusicPlay = false;
            if (!Audio.IsPlaying(confettiSound) && !hasEndMusicPlayed)
            {
                Audio.Play(confettiSound);
                hasEndMusicPlayed = true;
            }
            player.isPlayerDancing = true;
        }

        Text.Color = Color.Black;
        Text.Size = 20;
        Text.Draw($"WHY ARE YOU STILL PLAYING? WE HAVEN'T FINISHED THE LEVEL YET!!!\n\n\nSCORE: {player.playerScore}\n\n\n HOLD F TO PARTY", 55, 200);

        Graphics.Draw(cameraTexture, 0, 0);
    }
    void SplashScreen()
    {
        if (!Audio.IsPlaying(splashSound))
        {
            Audio.Play(splashSound);
        }

        Graphics.Draw(avarusSplashTexture, 0, 0);
        Draw.FillColor = blackSplashColor;
        Draw.Rectangle(0, 0, Window.Width, Window.Height);

        timeTracker += Time.DeltaTime;

        if (timeTracker < 5.0f)
        {
            blackSplashColor.A -= 2;
        }
        else
        {
            blackSplashColor.A += 2;
        }

        if (blackSplashColor.A >= 255)
        {
            Graphics.UnloadTexture(avarusSplashTexture);
            timeTracker = 0;
            currentScene = 1;
        }
    }

    void StartScreen()
    {
        Graphics.Draw(startScreenTexture, 0, 0);

        if (buttonTracker == 0)
        {
            Graphics.Draw(startButton, 0, 0);
        }
        else if (buttonTracker == 1)
        {
            Graphics.Draw(levelEditorButton, 0, 0);
        }
        else if (buttonTracker == 2)
        {
            Graphics.Draw(exitButton, 0, 0);
        }

        // Draw splash screen over all other graphics.
        if (blackSplashColor.A >= 1)
        {
            Draw.FillColor = blackSplashColor;
            Draw.Rectangle(0, 0, Window.Width, Window.Height);
            blackSplashColor.A -= 2;
        }

        // Manage button inputs
        if (Input.IsKeyboardKeyPressed(KeyboardInput.W))
        {
            if (buttonTracker > 0)
            {
                buttonTracker--;
            }
        }
        else if (Input.IsKeyboardKeyPressed(KeyboardInput.S))
        {
            if (buttonTracker < 2)
            {
                buttonTracker++;
            }
        }

        if (Input.IsKeyboardKeyPressed(KeyboardInput.Space))
        {
            if (buttonTracker == 0)
            {
                currentScene = 4;
            }
            else if (buttonTracker == 1)
            {
                currentScene = 2;
            }
            else if (buttonTracker == 2)
            {
                System.Environment.Exit(0);
            }
        }
    }

    void LevelEditor()
    {
        Graphics.Draw(backgroundTexture, 0, 0);
        Editor.Update();
    }

    void InitializeTextures()
    {
        backgroundTexture = Graphics.LoadTexture("../../../assets/textures/backgroundTexture1.png");
        avarusSplashTexture = Graphics.LoadTexture("../../../assets/textures/avarusStudiosSplash.png");
        cameraTexture = Graphics.LoadTexture("../../../assets/textures/cameraTexture.png");

        startScreenTexture = Graphics.LoadTexture("../../../assets/textures/startScreen.png");
        startButton = Graphics.LoadTexture("../../../assets/textures/startButton.png");
        levelEditorButton = Graphics.LoadTexture("../../../assets/textures/levelEditorButton.png");
        exitButton = Graphics.LoadTexture("../../../assets/textures/exitButton.png");
        bottomSpikeTexture = Graphics.LoadTexture("../../../assets/textures/bottomSpikeTexture.png");
        topSpikeTexture = Graphics.LoadTexture("../../../assets/textures/topSpikeTexture.png");

        caveTexture = Graphics.LoadTexture("../../../assets/textures/caveTexture.png");
        speedBoostTexture = Graphics.LoadTexture("../../../assets/textures/speedBoostTexture.png");
        jumpBoostTexture = Graphics.LoadTexture("../../../assets/textures/jumpBoostTexture.png");
    }
}
