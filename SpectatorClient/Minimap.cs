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
        String Platform;
        FileSystemWatcher watcher = new FileSystemWatcher();
        List<Int32> decodedChunkIds = new List<Int32>();
        List<Int32> decodedKeyFrameIds = new List<Int32>();
        List<Packet> chunkPackets = new List<Packet>();
        List<Packet> keyFramePackets = new List<Packet>();
        List<PictureBox> champIcons = new List<PictureBox>();
        List<PlayerInfo> players = new List<PlayerInfo>();
        Boolean PlayersUpdated = false;
        public Minimap(String GameId, String Platform)
        {
            this.Platform = Platform;
            this.GameId = GameId;
            InitializeComponent();
        }
        private void Minimap_Load(object sender, EventArgs e)
        {
            this.Text = GameId.ToString();
            Init();
            DecodeExistingKeyFrames();
            DecodeExistingChunks();
            KeyFrameStreamer();
            ChunkStreamer();
            trackbarNoFocus1.Visible = true;
        }
        private void DecodeExistingChunks()
        {
            Thread decode = new Thread(() =>
            {
                if (Directory.Exists(GameId + @"\Chunk"))
                {
                    foreach (String chunk in Directory.GetFiles(GameId + @"\Chunk"))
                    {
                        Int32 chunkId = Int32.Parse(chunk.Substring(chunk.IndexOf("Chunk") + 6));
                        if (!decodedChunkIds.Contains(chunkId))
                        {
                            decodedChunkIds.Add(chunkId);
                            lock (chunkPackets)
                            {
                                chunkPackets.AddRange(SpectatorDecoder.DecodeGameChunk(GameId, chunkId));
                                chunkPackets.Sort((a, b) => a.time.CompareTo(b.time));
                            }
                            UpdatePlayerInfo();
                        }
                    }
                }
            });
            decode.IsBackground = true;
            decode.Start();
        }
        private void DecodeExistingKeyFrames()
        {
            Thread decode = new Thread(() =>
            {
                if (Directory.Exists(GameId + @"\KeyFrame"))
                foreach (String keyFrame in Directory.GetFiles(GameId + @"\KeyFrame"))
                {
                    Int32 id = Int32.Parse(keyFrame.Substring(keyFrame.IndexOf("KeyFrame") + 9));
                    if (!decodedKeyFrameIds.Contains(id))
                    {
                        decodedKeyFrameIds.Add(id);
                        lock (keyFramePackets)
                        {
                            keyFramePackets.AddRange(SpectatorDecoder.DecodeGameKeyFrame(GameId, id));
                            keyFramePackets.Sort((a, b) => a.time.CompareTo(b.time));
                        }
                        foreach (Packet packet in keyFramePackets)
                        {
                            Console.WriteLine(packet.header + " (" + packet.param + ") : " + BitConverter.ToString(packet.content));
                        }
                        UpdatePlayerInfo();
                    }
                }
            });
            decode.IsBackground = true;
            decode.Start();
        }
        private void ChunkStreamer()
        {
            if (!Directory.Exists(GameId))
                Directory.CreateDirectory(GameId);
            if (!Directory.Exists(GameId + @"\Chunk"))
                Directory.CreateDirectory(GameId + @"\Chunk");
            watcher.Path = GameId + @"\Chunk";
            watcher.NotifyFilter = NotifyFilters.LastWrite;
            watcher.Filter = "*";
            watcher.Changed += new FileSystemEventHandler(OnChunkCreated);
            watcher.EnableRaisingEvents = true;
        }
        private void KeyFrameStreamer()
        {
            if (!Directory.Exists(GameId))
                Directory.CreateDirectory(GameId);
            if (!Directory.Exists(GameId + @"\KeyFrame"))
                Directory.CreateDirectory(GameId + @"\KeyFrame");
            watcher.Path = GameId + @"\KeyFrame";
            watcher.NotifyFilter = NotifyFilters.LastWrite;
            watcher.Filter = "*";
            watcher.Changed += new FileSystemEventHandler(OnKeyFrameCreated);
            watcher.EnableRaisingEvents = true;
        }
        private void OnKeyFrameCreated(object source, FileSystemEventArgs e)
        {
            if (!decodedKeyFrameIds.Contains(Int32.Parse(e.Name)))
            {
                decodedKeyFrameIds.Add(Int32.Parse(e.Name));
                lock (keyFramePackets)
                {
                    keyFramePackets.AddRange(SpectatorDecoder.DecodeGameChunk(GameId, Int32.Parse(e.Name)));
                    keyFramePackets.Sort((a, b) => a.time.CompareTo(b.time));
                }
                UpdatePlayerInfo();
            }
        }
        private void OnChunkCreated(object source, FileSystemEventArgs e)
        {
            Int32 chunkId = Int32.Parse(e.Name);
            if (!decodedChunkIds.Contains(chunkId))
            {
                decodedChunkIds.Add(chunkId);
                lock (chunkPackets)
                {
                    chunkPackets.AddRange(SpectatorDecoder.DecodeGameChunk(GameId, chunkId));
                    chunkPackets.Sort((a, b) => a.time.CompareTo(b.time));
                }
                UpdatePlayerInfo();
            }
        }
        private void Init()
        {
            Thread updateStartTime = new Thread(() =>
            {
                this.Invoke((MethodInvoker)(() =>
                {
                    this.Text = GameId.ToString() + " : " + ((Dictionary<Object, Object>)
                            SpectatorService.SpectatorDownloader.DownloadMetaData(Platform, GameId))["startTime"];
                }));
            });
            updateStartTime.IsBackground = false;
            updateStartTime.Start();
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
                lock (chunkPackets)
                {
                    foreach (Packet packet in chunkPackets.Where(packet => packet.header == (Byte)HeaderList.HeroSpawn))
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
            Int32 packetNum = chunkPackets.FindIndex(packet => packet.time > time) - 1;
            if (packetNum < 1)
                return;
            Dictionary<int, Vector> playerPositions = new Dictionary<int, Vector>();
            for (int i = packetNum; i > 0; i--)
            {
                if (chunkPackets[i].header == (Byte)HeaderList.Waypoints)
                {
                    Waypoints wp = new Waypoints(chunkPackets[i]);
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
        private void UpdateItems(Single time)
        {
            players.ForEach(playerInfo => playerInfo.player.Items.ForEach(item => item.ItemId = 0));
            for (int i = 0; i < chunkPackets.Count; i++)
            {
                Packet packet = chunkPackets[i];
                if (packet.time < 1)
                    continue;
                if (packet.time > time)
                    break;
                if (packet.header == (Byte)HeaderList.PurchaseItem)
                {
                    PurchaseItem purchase = new PurchaseItem(packet);
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
                        playerInfo.player.Items[(sell.Slot & 0x7F)].ItemCount = 0;
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
            lock (chunkPackets)
            {
                if (chunkPackets.Count > 0)
                {
                    Single beginTime = chunkPackets[0].time;
                    trackbarNoFocus1.Minimum = (Int32)chunkPackets[0].time;
                    trackbarNoFocus1.Maximum = (Int32)chunkPackets[chunkPackets.Count() - 1].time;
                    label1.Text = trackbarNoFocus1.Minimum + " / " + trackbarNoFocus1.Value + " / " + trackbarNoFocus1.Maximum;
                    Thread ParsePackets = new Thread(() =>
                    {
                        trackbarNoFocus1.Invoke((MethodInvoker)(() =>
                        {
                            SetPositions(trackbarNoFocus1.Value);
                            UpdateItems(trackbarNoFocus1.Value);
                        }));
                    });
                    ParsePackets.IsBackground = true;
                    ParsePackets.Start();
                }
            }
        }
    }
}
