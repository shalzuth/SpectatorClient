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
            this.savedGameList = new System.Windows.Forms.ListBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.riotAccount = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.searchSummoner = new System.Windows.Forms.TextBox();
            this.riotPw = new System.Windows.Forms.TextBox();
            this.regionBox = new System.Windows.Forms.ComboBox();
            this.label7 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // featuredGameList
            // 
            this.featuredGameList.DisplayMember = "gameId";
            this.featuredGameList.FormattingEnabled = true;
            this.featuredGameList.ItemHeight = 16;
            this.featuredGameList.Location = new System.Drawing.Point(12, 31);
            this.featuredGameList.Name = "featuredGameList";
            this.featuredGameList.Size = new System.Drawing.Size(120, 212);
            this.featuredGameList.TabIndex = 1;
            this.featuredGameList.ValueMember = "gameId";
            this.featuredGameList.SelectedIndexChanged += new System.EventHandler(this.featuredGameList_SelectedIndexChanged);
            // 
            // spectateButton
            // 
            this.spectateButton.Location = new System.Drawing.Point(13, 250);
            this.spectateButton.Name = "spectateButton";
            this.spectateButton.Size = new System.Drawing.Size(75, 23);
            this.spectateButton.TabIndex = 6;
            this.spectateButton.Text = "Spectate";
            this.spectateButton.UseVisualStyleBackColor = true;
            this.spectateButton.Click += new System.EventHandler(this.spectateButton_Click);
            // 
            // savedGameList
            // 
            this.savedGameList.FormattingEnabled = true;
            this.savedGameList.ItemHeight = 16;
            this.savedGameList.Location = new System.Drawing.Point(139, 31);
            this.savedGameList.Name = "savedGameList";
            this.savedGameList.Size = new System.Drawing.Size(124, 212);
            this.savedGameList.TabIndex = 2;
            this.savedGameList.SelectedIndexChanged += new System.EventHandler(this.savedGameList_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(14, 7);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(114, 17);
            this.label1.TabIndex = 5;
            this.label1.Text = "Featured Games";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(152, 7);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(97, 17);
            this.label2.TabIndex = 6;
            this.label2.Text = "Saved Games";
            // 
            // riotAccount
            // 
            this.riotAccount.Location = new System.Drawing.Point(269, 51);
            this.riotAccount.Name = "riotAccount";
            this.riotAccount.Size = new System.Drawing.Size(100, 22);
            this.riotAccount.TabIndex = 3;
            this.riotAccount.Enter += new System.EventHandler(this.summonerSearch_Focus);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(326, 7);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(116, 17);
            this.label3.TabIndex = 8;
            this.label3.Text = "Search for Game";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(267, 31);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(59, 17);
            this.label4.TabIndex = 9;
            this.label4.Text = "Account";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(389, 31);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(69, 17);
            this.label5.TabIndex = 10;
            this.label5.Text = "Password";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(270, 201);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(141, 17);
            this.label6.TabIndex = 11;
            this.label6.Text = "Summoner to Search";
            // 
            // searchSummoner
            // 
            this.searchSummoner.Location = new System.Drawing.Point(270, 221);
            this.searchSummoner.Name = "searchSummoner";
            this.searchSummoner.Size = new System.Drawing.Size(138, 22);
            this.searchSummoner.TabIndex = 5;
            this.searchSummoner.Enter += new System.EventHandler(this.summonerSearch_Focus);
            // 
            // riotPw
            // 
            this.riotPw.Location = new System.Drawing.Point(392, 51);
            this.riotPw.Name = "riotPw";
            this.riotPw.PasswordChar = '*';
            this.riotPw.Size = new System.Drawing.Size(100, 22);
            this.riotPw.TabIndex = 4;
            this.riotPw.Enter += new System.EventHandler(this.summonerSearch_Focus);
            // 
            // regionBox
            // 
            this.regionBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.regionBox.FormattingEnabled = true;
            this.regionBox.Location = new System.Drawing.Point(269, 96);
            this.regionBox.Name = "regionBox";
            this.regionBox.Size = new System.Drawing.Size(121, 24);
            this.regionBox.TabIndex = 12;
            this.regionBox.Enter += new System.EventHandler(this.summonerSearch_Focus);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(267, 76);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(53, 17);
            this.label7.TabIndex = 13;
            this.label7.Text = "Region";
            // 
            // MainWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(505, 283);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.regionBox);
            this.Controls.Add(this.riotPw);
            this.Controls.Add(this.searchSummoner);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.riotAccount);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.savedGameList);
            this.Controls.Add(this.spectateButton);
            this.Controls.Add(this.featuredGameList);
            this.Name = "MainWindow";
            this.Text = "Spectator";
            this.Load += new System.EventHandler(this.MainWindow_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox featuredGameList;
        private System.Windows.Forms.Button spectateButton;
        private System.Windows.Forms.ListBox savedGameList;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox riotAccount;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox searchSummoner;
        private System.Windows.Forms.TextBox riotPw;
        private System.Windows.Forms.ComboBox regionBox;
        private System.Windows.Forms.Label label7;

    }
}