using System;
using System.Numerics;

namespace Game10003;

public class Game
{
    public static int windowWidth = 800;
    public static int windowHeight = 600;

    SceneHandler sceneHandler;
    
    public void Setup()
    {
        InitializeWindow();

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
    }
}
