using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpectatorClient.Packets
{
    class SpawnParticle : Packet
    {
        //todo
        public Byte NumParticles { get { return content[0]; } }
        public SpawnParticle(Packet p)
            : base(p.param, p.header, p.time, p.content)
        {
        }
    }
}
