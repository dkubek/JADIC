using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

public class ControlElement
{
    public bool isRunning = false;

    public virtual Point NextPosition(Point currentPosition)
        => currentPosition;
}

public class Control
{
    private Queue<ControlElement> controls;
    private ControlElement currentControl;

    public Control() : this(new ControlElement()) { }

    public Control(ControlElement controlElement)
    {
        controls = new Queue<ControlElement>();
        currentControl = controlElement;
        controlElement.isRunning = true;
    }
    public Control(Queue<ControlElement> controls)
    {
        this.controls = controls;

        if (controls.Count == 0)
            throw new ArgumentException(
                "Empty control queue. At least one control element must be supplied");

        currentControl = controls.Dequeue();
        currentControl.isRunning = true;
    }

    public Point NextPosition(Point currentPosition)
    {
        Point newPosition = currentPosition;
        if (currentControl.isRunning)
        {
            newPosition = currentControl.NextPosition(currentPosition);
        }
        else if (controls.Count != 0)
        {
            currentControl = controls.Dequeue();
            currentControl.isRunning = true;
            newPosition = currentControl.NextPosition(currentPosition);
        }

        return newPosition;
    }
}

public class ConstantDisplacement : ControlElement
{
    private Vector displacement;

    public ConstantDisplacement(Vector displacement)
    {
        this.displacement = displacement;
    }

    public override Point NextPosition(Point currentPosition)
        => Vector.Add(currentPosition, displacement);
}

public class LinearTransition : ControlElement
{
    const int TransitionSpeed = 3;
    private Point destinationPosition;

    public LinearTransition(Point destination)
    {
        destinationPosition = destination;
    }

    public override Point NextPosition(Point currentPosition)
    {
        Vector displacement = new Vector(currentPosition, destinationPosition);

        if (displacement.Size() <= TransitionSpeed)
        {
            isRunning = false;
            return destinationPosition;
        }
        else
        {
            displacement.Normalize();
            displacement.Multiply(TransitionSpeed);
            return Vector.Add(currentPosition, displacement);
        }
    }
}

public class PlayerControl : ControlElement
{
    private Player player;
    public PlayerControl(Player player)
    {
        this.player = player;
    }
    public override Point NextPosition(Point currentPosition)
    {
        Point newPosition = new Point(
            currentPosition.X + player.HorizontalSign * Player.SPEED,
            currentPosition.Y + player.VerticalSign * Player.SPEED);

        return StayWithinBounds(newPosition);
    }

    private Point StayWithinBounds(Point position)
    {
        int rightBound =
            player.BoundingBox.Width + player.BoundingBox.X - player.HitboxSize.Width;
        int bottomBound =
            player.BoundingBox.Height + player.BoundingBox.Y - player.HitboxSize.Height;

        int boundedX = Math.Min(
            rightBound,
            Math.Max(position.X, player.BoundingBox.X));
        int boundedY = Math.Min(
            bottomBound,
            Math.Max(position.Y, player.BoundingBox.Y));

        return new Point(boundedX, boundedY);
    }

    public static void HandlePlayerKeys(
        Player player, Keys keyCode, bool release)
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
