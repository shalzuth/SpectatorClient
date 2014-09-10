using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpectatorClient.Packets
{
    class AbilityLevel : Packet
    {
        public UInt32 netId { get { return param; } }
        public Byte type { get { return content[0]; } }
        public Byte level { get { return content[1]; } }
        public AbilityLevel(Packet p)
            : base(p.param, p.header, p.time, p.content)
        {
        }
    }
}
