using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;

namespace JADIC.Scenes
{
    /// <summary>
    /// The "game over" message is displayed.
    /// </summary>
    public class GameOver : Scene
    {
        readonly Font gameOverMessageFont = 
            new Font("Gadugi", 120, FontStyle.Bold);
        readonly string gameOverMessage = "GAME OVER";

        readonly Font restartMessageFont = 
            new Font("Gadugi", 20);
        readonly string restartMessage = "Press ENTER to restart";

        readonly Brush messageBrush = new SolidBrush(Color.White);

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
            RenderRestartMessage(resolution, container);
        }

        private void RenderGameOverMessage(Size resolution, Graphics container)
        {
            int sx = resolution.Width / 2;
            int sy = resolution.Height / 2;

            var textSize = TextRenderer.MeasureText(
                gameOverMessage, gameOverMessageFont);

            Point messageLocation = new Point(
                sx - textSize.Width / 2, sy - textSize.Height / 2 - 100);

            container.DrawString(
                gameOverMessage,
                gameOverMessageFont,
                messageBrush,
                messageLocation);
        }

        private void RenderRestartMessage(Size resolution, Graphics container)
        {
            int sx = resolution.Width / 2;
            int sy = resolution.Height / 2;

            var textSize = TextRenderer.MeasureText(
                restartMessage, restartMessageFont);

            Point messageLocation = new Point(
                sx - textSize.Width / 2, sy - textSize.Height / 2 + 50);

            container.DrawString(
                restartMessage,
                restartMessageFont,
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