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
    private Queue<ControlElement> controlQueue;
    private ControlElement currentControl;

    public Control() : this(new ControlElement()) { }

    public Control(ControlElement controlElement)
    {
        controlQueue = new Queue<ControlElement>();
        currentControl = controlElement;
        controlElement.isRunning = true;
    }
    public Control(List<ControlElement> controls)
    {
        controlQueue = new Queue<ControlElement>(controls);

        if (controls.Count == 0)
            throw new ArgumentException(
                "Empty control queue. At least one control element must be supplied");

        currentControl = controlQueue.Dequeue();
        currentControl.isRunning = true;
    }

    public Point NextPosition(Point currentPosition)
    {
        Point newPosition = currentPosition;
        if (currentControl.isRunning)
        {
            newPosition = currentControl.NextPosition(currentPosition);
        }
        else if (controlQueue.Count != 0)
        {
            currentControl = controlQueue.Dequeue();
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
    readonly int TransitionSpeed = 3;
    private Point destinationPosition;

    public LinearTransition(Point destination)
    {
        destinationPosition = destination;
    }

    public LinearTransition(Point destination, int speed)
    {
        destinationPosition = destination;
        TransitionSpeed = speed;
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

public class Follow : ControlElement
{
    public readonly GameObject FollowObject;
    public int FollowSpeed;

    public Follow(GameObject followObject, int followSpeed)
    {
        FollowObject = followObject;
        FollowSpeed = followSpeed;
    }

    public override Point NextPosition(Point currentPosition)
    {
        Vector displacement = new Vector(currentPosition, FollowObject.Position);

        if (displacement.Size() <= FollowSpeed)
        {
            return FollowObject.Position;
        }
        else
        {
            displacement.Normalize();
            displacement.Multiply(FollowSpeed);
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

    public static void HandlePlayerMovementKeys(
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
