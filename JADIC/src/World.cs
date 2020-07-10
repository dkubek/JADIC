using System;
using System.Collections.Generic;
using System.Drawing;

public interface IDrawable
{
    void Render(Size resolution, Graphics container);
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
