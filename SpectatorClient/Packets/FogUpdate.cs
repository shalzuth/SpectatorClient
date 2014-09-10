using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpectatorClient.Packets
{
    class FogUpdate : Packet
    {
        /*
        23(0) : C8-00-00-00-00-00-00-00-FE-FF-FF-FF-12-00-00-40-78-00-00-40-00-00-00-00-1A-34-5D-46-69-E1-61-46-00-50-C3-46-CD-CC-B0-42-00-00-02-43-00-00-80-3F-00-00-00-00-07-00-00-00-00-00-C8-44 @ 0.2446057
        23(0) : C8-00-00-00-00-00-00-00-FE-FF-FF-FF-AD-00-00-40-AE-00-00-40-00-00-00-00-15-C8-0D-46-F4-71-AC-45-00-00-B4-42-00-00-A0-40-00-00-80-BF-00-00-80-3F-00-00-00-00-03-00-00-00-00-00-61-44 @ 36.94081
        23(0) : 64-00-00-00-00-00-00-00-FE-FF-FF-FF-00-00-00-00-B3-00-00-40-00-00-00-00-B5-0C-0D-46-9C-69-AB-45-00-00-80-3F-00-00-00-00-00-00-00-00-00-00-80-3F-00-00-00-00-02-00-00-00-00-00-7A-43 @ 38.37481
*/
        public UInt32 NetId { get { return BitConverter.ToUInt32(content, 12); } }
        public UInt32 AttachedToNetId { get { return BitConverter.ToUInt32(content, 16); } }
        public Single Range { get { return BitConverter.ToSingle(content, 56); } }
        public FogUpdate(Packet p)
            : base(p.param, p.header, p.time, p.content)
        {
        }
    }
}
