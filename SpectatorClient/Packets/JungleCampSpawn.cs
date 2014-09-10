using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpectatorClient.Packets
{
    class JungleCampSpawn : Packet
    {
        public Single x { get { return BitConverter.ToSingle(content, 0); } }
        public Single y { get { return BitConverter.ToSingle(content, 4); } }
        public Single z { get { return BitConverter.ToSingle(content, 8); } }
        public Byte campId { get { return  content[16]; } }
        public JungleCampSpawn(Packet p)
            : base(p.param, p.header, p.time, p.content)
        {
        }
    }
}
