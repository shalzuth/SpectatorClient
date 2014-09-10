using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpectatorClient.Packets
{
    class HeroSpawn : Packet
    {
        public UInt32 NetId { get { return BitConverter.ToUInt32(content, 0); } }
        public UInt32 PlayerNumber { get { return BitConverter.ToUInt32(content, 4); } }
        public Byte TeamId { get { return content[10]; } }
        public Byte Bot { get { return content[11]; } }
        public Byte BotRank { get { return content[12]; } }
        public Byte TeamPos { get { return content[13]; } }
        public Byte SkinId { get { return content[14]; } }
        public String PlayerName { get { return Encoding.UTF8.GetString(content, 18, Array.IndexOf(content, (Byte)0, 18) - 18); } }
        public String ChampionName { get { return Encoding.UTF8.GetString(content, 146, Array.IndexOf(content, (Byte)0, 146) - 146); } }
        public HeroSpawn(Packet p)
            : base(p.param, p.header, p.time, p.content)
        {
        }
    }
}
