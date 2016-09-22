namespace Aqua.UI.Winforms
{
    using System.Drawing.Drawing2D;
    using System.Linq;
    using Aqua.Core.UI;
    using System;
    using System.Drawing;
    using System.IO;
    using System.Reflection;
    using System.Windows.Forms;
    using Aqua.Core.Interfaces;
    using Aqua.Types;

    public partial class AquariumForm : Form
    {
        private readonly Bitmap background;
        private readonly UiAquarium fishTank;
        private Timer dataSaveTimer;
        private Timer uiRefreshTimer;
        IManualPeerList manualPeerList = Aqua.Core.Utils.IocContainer.Get<IManualPeerList>();
        private DateTime lastDraw = DateTime.UtcNow;

        private Point? dragStart;
        private Point? dragEnd;

        public AquariumForm()
        {
            InitializeComponent();

            SetStyle(
                ControlStyles.AllPaintingInWmPaint |
                ControlStyles.UserPaint |
                ControlStyles.DoubleBuffer,
                true);

            background = LoadBackground("Background_1280x800.jpg");

            fishTank = new UiAquarium(ClientSize);

            uiRefreshTimer = new Timer { Interval = 50 };
            uiRefreshTimer.Tick += (evt, obj) =>
                                   {
                                       fishTank.Process();
                                       Invalidate();
                                       Update();
                                   };

            dataSaveTimer = new Timer { Interval = 5 * 1000 };
            dataSaveTimer.Tick += (sender, args) => fishTank.Save();

            uiRefreshTimer.Start();
            dataSaveTimer.Start();

            Closing += (sender, args) => fishTank.Quit();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            e.Graphics.CompositingMode = CompositingMode.SourceCopy;
            e.Graphics.DrawImage(background, new Rectangle(new Point(), ClientSize));
            e.Graphics.CompositingMode = CompositingMode.SourceOver;

            var textFont = new Font("Segoe UI Light", 15, FontStyle.Bold);

            fishTank.Draw(e.Graphics);
            e.Graphics.FillRectangle(new SolidBrush(Color.FromArgb(fishTank.Dirtiness, 0, 240, 0)), ClientRectangle);

            e.Graphics.DrawString(
                string.Format("Peers: {0}", fishTank.PeerCount),
                textFont,
                new SolidBrush(Color.White),
                new PointF(10, 20));

            e.Graphics.DrawString(
                string.Format("Fish: {0}", fishTank.FishCount),
                textFont,
                new SolidBrush(Color.White),
                new PointF(10, 42));

            e.Graphics.DrawString(
                string.Format("Schools: {0}", fishTank.SchoolCount),
                textFont,
                new SolidBrush(Color.White),
                new PointF(10, 64));

            e.Graphics.DrawString(
               string.Format("Dirtiness: {0}", fishTank.Dirtiness),
               textFont,
               new SolidBrush(Color.YellowGreen),
               new PointF(10, 86));

            e.Graphics.DrawString(
              string.Format("Leaving: {0}", fishTank.Leaving),
              textFont,
              new SolidBrush(Color.DarkRed),
              new PointF(10, 108));

            e.Graphics.DrawString(
               string.Format("FPS: {0:0.0}", 1000 / (DateTime.UtcNow - this.lastDraw).TotalMilliseconds),
               textFont,
               new SolidBrush(Color.OrangeRed),
               new PointF(10, 130));

            var selectedFish = fishTank.SelectedFish;
            if (selectedFish.Count() == 1)
            {
                var singleFish = selectedFish.Single();

                e.Graphics.DrawString(
                    string.Format("School: {0}", singleFish.School.Name),
                    textFont,
                    new SolidBrush(Color.Yellow),
                    new PointF(10, 444));

                e.Graphics.DrawString(
                    string.Format("Selected: {0}", singleFish.Fish.Name),
                    textFont,
                    new SolidBrush(Color.Yellow),
                    new PointF(10, 466));

                e.Graphics.DrawString(
                    string.Format("Cleanliness: {0}", singleFish.Cleanliness),
                    textFont,
                    new SolidBrush(Color.Yellow),
                    new PointF(10, 488));

                //e.Graphics.DrawString(
                //   string.Format("Food: {0}", singleFish.Fish.Food),
                //   textFont,
                //   new SolidBrush(Color.Yellow),
                //   new PointF(10, 510));

                e.Graphics.DrawString(
                    string.Format("Speed: {0}", singleFish.Speed),
                    textFont,
                    new SolidBrush(Color.Yellow),
                    new PointF(10, 510));

                e.Graphics.DrawString(
                    string.Format("Size: {0}", singleFish.BodySize),
                    textFont,
                    new SolidBrush(Color.Yellow),
                    new PointF(10, 532));

                e.Graphics.DrawString(
                    string.Format("Birthplace: {0}", singleFish.Fish.BirthPlace),
                    textFont,
                    new SolidBrush(Color.Yellow),
                    new PointF(10, 554));
            }
            else if (selectedFish.Count() > 1)
            {
                e.Graphics.DrawString(
                    string.Format("Selected: {0}", string.Join(", ",  selectedFish.Select(f => f.Fish.Name))),
                    textFont,
                    new SolidBrush(Color.Yellow),
                    new PointF(10, 466));
            }

            if (this.manualPeerList.HostData != null)
            {
                e.Graphics.DrawString(
                    string.Format("Direct Connect: {0}:{1}", this.manualPeerList.HostData.Address, this.manualPeerList.HostData.Port),
                    textFont,
                    new SolidBrush(Color.Orange),
                    new PointF(850, 554));
            }

            if (this.dragStart.HasValue)
            {
                e.Graphics.DrawRectangle(new Pen(Brushes.Yellow), 
                                         new Rectangle(Math.Min(this.dragStart.Value.X, this.dragEnd.Value.X),
                                                       Math.Min(this.dragStart.Value.Y, this.dragEnd.Value.Y),
                                                       Math.Abs(this.dragStart.Value.X - this.dragEnd.Value.X),
                                                       Math.Abs(this.dragStart.Value.Y - this.dragEnd.Value.Y)));
            }

            this.lastDraw = DateTime.UtcNow;
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            this.dragStart = e.Location;
            this.dragEnd = e.Location;
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            var box = new Rectangle(Math.Min(this.dragStart.Value.X, this.dragEnd.Value.X),
                Math.Min(this.dragStart.Value.Y, this.dragEnd.Value.Y),
                Math.Abs(this.dragStart.Value.X - this.dragEnd.Value.X),
                Math.Abs(this.dragStart.Value.Y - this.dragEnd.Value.Y));

            if (box.Height < 2 && box.Height < 2)
            {
                fishTank.SelectFishAt(e.Location, (ModifierKeys & Keys.Control) == Keys.Control);
            }
            else
            {
                fishTank.SelectFishInside(box, (ModifierKeys & Keys.Control) == Keys.Control);
            }

            this.dragStart = null;
            this.dragEnd = null;
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            this.dragEnd = e.Location;
            fishTank.AvoidPosition(e.Location);
            base.OnMouseMove(e);
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            fishTank.AvoidPosition(null);
            base.OnMouseLeave(e);
        }

        private Bitmap LoadBackground(string resourceName)
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            string resource = "Aqua.UI.Winforms." + resourceName;

            using (Stream stream = assembly.GetManifestResourceStream(resource))
            {
                return new Bitmap(stream);
            }
        }

        private void breedToolStripMenuItem_Click(object sender, EventArgs e)
        {
            fishTank.BreedSelected();
        }

        private void sendToolStripMenuItem_Click(object sender, EventArgs e)
        {
            fishTank.SendSelected();
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == (Keys.Control | Keys.B))
            {
                fishTank.BreedSelected();
                return true;
            }

            if (keyData == (Keys.Control | Keys.S))
            {
                fishTank.SendSelected();
                return true;
            }

            if (keyData == (Keys.Control | Keys.A))
            {
                fishTank.SelectAll();
                return true;
            }

            if (keyData == (Keys.Control | Keys.R))
            {
                fishTank.Deactivate();
                return true;
            }

            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void addToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var peer = new Peer();
            if (peer.ShowDialog() == DialogResult.OK)
            {
                this.manualPeerList.AddOrUpdate(new ManualPeer { Address = peer.Address, Port = peer.Port });
            }
        }

        private void removeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            fishTank.Deactivate();
        }

        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {
            fishTank.Clean();
        }

        private void freeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.freeToolStripMenuItem.Checked = true;
            this.manualToolStripMenuItem.Checked = false;
        }

        private void manualToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.freeToolStripMenuItem.Checked = false;
            this.manualToolStripMenuItem.Checked = true;
        }

        private void copyLocalAddressToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(
                string.Format("{0}:{1}", 
                this.manualPeerList.HostData.Address, 
                this.manualPeerList.HostData.Port));
        }

        private void exitToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}