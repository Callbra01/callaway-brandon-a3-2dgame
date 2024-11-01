using System;
using System.Numerics;

namespace Game10003;

public class LevelOne
{
    LevelEditor levelOneEditor;

    public void Setup()
    {
        levelOneEditor = new LevelEditor();
        levelOneEditor.levelSize = 3;
        levelOneEditor.levelName = "levelEditor";
    }

    public void Update()
    {

    }
}
