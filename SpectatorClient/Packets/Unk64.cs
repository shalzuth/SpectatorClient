using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpectatorClient.Packets
{
    class Unk64 : Packet
    {
        public UInt32 Timer { get { return BitConverter.ToUInt32(content, 0); } }
        public UInt16 NumUnits { get { return BitConverter.ToUInt16(content, 4); } }
        public Byte VCount { get { return content[6]; } }
        public UInt32 NetId { get { return BitConverter.ToUInt32(content, 7); } }
        // float float float float
        public Unk64(Packet p)
            : base(p.param, p.header, p.time, p.content)
        {
        }
    }
}
