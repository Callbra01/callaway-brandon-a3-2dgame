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
    public static int windowWidth = 800;
    public static int windowHeight = 600;

    //Player player;
    SceneHandler sceneHandler;
    
    public void Setup()
    {
        InitializeWindow();

        //player = new Player();

        // Setup Scene Handler
        sceneHandler = new SceneHandler();
        sceneHandler.Setup();
    }

    void InitializeWindow()
    {
        Window.SetTitle("bGame");
        Window.SetSize(windowWidth, windowHeight);
        Window.TargetFPS = 60;
    }


    public void Update()
    {
        Window.ClearBackground(Color.Black);

        sceneHandler.Update();
        //player.Handle();
    }
}
