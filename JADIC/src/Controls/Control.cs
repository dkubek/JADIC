using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace JADIC.Controls
{
    /// <summary>
    /// The Control object is a collection of ControlElements. It oversees the
    /// correct sucession of individual control elements.
    /// </summary>
    public class Control
    {
        private readonly Queue<ControlElement> controlQueue;
        private ControlElement currentControl;

        public bool Ended = false;

        /// <summary>
        /// Create a dummy Control that does nothing.
        /// </summary>
        public Control() : this(new ControlElement()) { }

        /// <summary>
        /// Create Control object from one ControlElement.
        /// </summary>
        /// <param name="controlElement">Main ControlElement</param>
        public Control(ControlElement controlElement)
        {
            controlQueue = new Queue<ControlElement>();
            currentControl = controlElement;
            controlElement.isRunning = true;
        }

        /// <summary>
        /// Create Control from given list of control elements.
        /// </summary>
        /// <param name="controls">List of ControlElements.</param>
        public Control(List<ControlElement> controls)
        {
            if (controls.Count == 0)
            {
                Ended = true;
                return;
            }

            controlQueue = new Queue<ControlElement>(controls);

            currentControl = controlQueue.Dequeue();
            currentControl.isRunning = true;
        }

        /// <summary>
        /// Given current position return the next position based on the currently
        /// active ControlElement.
        /// </summary>
        /// <param name="currentPosition">Point of the current position.</param>
        /// <returns>Point of the next position.</returns>
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
            else
            {
                Ended = true;
            }

            return newPosition;
        }
    }

    /// <summary>
    /// ControlElement decides on the next position of an object based on
    /// it's current position.
    /// </summary>
    public class ControlElement
    {
        public bool isRunning = false;

        /// <summary>
        /// Given current position return the next position.
        /// </summary>
        /// <param name="currentPosition">Point of the current position</param>
        /// <returns>Point of the next position.</returns>
        public virtual Point NextPosition(Point currentPosition)
            => currentPosition;
    }

    /// <summary>
    /// ConstantDisplacement moves the object always by a given constant 
    /// displacement vector.
    /// </summary>
    public class ConstantDisplacement : ControlElement
    {
        private readonly Vector displacement;

        public ConstantDisplacement(Vector displacement)
        {
            this.displacement = displacement;
        }

        public override Point NextPosition(Point currentPosition)
            => Vector.Add(currentPosition, displacement);
    }

    /// <summary>
    /// LinearTransition moves object to the specified destination.
    /// </summary>
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

    /// <summary>
    /// Follow will generate next position so it will follow the specified
    /// GameObject.
    /// </summary>
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

    /// <summary>
    /// PlayerControl is a special ControlElement that generates new position
    /// based on the given state of player and pressed keys. It also specifies
    /// a static method for handlig key presses connected with player movement.
    /// </summary>
    public class PlayerControl : ControlElement
    {
        private readonly Player player;
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

        /// <summary>
        /// Update the player direction based on the key presses.
        /// </summary>
        /// <param name="player">The Player object to update</param>
        /// <param name="keyCode">The Key pressed.</param>
        /// <param name="release">Whether the key was released (true).</param>
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
}