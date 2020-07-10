using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace JADIC
{
    public partial class JADIC : Form
    {
        private Game mainGame;

        public JADIC()
        {
            InitializeComponent();
            mainTimer.Enabled = true;
            DoubleBuffered = true;

            // Workaround to get the actual window size without toolbar
            Rectangle screenRectangle = this.RectangleToScreen(this.ClientRectangle);
            Size windowAreaSize = new Size(screenRectangle.Width, screenRectangle.Height);

            // Main player initialization
            int edgeOffset = 50;
            Rectangle playerBounds = 
                new Rectangle(edgeOffset, edgeOffset, 
                windowAreaSize.Width / 2 - 2 * edgeOffset, 
                windowAreaSize.Height - 2 * edgeOffset);
            Point startingPosition = new Point(-100, 400);
            Player player =
                new Player(startingPosition, playerBounds);

            var controlElements = new List<ControlElement>(2);
            controlElements.Add(new LinearTransition(CenterOfRectangle(playerBounds)));
            controlElements.Add(new PlayerControl(player));
            Control playerControl = new Control(controlElements);

            player.Controls = playerControl;

            // Initialize main world
            World mainWorld = new World(windowAreaSize, player);

            // Initialize main game
            mainGame = new Game(new List<Scene>(), mainWorld);
        }

        static Point CenterOfRectangle(Rectangle rect)
        {
            int centerX = rect.X + rect.Width / 2;
            int centerY = rect.Y + rect.Height / 2;
            return new Point(centerX, centerY);
        }

        private void mainTimer_Tick(object sender, EventArgs e)
        {
            mainGame.NextFrame();
            Invalidate();
        }

        private void JADIC_KeyDown(object sender, KeyEventArgs e)
        {
            mainGame.HandleKey(e.KeyCode, false);
        }

        private void JADIC_KeyUp(object sender, KeyEventArgs e)
        {
            mainGame.HandleKey(e.KeyCode, true);
        }

        private void JADIC_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.DrawImage(mainGame.MainWorld.Frame, 0, 0);
        }
    }
}
