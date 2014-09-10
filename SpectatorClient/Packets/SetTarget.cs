using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpectatorClient.Packets
{
    class SetTarget : Packet
    {
        public UInt32 NetId { get { return param; } }
        public UInt32 TargetNetId { get { return BitConverter.ToUInt32(content, 0); } }
        public SetTarget(Packet p)
            : base(p.param, p.header, p.time, p.content)
        {
        }
    }
}
