// Include code libraries you need below (use the namespace).
using System;
using System.Numerics;

// The namespace your code is in.
namespace Game10003;

/// <summary>
///     Your game code goes inside this class!
/// </summary>
public class Game
{
    // Place your variables here:
    public static int windowWidth = 800;
    public static int windowHeight = 600;

    public static Texture2D caveTexture;
    static Texture2D backgroundTexture;

    //Player player;
    SceneHandler sceneHandler;
    


    /// <summary>
    ///     Setup runs once before the game loop begins.
    /// </summary>
    public void Setup()
    {
        InitializeWindow();
        InitializeTextures();

        //player = new Player();
        sceneHandler = new SceneHandler();
        sceneHandler.Setup();
    }

    void InitializeWindow()
    {
        Window.SetTitle("bGame");
        Window.SetSize(windowWidth, windowHeight);
        Window.TargetFPS = 60;
    }

    void InitializeTextures()
    {
        caveTexture = Graphics.LoadTexture("../../../assets/textures/caveTexture.png");
        backgroundTexture = Graphics.LoadTexture("../../../assets/textures/backgroundTexture.png");
    }
    public void Update()
    {
        Window.ClearBackground(Color.Black);
        Graphics.Draw(backgroundTexture, 0, 0);
        //player.Update();

        sceneHandler.Update();
    }
}
