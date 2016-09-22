namespace Aqua.UI.Winforms
{
    partial class Peer
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
            this.ipString = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.cancelButton = new System.Windows.Forms.Button();
            this.addButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // ipString
            // 
            this.ipString.Location = new System.Drawing.Point(12, 25);
            this.ipString.Name = "ipString";
            this.ipString.Size = new System.Drawing.Size(310, 20);
            this.ipString.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(70, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Peer Address";
            // 
            // cancelButton
            // 
            this.cancelButton.Location = new System.Drawing.Point(247, 51);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(75, 23);
            this.cancelButton.TabIndex = 4;
            this.cancelButton.Text = "Cancel";
            this.cancelButton.UseVisualStyleBackColor = true;
            this.cancelButton.Click += new System.EventHandler(this.cancelButton_Click);
            // 
            // addButton
            // 
            this.addButton.Location = new System.Drawing.Point(166, 51);
            this.addButton.Name = "addButton";
            this.addButton.Size = new System.Drawing.Size(75, 23);
            this.addButton.TabIndex = 5;
            this.addButton.Text = "Add";
            this.addButton.UseVisualStyleBackColor = true;
            this.addButton.Click += new System.EventHandler(this.button2_Click);
            // 
            // Peer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(328, 81);
            this.Controls.Add(this.addButton);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.ipString);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "Peer";
            this.Text = "Add a peer";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox ipString;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.Button addButton;
    }
}