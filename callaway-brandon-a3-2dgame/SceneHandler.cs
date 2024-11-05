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
    static Texture2D avarusSplashTexture;
    Color blackSplashColor = new Color(0, 0, 0, 254);

    public static Texture2D caveTexture;
    static Texture2D backgroundTexture;
    static Texture2D startScreenTexture;

    static Texture2D startButton;
    static Texture2D levelEditorButton;
    static Texture2D exitButton;

    int buttonTracker = 0;
    float timeTracker = 0;

    Player player;

    LevelEditor Editor;
    LevelHandler levelHandler;

    Level levelOne;

    bool isPlayerTyping = false;
    string tempString = string.Empty;

    public void Setup()
    {
        InitializeTextures();

        Editor = new LevelEditor(2, 2);
        Editor.Setup();

        levelHandler = new LevelHandler();

        levelOne = new Level(2, 1);
        levelOne.Setup();

        player = new Player();
        
    }

    public void Update()
    {
        HandleScenes();
    }

    // Handle scene transitions: 0 beginning splash screen, 1 start screen, 2 leveleditor, 3 character creator
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
            CharacterCreator();
        }
        else if (currentScene == 4)
        {
            Graphics.Draw(backgroundTexture, 0, 0);
            Draw.FillColor = Color.White;
            Draw.Rectangle(0, 0, 800, 600);
            levelOne.Render();
            player.Handle();

            for (int i = 0; i < levelOne.tileArray.Length; i++)
            {
                if (levelOne.tileArray[i].canCollide)
                {
                    player.HandleCollision(levelOne.tileArray[i]);
                }

                //Console.WriteLine(player.position.X);
            }
            
        }
    }

    void SplashScreen()
    {
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

        // DEBUG INPUTS

        if (Input.IsKeyboardKeyPressed(KeyboardInput.Backslash))
        {
            currentScene = 3;
        }
    }

    void CharacterCreator()
    {
        Text.Color = Color.White;
    }

    void LevelEditor()
    {
        Graphics.Draw(backgroundTexture, 0, 0);
        Editor.Update();
    }

    void InitializeTextures()
    {
        backgroundTexture = Graphics.LoadTexture("../../../assets/textures/backgroundTexture.png");
        caveTexture = Graphics.LoadTexture("../../../assets/textures/caveTexture.png");
        avarusSplashTexture = Graphics.LoadTexture("../../../assets/textures/avarusStudiosSplash.png");

        startScreenTexture = Graphics.LoadTexture("../../../assets/textures/startScreen.png");
        startButton = Graphics.LoadTexture("../../../assets/textures/startButton.png");
        levelEditorButton = Graphics.LoadTexture("../../../assets/textures/levelEditorButton.png");
        exitButton = Graphics.LoadTexture("../../../assets/textures/exitButton.png");
    }

    void TextInputUpdate()
    {
        if (Input.IsKeyboardKeyPressed(KeyboardInput.Enter))
        {
            tempString = "";
        }

        CheckForTextInput();

        Text.Color = Color.White;
        Text.Draw($"{tempString}", 40, 40);
    }

    void CheckForTextInput()
    {
        string myString = "";

        int maxCharacterCount = 10;

        if (isPlayerTyping && tempString.Length < 10)
        {
            // 65-90 is defined as the alphabetic keys in Raylibs KeyboardKey Enum
            for (int kbChar = 65; kbChar <= 90; kbChar++)
            {
                if (Raylib.IsKeyPressed((Raylib_cs.KeyboardKey)kbChar))
                {
                    // 82 is used for both "R" and android's "menu" key, manually place "R"
                    if (kbChar == 82)
                    {
                        //Console.WriteLine("R");
                        myString = tempString.Insert(tempString.Length, "R");
                        tempString = myString;
                    }
                    else
                    {
                        //Console.WriteLine((Raylib_cs.KeyboardKey)kbChar);
                        myString = tempString.Insert(tempString.Length, ((Raylib_cs.KeyboardKey)kbChar).ToString());
                        tempString = myString;
                    }
                }

                // When the backspace key is pressed, wipe the string
                // TODO: REMOVE LAST INDEX OF STRING
                if (Input.IsKeyboardKeyPressed(KeyboardInput.Backspace))
                {
                    tempString = "";
                }

                // TODO THIS IS BROKEN, ADDS MULTIPLE SPACES
                /* When the spacebar is pressed, add a space to the current index in the string
                if (Raylib.IsKeyPressed((Raylib_cs.KeyboardKey)32))
                {
                    myString = tempString.Insert(tempString.Length, " ");
                    tempString = myString;
                }
                */

            }
        }
    }
}
