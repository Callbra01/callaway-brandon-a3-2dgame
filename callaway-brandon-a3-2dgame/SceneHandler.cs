using System;
using System.Numerics;

namespace Game10003;

public class SceneHandler
{
    LevelHandler levelHandler;
    LevelEditor levelEditor;
    Player player;
    int scene;

    // Levels
    LevelOne levelOne;
    

    public void Setup()
    {
        levelEditor = new LevelEditor();
        levelEditor.Setup();

        levelOne = new LevelOne();
        
        player = new Player();
        
        scene = 0;
    }

    void HandleScenes()
    {
        // 0 is level editor, 1 is main menu
        if (scene == 0)
        {
            levelEditor.Update();
        }
        else if (scene == 1)
        {
            levelOne.Update();
            player.Update();
        }
    }

    public void Update()
    {
        HandleScenes();
    }
}
