using System;
using System.Collections.Generic;
using System.Drawing;

public interface IDrawable
{
    void Render(Size resolution, Graphics container);
}

public abstract class GameObject : IDrawable
{
    public int PARTICLE_MIN_SIZE = 3;
    public int PARTICLE_MAX_SIZE = 10;

    public Point Position;
    public Size HitboxSize;
    public Rectangle BoundingBox;
    public Control Controls = new Control();
    public Color KeyColor = Color.White;
    public int Lives;
    public int Score = 0;

    public virtual void Update()
    {
        Position = Controls.NextPosition(Position);
    }

    public virtual List<GameObject> Action(World world)
        => new List<GameObject>();

    public abstract void Render(Size resolution, Graphics container);

    public virtual bool DetectCollision(GameObject other)
        => other.DetectCollisionRect(new Rectangle(Position, HitboxSize));

    public virtual bool DetectCollisionRect(Rectangle other)
        => other.IntersectsWith(new Rectangle(Position, HitboxSize));

    public virtual List<Particle> Destroy()
        => DestructionParticles(PARTICLE_MIN_SIZE, PARTICLE_MAX_SIZE);

    private List<Particle> DestructionParticles(int minSize, int maxSize)
    {
        Random rng = new Random();
        var particles = new List<Particle>();

        int matter = HitboxSize.Width * HitboxSize.Height;

        while (matter > 0)
        {
            double randomAngle = 2 * rng.NextDouble() * Math.PI;
            int particleSize = rng.Next(minSize, maxSize);

            var disp = Vector.FromPolar(randomAngle, 30 * (0.7 * rng.NextDouble() + 0.3) );
            var particleControl = new Control(new ConstantDisplacement(disp));
            var particle = new Particle(
                Position, particleSize, particleControl, KeyColor
                );

            matter -= particleSize * particleSize;

            particles.Add(particle);
        }

        return particles;
    }
}

public class Particle : IDrawable
{
    private Point position;
    private Size size;
    private Control controls;
    private Brush keyBrush;

    public Point Position { get => position; }

    public Particle(
        Point startLocation, int size, Control controls, Color color)
    {
        position = startLocation;
        this.size = new Size(size, size);
        this.controls = controls;
        keyBrush = new SolidBrush(color);
   }

   public void Render(Size resolution, Graphics container)
    {
        UpdatePosition();

        container.FillRectangle(
            keyBrush,
            new Rectangle(position, size));
    }

    private void UpdatePosition()
    {
        position = controls.NextPosition(position);
    }
}

public class World
{
    public readonly Bitmap Frame;
    public readonly Size Resolution;
    public readonly Graphics bmGraphics;
    public Random RandomGen = new Random();

    public Player MainPlayer;
    public List<GameObject> GameObjects = new List<GameObject>();
    public List<Particle> Particles = new List<Particle>();
    public Rectangle DespawnBounds;

    public CosmicBackground bg;
    public Overlay overlay;

    public int TotalScore = 0;

    public World(Size resolution, Player player)
    {
        Resolution = resolution;
        MainPlayer = player;

        Frame = new Bitmap(Resolution.Width, Resolution.Height);
        bmGraphics = Graphics.FromImage(Frame);

        int despawnOffset = 500;
        DespawnBounds = new Rectangle(
            -despawnOffset, -despawnOffset,
            resolution.Width + 2 * despawnOffset, resolution.Height + 2 * despawnOffset);

        bg = new CosmicBackground(30, 1, resolution);
        overlay = new Overlay(this);
    }
}
