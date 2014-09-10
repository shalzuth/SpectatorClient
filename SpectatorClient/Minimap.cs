using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

using SpectatorClient.Packets;
using SpectatorClient.Game;

namespace SpectatorClient
{
    public partial class Minimap : Form
    {
        List<Packet> packets = new List<Packet>();
        List<PictureBox> champIcons = new List<PictureBox>();
        List<PictureBox> itemIcons = new List<PictureBox>();
        List<Player> players = new List<Player>();
        Dictionary<UInt32, byte[]> itemImages = new Dictionary<UInt32, byte[]>();
        public Minimap()
        {
            InitializeComponent();
        }

        private void Minimap_Load(object sender, EventArgs e)
        {
            //ChunkDownloader.DownloadFeaturedGamesChunks();
            new System.Threading.Thread(delegate()
            {
                while (true)
                {
                    Console.WriteLine("dl");

                    ChunkDownloader.DownloadGameChunks("1516118667", "NA1", "ZTml5KZFHohhn/PW8mY5wqTOmKG1m9HL");
                    System.Threading.Thread.Sleep(10000);
                }
            });//.Start();
            trackbarNoFocus1.Visible = false;
            List<String> games = Directory.GetDirectories(".", "*").ToList();
            foreach (String game in games)
            {
                if (!game.Contains("8667"))
                    continue;
                packets = ChunkDecoder.DecodeGameChunks(game);
                //foreach (Packet packet in packets)
                {
                    //Console.WriteLine(packet.param.ToString("X") + " : " + BitConverter.ToString(packet.content) + " @ " + packet.time);
                }
            }
            InitGame();
            trackbarNoFocus1.Visible = true;
        }
        private void InitGame()
        {
            foreach (Packet packet in packets)
            {
                if (packet.header == (Byte)HeaderList.HeroSpawn)
                {
                    HeroSpawn hero = new HeroSpawn(packet);
                    if (players.Count(player => player.NetId == hero.NetId) == 0)
                    {
                        players.Add(new Player(hero.NetId, hero.PlayerName, hero.ChampionName, hero.PlayerNumber));
                    }
                }
                if (packet.header == (Byte)HeaderList.PurchaseItem && packet.time > 1)
                {
                    PurchaseItem item = new PurchaseItem(packet);
                    WebClient client = new WebClient();
                    if (!itemImages.ContainsKey(item.ItemId))
                        itemImages.Add(item.ItemId, client.DownloadData("http://ddragon.leagueoflegends.com/cdn/4.15.1/img/item/" + item.ItemId + ".png"));
                }
            }
            List<Label> labels = new List<Label>();
            labels.Add(label2); labels.Add(label3); labels.Add(label4); labels.Add(label5); labels.Add(label6);
            labels.Add(label7); labels.Add(label8); labels.Add(label9); labels.Add(label10); labels.Add(label11);
            int p = 0;
            foreach (Player player in players)
            {
                labels[(int)player.Num].Text = player.Name + " : " + player.Champ;
                PictureBox champIcon = new PictureBox();

                WebClient Client = new WebClient();
                byte[] imageData = Client.DownloadData("http://ddragon.leagueoflegends.com/cdn/4.15.1/img/champion/" + player.Champ + ".png");
                MemoryStream stream = new MemoryStream(imageData);
                champIcon.Image = Image.FromStream(stream);
                stream.Close();

                champIcon.SizeMode = PictureBoxSizeMode.StretchImage;
                champIcon.Size = new Size(20, 20);
                champIcons.Add(champIcon);
                pictureBox1.Controls.Add(champIcon);
                int q = 0;
                foreach (Item item in player.Items)
                {
                    PictureBox itemIcon = new PictureBox();
                    itemIcon.SizeMode = PictureBoxSizeMode.StretchImage;
                    itemIcon.Size = new Size(20, 20);
                    itemIcon.Location = new Point(labels[p].Location.X + (q * 25) + 150, labels[p].Location.Y);
                    q++;
                    itemIcons.Add(itemIcon);
                    this.Controls.Add(itemIcon);
                }
                p++;
            }
        }
        private void SetPositions(Single time)
        {
            label1.Text = time + " / " + packets[packets.Count() - 1].time;
            int i = 0;
            for (i = 0; i < packets.Count(); i++)
            {
                if (packets[i].time > time)
                    break;
            }
            i--;
            Dictionary<int, Vector> playerPositions = new Dictionary<int, Vector>();
            for (; i > 0; i--)
            {
                if (packets[i].header == (Byte)HeaderList.Waypoints)
                {
                    Waypoints wp = new Waypoints(packets[i]);
                    foreach (Unit unit in wp.units)
                    {
                        if ((unit.netId - 0x40000019) < 10)
                        {
                            if (!playerPositions.ContainsKey((int)(unit.netId - 0x40000019)))
                            {
                                playerPositions.Add((int)(unit.netId - 0x40000019), unit.destCoords[0]);
                            }
                        }
                    }
                }
                if (playerPositions.Count() == 10)
                    break;
            }
            foreach (KeyValuePair<int, Vector> pair in playerPositions)
            {
                int x = -10 + (int)(pictureBox1.Width * ((float)(pair.Value.x * 2 + (14545 / 2)) / (float)(14545)));
                int y = -10 + pictureBox1.Height - (int)(pictureBox1.Height * ((float)(pair.Value.y * 2 + (14604 / 2)) / (float)(14604)));
                champIcons[pair.Key].Location = new Point(x, y);
            }
        }
        private void UpdateItems(Single time)
        {
            foreach (Player player in players)
            {
                foreach (Item item in player.Items)
                {
                    item.ItemId = 0;
                }
            }
            foreach (Packet packet in packets)
            {
                if (packet.time < 1)
                    continue;
                if (packet.time > time)
                    break;
                if (packet.header == (Byte)HeaderList.PurchaseItem)
                {
                    PurchaseItem purchase = new PurchaseItem(packet);
                    players[players.FindIndex(player => player.NetId == purchase.NetId)].Items[purchase.SlotId] = new Item(purchase);
                }
                if (packet.header == (Byte)HeaderList.SellItem)
                {
                    SellItem sell = new SellItem(packet);
                    if (sell.NetId >= 0x40000019)
                    {
                        Player tempPlayer = players[players.FindIndex(player => player.NetId == sell.NetId)];
                        tempPlayer.Items[(sell.Slot & 0x7F)].ItemId = 0;
                        tempPlayer.Items[(sell.Slot & 0x7F)].ItemCount = 0;
                        players[players.FindIndex(player => player.NetId == sell.NetId)] = tempPlayer;
                    }
                }
                if (packet.header == (Byte)HeaderList.SwapItems)
                {
                    SwapItems swap = new SwapItems(packet);
                    Player tempPlayer = players[players.FindIndex(player => player.NetId == swap.NetId)];
                    Item item1 = tempPlayer.Items[swap.Slot1];
                    Item item2 = tempPlayer.Items[swap.Slot2];
                    tempPlayer.Items[swap.Slot1] = item2;
                    tempPlayer.Items[swap.Slot2] = item1;
                    players[players.FindIndex(player => player.NetId == swap.NetId)] = tempPlayer;
                }
            }
            int p = 0;
            foreach (Player player in players)
            {
                int q = 0;
                foreach (Item item in player.Items)
                {
                    PictureBox icon = itemIcons[7 * p + q];
                    if (item.ItemId > 100)
                    {
                        //WebClient Client = new WebClient();
                        byte[] imageData = itemImages[item.ItemId];// Client.DownloadData("http://ddragon.leagueoflegends.com/cdn/4.15.1/img/item/" + item.ItemId + ".png");
                        MemoryStream stream = new MemoryStream(imageData);
                        icon.Image = Image.FromStream(stream);
                        stream.Close();
                    }
                    else
                    {
                        icon.Image = null;
                    }
                    itemIcons[7 * p + q] = icon;
                    q++;
                }
                p++;
            }
        }
        private void trackbarNoFocus1_Scroll(object sender, EventArgs e)
        {
            Single time = packets[packets.Count() - 1].time * trackbarNoFocus1.Value / 100;
            SetPositions(time);
            UpdateItems(time);
        }

    }
}
