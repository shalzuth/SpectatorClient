using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpectatorClient.Packets
{
    class KhazixEvo : Packet
    {
        public UInt32 NetId { get { return param; } }
        public String EvoString { get { return Encoding.UTF8.GetString(content, 1, Array.IndexOf(content, (Byte)0, 1) - 1); } }
        public KhazixEvo(Packet p)
            : base(p.param, p.header, p.time, p.content)
        {
        }
    }
}
