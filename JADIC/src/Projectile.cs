using System.Drawing;

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
