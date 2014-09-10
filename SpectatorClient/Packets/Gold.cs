using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpectatorClient.Packets
{
    class Gold : Packet
    {
        public UInt32 netId { get { return BitConverter.ToUInt32(content, 0); } }
        public Single goldGained { get { return BitConverter.ToSingle(content, 4); } }
        public Gold(Packet p)
            : base(p.param, p.header, p.time, p.content)
        {
        }
    }
}
