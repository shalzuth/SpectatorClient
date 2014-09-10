using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpectatorClient.Packets
{
    class Unk7C : Packet
    {
        public UInt32 NetId { get { return BitConverter.ToUInt32(content, 0); } }
        public UInt32 SourceNetId { get { return BitConverter.ToUInt32(content, 4); } }
        public Single X { get { return BitConverter.ToSingle(content, 9); } }
        public Single Z { get { return BitConverter.ToSingle(content, 13); } }
        public Single Y { get { return BitConverter.ToSingle(content, 17); } }
        //also contains weird string info, like wards or stuff?
        public Unk7C(Packet p)
            : base(p.param, p.header, p.time, p.content)
        {
        }
    }
}
