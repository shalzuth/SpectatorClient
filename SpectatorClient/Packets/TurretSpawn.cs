using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpectatorClient.Packets
{
    class TurretSpawn : Packet
    {
        public UInt32 NetId { get { return BitConverter.ToUInt32(content, 0); } }
        public String TurretName { get { return Encoding.UTF8.GetString(content, 4, Array.IndexOf(content, (Byte)0, 4) - 4); } }
        public TurretSpawn(Packet p)
            : base(p.param, p.header, p.time, p.content)
        {
        }
    }
}
