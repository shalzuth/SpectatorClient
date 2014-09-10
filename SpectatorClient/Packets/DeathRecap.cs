using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpectatorClient.Packets
{
    class DeathRecap : Packet
    {
        public UInt32 NetId { get { return param; } }
        public enum RecapType
        {
            Death = 0x4,
            Killer = 0x6,
            InhibDragBaron = 0x1F,//not sure which
            Turret = 0x24,
            SoloKill = 0x39,
            Assist = 0x45,
        };
        public RecapType Recap { get { return (RecapType)content[0]; } }
        public UInt32 TargetNetId { get { return BitConverter.ToUInt32(content, 1); } }
        public DeathRecap(Packet p)
            : base(p.param, p.header, p.time, p.content)
        {
        }
    }
}
