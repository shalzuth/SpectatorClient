using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpectatorClient.Packets
{
    class Damage : Packet
    {
        public Byte Type { get { return content[0]; } }
        public UInt32 ReceiverId { get { return BitConverter.ToUInt32(content, 4); } }
        public UInt32 DealerId { get { return BitConverter.ToUInt32(content, 8); } }
        public Single Amount { get { return BitConverter.ToSingle(content, 12); } }
        public Damage(Packet p)
            : base(p.param, p.header, p.time, p.content)
        {
        }
    }
}
