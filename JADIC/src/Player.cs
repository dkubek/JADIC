using JADIC.Controls;
using System.Drawing;

/// <summary>
/// The Player class contains data and information about the player. It is a
/// special case of an game object with special functionality and specific
/// methods.
/// </summary>
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
        Lives = 3;

        brush = new SolidBrush(KeyColor);
    }

    public override void Update()
    {
        if (projectileCooldownCurrent != 0)
            projectileCooldownCurrent--;

        if (Lives > 0)
            base.Update();
    }

    /// <summary>
    /// Spawn projectile before the player. Projectile is always being shot
    /// from from before the player to the right. After firing projectile it
    /// goes on a cooldown.
    /// </summary>
    /// <returns>A new PlayerProjectile object.</returns>
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

    /// <summary>
    /// THe center of hitbox.
    /// </summary>
    /// <returns>Point of the center of the hitbox.</returns>
    private Point Center()
    {
        return new Point(
            Position.X + HitboxSize.Width / 2,
            Position.Y + HitboxSize.Height / 2);
    }

    public override void Render(Size resolution, Graphics container)
    {
        var trianglePoints = GenerateTrianglePoints();
        container.FillPolygon(brush, trianglePoints);
    }

    /// <summary>
    /// Array of points to be used with the FillPolygon method. Points 
    /// represent the player's spaseship.
    /// </summary>
    /// <returns>Array of Points.</returns>
    private Point[] GenerateTrianglePoints()
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
        // Ignore own projectiles.
        if (other is PlayerProjectile)
            return false;

        return base.DetectCollision(other);
    }
}

/// <summary>
/// PlayerProjectile contains logic and customization of the player's 
/// projectile.
/// </summary>
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
