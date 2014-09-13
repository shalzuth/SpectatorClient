using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SpectatorClient
{
    public partial class MainWindow : Form
    {
        Object[] featuredGames;
        public MainWindow()
        {
            InitializeComponent();
        }

        private void MainWindow_Load(object sender, EventArgs e)
        {
            Thread featureThread = new Thread(() =>
            {
                featuredGames = SpectatorService.SpectatorDownloader.GetFeaturedGames();
                foreach (Dictionary<String, Object> game in featuredGames)
                    featuredGameList.Invoke((MethodInvoker)(() => { featuredGameList.Items.Add((game["gameId"]).ToString());}));
            });
            featureThread.IsBackground = false;
            featureThread.Start();

            foreach (String game in Directory.GetDirectories("."))
                savedGameList.Items.Add(game.Substring(2));
            var values = Enum.GetValues(typeof(Login.Region)).Cast<Login.Region>();
            foreach (var val in values)
            {
                regionBox.Items.Add(val);
            }
        }

        private void savedGameList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (savedGameList.SelectedItem != null)
            {
                featuredGameList.SelectedItem = null;
                label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8f, FontStyle.Bold);
                label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8f);
            }
            else
                label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8f);
        }

        private void featuredGameList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (featuredGameList.SelectedItem != null)
            {
                label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8f);
                label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8f, FontStyle.Bold);
                savedGameList.SelectedItem = null;
            }
            else
                label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8f);
        }

        private void summonerSearch_Focus(object sender, EventArgs e)
        {
            featuredGameList.SelectedItem = null;
            savedGameList.SelectedItem = null;
            label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8f, FontStyle.Bold);
        }
        private void spectateButton_Click(object sender, EventArgs e)
        {
            String gameId = featuredGameList.SelectedItem != null ? featuredGameList.SelectedItem.ToString() :
                savedGameList.SelectedItem != null ? savedGameList.SelectedItem.ToString() : String.Empty;
            if (featuredGameList.SelectedItem != null)
            {
                Dictionary<String, Object> selectedGame =
                    (Dictionary<String, Object>)Array.Find(
                        featuredGames,
                        game =>
                            (((Dictionary<String, Object>)game)["gameId"]).ToString() == featuredGameList.SelectedItem.ToString());
                Thread t = new Thread(() =>
                {
                    while (!(Boolean)
                        ((Dictionary<Object, Object>)
                        SpectatorService.SpectatorDownloader.DownloadMetaData(((int)selectedGame["gameId"]).ToString(), (String)selectedGame["platformId"]))["gameEnded"])
                    {
                        SpectatorService.SpectatorDownloader.DownloadGameFiles(((int)selectedGame["gameId"]).ToString(),
                                               (String)selectedGame["platformId"],
                                               (String)((Dictionary<String, Object>)selectedGame["observers"])["encryptionKey"]);
                        Thread.Sleep(15000);
                    }
                });
                t.IsBackground = true;
                t.Start();
            }
            if (!String.IsNullOrEmpty(gameId))
            {
                Minimap m = new Minimap(gameId);
                m.Show();
            }
            else
            {
                Login.LoginClient client = new Login.LoginClient(riotAccount.Text, riotPw.Text, (Login.Region)regionBox.SelectedItem);
                client.connection.OnLogin += GetSpectateInfo;
            }
        }
        void GetSpectateInfo(object sender, string username, string ipAddress)
        {
            Thread spectateThread = new Thread(async delegate()
            {
                Login.RiotObjects.Platform.Game.PlatformGameLifecycleDTO spectateInfo =
                    await ((Login.LoginClient)sender).connection.RetrieveInProgressSpectatorGameInfo(searchSummoner.Text);
                Console.WriteLine("key:" + spectateInfo.PlayerCredentials.ObserverEncryptionKey);
                Console.WriteLine("id:" + spectateInfo.Game.PlayerChampionSelections[0].ChampionId);
            });
            spectateThread.IsBackground = true;
            spectateThread.Start();
        }
    }
}
