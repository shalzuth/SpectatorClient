using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpectatorClient.Packets
{
    class AutoAttack : Packet
    {
        //need to decode
        /*
        1A : 02-01-00-40-7A-30-02-00-40-40-56-F5-38-46-D9-C9-E3-44 @ 126.2049
        1A : 0C-01-00-40-7A-31-02-00-40-41-6F-5F-0A-45-AB-E8-45-46 @ 126.2049
        1A : 0D-01-00-40-80-71-02-00-40-40-9B-D3-43-46-1B-AE-26-45 @ 126.2699
        1A : FC-00-00-40-80-72-02-00-40-40-99-CD-38-46-8A-50-D5-44 @ 126.2699
        */
        public UInt32 netId { get { return BitConverter.ToUInt32(content, 0); } }
        public UInt32 victimNetId { get { return BitConverter.ToUInt32(content, 4); } }
        public Single experienceGained { get { return BitConverter.ToSingle(content, 8); } }
        public AutoAttack(Packet p)
            : base(p.param, p.header, p.time, p.content)
        {
        }
    }
}
