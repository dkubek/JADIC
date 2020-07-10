using System;
using System.Collections.Generic;
using System.Drawing;

public class Player : GameObject 
{
    public const int PROJECTILE_COOLDOWN = 25;
    public const int SPEED = 5;

    const int PLAYER_SIZE = 50;

    public int HorizontalSign = 0;
    public int VerticalSign = 0;
    public int projectileCooldownCurrent = 0;

    readonly Brush brush;

    public Player(
        Point startPosition, Rectangle boundingBox)
    {
        Position = startPosition;
        HitboxSize = new Size(PLAYER_SIZE, PLAYER_SIZE);
        BoundingBox = boundingBox;
        Lives = 5;

        brush = new SolidBrush(KeyColor);
    }

    public override void Update()
    {
        if (projectileCooldownCurrent != 0)
            projectileCooldownCurrent--;

        if (Lives > 0)
            base.Update();
    }

    public PlayerProjectile SpawnProjectile()
    {
        if (projectileCooldownCurrent == 0)
        {
            var center = Center();
            var projectile = new PlayerProjectile(
                new Point(center.X + HitboxSize.Width / 2, center.Y),
                new Size(10, 10));

            projectileCooldownCurrent = PROJECTILE_COOLDOWN;
            return projectile;
        }

        return null;
    }

    private Point Center()
    {
        return new Point(
            Position.X + HitboxSize.Width / 2,
            Position.Y + HitboxSize.Height / 2);
    }

    public override void Render(Size resolution, Graphics container)
    {
        var trianglePoints = generateTrianglePoints();
        container.FillPolygon(brush, trianglePoints);
    }

    private Point[] generateTrianglePoints()
    {
        Point center = Center();
        Point topLeft = new Point(
            center.X - HitboxSize.Width / 2,
            center.Y - HitboxSize.Height / 2);
        Point bottomLeft = new Point(
            center.X - HitboxSize.Width / 2,
            center.Y + HitboxSize.Height / 2);
        Point middleRight = new Point(
            center.X + HitboxSize.Width / 2,
            center.Y);

        Point[] points = { topLeft, middleRight, bottomLeft };
        return points;
    }

    public override bool DetectCollision(GameObject other)
    {
        if (other is PlayerProjectile)
            return false;

        return base.DetectCollision(other);
    }
}

public class Projectile : GameObject
{
    protected Brush keyBrush;
    
    public Projectile(
        Point startPosition, Size hitboxSize)
    {
        Position = startPosition;
        HitboxSize = hitboxSize;
        Lives = 1;

        PARTICLE_MAX_SIZE = 3;
        PARTICLE_MIN_SIZE = 1;

        keyBrush = new SolidBrush(Color.White);
    }
    public override void Render(Size resolution, Graphics container)
    {
        container.FillRectangle(
            keyBrush,
            new Rectangle(Position, HitboxSize));
    }
}

public class PlayerProjectile : Projectile 
{
    const int SPEED = 15;

    public PlayerProjectile(Point startPosition, Size hitboxSize) : base(startPosition, hitboxSize)
    {
        HitboxSize = hitboxSize;

        Vector displacement = new Vector(SPEED, 0);
        Controls = new Control(new ConstantDisplacement(displacement));

        keyBrush = new SolidBrush(Color.White);
    }

    public override bool DetectCollision(GameObject other)
    {
        if (other is Player)
            return false;
        
        return base.DetectCollision(other);
    }
}
