using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpectatorClient.Packets
{
    class SpawnProjectile : Packet
    {
        //todo
        public Single X { get { return BitConverter.ToSingle(content, 0); } }
        public Single Z { get { return BitConverter.ToSingle(content, 4); } }
        public Single Y { get { return BitConverter.ToSingle(content, 8); } }
        //public Single X2 { get { return BitConverter.ToSingle(content, 12); } }
        //public Single Z2 { get { return BitConverter.ToSingle(content, 16); } }
        //public Single Y2 { get { return BitConverter.ToSingle(content, 20); } }
        public SpawnProjectile(Packet p)
            : base(p.param, p.header, p.time, p.content)
        {
        }
    }
}