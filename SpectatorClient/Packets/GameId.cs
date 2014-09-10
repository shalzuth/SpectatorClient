using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpectatorClient.Packets
{
    class GameId : Packet
    {
        //public UInt32 NetId { get { return param; } }
        public String PlayerName { get { return Encoding.UTF8.GetString(content, 8, Array.IndexOf(content, (Byte)0, 8) - 8); } }
        public GameId(Packet p)
            : base(p.param, p.header, p.time, p.content)
        {
        }
    }
}
