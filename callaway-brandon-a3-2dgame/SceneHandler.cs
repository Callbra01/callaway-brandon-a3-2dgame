using Raylib_cs;
using System;
using System.Linq;
using System.Numerics;
using System.Security.Cryptography.X509Certificates;

namespace Game10003;

/*
 * All levels and scenes within the game should be added and modified within this class.
 */
public class SceneHandler
{
    public int currentScene = 4;

    // Splash screen variables
    static Texture2D avarusSplashTexture;
    Color blackSplashColor = new Color(0, 0, 0, 254);
    Sound splashSound;

    static Texture2D backgroundTexture;
    public static Texture2D caveTexture;
    public static Texture2D speedBoostTexture;
    public static Texture2D jumpBoostTexture;

    // Start screen variables
    static Texture2D startScreenTexture;
    static Texture2D startButton;
    static Texture2D levelEditorButton;
    static Texture2D exitButton;
    int buttonTracker = 0;
    float timeTracker = 0;

    LevelEditor Editor;
    LevelHandler levelHandler;
    Level levelOne;
    Level levelTwo;
    Player player;

    Music levelMusic;


    public void Setup()
    {
        InitializeTextures();

        splashSound = Audio.LoadSound("../../../assets/audio/music/splashScreenMusic.wav");
        Audio.SetVolume(splashSound, 0.5f);

        levelMusic = Audio.LoadMusic("../../../assets/audio/music/levelMusic.wav");
        Audio.SetVolume(levelMusic, 0.8f);

        Editor = new LevelEditor(8, 1);
        levelHandler = new LevelHandler();
        levelOne = new Level(8, 1, "LevelOne");
        levelTwo = new Level(8, 1, "LevelTwo");

        player = new Player(new Vector2(400, 250));

        Editor.Setup();
        levelOne.Setup();
        levelTwo.Setup();
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
            Audio.SetVolume(levelMusic, 0.2f);
        }
        else if (currentScene >= 4)
        {
            Graphics.Draw(backgroundTexture, 0, 0);
            Draw.FillColor = Color.White;
            Draw.Rectangle(0, 0, 800, 800);
            Audio.Play(levelMusic);

            if (currentScene == 4)
            {
                Graphics.Draw(backgroundTexture, 0, 0);
                Draw.FillColor = Color.White;
                Draw.Rectangle(0, 0, 800, 800);
                Audio.Play(levelMusic);

                LevelLogic(levelOne);

                if (player.playerHasExited)
                {
                    ResetLevel();
                    currentScene = 5;
                }
            }
            else if (currentScene == 5)
            {
                player.playerHasExited = false;
                LevelLogic(levelTwo);
                if (player.playerHasExited)
                {
                    ResetLevel();
                    currentScene = 6;
                }
            }
            else if (currentScene == 6)
            {

            }
        }
    }

    // Resets player position and tile positions back to origin
    void ResetLevel()
    {
        player.position.X = 400;
        player.position.Y = 250;

        for (int i = 0; i < levelOne.tileArray.Length; i++)
        {
            levelOne.tileArray[i].position.X = levelOne.tilePositions[i].X;
        }
    }

    // Render level, handle player collision
    void LevelLogic(Level level)
    {
        level.Render();
        player.Handle(level.tileArray, currentScene);

        for (int i = 0; i < level.tileArray.Length; i++)
        {
            player.HandleCollision(level.tileArray[i]);
        }

        if (player.position.Y > Window.Height)
        {
            currentScene = 3;
        }
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
        backgroundTexture = Graphics.LoadTexture("../../../assets/textures/backgroundTexture.png");
        avarusSplashTexture = Graphics.LoadTexture("../../../assets/textures/avarusStudiosSplash.png");

        startScreenTexture = Graphics.LoadTexture("../../../assets/textures/startScreen.png");
        startButton = Graphics.LoadTexture("../../../assets/textures/startButton.png");
        levelEditorButton = Graphics.LoadTexture("../../../assets/textures/levelEditorButton.png");
        exitButton = Graphics.LoadTexture("../../../assets/textures/exitButton.png");

        caveTexture = Graphics.LoadTexture("../../../assets/textures/caveTexture.png");
        speedBoostTexture = Graphics.LoadTexture("../../../assets/textures/speedBoostTexture.png");
        jumpBoostTexture = Graphics.LoadTexture("../../../assets/textures/jumpBoostTexture.png");
    }
}
