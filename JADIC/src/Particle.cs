using JADIC.Controls;
using System.Drawing;

/// <summary>
/// Generic Particle object.
/// </summary>
public class Particle : IDrawable
{
    private Point position;
    private Size size;
    private readonly Control controls;
    private readonly Brush keyBrush;

    public Point Position { get => position; }

    /// <summary>
    /// Create new generic Particle.
    /// </summary>
    /// <param name="startLocation">Point of the starting location.</param>
    /// <param name="size">Size of the particle.</param>
    /// <param name="controls">Control object for the particle.</param>
    /// <param name="color">Color of the particle.</param>
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
