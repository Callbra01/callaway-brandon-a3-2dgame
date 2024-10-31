using System;
using System.Numerics;

namespace Game10003;

public class SceneHandler
{
    int currentScene = 0;
    static Texture2D avarusSplashTexture;
    Color blackSplashColor = new Color(0, 0, 0, 254);

    public static Texture2D caveTexture;
    static Texture2D backgroundTexture;

    LevelEditor Editor;
    public void Setup()
    {
        InitializeTextures();

        Editor = new LevelEditor(2, 2);
        Editor.Setup();
    }

    public void Update()
    {
        HandleScenes();
    }

    void HandleScenes()
    {
        if (currentScene == 0)
        {
            SceneOne();
        }
        else if (currentScene == 1)
        {
            LevelEditor();
        }
    }

    void SceneOne()
    {
        Graphics.Draw(avarusSplashTexture, 0, 0);
        Draw.FillColor = blackSplashColor;
        Draw.Rectangle(0, 0, Game.windowWidth, Game.windowHeight);

        if (Time.SecondsElapsed < 5.0f)
        {
            blackSplashColor.A -= 2;
        }
        else
        {
            blackSplashColor.A += 2;
        }

        if (blackSplashColor.A >= 255)
        {
            currentScene = 1;
        }
    }

    void SceneTwo()
    {

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
    }

}
