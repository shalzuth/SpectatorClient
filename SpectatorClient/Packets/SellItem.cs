using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpectatorClient.Packets
{
    class SellItem : Packet
    {
        public UInt32 NetId { get { return param; } }
        public Byte Slot { get { return content[0]; } }
        public SellItem(Packet p)
            : base(p.param, p.header, p.time, p.content)
        {
        }
    }
}
