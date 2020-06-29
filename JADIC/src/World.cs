using System.Collections.Generic;
using System.Drawing;

public interface IDrawable
{
    void Render(Size resolution, Graphics container);
}

public abstract class GameObject : IDrawable
{
    public Point Location;
    public Size Hitbox;
    public abstract void Update();
    public abstract void Render(Size resolution, Graphics container);
}

public abstract class Enemy : GameObject
{
}

public class World
{
    public readonly Bitmap Frame;
    public readonly Size Resolution;
    public readonly Graphics bmGraphics;

    public Player MainPlayer;
    public List<GameObject> GameObjects = new List<GameObject>();

    public CosmicBackground bg;
    public Overlay overlay;

    public World(Size resolution, Player player)
    {
        Resolution = resolution;
        MainPlayer = player;

        Frame = new Bitmap(Resolution.Width, Resolution.Height);
        bmGraphics = Graphics.FromImage(Frame);

        bg = new CosmicBackground(30, 1, resolution);
        overlay = new Overlay(this);
    }
}
