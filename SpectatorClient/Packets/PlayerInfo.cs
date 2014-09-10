using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpectatorClient.Packets
{
    class PlayerInfo : Packet
    {
        public UInt32 NetId { get { return param; } }
        public UInt32 Skill1 { get { return BitConverter.ToUInt32(content, 120); } }
        public UInt32 Skill2 { get { return BitConverter.ToUInt32(content, 124); } }
        public PlayerInfo(Packet p)
            : base(p.param, p.header, p.time, p.content)
        {
        }
    }
}
