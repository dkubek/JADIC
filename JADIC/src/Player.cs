using System.Drawing;

public class Player : GameObject 
{
    public int PROJECTILE_COOLDOWN = 25;

    public readonly int SPEED = 5;
    public int HorizontalSign = 0;
    public int VerticalSign = 0;
    public Rectangle BoundingBox;
    public Control Controls;
    public int Lives = 3;

    public int projectileCooldownCurrent = 0;

    private readonly Brush myBrush;
    public Player(
        Point startPosition, Size hitbox, Rectangle boundingBox)
    {
        Location = startPosition;
        Hitbox = hitbox;
        BoundingBox = boundingBox;

        // TODO: DEFAULT CONTROL

        myBrush = new SolidBrush(Color.White);
    }

    public override void Update()
    {
        if (projectileCooldownCurrent != 0)
        {
            projectileCooldownCurrent--;
        }

        Controls.Move(this);
    }

    public PlayerProjectile SpawnProjectile()
    {
        if (projectileCooldownCurrent == 0)
        {
            var center = Center();
            var projectile = new PlayerProjectile(
                new Point(center.X + Hitbox.Width / 2, center.Y));

            projectileCooldownCurrent = PROJECTILE_COOLDOWN;
            return projectile;
        }

        return null;
    }

    private Point Center()
    {
        return new Point(
            Location.X + Hitbox.Width / 2,
            Location.Y + Hitbox.Height / 2);
    }

    public override void Render(Size resolution, Graphics container)
    {
        var trianglePoints = generateTrianglePoints();
        container.FillPolygon(myBrush, trianglePoints);
    }

    private Point[] generateTrianglePoints()
    {
        Point center = Center();
        Point topLeft = new Point(
            center.X - Hitbox.Width / 2,
            center.Y - Hitbox.Height / 2);
        Point bottomLeft = new Point(
            center.X - Hitbox.Width / 2,
            center.Y + Hitbox.Height / 2);
        Point middleRight = new Point(
            center.X + Hitbox.Width / 2,
            center.Y);

        Point[] points = { topLeft, middleRight, bottomLeft };
        return points;
    }
}

public class PlayerProjectile : GameObject
{
    public readonly int SPEED = 15;
    public Control Controls;

    private readonly Brush myBrush;

    public PlayerProjectile(Point startPosition)
    {
        Location = startPosition;
        Hitbox = new Size(10, 10);

        Vector displacement = new Vector(SPEED, 0);
        Controls = new Control(new ConstantDisplacement(displacement));

        myBrush = new SolidBrush(Color.White);
    }

    public override void Update()
    {
        Controls.Move(this);
    }

    public override void Render(Size resolution, Graphics container)
    {
        container.FillRectangle(
            myBrush,
            new Rectangle(Location, Hitbox));
    }
}
