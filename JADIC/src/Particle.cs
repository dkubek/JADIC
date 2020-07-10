using JADIC.Controls;
using System.Drawing;

/// <summary>
/// Generic Particle object.
/// </summary>
public class Particle : IDrawable
{
    private Point position;
    private Size size;
    private Control controls;
    private Brush keyBrush;

    public Point Position { get => position; }

    public Particle(
        Point startLocation, int size, Control controls, Color color)
    {
        position = startLocation;
        this.size = new Size(size, size);
        this.controls = controls;
        keyBrush = new SolidBrush(color);
    }

    public void Render(Size resolution, Graphics container)
    {
        UpdatePosition();

        container.FillRectangle(
            keyBrush,
            new Rectangle(position, size));
    }

    private void UpdatePosition()
    {
        position = controls.NextPosition(position);
    }
}
