using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpectatorClient.Packets
{
    class DeathTimer : Packet
    {
        public UInt32 NetId { get { return BitConverter.ToUInt32(content, 0); } }
        public UInt32 Timer { get { return BitConverter.ToUInt32(content, 12); } }
        public DeathTimer(Packet p)
            : base(p.param, p.header, p.time, p.content)
        {
        }
    }
}
