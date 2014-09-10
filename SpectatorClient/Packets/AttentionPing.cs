using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpectatorClient.Packets
{
    class AttentionPing : Packet
    {
        public UInt32 sourceNetId { get { return BitConverter.ToUInt32(content, 12); } }
        public UInt32 targetNetId { get { return BitConverter.ToUInt32(content, 8); } }
        public Single x { get { return BitConverter.ToSingle(content, 0); } }
        public Single y { get { return BitConverter.ToSingle(content, 4); } }
        public Byte pingType { get { return content[16]; } }
        public AttentionPing(Packet p)
            : base(p.param, p.header, p.time, p.content)
        {
        }
    }
}

