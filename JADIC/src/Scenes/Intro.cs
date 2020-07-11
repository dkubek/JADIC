using System.Drawing;
using System.Windows.Forms;

namespace JADIC.Scenes
{
    /// <summary>
    /// The title screen.
    /// </summary>
    public class Intro : Scene
    {
        const int BLINK_INTERVAL = 10;
        readonly Font messageFont = new Font("Gadugi", 20, FontStyle.Italic);

        private int timer = 0;
        private bool showingMessage = true;

        public Intro(World world) : base(world) { }

        public override void Initialize() { }

        public override void Update()
        {
            timer++;
            if (timer >= BLINK_INTERVAL)
            {
                showingMessage = !showingMessage;
                timer = 0;
            }
        }

        public override void HandleKeys(Keys keycode, bool release)
        {
            if (keycode == Keys.Enter && !release)
            {
                isRunning = false;
            }
        }

        public override void Render(Size resolution, Graphics container)
        {
            base.Render(resolution, container);

            RenderLogo(resolution, container);
            RenderWASD(container);
            RenderSpacebar(container);

            if (showingMessage)
                RenderMessage(container);

        }

        private static void RenderLogo(Size resolution, Graphics container)
        {
            int sx = resolution.Width / 2;
            int sy = resolution.Height / 2;

            var logo = Properties.Resources.JADIC_Logo;
            double logoScale = 0.5;
            Size logoSize = new Size(
                (int)(logo.Width * logoScale),
                (int)(logo.Height * logoScale));

            Rectangle logoDimensions = new Rectangle(
                sx - logoSize.Width / 2, sy - logoSize.Height / 2 - 100,
                logoSize.Width, logoSize.Height);

            container.DrawImage(logo, logoDimensions);
        }

        private static void RenderWASD(Graphics container)
        {
            var wasd = Properties.Resources.WASD;
            Rectangle wasdRect = new Rectangle(
                270, 520,
                wasd.Width, wasd.Height);
            container.DrawImage(wasd, wasdRect);
        }

        private static void RenderSpacebar(Graphics container)
        {
            var spacebar = Properties.Resources.Spacebar;
            Rectangle spacebarRect = new Rectangle(
                570, 575,
                spacebar.Width, spacebar.Height);
            container.DrawImage(spacebar, spacebarRect);
        }

        private void RenderMessage(Graphics container)
        {
            container.DrawString(
                "Press ENTER to continue",
                messageFont,
                new SolidBrush(Color.White),
                450, 450);
        }
    }
}