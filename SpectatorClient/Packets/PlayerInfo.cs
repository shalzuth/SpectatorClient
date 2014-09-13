using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpectatorClient.Packets
{
    class PlayerInfo : Packet
    {
        public UInt32 NetId { get { return param; } }
        public List<UInt32> Runes { get; set; }
        public enum Skills : uint
        {
            Ignite = 0x244f3606,
            Flash = 0xa86e4906,
            Heal = 0x1caf6403,
            Smite = 0x95865e06,
            Teleport = 0x64134f00,
            Exhaust = 0xe4baa808,
            Barrier = 0x82b9cf0c,
            Clarity = 0x21746503,
            Ghost = 0x95cc4a06,
            Garrison = 0xeeaf870d,
            Clairvoyance = 0x65678909,
            Cleanse = 0x94204d06,
            Revive = 0xa5b3c805,
        }
        public Skills Skill1 { get { return (Skills)BitConverter.ToUInt32(content, 120); } }
        public Skills Skill2 { get { return (Skills)BitConverter.ToUInt32(content, 124); } }
        public class MasteryEntry
        {
            private Byte[] content = new Byte[5];
            public UInt32 TalentId { get {
                return (UInt32)(4100 + (content[1] - 0x74) * 0x64 + ((content[0] >> 4) - 0x03) * 0x0A + (content[0] & 0x0F));
            } }
            public Byte Level { get { return content[4]; } }
            public MasteryEntry(Byte[] bytes)
            {
                Array.Copy(bytes, content, 5);
            }
        }
        public List<MasteryEntry> Masteries { get; set; }
        public Byte Level { get { return content[0x210]; } }
        public PlayerInfo(Packet p)
            : base(p.param, p.header, p.time, p.content)
        {
            Runes = new List<UInt32>();
            for (int i = 0; i < 30; i++)
            {
                Runes.Add(BitConverter.ToUInt32(content, i * 4));
            }
            Masteries = new List<MasteryEntry>();
            for (int i = 0; i < 80; i++)
            {
                Masteries.Add(new MasteryEntry(content.ToList().GetRange(0x80 + i * 5, 5).ToArray()));
            }
        }
    }
}
