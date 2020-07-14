using System;
using System.Drawing;
using System.Windows.Forms;

namespace JADIC
{
    public partial class JADIC : Form
    {
        private readonly Game mainGame;

        public JADIC()
        {
            InitializeComponent();
            mainTimer.Enabled = true;
            DoubleBuffered = true;

            Cursor.Hide();


            // Workaround to get the actual window size without toolbar
            Rectangle screenRectangle = 
                RectangleToScreen(ClientRectangle);
            Size windowAreaSize = 
                new Size(screenRectangle.Width, screenRectangle.Height);

            // Initialize main game
            mainGame = new Game(windowAreaSize);
        }

        private void MainTimer_Tick(object sender, EventArgs e)
        {
            mainGame.NextFrame();
            Invalidate();
        }

        private void JADIC_KeyDown(object sender, KeyEventArgs e)
        {
            mainGame.HandleKeys(e.KeyCode, false);
        }

        private void JADIC_KeyUp(object sender, KeyEventArgs e)
        {
            mainGame.HandleKeys(e.KeyCode, true);
        }

        private void JADIC_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.DrawImage(mainGame.MainWorld.Frame, 0, 0);
        }
    }
}
