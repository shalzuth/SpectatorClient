using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SpectatorClient.Packets;

namespace SpectatorClient
{
    static class ChunkDecoder
    {
        public static List<Packet> DecodeGameChunks(String gameId)
        {
            List<Packet> packets = new List<Packet>();
            List<String> chunks = Directory.GetFiles(gameId, "chunk_*").ToList();
            chunks.Sort(delegate(String file1, String file2)
            {
                return Convert.ToInt32(file1.Substring(file1.IndexOf("_") + 1)).CompareTo(Convert.ToInt32(file2.Substring(file2.IndexOf("_") + 1)));
            });
            foreach (String chunk in chunks)
            {
                Byte[] chunkBytes = File.ReadAllBytes(chunk);
                Byte marker;
                Int32 offset = 0;
                Single lastTime = 0;
                UInt32 contentLength = 0;
                Byte packetHeader = 0;
                UInt32 blockParam = 0;
                while (offset < chunkBytes.Count() - 5)
                {
                    marker = chunkBytes[offset++];
                    if (((marker >> 7) & 1) == 1)
                        lastTime += chunkBytes[offset++] / 1000.0f;
                    else
                        lastTime = BitConverter.ToSingle(new Byte[4] { chunkBytes[offset++], chunkBytes[offset++], chunkBytes[offset++], chunkBytes[offset++] }, 0);
                    if (((marker >> 4) & 1) == 1)
                        contentLength = chunkBytes[offset++];
                    else
                        contentLength = BitConverter.ToUInt32(new Byte[4] { chunkBytes[offset++], chunkBytes[offset++], chunkBytes[offset++], chunkBytes[offset++] }, 0);
                    if (((marker >> 6) & 1) == 0)
                        packetHeader = chunkBytes[offset++];
                    if (((marker >> 5) & 1) == 1)
                    {
                        Byte b = chunkBytes[offset++];
                        if (b >> 7 == 1)
                        {
                            b = (byte)(0xff - b);
                            blockParam -= b;
                        }
                        else
                            blockParam += b;
                    }
                    else
                        blockParam = BitConverter.ToUInt32(new Byte[4] { chunkBytes[offset++], chunkBytes[offset++], chunkBytes[offset++], chunkBytes[offset++] }, 0);
                    List<Byte> content = new List<Byte>();
                    for (UInt32 j = 0; j < contentLength; j++)
                    {
                        content.Add(chunkBytes[offset++]);
                    }
                    packets.Add(new Packet(blockParam, packetHeader, lastTime, content.ToArray()));
                }
            }
            return packets;
        }
    }
}
