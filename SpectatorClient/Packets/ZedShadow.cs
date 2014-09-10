using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpectatorClient.Packets
{
    class ZedShadow : Packet
    {
        public UInt32 NetId { get { return param; } }
        public String ShadowString { get { return Encoding.UTF8.GetString(content, 9, Array.IndexOf(content, (Byte)0, 9) - 9); } }
        public ZedShadow(Packet p)
            : base(p.param, p.header, p.time, p.content)
        {
        }
    }
}
