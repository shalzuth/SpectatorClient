using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpectatorClient.Packets
{
    class LevelPropSpawn : Packet
    {
        public UInt32 NetId { get { return BitConverter.ToUInt32(content, 0); } }
        public Single X { get { return BitConverter.ToSingle(content, 9); } }
        public Single Z { get { return BitConverter.ToSingle(content, 13); } }
        public Single Y { get { return BitConverter.ToSingle(content, 17); } }
        public String Name { get { return Encoding.UTF8.GetString(content, 62, Array.IndexOf(content, (Byte)0, 62) - 62); } }
        public String Type { get { return Encoding.UTF8.GetString(content, 126, Array.IndexOf(content, (Byte)0, 126) - 126); } }
        public LevelPropSpawn(Packet p)
            : base(p.param, p.header, p.time, p.content)
        {
        }
    }
}
