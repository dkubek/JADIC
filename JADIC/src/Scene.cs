using System;
using System.Drawing;
using System.Windows.Forms;

public class Scene
{
    public World MainWorld;
    public bool isRunning;

    private int enemies = 0;
    private Random rng = new Random();
    private int spawnCooldown = 0;

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

        HandleCollisions();
        RemoveExpired();

        if (spawnCooldown == 0)
        {
            if (enemies < 20)
            {
                if (rng.NextDouble() < 0.30)
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
                rng.Next(100, MainWorld.Resolution.Height - 100)));

        var displacement = new Vector(-3, 0);
        enemy.Controls = new Control(new ConstantDisplacement(displacement));

        MainWorld.GameObjects.Add(enemy);
    }

    private void HandleCollisions()
    {
        HandleCollisionsWithPlayer();
        HandleCollisionsBetweenGameObjects();
    }

    private void HandleCollisionsWithPlayer()
    {
        for (int i = 0; i < MainWorld.GameObjects.Count; i++)
        {
            var other = MainWorld.GameObjects[i];
            if (MainWorld.MainPlayer.DetectCollision(other))
            {
                other.Lives--;
                MainWorld.MainPlayer.Lives--;
            }
        }
    }

    private void HandleCollisionsBetweenGameObjects()
    {
        for (int i = 0; i < MainWorld.GameObjects.Count; i++)
        {
            for (int j = i + 1; j < MainWorld.GameObjects.Count; j++)
            {
                var main = MainWorld.GameObjects[i];
                var other = MainWorld.GameObjects[j];
                if (main.DetectCollision(other))
                {
                    other.Lives--;
                    main.Lives--;
                }
            }
        }
    }

    private void RemoveExpired()
    {
        RemoveDestoyedGameObjects();
        RemoveOutOfBounds();
    }

    private void RemoveDestoyedGameObjects()
    {
        var player = MainWorld.MainPlayer;
        if (player.Lives <= 0)
        {
            MainWorld.Particles.AddRange(player.Destroy());
        }

        int destroyed = 0;
        for (int i = 0; i < MainWorld.GameObjects.Count; i++)
        {
            var gameObject = MainWorld.GameObjects[i];

            if (gameObject.Lives <= 0)
            {
                MainWorld.Particles.AddRange(gameObject.Destroy());
                destroyed++;
            }
        }

        if (destroyed != 0)
        {
            MainWorld.GameObjects.RemoveAll(go => go.Lives <= 0);
        }
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
        PlayerControl.HandlePlayerKeys(MainWorld.MainPlayer, keycode, release);

        // TODO: SCENE SPECIFIC
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
