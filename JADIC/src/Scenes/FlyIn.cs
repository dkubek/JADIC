using System.Drawing;
using System.Windows.Forms;
using JADIC.Controls;

namespace JADIC.Scenes
{
    /// <summary>
    /// The player space ship flies in to the designated location.
    /// </summary>
    public class FlyIn : Scene
    {
        public FlyIn(World world) : base(world) { }

        public override void Initialize()
        {
            TransitionPlayerToCenterOfBoundingBox();
        }

        private void TransitionPlayerToCenterOfBoundingBox()
        {
            var destination = 
                CenterOfRectangle(MainWorld.MainPlayer.BoundingBox);
            var controls = 
                new Controls.Control(new LinearTransition(destination, 3));
            MainWorld.MainPlayer.Controls = controls;
        }

        private Point CenterOfRectangle(Rectangle rect)
        {
            int centerX = rect.X + rect.Width / 2;
            int centerY = rect.Y + rect.Height / 2;
            return new Point(centerX, centerY);
        }

        public override void Update()
        {
            UpdatePlayer();

            isRunning = !MainWorld.MainPlayer.Controls.Ended;
        }

        public override void HandleKeys(Keys keycode, bool release)
        {
            // Ignore player keys
        }
    }
}