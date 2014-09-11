using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace SpectatorClient.SpectatorService
{
    static class SpectatorDownloader
    {
        static String specHtml = "http://spectator.na.lol.riotgames.com:80/observer-mode/rest/";
        public static Object[] GetFeaturedGames()
        {
            return (Object[])new JavaScriptSerializer().Deserialize<Dictionary<Object, Object>>(new WebClient().DownloadString(specHtml + "featured"))["gameList"];
        }
        public static Dictionary<Object, Object> DownloadMetaData(String platformId, String gameId)
        {
            return new JavaScriptSerializer().Deserialize<Dictionary<Object, Object>>(new WebClient().DownloadString(specHtml + "consumer/getGameMetaData/" + platformId + "/" + gameId + "/1/token"));
        }
        static Byte[] DownloadFile(String platformId, String gameId, String type, int id)
        {
            try
            {
                return new WebClient().DownloadData(specHtml + "consumer/" + type + "/" + platformId + "/" + gameId + "/" + id + "/token");
            }
            catch
            {
                return new Byte[] { 0xde };
            }
        }
        static Byte[] DownloadChunk(String platformId, String gameId, int chunkId)
        {
            return DownloadFile(platformId, gameId, "getGameDataChunk", chunkId);
        }
        static Byte[] DownloadKeyFrame(String platformId, String gameId, int keyFrame)
        {
            return DownloadFile(platformId, gameId, "getKeyFrame", keyFrame);
        }
        static Byte[] Decrypt(Byte[] encryptKey, Byte[] data)
        {
            return new Blowfish(encryptKey).Decrypt_ECB(data);
        }
        static Byte[] Decompress(Byte[] data)
        {
            GZipStream gzip = new GZipStream(new MemoryStream(data), CompressionMode.Decompress);
            const int size = 4096;
            Byte[] buffer = new Byte[size];
            MemoryStream memory = new MemoryStream();
            int count = 0;
            do
            {
                count = gzip.Read(buffer, 0, size);
                if (count > 0)
                {
                    memory.Write(buffer, 0, count);
                }
            }
            while (count > 0);
            return memory.ToArray();
        }
        public static void DownloadGameFiles(String gameId, String platformId, String encryptionKeyString)
        {
            DownloadGameChunks(gameId, platformId, encryptionKeyString);
            DownloadGameKeyFrames(gameId, platformId, encryptionKeyString);
        }
        public static void DownloadGameFiles(String gameId, String platformId, String encryptionKeyString, String type)
        {
            List<Byte> encryptionKey = Decrypt(Encoding.ASCII.GetBytes(gameId), Convert.FromBase64String(encryptionKeyString)).ToList();
            encryptionKey.RemoveRange(16, encryptionKey.Count() - 16);
            Byte[] encryptionKey2 = encryptionKey.ToArray();
            Dictionary<Object, Object> metadata = DownloadMetaData(platformId, gameId);
            for (int i = 1; i <= (int)metadata["last" + type + "Id"]; i++)
            {
                if (!Directory.Exists(gameId))
                    Directory.CreateDirectory(gameId);
                if (!Directory.Exists(gameId + @"\" + type))
                    Directory.CreateDirectory(gameId + @"\" + type);
                if (File.Exists(gameId + @"\" + type + @"\" + i.ToString()))
                    continue;
                Byte[] encryptedChunk = DownloadChunk(platformId, gameId, i);
                if (encryptedChunk.Count() > 5)
                {
                    Byte[] compressedChunk = Decrypt(encryptionKey2, encryptedChunk);
                    Byte[] chunk = Decompress(compressedChunk);
                    if (!File.Exists(gameId + @"\" + type + @"\" + i.ToString()))
                        File.WriteAllBytes(gameId + @"\" + type + @"\" + i.ToString(), chunk);
                }
            }
        }
        public static void DownloadGameKeyFrames(String gameId, String platformId, String encryptionKeyString)
        {
            DownloadGameFiles(gameId, platformId, encryptionKeyString, "KeyFrame");
        }
        public static void DownloadGameChunks(String gameId, String platformId, String encryptionKeyString)
        {
            DownloadGameFiles(gameId, platformId, encryptionKeyString, "Chunk");            
        }
        public static void DownloadFeaturedGamesChunks()
        {
            Object[] featuredGames = GetFeaturedGames();
            foreach (Dictionary<String, Object> game in featuredGames)
            {
                DownloadGameChunks(((int)game["gameId"]).ToString(),
                                   (String)game["platformId"],
                                   (String)((Dictionary<String, Object>)game["observers"])["encryptionKey"]);
            }
        }
    }
}
