using System.Collections.Generic;
using System.Windows.Forms;

public class Game
{
    public List<Scene> Scenes;
    public World MainWorld;
    private Scene currentScene;

    public Game(List<Scene> scenes, World world)
    {
        Scenes = scenes;
        MainWorld = world;

        // TODO: TEMPORARY
        currentScene = new Scene(MainWorld);
        currentScene.isRunning = true;
    }

    public void NextFrame()
    {
        if (currentScene.isRunning)
        {
            currentScene.Update();
            currentScene.Render();
        }

        // TODO: Load next scene
    }

    public void HandleKey(Keys keycode, bool release)
    {
        // TODO: Pass the rest to scene
        currentScene.HandleKeys(keycode, release);
    }
}
