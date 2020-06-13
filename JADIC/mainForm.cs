using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace JADIC
{
    public partial class JADIC : Form
    {
        const int SPEED = 5;

        private World mainWorld;

        public JADIC()
        {
            InitializeComponent();
            mainTimer.Enabled = true;
            DoubleBuffered = true;

            mainWorld = new World(
                this.Size,
                new Player(new Point(0, 0), new Size(50, 50)));
        }

        private void mainTimer_Tick(object sender, EventArgs e)
        {
            // Render world
            mainWorld.Update();
            mainWorld.Render();
            Invalidate();
        }

        private void JADIC_KeyDown(object sender, KeyEventArgs e)
        {
            mainWorld.HandleKey(e.KeyCode, false);
        }

        private void JADIC_KeyUp(object sender, KeyEventArgs e)
        {
            mainWorld.HandleKey(e.KeyCode, true);
        }

        private void JADIC_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.DrawImage(mainWorld.Frame, 0, 0);
        }
    }
}
