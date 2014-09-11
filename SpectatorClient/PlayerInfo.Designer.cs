namespace SpectatorClient
{
    partial class PlayerInfo
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.champIcon = new System.Windows.Forms.PictureBox();
            this.summonerName = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.champIcon)).BeginInit();
            this.SuspendLayout();
            // 
            // champIcon
            // 
            this.champIcon.Location = new System.Drawing.Point(3, 3);
            this.champIcon.Name = "champIcon";
            this.champIcon.Size = new System.Drawing.Size(20, 20);
            this.champIcon.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.champIcon.TabIndex = 0;
            this.champIcon.TabStop = false;
            // 
            // summonerName
            // 
            this.summonerName.AutoSize = true;
            this.summonerName.Location = new System.Drawing.Point(29, 4);
            this.summonerName.Name = "summonerName";
            this.summonerName.Size = new System.Drawing.Size(0, 17);
            this.summonerName.TabIndex = 1;
            // 
            // PlayerInfo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.summonerName);
            this.Controls.Add(this.champIcon);
            this.Name = "PlayerInfo";
            this.Size = new System.Drawing.Size(342, 26);
            this.Load += new System.EventHandler(this.PlayerInfo_Load);
            ((System.ComponentModel.ISupportInitialize)(this.champIcon)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public System.Windows.Forms.PictureBox champIcon;
        public System.Windows.Forms.Label summonerName;
    }
}
