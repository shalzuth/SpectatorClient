using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpectatorClient.Packets
{
    class CharacterNames : Packet
    {
        public UInt32 PlayerNum { get { return BitConverter.ToUInt32(content, 8); } }
        public String CharacterName { get { return Encoding.UTF8.GetString(content, 12, content.Length - 13); } }
        public CharacterNames(Packet p)
            : base(p.param, p.header, p.time, p.content)
        {
        }
    }
}
