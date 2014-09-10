using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpectatorClient.Packets
{
    class SpellCast : Packet
    {
        //need to decode
        /*
        1A : 02-01-00-40-7A-30-02-00-40-40-56-F5-38-46-D9-C9-E3-44 @ 126.2049
        1A : 0C-01-00-40-7A-31-02-00-40-41-6F-5F-0A-45-AB-E8-45-46 @ 126.2049
        1A : 0D-01-00-40-80-71-02-00-40-40-9B-D3-43-46-1B-AE-26-45 @ 126.2699
        1A : FC-00-00-40-80-72-02-00-40-40-99-CD-38-46-8A-50-D5-44 @ 126.2699
        */
        public UInt32 spellHash { get { return BitConverter.ToUInt32(content, 7); } }
        //public UInt32 unkNetId { get { return BitConverter.ToUInt32(content, 11); } }
        //public Byte zero { get { return content[15]; } }
        //public Single one { get { return BitConverter.ToSingle(content, 16); } }
        public UInt32 sourceNetId { get { return BitConverter.ToUInt32(content, 20); } }
        //public UInt32 dupeNetId { get { return BitConverter.ToUInt32(content, 24); } }
        public UInt32 champHash { get { return BitConverter.ToUInt32(content, 28); } }
        public UInt32 newNetId { get { return BitConverter.ToUInt32(content, 32); } }
        public Single x { get { return BitConverter.ToSingle(content, 36); } }
        public Single z { get { return BitConverter.ToSingle(content, 40); } }
        public Single y { get { return BitConverter.ToSingle(content, 44); } }
        public Single x2 { get { return BitConverter.ToSingle(content, 48); } }
        public Single z2 { get { return BitConverter.ToSingle(content, 52); } }
        public Single y2 { get { return BitConverter.ToSingle(content, 56); } }
        public Byte Multiple { get { return content[60]; } }
        public Single CastTime { get { return BitConverter.ToSingle(content, 61 ); } }
        //public Byte oneorzero { return BitConverter.ToSingle(content, 65); } }
        //public Byte oneorzero { return BitConverter.ToSingle(content, 69); } }
        public Single Cooldown { get { return BitConverter.ToSingle(content, 73); } }
        //public Byte oneorzero { return BitConverter.ToSingle(content, 77); } }
        //public Byte zero { get { return content[81]; } }
        public SpellCast(Packet p)
            : base(p.param, p.header, p.time, p.content)
        {
        }
    }
}
