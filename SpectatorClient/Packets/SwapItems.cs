using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpectatorClient.Packets
{
    class SwapItems : Packet
    {
        public UInt32 NetId { get { return param; } }
        public Byte Slot1 { get { return content[0]; } }
        public Byte Slot2 { get { return content[1]; } }
        public SwapItems(Packet p)
            : base(p.param, p.header, p.time, p.content)
        {
        }
    }
}
