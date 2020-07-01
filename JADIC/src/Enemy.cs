using System;
using System.Collections.Generic;
using System.Drawing;

public class Enemy : GameObject
{
    const int ENEMY_SIZE = 70;

    private readonly Brush brush;
    public Enemy(Point startingPosition)
    {
        Position = startingPosition;
        HitboxSize = new Size(ENEMY_SIZE, ENEMY_SIZE);
        Controls = new Control();
        Lives = 1;

        KeyColor = ColorTranslator.FromHtml("#c70039");
        brush = new SolidBrush(KeyColor);
    } 

    public override void Render(Size resolution, Graphics container)
    {
        var diamondPoints = generateDiamondPoints();
        container.FillPolygon(brush, diamondPoints);
    }

    public override bool DetectCollision(GameObject other)
    {
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
