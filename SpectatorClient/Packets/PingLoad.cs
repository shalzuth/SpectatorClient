using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpectatorClient.Packets
{
    class PingLoad : Packet
    {
        public Byte PlayerNum { get { return content[0]; } }
        public Single Loaded { get { return BitConverter.ToSingle(content, 12); } }
        public Single Ping { get { return BitConverter.ToSingle(content, 16); } }
        public PingLoad(Packet p)
            : base(p.param, p.header, p.time, p.content)
        {
        }
    }
}
