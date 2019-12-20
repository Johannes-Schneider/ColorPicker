namespace ColorPicker.Executable.UserInterface
{
    partial class ColorPicker
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
            this.MagnifierHost = new System.Windows.Forms.Panel();
            this.SelectedColorLabel = new System.Windows.Forms.Label();
            this.SelectedColorDisplay = new System.Windows.Forms.Panel();
            this.SuspendLayout();
            // 
            // MagnifierHost
            // 
            this.MagnifierHost.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.MagnifierHost.Location = new System.Drawing.Point(3, 3);
            this.MagnifierHost.Name = "MagnifierHost";
            this.MagnifierHost.Size = new System.Drawing.Size(128, 128);
            this.MagnifierHost.TabIndex = 0;
            // 
            // SelectedColorLabel
            // 
            this.SelectedColorLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.SelectedColorLabel.Font = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.SelectedColorLabel.Location = new System.Drawing.Point(1, 131);
            this.SelectedColorLabel.Margin = new System.Windows.Forms.Padding(0);
            this.SelectedColorLabel.Name = "SelectedColorLabel";
            this.SelectedColorLabel.Size = new System.Drawing.Size(132, 22);
            this.SelectedColorLabel.TabIndex = 1;
            this.SelectedColorLabel.Text = "#FFFFFF";
            this.SelectedColorLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // SelectedColorDisplay
            // 
            this.SelectedColorDisplay.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.SelectedColorDisplay.Location = new System.Drawing.Point(1, 1);
            this.SelectedColorDisplay.Name = "SelectedColorDisplay";
            this.SelectedColorDisplay.Size = new System.Drawing.Size(132, 152);
            this.SelectedColorDisplay.TabIndex = 2;
            // 
            // ColorPicker
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(134, 154);
            this.Controls.Add(this.SelectedColorLabel);
            this.Controls.Add(this.MagnifierHost);
            this.Controls.Add(this.SelectedColorDisplay);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "ColorPicker";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "ColorPicker";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.MainWindow_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel MagnifierHost;
        private System.Windows.Forms.Label SelectedColorLabel;
        private System.Windows.Forms.Panel SelectedColorDisplay;
    }
}