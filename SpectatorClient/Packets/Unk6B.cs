using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpectatorClient.Packets
{
    class Unk6B : Packet
    {
        private List<UInt32> flags;
        private List<String> strings;
        public UInt32 NetId { get { return param; } }
        public Byte NumStrings { get { return content[0]; } }
        public List<UInt32> Flags { get { return flags; } }
        public List<String> Strings { get { return strings; } }
        public Unk6B(Packet p)
            : base(p.param, p.header, p.time, p.content)
        {
            int offset = 1;
            flags = new List<UInt32>();
            strings = new List<String>();
            for (int i = 0; i < NumStrings; i++)
            {
                flags.Add(BitConverter.ToUInt32(content, offset));
                offset += 4;
                int next = Array.IndexOf(content, (Byte)0, offset) - 1;
                strings.Add(Encoding.UTF8.GetString(content, offset, next - offset));
                offset = next;
            }
        }
    }
}
