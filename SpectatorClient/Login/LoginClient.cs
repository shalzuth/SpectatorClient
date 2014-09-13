using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Text.RegularExpressions;
using System.Runtime.InteropServices;

using System.Windows.Forms;

namespace SpectatorClient.Login
{
    class LoginClient
    {
        #region Initial Vars
        public RiotObjects.Platform.Clientfacade.Domain.LoginDataPacket loginPacket = new RiotObjects.Platform.Clientfacade.Domain.LoginDataPacket();
        public List<RiotObjects.Platform.Catalog.Champion.ChampionDTO> availableChamps = new List<RiotObjects.Platform.Catalog.Champion.ChampionDTO>();
        public Region region = new Region();
        public LoLConnection connection = new LoLConnection();
        public String userName { get; set; }
        public String password { get; set; }
        public String spectateSummoner { get; set; }
        public String log { get; set; }
        public String status { get; set; }
        public RiotObjects.Platform.Catalog.Champion.ChampionDTO[] availableChampsArray;
        #endregion
        #region Constructor
        public LoginClient(String username, String pwd, Region reg)
        {
            userName = username;
            password = pwd;
            region = reg;
            connection.OnConnect += connection_OnConnect;
            connection.OnDisconnect += connection_OnDisconnect;
            connection.OnError += connection_OnError;
            connection.OnLogin += connection_OnLogin;
            connection.OnLoginQueueUpdate += connection_OnLoginQueueUpdate;
            connection.OnMessageReceived += connection_OnMessageReceived;
            connection.Connect(userName, password, region, Riot.DataDragon.LatestVersion);
        }
        #endregion
        #region Events
        async void RegisterNotifications()
        {
            await connection.Subscribe("bc", connection.AccountID());
            await connection.Subscribe("cn", connection.AccountID());
            await connection.Subscribe("gn", connection.AccountID());
        }
        void connection_OnMessageReceived(object sender, object message)
        {
        }

        void connection_OnLoginQueueUpdate(object sender, int positionInLine)
        {
        }

        void connection_OnLogin(object sender, string username, string ipAddress)
        {
            new Thread(async delegate()
            {
                RegisterNotifications();
                loginPacket = await connection.GetLoginDataPacketForUser();
                if (loginPacket.AllSummonerData == null)
                {
                    Random rnd = new Random();
                    String summonerName = username;
                    if (summonerName.Length > 16)
                        summonerName = summonerName.Substring(0, 12) + new Random().Next(1000, 9999).ToString();
                    RiotObjects.Platform.Summoner.AllSummonerData sumData = await connection.CreateDefaultSummoner(summonerName);
                    loginPacket.AllSummonerData = sumData;
                }
                RiotObjects.Platform.Matchmaking.GameQueueConfig[] availableQueues = await connection.GetAvailableQueues();
                //LoLLauncher.RiotObjects.Platform.Summoner.Boost.SummonerActiveBoostsDTO boosts = await connection.GetSumonerActiveBoosts();
                //LoLLauncher.RiotObjects.Platform.Leagues.Client.Dto.SummonerLeagueItemAndProgresssDTO leaguePosProg = await connection.GetMyLeaguePositionsAndProgress();
                availableChampsArray = await connection.GetAvailableChampions();
                RiotObjects.Platform.Summoner.Runes.SummonerRuneInventory sumRuneInven = await connection.GetSummonerRuneInventory(loginPacket.AllSummonerData.Summoner.SumId);
                RiotObjects.Platform.Leagues.Client.Dto.SummonerLeagueItemsDTO leaguePos = await connection.GetMyLeaguePositions();
                object preferences = await connection.LoadPreferencesByKey("KEY BINDINGS", 1, false);
                RiotObjects.Platform.Summoner.Masterybook.MasteryBookDTO masteryBook = await connection.GetMasteryBook(loginPacket.AllSummonerData.Summoner.SumId);
                RiotObjects.Team.Dto.PlayerDTO player = await connection.CreatePlayer();
            }).Start();
        }
        void connection_OnError(object sender, Error error)
        {
        }
        void connection_OnDisconnect(object sender, EventArgs e)
        {
        }
        void connection_OnConnect(object sender, EventArgs e)
        {
        }
        #endregion

    }
}
