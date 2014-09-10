using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpectatorClient.Packets
{
    class JungleSpawn : Packet
    {
        public UInt32 netId { get { return BitConverter.ToUInt32(content, 0); } }
        public Single x { get { return BitConverter.ToSingle(content, 4); } }
        public Single y { get { return BitConverter.ToSingle(content, 8); } }
        public Single z { get { return BitConverter.ToSingle(content, 12); } }
        public Single campX { get { return BitConverter.ToSingle(content, 16); } }
        public Single campY { get { return BitConverter.ToSingle(content, 20); } }
        public Single campZ { get { return BitConverter.ToSingle(content, 24); } }
        public Single campRoundX { get { return BitConverter.ToSingle(content, 28); } }
        public Single campRoundY { get { return BitConverter.ToSingle(content, 32); } }
        public Single campRoundZ { get { return BitConverter.ToSingle(content, 36); } }
        public Byte campId { get { return  content[40]; } }
        public String creepName { get { return BitConverter.ToString(content, 41); } }
        public JungleSpawn(Packet p)
            : base(p.param, p.header, p.time, p.content)
        {
        }
    }
}
