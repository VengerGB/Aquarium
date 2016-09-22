namespace Aqua.UI.Winforms
{
    partial class AquariumForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AquariumForm));
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.fishToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.breedToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.sendToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.removeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.peersToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.addToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.copyLocalAddressToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tankToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
            this.modeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.freeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.manualToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.fishToolStripMenuItem,
            this.peersToolStripMenuItem,
            this.tankToolStripMenuItem,
            this.modeToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1184, 24);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.exitToolStripMenuItem.Text = "&Exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click_1);
            // 
            // fishToolStripMenuItem
            // 
            this.fishToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.breedToolStripMenuItem,
            this.sendToolStripMenuItem,
            this.removeToolStripMenuItem});
            this.fishToolStripMenuItem.Name = "fishToolStripMenuItem";
            this.fishToolStripMenuItem.Size = new System.Drawing.Size(40, 20);
            this.fishToolStripMenuItem.Text = "&Fish";
            // 
            // breedToolStripMenuItem
            // 
            this.breedToolStripMenuItem.Name = "breedToolStripMenuItem";
            this.breedToolStripMenuItem.Size = new System.Drawing.Size(117, 22);
            this.breedToolStripMenuItem.Text = "&Breed";
            this.breedToolStripMenuItem.Click += new System.EventHandler(this.breedToolStripMenuItem_Click);
            // 
            // sendToolStripMenuItem
            // 
            this.sendToolStripMenuItem.Name = "sendToolStripMenuItem";
            this.sendToolStripMenuItem.Size = new System.Drawing.Size(117, 22);
            this.sendToolStripMenuItem.Text = "&Send";
            this.sendToolStripMenuItem.Click += new System.EventHandler(this.sendToolStripMenuItem_Click);
            // 
            // removeToolStripMenuItem
            // 
            this.removeToolStripMenuItem.Name = "removeToolStripMenuItem";
            this.removeToolStripMenuItem.Size = new System.Drawing.Size(117, 22);
            this.removeToolStripMenuItem.Text = "&Remove";
            this.removeToolStripMenuItem.Click += new System.EventHandler(this.removeToolStripMenuItem_Click);
            // 
            // peersToolStripMenuItem
            // 
            this.peersToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addToolStripMenuItem,
            this.copyLocalAddressToolStripMenuItem});
            this.peersToolStripMenuItem.Name = "peersToolStripMenuItem";
            this.peersToolStripMenuItem.Size = new System.Drawing.Size(47, 20);
            this.peersToolStripMenuItem.Text = "Peers";
            // 
            // addToolStripMenuItem
            // 
            this.addToolStripMenuItem.Name = "addToolStripMenuItem";
            this.addToolStripMenuItem.Size = new System.Drawing.Size(178, 22);
            this.addToolStripMenuItem.Text = "Add";
            this.addToolStripMenuItem.Click += new System.EventHandler(this.addToolStripMenuItem_Click);
            // 
            // copyLocalAddressToolStripMenuItem
            // 
            this.copyLocalAddressToolStripMenuItem.Name = "copyLocalAddressToolStripMenuItem";
            this.copyLocalAddressToolStripMenuItem.Size = new System.Drawing.Size(178, 22);
            this.copyLocalAddressToolStripMenuItem.Text = "Copy Local Address";
            this.copyLocalAddressToolStripMenuItem.Click += new System.EventHandler(this.copyLocalAddressToolStripMenuItem_Click);
            // 
            // tankToolStripMenuItem
            // 
            this.tankToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem2});
            this.tankToolStripMenuItem.Name = "tankToolStripMenuItem";
            this.tankToolStripMenuItem.Size = new System.Drawing.Size(45, 20);
            this.tankToolStripMenuItem.Text = "Tank";
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(104, 22);
            this.toolStripMenuItem2.Text = "Clean";
            this.toolStripMenuItem2.Click += new System.EventHandler(this.toolStripMenuItem2_Click);
            // 
            // modeToolStripMenuItem
            // 
            this.modeToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.freeToolStripMenuItem,
            this.manualToolStripMenuItem});
            this.modeToolStripMenuItem.Name = "modeToolStripMenuItem";
            this.modeToolStripMenuItem.Size = new System.Drawing.Size(50, 20);
            this.modeToolStripMenuItem.Text = "Mode";
            // 
            // freeToolStripMenuItem
            // 
            this.freeToolStripMenuItem.Name = "freeToolStripMenuItem";
            this.freeToolStripMenuItem.Size = new System.Drawing.Size(114, 22);
            this.freeToolStripMenuItem.Text = "Free";
            this.freeToolStripMenuItem.Click += new System.EventHandler(this.freeToolStripMenuItem_Click);
            // 
            // manualToolStripMenuItem
            // 
            this.manualToolStripMenuItem.Checked = true;
            this.manualToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.manualToolStripMenuItem.Name = "manualToolStripMenuItem";
            this.manualToolStripMenuItem.Size = new System.Drawing.Size(114, 22);
            this.manualToolStripMenuItem.Text = "Manual";
            this.manualToolStripMenuItem.Click += new System.EventHandler(this.manualToolStripMenuItem_Click);
            // 
            // AquariumForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1184, 601);
            this.Controls.Add(this.menuStrip1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.MainMenuStrip = this.menuStrip1;
            this.MaximizeBox = false;
            this.Name = "AquariumForm";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "Aquarium Simulator 2014";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fishToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem breedToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem sendToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem peersToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem addToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem removeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem tankToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem modeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem freeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem manualToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem copyLocalAddressToolStripMenuItem;
    }
}

