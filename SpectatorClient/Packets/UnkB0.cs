using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpectatorClient.Packets
{
    class UnkB0 : Packet
    {
        public UInt32 NetId { get { return param; } } //sometimes it is just an offset
        //public Byte twoorthree { get { return content[0]; } }
        public String AnimationName { get { return Encoding.UTF8.GetString(content, 16, Array.IndexOf(content, (Byte)0, 16) - 16); } }
        public UnkB0(Packet p)
            : base(p.param, p.header, p.time, p.content)
        {
        }
    }
}
