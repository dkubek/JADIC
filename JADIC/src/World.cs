using System;
using System.Collections.Generic;
using System.Drawing;

/// <summary>
/// IDrawable is an interface for everything that can be drawn on the screen.
/// </summary>
public interface IDrawable
{
    /// <summary>
    /// Renders object on the given Graphics container given the resolution.
    /// </summary>
    /// <param name="resolution">Resolution or dimensions of the drawable area.</param>
    /// <param name="container">Graphics container.</param>
    void Render(Size resolution, Graphics container);
}

/// <summary>
/// The World object is the main data storage of the game world. It is supposed
/// to be used mainly as an shared data structure for the Scene objects. Data
/// can be accessed by other objects but mutation should be exclusive to the
/// Scene objects.
/// </summary>
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
