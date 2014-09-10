using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpectatorClient.Packets
{
    class Teleport : Packet
    {
        public UInt32 NetId { get { return param; } }
        public enum RecallStatus
        {
            Teleport = 0xBD,
            Recall = 0xD7,
            Complete = 0xDA,
        }
        public RecallStatus Status { get { return (RecallStatus)content[35]; } }
        public Teleport(Packet p)
            : base(p.param, p.header, p.time, p.content)
        {
        }
    }
}
