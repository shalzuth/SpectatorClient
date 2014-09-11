namespace SpectatorClient
{
    partial class MainWindow
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
            this.featuredGameList = new System.Windows.Forms.ListBox();
            this.spectateButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // featuredGameList
            // 
            this.featuredGameList.DisplayMember = "gameId";
            this.featuredGameList.FormattingEnabled = true;
            this.featuredGameList.ItemHeight = 16;
            this.featuredGameList.Location = new System.Drawing.Point(12, 12);
            this.featuredGameList.Name = "featuredGameList";
            this.featuredGameList.Size = new System.Drawing.Size(120, 212);
            this.featuredGameList.TabIndex = 1;
            this.featuredGameList.ValueMember = "gameId";
            this.featuredGameList.SelectedIndexChanged += new System.EventHandler(this.featuredGameList_SelectedIndexChanged);
            // 
            // spectateButton
            // 
            this.spectateButton.Location = new System.Drawing.Point(13, 231);
            this.spectateButton.Name = "spectateButton";
            this.spectateButton.Size = new System.Drawing.Size(75, 23);
            this.spectateButton.TabIndex = 3;
            this.spectateButton.Text = "Spectate";
            this.spectateButton.UseVisualStyleBackColor = true;
            this.spectateButton.Click += new System.EventHandler(this.spectateButton_Click);
            // 
            // MainWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1170, 396);
            this.Controls.Add(this.spectateButton);
            this.Controls.Add(this.featuredGameList);
            this.Name = "MainWindow";
            this.Text = "MainWindow";
            this.Load += new System.EventHandler(this.MainWindow_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListBox featuredGameList;
        private System.Windows.Forms.Button spectateButton;

    }
}