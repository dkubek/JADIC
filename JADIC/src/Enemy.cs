using System;
using System.Collections.Generic;
using System.Drawing;

public class Enemy : GameObject
{
    const int ENEMY_SIZE = 70;
    const int PROJECTILE_COOLDOWN = 100;

    private int currentProjectileCooldown = PROJECTILE_COOLDOWN;
    private double PROJECTILE_CHANCE = 0.3;

    private readonly Brush brush;
    public Enemy(Point startingPosition)
    {
        Position = startingPosition;
        HitboxSize = new Size(ENEMY_SIZE, ENEMY_SIZE);
        Lives = 1;
        Score = 100;

        KeyColor = ColorTranslator.FromHtml("#c70039");
        brush = new SolidBrush(KeyColor);
    } 

    public override void Render(Size resolution, Graphics container)
    {
        var diamondPoints = generateDiamondPoints();
        container.FillPolygon(brush, diamondPoints);
    }

    public override List<GameObject> Action(World world)
    {
        var projectiles = new List<GameObject>();
        if (currentProjectileCooldown > 0)
        {
            currentProjectileCooldown--;
        }
        else
        {
            if (world.RandomGen.NextDouble() < PROJECTILE_CHANCE)
            {
                var projectile = SpawnProjectile(world.MainPlayer.Position);
                projectiles.Add(projectile);
                currentProjectileCooldown = PROJECTILE_COOLDOWN;
            }
        }

        return projectiles;
    }

    private EnemyProjectile SpawnProjectile(Point destination)
        => new EnemyProjectile(Position, destination);

    public override bool DetectCollision(GameObject other)
    {
        if (other is EnemyProjectile)
            return false;

        return base.DetectCollision(other);
    }

    private Point[] generateDiamondPoints()
    {
        Point top = new Point(
            Position.X + HitboxSize.Width / 2, Position.Y);
        Point right = new Point(
            Position.X + HitboxSize.Width, Position.Y + HitboxSize.Height / 2);
        Point bottom = new Point(
            Position.X + HitboxSize.Width / 2, Position.Y + HitboxSize.Height);
        Point left = new Point(
            Position.X, Position.Y + HitboxSize.Height / 2);

        Point[] points = { top, right, bottom, left };
        return points;
    }
}

public class EnemyProjectile : Projectile
{
    const int ENEMY_PROJECTILE_SPEED = 10;
    static Size hitboxSize = new Size(10, 10);

    public EnemyProjectile(Point startPosition, Point destination)
        : base(startPosition, hitboxSize)
    {
        Vector displacement = new Vector(startPosition, destination);
        displacement.Normalize();
        displacement.Multiply(ENEMY_PROJECTILE_SPEED);
        Controls = new Control(new ConstantDisplacement(displacement));

        keyBrush = new SolidBrush(Color.Red);
    }

    public override bool DetectCollision(GameObject other)
    {
        if (other is Enemy)
            return false;
        
        return base.DetectCollision(other);
    }
}
