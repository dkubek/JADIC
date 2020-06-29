using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

public class ControlElement
{
    public bool isRunning = false;

    public virtual void Move(GameObject go)
    {
    }
}

public class Control
{
    private Queue<ControlElement> controls;
    private ControlElement currentControl;

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

    public void Move(GameObject go)
    {
        if (currentControl.isRunning)
        {
            currentControl.Move(go);
        }
        else if (controls.Count != 0)
        {
            currentControl = controls.Dequeue();
            currentControl.isRunning = true;
            currentControl.Move(go);
        }
    }
}

public class ConstantDisplacement : ControlElement
{
    private Vector displacement;

    public ConstantDisplacement(Vector displacement)
    {
        this.displacement = displacement;
    }

    public override void Move(GameObject go)
    {
        go.Location = Vector.Add(go.Location, displacement);
    }
}

public class LinearTransition : ControlElement
{
    const int TransitionSpeed = 3;
    private Point dest;

    public LinearTransition(Point destination)
    {
        dest = destination;
    }

    public override void Move(GameObject go)
    {
        Vector displacement = new Vector(go.Location, dest);

        if (displacement.Size() <= TransitionSpeed)
        {
            go.Location = dest;
            isRunning = false;
        }
        else
        {
            displacement.Normalize();
            displacement.Multiply(TransitionSpeed);
            go.Location = Vector.Add(go.Location, displacement);
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
    public override void Move(GameObject go)
    {
        Point newLocation = new Point(
            go.Location.X + player.HorizontalSign * player.SPEED,
            go.Location.Y + player.VerticalSign * player.SPEED);

        go.Location = StayWithinBounds(newLocation);
    }

    private Point StayWithinBounds(Point location)
    {
        int rightBound =
            player.BoundingBox.Width + player.BoundingBox.X - player.Hitbox.Width;
        int bottomBound =
            player.BoundingBox.Height + player.BoundingBox.Y - player.Hitbox.Height;

        int boundedX = Math.Min(
            rightBound,
            Math.Max(location.X, player.BoundingBox.X));
        int boundedY = Math.Min(
            bottomBound,
            Math.Max(location.Y, player.BoundingBox.Y));

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
