using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;

public abstract class Scene
{
    public World MainWorld;
    public bool isRunning;

    protected bool isPlayerCollidedd = false;

    public Scene(World world)
    {
        MainWorld = world;
        isRunning = false;
    }

    public abstract void Update();

    protected virtual void UpdatePlayer()
    {
        MainWorld.MainPlayer.Update();
    }

    protected virtual void UpdateGameObjects()
    {
        foreach (GameObject go in MainWorld.GameObjects)
        {
            go.Update();
        }
    }

    protected void PerformActions()
    {
        var actionGameObjects = new List<GameObject>();
        foreach (GameObject go in MainWorld.GameObjects)
        {
            actionGameObjects.AddRange(go.Action(MainWorld));
        }

        MainWorld.GameObjects.AddRange(actionGameObjects);
    }

    protected void HandleCollisions(List<GameObject> collisions)
    {
        SubtractLife(collisions);
        AddScore(collisions);
        RemoveDestroyed(collisions);

        // Add rest back to Game objects
        MainWorld.GameObjects.AddRange(collisions);
    }

    private void SubtractLife(List<GameObject> collisions)
    {
        if (isPlayerCollidedd)
        {
            MainWorld.MainPlayer.Lives--;
        }

        collisions.ForEach(go => go.Lives--);
    }

    private void AddScore(List<GameObject> collisions)
    {
        foreach (GameObject go in collisions)
        {
            if (go.Lives <= 0)
            {
                MainWorld.TotalScore += go.Score;
            }
        }
    }

    private void RemoveDestroyed(List<GameObject> collisions)
    {
        foreach (GameObject go in collisions)
        {
            if (go.Lives <= 0)
            {
                MainWorld.Particles.AddRange(go.Destroy());
            }
        }

        collisions.RemoveAll(go => go.Lives <= 0);
    }

    protected List<GameObject> ExtractCollisions()
    {
        isPlayerCollidedd = false;
        bool[] collisions = new bool[MainWorld.GameObjects.Count];

        DetectCollisionsWithPlayer(collisions);
        DetectCollisionsBetweenGameObjects(collisions);

        return ExtractCollidedGameObjects(collisions);
    }

    private void DetectCollisionsWithPlayer(bool[] collided)
    {
        for (int i = 0; i < MainWorld.GameObjects.Count; i++)
        {
            var other = MainWorld.GameObjects[i];
            if (MainWorld.MainPlayer.DetectCollision(other))
            {
                collided[i] = true;
                isPlayerCollidedd = true;
            }
        }
    }

    private void DetectCollisionsBetweenGameObjects(bool[] collided)
    {
        for (int i = 0; i < MainWorld.GameObjects.Count; i++)
        {
            for (int j = i + 1; j < MainWorld.GameObjects.Count; j++)
            {
                var main = MainWorld.GameObjects[i];
                var other = MainWorld.GameObjects[j];
                if (main.DetectCollision(other))
                {
                    collided[i] = true;
                    collided[j] = true;
                }
            }
        }
    }

    private List<GameObject> ExtractCollidedGameObjects(bool[] collided)
    {
        var newGameObjects = new List<GameObject>();
        var collisions = new List<GameObject>();
        for (int i = 0; i < MainWorld.GameObjects.Count; i++)
        {
            if (collided[i])
            {
                collisions.Add(MainWorld.GameObjects[i]);
            }
            else
            {
                newGameObjects.Add(MainWorld.GameObjects[i]);
            }
        }

        MainWorld.GameObjects = newGameObjects;
        return collisions;
    }

    protected void CleanUp()
    {
        isPlayerCollidedd = false;
        RemoveOutOfBounds();
    }

    private void RemoveOutOfBounds()
    {
        RemoveOutOfBoundsGameObjects();
        RemoveOutOfBoundsParticles();
    }

    private void RemoveOutOfBoundsGameObjects()
    {
        MainWorld.GameObjects.RemoveAll(
            particle => !MainWorld.DespawnBounds.Contains(particle.Position));
    }
    private void RemoveOutOfBoundsParticles()
    { 
        MainWorld.Particles.RemoveAll(
            particle => !MainWorld.DespawnBounds.Contains(particle.Position));
    }

    public void Render()
    {
        var resolution = MainWorld.Resolution;
        var graphics_container = MainWorld.bmGraphics;

        RenderBackground(resolution, graphics_container);
        RenderPlayer(resolution, graphics_container);
        RenderGameObjects(resolution, graphics_container);
        RenderParticles(resolution, graphics_container);
        RenderOverlay(resolution, graphics_container);
    }

    private void RenderBackground(Size resolution, Graphics graphics_container)
    {
        MainWorld.bg.Render(resolution, graphics_container);
    }

    private void RenderPlayer(Size resolution, Graphics graphics_container)
    {
        MainWorld.MainPlayer.Render(resolution, graphics_container);
    }

    private void RenderOverlay(Size resolution, Graphics graphics_container)
    {
        MainWorld.overlay.Render(resolution, graphics_container);
    }

    private void RenderGameObjects(Size resolution, Graphics graphics_container)
    {
        foreach (IDrawable go in MainWorld.GameObjects)
        {
            go.Render(resolution, graphics_container);
        }
    }

    private void RenderParticles(Size resolution, Graphics graphics_container)
    {
        foreach (IDrawable particle in MainWorld.Particles)
        {
            particle.Render(resolution, graphics_container);
        }
    }

    public void HandleKeys(Keys keycode, bool release)
    {
        HandlePlayerKeys(keycode, release);
    }

    protected void HandlePlayerKeys(Keys keycode, bool release)
    {
        PlayerControl.HandlePlayerMovementKeys(
            MainWorld.MainPlayer, keycode, release);

        if (keycode == Keys.Space && !release)
        {
            var projectile = MainWorld.MainPlayer.SpawnProjectile();
            if (projectile != null)
            {
                MainWorld.GameObjects.Add(projectile);
            }
        }
    }
}

public class EndlessMode : Scene
{
    private int enemies = 0;
    private int spawnCooldown = 0;

    public EndlessMode(World world) : base(world) { }

    public override void Update()
    {
        UpdatePlayer();
        UpdateGameObjects();

        var collisions = ExtractCollisions();
        HandleCollisions(collisions);

        PerformActions();

        SpawnEnemies();

        CleanUp();
    }

    private void SpawnEnemies()
    {
        if (spawnCooldown == 0)
        {
            if (enemies < 20)
            {
                if (MainWorld.RandomGen.NextDouble() < 0.30)
                {
                    generateEnemy();
                    enemies++;
                    spawnCooldown = 100;
                }
            }
        }
        else
        {
            spawnCooldown--;
        }
    }

    private void generateEnemy()
    {
        var enemy = new Enemy(
            new Point(
                MainWorld.Resolution.Width,
                MainWorld.RandomGen.Next(100, MainWorld.Resolution.Height - 100)));

        var destination = new Point(MainWorld.Resolution.Width / 2, enemy.Position.Y);

        var controls = new List<ControlElement>();
        controls.Add(new LinearTransition(destination, 3));
        controls.Add(new Follow(MainWorld.MainPlayer, 5));
        enemy.Controls = new Control(controls);

        MainWorld.GameObjects.Add(enemy);
    }

}
