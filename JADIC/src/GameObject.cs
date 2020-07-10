using JADIC.Controls;
using System;
using System.Collections.Generic;
using System.Drawing;

/// <summary>
/// GameObject is a collection of data related to a generic game object.
/// </summary>
public abstract class GameObject : IDrawable
{
    public int PARTICLE_MIN_SIZE = 3;
    public int PARTICLE_MAX_SIZE = 8;

    public Point Position;
    public Size HitboxSize;
    public Rectangle BoundingBox;
    public Control Controls = new Control();
    public Color KeyColor = Color.White;
    public int Lives;
    public int Score = 0;

    /// <summary>
    /// The Update method is used mainly to update GameObject's position
    /// and state.
    /// </summary>
    public virtual void Update()
    {
        Position = Controls.NextPosition(Position);
    }

    /// <summary>
    /// The Action method is able to decide and create new objects based
    /// on the state of the containing World object. Note that it is not
    /// recommended to change anything in the World object from within this
    /// method and any new GameObject created should be added to the return
    /// value.
    /// </summary>
    /// <param name="world"></param>
    /// <returns>List of new GameObjects to be added to the World.</returns>
    public virtual List<GameObject> Action(World world)
        => new List<GameObject>();

    /// <summary>
    /// Render the particular GameObject.
    /// </summary>
    /// <param name="resolution"></param>
    /// <param name="container"></param>
    public abstract void Render(Size resolution, Graphics container);

    /// <summary>
    /// Basic collision detection based on rectangle intersection.
    /// </summary>
    /// <param name="other"></param>
    /// <returns>True if collision is detected. False otherwise.</returns>
    public virtual bool DetectCollision(GameObject other)
        => other.DetectCollisionRect(new Rectangle(Position, HitboxSize));

    public virtual bool DetectCollisionRect(Rectangle other)
        => other.IntersectsWith(new Rectangle(Position, HitboxSize));

    /// <summary>
    /// The Destroy method returns a collection of particles to be used as
    /// the destruction animation.
    /// </summary>
    /// <returns>List of Particles.</returns>
    public virtual List<Particle> Destroy()
        => DestructionParticles(PARTICLE_MIN_SIZE, PARTICLE_MAX_SIZE);

    private List<Particle> DestructionParticles(int minSize, int maxSize)
    {
        Random rng = new Random();
        var particles = new List<Particle>();

        int matter = HitboxSize.Width * HitboxSize.Height / 4;

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
