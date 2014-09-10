using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpectatorClient.Packets
{
    class UnitSpawn : Packet
    {
        public Byte Spawn { get { return content[30]; } }
        public UInt32 Time { get { return BitConverter.ToUInt32(content, 31); } }
        public Waypoints Waypoint
        {
            get
            {
                Byte[] b = new Byte[content.Length - 35 + 6];
                Array.Copy(content, 35, b, 5, content.Length - 35);
                Packet p = new Packet(0, 0, 0, b);
                return new Waypoints(p);
            }
        }
        public UnitSpawn(Packet p)
            : base(p.param, p.header, p.time, p.content)
        {
        }
    }
}
