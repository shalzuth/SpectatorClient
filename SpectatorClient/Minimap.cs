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
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

using SpectatorClient.SpectatorService;
using SpectatorClient.Game;
using SpectatorClient.Riot;
using SpectatorClient.Packets;

namespace SpectatorClient
{
    public partial class Minimap : Form
    {
        String GameId;
        Dictionary<String, List<Int32>> decodedIds = new Dictionary<String,List<Int32>>()
        {
            {"Chunk", new List<Int32>()},
            {"KeyFrame", new List<Int32>()},
        };
        Dictionary<String, FileSystemWatcher> fileWatchers = new Dictionary<String, FileSystemWatcher>()
        {
            {"Chunk", new FileSystemWatcher()},
            {"KeyFrame", new FileSystemWatcher()},
        };
        Dictionary<String, List<Packet>> packets = new Dictionary<String, List<Packet>>()
        {
            {"Chunk", new List<Packet>()},
            {"KeyFrame", new List<Packet>()},
        };
        List<PictureBox> champIcons = new List<PictureBox>();
        List<PlayerInfo> players = new List<PlayerInfo>();
        Boolean PlayersUpdated = false;
        public Minimap(String GameId)
        {
            this.GameId = GameId;
            InitializeComponent();
        }
        private void Minimap_Load(object sender, EventArgs e)
        {
            this.Text = GameId.ToString();
            Init();
            Thread decodeThread = new Thread(() =>
            {
                DecodeExistingFiles();
            });
            decodeThread.IsBackground = true;
            decodeThread.Start();
            Streamer();
            trackbarNoFocus1.Visible = true;
        }
        private void DecodeById(Int32 id, String type)
        {
            if (!decodedIds[type].Contains(id))
            {
                decodedIds[type].Add(id);
                lock (packets[type])
                {
                    packets[type].AddRange(SpectatorDecoder.DecodeFile(GameId, type, id));
                    packets[type].Sort((a, b) => a.time.CompareTo(b.time));
                }
                if (type.Equals("KeyFrame"))
                    UpdatePlayerInfo();
            }
        }
        private void DecodeExistingFiles()
        {
            DecodeExistingFiles("KeyFrame");
            DecodeExistingFiles("Chunk");
        }
        private void DecodeExistingFiles(String type)
        {
            if (Directory.Exists(GameId + @"\" + type))
            {
                foreach (String file in Directory.GetFiles(GameId + @"\" + type))
                {
                    Int32 id = Int32.Parse(file.Substring(file.IndexOf(type) + type.Length + 1));
                    DecodeById(id, type);
                }
            }
        }
        private void Streamer()
        {
            Streamer("KeyFrame");
            Streamer("Chunk");
        }
        private void Streamer(String type)
        {
            if (!Directory.Exists(GameId))
                Directory.CreateDirectory(GameId);
            if (!Directory.Exists(GameId + @"\" + type))
                Directory.CreateDirectory(GameId + @"\" + type);
            fileWatchers[type].Path = GameId + @"\" + type;
            fileWatchers[type].NotifyFilter = NotifyFilters.LastWrite;
            fileWatchers[type].Filter = "*";
            fileWatchers[type].Changed += new FileSystemEventHandler(OnFileCreated);
            fileWatchers[type].EnableRaisingEvents = true;
        }
        private void OnFileCreated(object source, FileSystemEventArgs e)
        {
            Int32 id = Int32.Parse(e.Name);
            Thread decodeThread = new Thread(() =>
            {
                DecodeById(id, Directory.GetParent(e.FullPath).Name);
            });
            decodeThread.IsBackground = true;
            decodeThread.Start();
        }
        private void Init()
        {
            for (int i = 0; i < 10; i++)
            {
                PictureBox champIcon = new PictureBox();
                champIcon.SizeMode = PictureBoxSizeMode.StretchImage;
                champIcon.Size = new Size(20, 20);
                champIcons.Add(champIcon);
                pictureBox1.Controls.Add(champIcon);
                PlayerInfo playerInfo = new PlayerInfo();
                playerInfo.Location = new Point(pictureBox1.Location.X + pictureBox1.Size.Width + 20, pictureBox1.Location.Y + i * 36);
                this.Controls.Add(playerInfo);
                players.Add(playerInfo);
            }
        }
        void UpdatePlayerInfo()
        {
            if (!PlayersUpdated)
            {
                lock (packets["KeyFrame"])
                {
                    foreach (Packet packet in packets["KeyFrame"].Where(packet => packet.header == (Byte)HeaderList.HeroSpawn))
                    {
                        HeroSpawn hero = new HeroSpawn(packet);
                        players[(Int32)hero.PlayerNumber].player.Update(hero.NetId, hero.PlayerName, hero.ChampionName, hero.PlayerNumber);
                        players[(Int32)hero.PlayerNumber].summonerName.Invoke((MethodInvoker)(() =>
                        {
                            champIcons[(Int32)hero.PlayerNumber].Image = players[(Int32)hero.PlayerNumber].champIcon.Image = DataDragon.GetChampImage(hero.ChampionName);
                            players[(Int32)hero.PlayerNumber].summonerName.Text = hero.PlayerName;
                        }));
                        PlayersUpdated = true;
                    }
                }
            }
        }
        private void SetPositions(Single time)
        {
            Int32 packetNum = packets["Chunk"].FindIndex(packet => packet.time > time) - 1;
            if (packetNum < 1)
                return;
            Dictionary<int, Vector> playerPositions = new Dictionary<int, Vector>();
            for (int i = packetNum; i > 0; i--)
            {
                if (packets["Chunk"][i].header == (Byte)HeaderList.Waypoints)
                {
                    Waypoints wp = new Waypoints(packets["Chunk"][i]);
                    foreach (Unit unit in wp.units)
                    {
                        PlayerInfo playerInfo = players.Find(pInfo => pInfo.player.NetId == unit.netId);
                        if (playerInfo != null)
                        {
                            Int32 playerNum = (Int32)playerInfo.player.Num;
                            if (!playerPositions.ContainsKey(playerNum))
                            {
                                playerPositions.Add(playerNum, unit.destCoords[0]);
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
        private Single UpdateItemsFromKeyFrames(Single time)
        {
            time = 60 * (Single)(Math.Ceiling(time / 60) + 1);
            List<UInt32> netidsadded = new List<UInt32>();
            Single updateTime = 0;
            for (int i = packets["KeyFrame"].Count - 1; i > 0; i--)
            {
                Packet packet = packets["KeyFrame"][i];
                if (packet.time > time)
                    continue;
                if (packet.header == (Byte)HeaderList.ChangeTarget_InventoryUpdate)
                {
                    updateTime = packet.time;
                    InventoryUpdate inventory = new InventoryUpdate(packet);
                    PlayerInfo playerInfo = players.Find(pInfo => pInfo.player.NetId == inventory.NetId);
                    if (playerInfo != null && !netidsadded.Contains(inventory.NetId) && inventory.IsInventory)
                    {
                        netidsadded.Add(inventory.NetId);
                        inventory.Items.ForEach(item =>
                        {
                            if (item.SlotId < 7)
                                playerInfo.player.Items[item.SlotId] = new Item(item);
                        });
                    }
                }
                if (netidsadded.Count() == 10)
                    break;
            }
            return updateTime - 60;
        }
        private void UpdateItemsFromChunks(Single time)
        {
            int updateTime = (int)Math.Floor(UpdateItemsFromKeyFrames(time));
            for (int i = 0; i < packets["Chunk"].Count; i++)
            {
                Packet packet = packets["Chunk"][i];
                if (packet.time < 1)
                    continue;
                if (packet.time < updateTime)
                    continue;
                if (packet.time > time)
                    break;
                if (packet.header == (Byte)HeaderList.BuyItem)
                {
                    BuyItem purchase = new BuyItem(packet);
                    PlayerInfo playerInfo = players.Find(pInfo => pInfo.player.NetId == purchase.NetId);
                    if (playerInfo != null)
                        playerInfo.player.Items[purchase.SlotId] = new Item(purchase);
                }
                if (packet.header == (Byte)HeaderList.SellItem)
                {
                    SellItem sell = new SellItem(packet);
                    PlayerInfo playerInfo = players.Find(pInfo => pInfo.player.NetId == sell.NetId);
                    if (playerInfo != null)
                    {
                        playerInfo.player.Items[(sell.Slot & 0x7F)].ItemId = 0;
                        playerInfo.player.Items[(sell.Slot & 0x7F)].Quantity = 0;
                    }
                }
                if (packet.header == (Byte)HeaderList.SwapItems)
                {
                    SwapItems swap = new SwapItems(packet);
                    PlayerInfo playerInfo = players.Find(player => player.player.NetId == swap.NetId);
                    if (playerInfo != null)
                    {
                        Item item1 = playerInfo.player.Items[swap.Slot1];
                        playerInfo.player.Items[swap.Slot1] = playerInfo.player.Items[swap.Slot2];
                        playerInfo.player.Items[swap.Slot2] = item1;
                    }
                }
            }
            foreach (PlayerInfo playerInfo in players)
            {
                for (int i = 0; i < playerInfo.player.Items.Count; i++)
                {
                    if (playerInfo.player.Items[i].ItemId > 0)
                        playerInfo.items[i].Image = Riot.DataDragon.GetItemImage(playerInfo.player.Items[i].ItemId.ToString());
                    else
                        playerInfo.items[i].Image = null;
                }
            }
        }
        private void trackbarNoFocus1_Scroll(object sender, EventArgs e)
        {
            lock (packets["Chunk"])
            {
                if (packets["Chunk"].Count > 0)
                {
                    trackbarNoFocus1.Minimum = (Int32)packets["Chunk"].First(packet => packet.time > 0).time;
                    trackbarNoFocus1.Maximum = (Int32)packets["Chunk"][packets["Chunk"].Count() - 1].time;
                    label1.Text = trackbarNoFocus1.Minimum + " / " + trackbarNoFocus1.Value + " / " + trackbarNoFocus1.Maximum;
                    Thread ParsePackets = new Thread(() =>
                    {
                        trackbarNoFocus1.Invoke((MethodInvoker)(() =>
                        {
                            SetPositions(trackbarNoFocus1.Value);
                            UpdateItemsFromChunks(trackbarNoFocus1.Value);
                        }));
                    });
                    ParsePackets.IsBackground = true;
                    ParsePackets.Start();
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            lock (packets["KeyFrame"])
            {
                if (File.Exists("keyframedmp.txt"))
                    File.Delete("keyframedmp.txt");
                foreach (Packet packet in packets["KeyFrame"])
                {
                    File.AppendAllText("keyframedmp.txt", packet.header.ToString("X") + " (" + packet.param.ToString("X") + ", " + packet.time + ") : " + BitConverter.ToString(packet.content) + "\n");
                }
            }
            lock (packets["Chunk"])
            {
                if (File.Exists("Chunkdmp.txt"))
                    File.Delete("Chunkdmp.txt");
                foreach (Packet packet in packets["Chunk"])
                {
                    File.AppendAllText("Chunkdmp.txt", packet.header.ToString("X") + " (" + packet.param.ToString("X") + ", " + packet.time + ") : " + BitConverter.ToString(packet.content) + "\n");
                }
            }
        }
    }
}
