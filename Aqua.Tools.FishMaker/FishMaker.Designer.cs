namespace FishMaker
{
    partial class FishMaker
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FishMaker));
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.loadToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.dataToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.clearCurrentToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.traceTailToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.traceBodyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.traceEyeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.imagesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.loadToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.dataToolStripMenuItem,
            this.imagesToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(438, 24);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.saveToolStripMenuItem,
            this.loadToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // saveToolStripMenuItem
            // 
            this.saveToolStripMenuItem.Enabled = false;
            this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            this.saveToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.saveToolStripMenuItem.Text = "Save";
            this.saveToolStripMenuItem.Click += new System.EventHandler(this.saveToolStripMenuItem_Click);
            // 
            // loadToolStripMenuItem
            // 
            this.loadToolStripMenuItem.Name = "loadToolStripMenuItem";
            this.loadToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.loadToolStripMenuItem.Text = "Load";
            this.loadToolStripMenuItem.Click += new System.EventHandler(this.loadToolStripMenuItem_Click);
            // 
            // dataToolStripMenuItem
            // 
            this.dataToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.clearCurrentToolStripMenuItem,
            this.traceBodyToolStripMenuItem,
            this.traceTailToolStripMenuItem,
            this.traceEyeToolStripMenuItem});
            this.dataToolStripMenuItem.Name = "dataToolStripMenuItem";
            this.dataToolStripMenuItem.Size = new System.Drawing.Size(43, 20);
            this.dataToolStripMenuItem.Text = "Data";
            // 
            // clearCurrentToolStripMenuItem
            // 
            this.clearCurrentToolStripMenuItem.Name = "clearCurrentToolStripMenuItem";
            this.clearCurrentToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.clearCurrentToolStripMenuItem.Text = "Clear current";
            this.clearCurrentToolStripMenuItem.Click += new System.EventHandler(this.clearCurrentToolStripMenuItem_Click_1);
            // 
            // traceTailToolStripMenuItem
            // 
            this.traceTailToolStripMenuItem.Name = "traceTailToolStripMenuItem";
            this.traceTailToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.traceTailToolStripMenuItem.Text = "Trace Tail";
            this.traceTailToolStripMenuItem.Click += new System.EventHandler(this.traceTailToolStripMenuItem_Click_1);
            // 
            // traceBodyToolStripMenuItem
            // 
            this.traceBodyToolStripMenuItem.Name = "traceBodyToolStripMenuItem";
            this.traceBodyToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.traceBodyToolStripMenuItem.Text = "Trace Body";
            this.traceBodyToolStripMenuItem.Click += new System.EventHandler(this.traceBodyToolStripMenuItem_Click_1);
            // 
            // traceEyeToolStripMenuItem
            // 
            this.traceEyeToolStripMenuItem.Name = "traceEyeToolStripMenuItem";
            this.traceEyeToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.traceEyeToolStripMenuItem.Text = "Trace Eye";
            this.traceEyeToolStripMenuItem.Click += new System.EventHandler(this.traceEyeToolStripMenuItem_Click_1);
            // 
            // imagesToolStripMenuItem
            // 
            this.imagesToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.loadToolStripMenuItem1});
            this.imagesToolStripMenuItem.Name = "imagesToolStripMenuItem";
            this.imagesToolStripMenuItem.Size = new System.Drawing.Size(57, 20);
            this.imagesToolStripMenuItem.Text = "Images";
            // 
            // loadToolStripMenuItem1
            // 
            this.loadToolStripMenuItem1.Name = "loadToolStripMenuItem1";
            this.loadToolStripMenuItem1.Size = new System.Drawing.Size(152, 22);
            this.loadToolStripMenuItem1.Text = "Load";
            this.loadToolStripMenuItem1.Click += new System.EventHandler(this.loadToolStripMenuItem1_Click);
            // 
            // FishMaker
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = global::FishMaker.Properties.Resources.Attachment_1;
            this.ClientSize = new System.Drawing.Size(438, 416);
            this.Controls.Add(this.menuStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "FishMaker";
            this.Text = "Fish Maker";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem loadToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem dataToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem clearCurrentToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem traceTailToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem traceBodyToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem traceEyeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem imagesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem loadToolStripMenuItem1;
    }
}

