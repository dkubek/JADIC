using System.Collections.Generic;
using System.Windows.Forms;
using System.Drawing;
using JADIC.Scenes;

/// <summary>
/// Game is a collection of scenes. The Game class oversees and manages 
/// the order of scenes.
/// </summary>
public class Game
{
    private Queue<Scene> Scenes;
    public World MainWorld;
    private Scene currentScene = null;

    /// <summary>
    /// Create new Game.
    /// </summary>
    /// <param name="windowAreaSize">The size of the playable area.</param>
    public Game(Size windowAreaSize)
    {
        Initialize(windowAreaSize);
    }

    private void Initialize(Size windowAreaSize)
    {
        Player player = InitializePlayer(windowAreaSize);
        InitializeWorld(windowAreaSize, player);
        InitializeScenes();
    }

    private Player InitializePlayer(Size windowAreaSize)
    {
        Point startingPosition = new Point(-100, 400);
        Rectangle playerBounds = GetPlayerBounds(windowAreaSize, 50);
        return new Player(startingPosition, playerBounds);
    }

    private Rectangle GetPlayerBounds(Size windowAreaSize, int edgeOffset)
    {
        Rectangle playerBounds = 
            new Rectangle(
                edgeOffset, edgeOffset, 
                windowAreaSize.Width / 2 - 2 * edgeOffset, 
                windowAreaSize.Height - 2 * edgeOffset);

        return playerBounds;
    }
  
    private void InitializeWorld(Size windowAreaSize, Player player)
    {
        MainWorld = new World(windowAreaSize, player);
    }

    private void InitializeScenes()
    {
        Scenes = new Queue<Scene>();
        Scenes.Enqueue(new Intro(MainWorld));
        Scenes.Enqueue(new FlyIn(MainWorld));
        Scenes.Enqueue(new EndlessMode(MainWorld));
        Scenes.Enqueue(new GameOver(MainWorld));
    }

    /// <summary>
    /// This method should be called on every tick to update the game and 
    /// progress to the next frame.
    /// </summary>
    public void NextFrame()
    {
        if (currentScene != null && currentScene.isRunning)
        {
            currentScene.Update();
            currentScene.Render(MainWorld.Resolution, MainWorld.bmGraphics);
        }
        else if (Scenes.Count != 0)
        {
            currentScene = Scenes.Dequeue();
            currentScene.isRunning = true;
            currentScene.Initialize();
        }
        else
        {
            RestartGame();
        }
    }

    private void RestartGame()
    {
        var windowAreaSize = MainWorld.Resolution;
        Player player = InitializePlayer(windowAreaSize);
        InitializeWorld(windowAreaSize, player);

        Scenes.Enqueue(new FlyIn(MainWorld));
        Scenes.Enqueue(new EndlessMode(MainWorld));
        Scenes.Enqueue(new GameOver(MainWorld));
    }

    public void HandleKeys(Keys keycode, bool release)
    {
        currentScene.HandleKeys(keycode, release);
    }
}
