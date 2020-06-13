using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

public interface IDrawable
{
    void Render(Size resolution, Graphics container);
}

public abstract class GameObject
{
    public Point Location;
    public Size Hitbox;
}

public abstract class Enemy : GameObject
{
}

public class Scene: IDrawable
{
    private Size resolution;
    public Scene(Size resolution)
    {
        this.resolution = resolution;
    }

    public void Render(Size resolution, Graphics container)
    {
    }
}

public class PlayerControl
{
    private Player player;
    public PlayerControl(Player player)
    {
        this.player = player;
    }
    public void HandleKeys(Keys keyCode, bool release)
    {
        int verticalChange = 0;
        int horizontalChange = 0;
        switch (keyCode)
        {
            case Keys.W:
                verticalChange = -1;
                break;
            case Keys.A:
                horizontalChange = -1;
                break;
            case Keys.S:
                verticalChange = 1;
                break;
            case Keys.D:
                horizontalChange = 1;
                break;
        }

        if (release)
        {
            if (Math.Abs(player.HorizontalSign - horizontalChange) <= 1)
                player.HorizontalSign -= horizontalChange;

            if (Math.Abs(player.VerticalSign - verticalChange) <= 1)
                player.VerticalSign -= verticalChange;
        }
        else
        {
            if (Math.Abs(player.HorizontalSign + horizontalChange) <= 1)
                player.HorizontalSign += horizontalChange;

            if (Math.Abs(player.VerticalSign + verticalChange) <= 1)
                player.VerticalSign += verticalChange;
        }
    }
}

public class Player : GameObject, IDrawable
{
    const int SPEED = 5;

    public int HorizontalSign = 0;
    public int VerticalSign = 0;

    private readonly Brush myBrush;
    public Player(Point startPosition, Size hitbox)
    {
        Location = startPosition;
        Hitbox = hitbox;
        myBrush = new SolidBrush(Color.Tomato);
    }

    public void Update()
    {
        Location = new Point(
            Location.X + HorizontalSign * SPEED, 
            Location.Y + VerticalSign * SPEED);
    }

    public void Render(Size resolution, Graphics container)
    {
        container.FillRectangle(
            myBrush,
            new Rectangle(Location, Hitbox));
    }
}

public class CosmicBackground : IDrawable
{
    private Point[] FirstLevel;
    private Point[] SecondLevel;
    private Point[] ThirdLevel;

    private readonly int baseSpeed;
    private Size bounds;
    private Random rnd = new Random();

    private SolidBrush starBrush;

    public CosmicBackground(int stars, int baseSpeed, Size bounds)
    {
        this.baseSpeed = baseSpeed;
        this.bounds = bounds;

        starBrush = new SolidBrush(Color.White);

        Initialize(stars);
    }

    private void Initialize(int stars)
    {
        FirstLevel = new Point[stars];
        SecondLevel = new Point[2 * stars];
        ThirdLevel = new Point[3 * stars];

        InitializeLevel(FirstLevel);
        InitializeLevel(SecondLevel);
        InitializeLevel(ThirdLevel);
    }

    private void InitializeLevel(Point[] stars)
    {
        for (int i = 0; i < stars.Length; i++)
        {
            Point starLocation = new Point(
                rnd.Next(0, bounds.Width),
                rnd.Next(0, bounds.Height)
                );

            stars[i] = starLocation;
        }
    }

    public void Update()
    {
        UpdateLevel(FirstLevel, 3 * baseSpeed);
        UpdateLevel(SecondLevel, 2 * baseSpeed);
        UpdateLevel(ThirdLevel, baseSpeed);
    }

    private void UpdateLevel(Point[] stars, int speed)
    {
        for (int i = 0; i < stars.Length; i++)
        {
            if (OutOfBounds(stars[i]))
            {
                // Create new star
                stars[i] = new Point(
                    bounds.Width,
                    rnd.Next(0, bounds.Height)
                );
            } 
            else
            {
                stars[i].X -= speed;
            }
        }
    }

    private bool OutOfBounds(Point star)
        => star.X < 0;

    public void Render(Size resolution, Graphics container)
    {
        container.Clear(Color.Black);

        RenderLevel(container, FirstLevel, 5);
        RenderLevel(container, SecondLevel, 3);
        RenderLevel(container, ThirdLevel, 2);
    }

    private void RenderLevel(Graphics container, Point[] stars, int size)
    {
        foreach (Point star in stars)
        {
            var boundingBox = new Rectangle(star.X, star.Y, size, size);

            container.FillEllipse(starBrush, boundingBox);
        }
    }
}

public class World
{
    public Player MainPlayer;
    public List<GameObject> GameObjects = new List<GameObject>();

    public Bitmap Frame;
    public Size Resolution;
    private Graphics bmGraphics;

    public CosmicBackground bg; 

    private Scene currentScene;

    public World(Size resolution, Player player)
    {
        Resolution = resolution;

        MainPlayer = player;

        Frame = new Bitmap(Resolution.Width, Resolution.Height);
        bg = new CosmicBackground(30, 1, resolution);

        bmGraphics = Graphics.FromImage(Frame);
    }

    public void HandleKey(Keys keyCode, bool release)
    {
        // TODO: Only if it scene allows and game is not paused
        MainPlayer.ProcessMovementKeys(keyCode, release);

        // TODO: Pass extra keys to scene
    }

    public void Update()
    {
        MainPlayer.Update();
        bg.Update();
    }

    public void Render()
    {
        bg.Render(Resolution, bmGraphics);
        MainPlayer.Render(Resolution, bmGraphics);
    }
}
