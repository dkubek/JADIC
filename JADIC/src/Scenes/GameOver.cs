using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;

namespace JADIC.Scenes
{
    public class GameOver : Scene
    {
        readonly Font messageFont = 
            new Font("Gadugi", 120, FontStyle.Bold);
        readonly Brush messageBrush = new SolidBrush(Color.White);
        readonly string message = "GAME OVER";

        public GameOver(World world) : base(world) 
        {
        }

        private void RemoveGameObjects()
        {
            MainWorld.GameObjects = new List<GameObject>();
        }

        public override void Initialize() 
        {
            RemoveGameObjects();
        } 

        public override void Update() { }

        public override void Render(Size resolution, Graphics container)
        {
            RenderBackground(resolution, container);
            RenderGameObjects(resolution, container);
            RenderParticles(resolution, container);
            RenderOverlay(resolution, container);

            RenderGameOverMessage(resolution, container);
        }

        private void RenderGameOverMessage(Size resolution, Graphics container)
        {
            int sx = resolution.Width / 2;
            int sy = resolution.Height / 2;

            var textSize = TextRenderer.MeasureText(
                message, messageFont);

            Point messageLocation = new Point(
                sx - textSize.Width / 2, sy - textSize.Height / 2 - 20);

            container.DrawString(
                message,
                messageFont,
                messageBrush,
                messageLocation);
        }

        public override void HandleKeys(Keys keycode, bool release)
        {
            if (keycode == Keys.Enter && !release)
            {
                isRunning = false;
            }
        }
    }
}