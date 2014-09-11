namespace SpectatorClient
{
    partial class Minimap
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Minimap));
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.label1 = new System.Windows.Forms.Label();
            this.trackbarNoFocus1 = new SpectatorClient.TrackBarNoFocus();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackbarNoFocus1)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(0, 0);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(512, 512);
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(434, 522);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(46, 17);
            this.label1.TabIndex = 3;
            this.label1.Text = "label1";
            // 
            // trackbarNoFocus1
            // 
            this.trackbarNoFocus1.LargeChange = 10;
            this.trackbarNoFocus1.Location = new System.Drawing.Point(0, 518);
            this.trackbarNoFocus1.Maximum = 100;
            this.trackbarNoFocus1.Name = "trackbarNoFocus1";
            this.trackbarNoFocus1.Size = new System.Drawing.Size(428, 56);
            this.trackbarNoFocus1.TabIndex = 1;
            this.trackbarNoFocus1.TickStyle = System.Windows.Forms.TickStyle.None;
            this.trackbarNoFocus1.Scroll += new System.EventHandler(this.trackbarNoFocus1_Scroll);
            // 
            // Minimap
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(894, 548);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.trackbarNoFocus1);
            this.Controls.Add(this.pictureBox1);
            this.Name = "Minimap";
            this.Text = "test";
            this.Load += new System.EventHandler(this.Minimap_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackbarNoFocus1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox1;
        private TrackBarNoFocus trackbarNoFocus1;
        private System.Windows.Forms.Label label1;
    }
}

