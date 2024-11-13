using System;
using System.Numerics;

namespace Game10003;

public class Game
{
    static int windowWidth = 800;
    static int windowHeight = 600;

    SceneHandler sceneHandler;
    
    public void Setup()
    {
        Window.SetTitle("Potato Adventure");
        Window.SetSize(windowWidth, windowHeight);
        Window.TargetFPS = 60;

        sceneHandler = new SceneHandler();
        sceneHandler.Setup();
    }

    public void Update()
    {
        Window.ClearBackground(Color.Black);

        // All game scenes and logic is handle in sceneHandler class
        sceneHandler.Update();
    }
}
