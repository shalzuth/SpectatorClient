using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpectatorClient.Packets
{
    class Emotion : Packet
    {
        public UInt32 NetId { get { return param; } }
        public UInt32 EmotionType { get { return content[0]; } }
        public Emotion(Packet p)
            : base(p.param, p.header, p.time, p.content)
        {
        }
    }
}
