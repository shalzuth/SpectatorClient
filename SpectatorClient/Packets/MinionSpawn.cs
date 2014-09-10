using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpectatorClient.Packets
{
    class MinionSpawn : Packet
    {
        public UInt32 NetId { get { return BitConverter.ToUInt32(content, 0); } }
        public enum WavePos : uint // these probably aren't correctly assigned
        {
            BlueTop = 0xB7717140,
            BlueMid = 0xBA00E840,
            BlueBot = 0xE647D540,
            PurpleTop = 0xEB363C40,
            PurpleMid = 0x53B83640,
            PurpleBot = 0x5EC9AF40
        }
        public WavePos WavePosId { get { return (WavePos)BitConverter.ToUInt32(content, 4); } }
        //public UInt32 FF { get { return content[9]; } }
        public UInt32 WaveNum { get { return content[10]; } }
        public enum MinionTypes {
            Melee,
            Super,
            Cannon,
            Caster
        };
        public MinionTypes MinionType { get { return (MinionTypes)content[11]; } }
        public MinionSpawn(Packet p)
            : base(p.param, p.header, p.time, p.content)
        {
        }
    }
}
