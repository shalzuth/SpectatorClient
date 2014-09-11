using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
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
        }

        private void featuredGameList_SelectedIndexChanged(object sender, EventArgs e)
        {
            /*
            Dictionary<String, Object> selectedGame = 
                (Dictionary<String, Object>)Array.Find(
                featuredGames, 
                game => 
                    (((Dictionary<String, Object>)game)["gameId"]).ToString() == featuredGameList.SelectedItem.ToString());
            
            Dictionary<Object, Object> metadata = 
                SpectatorService.ChunkDownloader.DownloadMetaData(
                    selectedGame["platformId"].ToString(), selectedGame["gameId"].ToString());
            String stuff = "";
            foreach (Object key in (object[])selectedGame["participants"])
            {
                foreach (KeyValuePair<string, object> k in (Dictionary<String, Object>)key)
                {
                    featuredGameData.Text += k.Key + " : " + k.Value + "\n";
                }
            }*/
        }

        private void spectateButton_Click(object sender, EventArgs e)
        {
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
                        SpectatorService.SpectatorDownloader.DownloadMetaData(
                            (String)selectedGame["platformId"], ((int)selectedGame["gameId"]).ToString()))["gameEnded"])
                    {
                        SpectatorService.SpectatorDownloader.DownloadGameFiles(((int)selectedGame["gameId"]).ToString(),
                                               (String)selectedGame["platformId"],
                                               (String)((Dictionary<String, Object>)selectedGame["observers"])["encryptionKey"]);
                        Thread.Sleep(15000);
                    }
                });
                t.IsBackground = true;
                t.Start();
                Console.WriteLine(((int)selectedGame["gameId"]).ToString());
                Minimap m = new Minimap(((int)selectedGame["gameId"]).ToString(), (String)selectedGame["platformId"]);
                m.Show();
            }
        }
    }
}
