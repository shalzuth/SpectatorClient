using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpectatorClient.Packets
{
    class GameTimerUpdate : Packet
    {
        public Single Timer { get { return BitConverter.ToSingle(content, 0); } }
        public GameTimerUpdate(Packet p)
            : base(p.param, p.header, p.time, p.content)
        {
        }
    }
}
