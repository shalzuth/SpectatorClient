using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpectatorClient.Packets
{
    class CreateEffect : Packet
    {
        public Boolean IsEffect { get { return (content[0] > 0x03 && content[0] != 0xA); } }
        public String Effect { get { return Encoding.UTF8.GetString(content, 2, content.Length - 3); } }
        public CreateEffect(Packet p)
            : base(p.param, p.header, p.time, p.content)
        {
        }
    }
}
