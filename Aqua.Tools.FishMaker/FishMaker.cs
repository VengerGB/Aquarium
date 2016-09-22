namespace FishMaker
{
    using System;
    using System.Drawing;
    using System.IO;
    using System.Linq;
    using System.Windows.Forms;
    using Aqua.Core.UI;

    enum BodyMode
    {
        Body,
        Tail,
        Eye
    }

    public partial class FishMaker : Form
    {
        UIFishData fish = new UIFishData();
        private PointF? currentMousePosition;

        public FishMaker()
        {
            InitializeComponent();

            SetStyle(
               ControlStyles.AllPaintingInWmPaint |
               ControlStyles.UserPaint |
               ControlStyles.DoubleBuffer,
               true);

            this.BodyMode = BodyMode.Body;
        }

        private BodyMode _bodyMode;
        private BodyMode BodyMode
        {
            get
            {
                return this._bodyMode;
            }

            set
            {
                switch (value)
                {
                    case BodyMode.Body:
                        this.Text = "BodyMode.Body";
                        break;

                    case BodyMode.Tail:
                        this.Text = "BodyMode.Tail";
                        break;

                    case BodyMode.Eye:
                        this.Text = "BodyMode.Eye";
                        break;
                }

                this._bodyMode = value;
            }
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            switch(this.BodyMode)
            {
                case BodyMode.Body:
                    this.fish.Body.Add(new PointF(e.Location.X, e.Location.Y));
                    break;

                case BodyMode.Tail:
                    this.fish.Tail.Add(new PointF(e.Location.X, e.Location.Y));
                    break;

                case BodyMode.Eye:
                    this.fish.Eye = new PointF(e.Location.X, e.Location.Y);
                    break;
            }
            
            this.Invalidate();
            this.saveToolStripMenuItem.Enabled = this.ValidData();
            base.OnMouseDown(e);
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            this.currentMousePosition = e.Location;
            this.Invalidate();
            base.OnMouseMove(e);
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            this.currentMousePosition = null;
            base.OnMouseLeave(e);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            if (this.fish.Body.Count() >= 2)
            {
                e.Graphics.DrawLines(new Pen(Brushes.Red), this.fish.Body.ToArray());
            }

            if (this.fish.Body.Any() && this.BodyMode == BodyMode.Body && this.currentMousePosition.HasValue)
            {
                e.Graphics.DrawLine(new Pen(Brushes.Red), this.fish.Body.Last(), this.currentMousePosition.Value);
            }

            if (this.fish.Body.Any())
            {
                foreach (var body in this.fish.Body)
                {
                    e.Graphics.FillEllipse(Brushes.DarkRed, new RectangleF(body.X - 3, body.Y - 3, 6, 6));
                }
            }

            if (this.fish.Tail.Count() >= 2)
            {
                e.Graphics.DrawLines(new Pen(Brushes.Blue), this.fish.Tail.ToArray());
            }

            if (this.fish.Tail.Any() && this.BodyMode == BodyMode.Tail && this.currentMousePosition.HasValue)
            {
                e.Graphics.DrawLine(new Pen(Brushes.Blue), this.fish.Tail.Last(), this.currentMousePosition.Value);
            }

            if (this.fish.Tail.Any())
            {
                foreach (var tail in this.fish.Tail)
                {
                    e.Graphics.FillEllipse(Brushes.RoyalBlue, new RectangleF(tail.X - 3.0f, tail.Y - 3, 6, 6));
                }
            }

            e.Graphics.FillEllipse(Brushes.Black, new RectangleF(this.fish.Eye.X - 5, this.fish.Eye.Y - 5, 10, 10));

            base.OnPaint(e);
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.Delete)
            {
                switch (this.BodyMode)
                {
                    case BodyMode.Body:
                        this.fish.Body.Remove(this.fish.Body.Last());
                        break;

                    case BodyMode.Tail:
                        this.fish.Tail.Remove(this.fish.Tail.Last());
                        break;

                    case BodyMode.Eye:
                        this.fish.Eye = new PointF();
                        break;
                }

                this.Invalidate();
                return true;
            }

            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void SaveFishData(string fileName)
        {
            using (var fs = File.Create(fileName))
            {
                var translated = this.fish.TranslateToOrigin();
                translated.ToStream(fs);
            }
        }

        private bool ValidData()
        {
            if (this.fish.Body.Count() >= 2 &&
                this.fish.Tail.Count() >= 2 &&
                this.fish.Eye != new PointF())
            {
                return true;
            }

            return false;
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog save = new SaveFileDialog();
            save.Filter = "Fish Data|*.dat";

            if (save.ShowDialog() == DialogResult.OK)
            {
                this.SaveFishData(save.FileName);
            }
        }

        private void loadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog open = new OpenFileDialog();
            open.Filter = "Fish Data|*.dat";

            if (open.ShowDialog() == DialogResult.OK)
            {
                LoadFishData(open.FileName);
            }
        }

        private void LoadFishData(string fileName)
        {
            using (var fs = File.OpenRead(fileName))
            {
                var data = UIFishData.FromStream(fs);
                this.fish = data.TranslateToPoint(new PointF(this.ClientSize.Width/2, this.ClientSize.Height/2));
            }

            this.saveToolStripMenuItem.Enabled = this.ValidData();
        }

        private void clearCurrentToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            switch (this.BodyMode)
            {
                case BodyMode.Body:
                    this.fish.Body.Clear();
                    break;

                case BodyMode.Tail:
                    this.fish.Tail.Clear();
                    break;

                case BodyMode.Eye:
                    this.fish.Eye = new Point();
                    break;
            }

            this.Invalidate();
        }

        private void traceTailToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            this.BodyMode = BodyMode.Tail;
        }

        private void traceBodyToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            this.BodyMode = BodyMode.Body;
        }

        private void traceEyeToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            this.BodyMode = BodyMode.Eye;
        }

        private void loadToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            OpenFileDialog open = new OpenFileDialog();
            open.Filter = "Image|*.jpg,*.png";

            if (open.ShowDialog() == DialogResult.OK)
            {
                this.BackgroundImage = new Bitmap(open.FileName);
            }
        }
    }
}
