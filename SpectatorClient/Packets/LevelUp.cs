using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpectatorClient.Packets
{
    class LevelUp : Packet
    {
        public UInt32 NetId { get { return param; } }
        public UInt16 Level { get { return BitConverter.ToUInt16(content, 0); } }
        public LevelUp(Packet p)
            : base(p.param, p.header, p.time, p.content)
        {
        }
    }
}
