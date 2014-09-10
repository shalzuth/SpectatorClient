using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpectatorClient.Packets
{
    class ZyraPassive : Packet
    {
        public UInt32 NetId { get { return param; } }
        public ZyraPassive(Packet p)
            : base(p.param, p.header, p.time, p.content)
        {
        }
    }
}
