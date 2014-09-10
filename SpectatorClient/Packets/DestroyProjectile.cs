using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpectatorClient.Packets
{
    class DestroyProjectile : Packet
    {
        public UInt32 NetId { get { return param; } }
        public DestroyProjectile(Packet p)
            : base(p.param, p.header, p.time, p.content)
        {
        }
    }
}
