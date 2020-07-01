using System;
using System.Drawing;

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
        Update();

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
