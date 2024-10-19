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

    Player player;
    LevelEditor Editor;


    /// <summary>
    ///     Setup runs once before the game loop begins.
    /// </summary>
    public void Setup()
    {
        Window.SetTitle("bGame");
        Window.SetSize(windowWidth, windowHeight);
        Window.TargetFPS = 60;

        //player = new Player();

        // Setup Editor
        Editor = new LevelEditor();
        Editor.Setup();

    }

    /// <summary>
    ///     Update runs every frame.
    /// </summary>
    public void Update()
    {
        Window.ClearBackground(Color.Black);

        //player.Handle();

        Editor.Update();
    }

    void DEBUGINPUT()
    {
        /*
        if (Input.IsKeyboardKeyDown(KeyboardInput.A))
        {
            for (int tile = 0; tile < tileArray.Length; tile++)
            {
                tileArray[tile].position.X -= 50 * Time.DeltaTime;
            }
        }
        else if (Input.IsKeyboardKeyDown(KeyboardInput.D))
        {
            for (int tile = 0; tile < tileArray.Length; tile++)
            {
                tileArray[tile].position.X += 50 * Time.DeltaTime;
            }
        }
        */
    }

}
