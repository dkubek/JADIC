using System.Windows.Forms;

public class Scene
{
    public World MainWorld;
    public bool isRunning;
    public Scene(World world)
    {
        MainWorld = world;
        isRunning = false;
    }

    public void Update()
    {
        // Update player position
        MainWorld.MainPlayer.Update();

        // Update game objects
        foreach (GameObject go in MainWorld.GameObjects)
        {
            go.Update();
        }

        // Update background
        MainWorld.bg.Update();
        MainWorld.overlay.Update();
    }

    public void Render()
    {
        var resolution = MainWorld.Resolution;
        var graphics_container = MainWorld.bmGraphics;

        MainWorld.bg.Render(resolution, graphics_container);

        MainWorld.MainPlayer.Render(resolution, graphics_container);

        foreach (GameObject go in MainWorld.GameObjects)
        {
            go.Render(resolution, graphics_container);
        }

        MainWorld.overlay.Render(resolution, graphics_container);
    }

    public void HandleKeys(Keys keycode, bool release)
    {
        // TODO: Handle player keys
        PlayerControl.HandlePlayerKeys(MainWorld.MainPlayer, keycode, release);

        // TODO: Decide what to do, can also decide to do nothing
        if (keycode == Keys.Space && ! release)
        {
            var projectile = MainWorld.MainPlayer.SpawnProjectile();
            if (projectile != null)
            {
                MainWorld.GameObjects.Add(projectile);
            }
        }
    }
}
