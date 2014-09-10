using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
namespace SpectatorClient
{
    static class ChunkDownloader
    {
        static String specHtml = "http://spectator.na.lol.riotgames.com:80/observer-mode/rest/";
        static Object[] GetFeaturedGames()
        {
            return (Object[])new JavaScriptSerializer().Deserialize<Dictionary<Object, Object>>(new WebClient().DownloadString(specHtml + "featured"))["gameList"];
        }
        static Dictionary<Object, Object> DownloadMetaData(String platformId, String gameId)
        {
            return new JavaScriptSerializer().Deserialize<Dictionary<Object, Object>>(new WebClient().DownloadString(specHtml + "consumer/getGameMetaData/" + platformId + "/" + gameId + "/1/token"));
        }
        static Byte[] DownloadChunk(String platformId, String gameId, int chunkId)
        {
            try
            {
                return new WebClient().DownloadData(specHtml + "consumer/getGameDataChunk/" + platformId + "/" + gameId + "/" + chunkId + "/token");
            }
            catch
            {
                return new Byte[] { 0xde };
            }
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
        public static void DownloadGameChunks(String gameId, String platformId, String encryptionKeyString)
        {
            List<Byte> encryptionKey = Decrypt(Encoding.ASCII.GetBytes(gameId), Convert.FromBase64String(encryptionKeyString)).ToList();
            encryptionKey.RemoveRange(16, encryptionKey.Count() - 16);
            Byte[] encryptionKey2 = encryptionKey.ToArray();
            Console.WriteLine("game:{0}, key:{1}", gameId, encryptionKeyString);
            Dictionary<Object, Object> metadata = DownloadMetaData(platformId, gameId);
            for (int i = 1; i <= (int)metadata["lastChunkId"]; i++)
            {
                if (!Directory.Exists(gameId))
                    Directory.CreateDirectory(gameId);
                if (File.Exists(gameId + "\\chunk_" + i.ToString()))
                    continue;
                Byte[] encryptedChunk = DownloadChunk(platformId, gameId, i);
                if (encryptedChunk.Count() > 5)
                {
                    Console.WriteLine("got chunk: " + i.ToString());
                    Byte[] compressedChunk = Decrypt(encryptionKey2, encryptedChunk);
                    Byte[] chunk = Decompress(compressedChunk);
                    if (!Directory.Exists(gameId))
                    {
                        Directory.CreateDirectory(gameId);
                    }
                    if (!File.Exists(Path.Combine(gameId, "chunk_" + i.ToString())))
                    {
                        File.WriteAllBytes(Path.Combine(gameId, "chunk_" + i.ToString()), chunk);
                    }
                }
                else
                {
                    Console.WriteLine("empty chunk: " + i.ToString());
                }
            }
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
