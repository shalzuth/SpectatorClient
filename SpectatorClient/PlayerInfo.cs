using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using SpectatorClient.Game;

namespace SpectatorClient
{
    public partial class PlayerInfo : UserControl
    {
        public List<PictureBox> items = new List<PictureBox>();
        public Player player = new Player();

        public PlayerInfo()
        {
            InitializeComponent();
        }

        private void PlayerInfo_Load(object sender, EventArgs e)
        {
            for (int i = 0; i < 7; i++)
            {
                PictureBox item = new PictureBox();
                item.Size = new Size(20, 20);
                item.SizeMode = PictureBoxSizeMode.StretchImage;
                item.Location = new Point(this.Size.Width - (item.Size.Width*(7 - i)), champIcon.Location.Y);
                items.Add(item);
                this.Controls.Add(item);
            }
        }
        void Items_ListChanged(object sender, ListChangedEventArgs e)
        {
            for (int i = 0; i < items.Count; i++)
            {
                if (player.Items[i].ItemId > 0)
                    items[i].Image = Riot.DataDragon.GetItemImage(player.Items[i].ItemId.ToString());
            }
        }
    }
}
