using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpectatorClient.Packets
{
    public class InventoryUpdate: Packet
    {
        public Boolean IsInventory { get { return BitConverter.ToUInt16(content, 0) == 0x10C; } }
        public UInt32 NetId { get { return param; } }
        public class ItemPacket
        {
            private Byte[] content = new Byte[7];
            private Byte[] content2 = new Byte[4];
            private Byte[] content3 = new Byte[4];
            public UInt32 ItemId { get { return BitConverter.ToUInt32(content, 0); } }
            public Byte SlotId { get { return content[4]; } }
            public Byte Quantity { get { return content[5]; } }
            public Byte Charges { get { return content[6]; } }
            public UInt32 Cooldown { get { return BitConverter.ToUInt32(content2, 0);}}
            public UInt32 BaseCooldown { get { return BitConverter.ToUInt32(content2, 0);}}
            public ItemPacket(Byte[] bytes, Byte[] bytes2, Byte[] bytes3)
            {
                Array.Copy(bytes, content, 7);
                Array.Copy(bytes2, content2, 4);
                Array.Copy(bytes3, content3, 4);
            }
        }
        public List<ItemPacket> Items { get; set; }
        public InventoryUpdate(Packet p)
            : base(p.param, p.header, p.time, p.content)
        {
            Items = new List<ItemPacket>();
            if (BitConverter.ToUInt16(content, 0) == 0x10C && content.Length > 0x70 + 30)
            {
                for (int i = 0; i < 10; i++)
                {
                    /*Byte[] byte1 = new Byte[7];
                    for (int j = 0; j < 7; j++)
                    {
                        byte1[j] = content[2 + i * 7 + j];
                    }
                    Byte[] byte2 = new Byte[4];
                    for (int j = 0; j < 4; j++)
                    {
                        byte2[j] = content[0x48 + i * 4 + j];
                    }
                    Byte[] byte3 = new Byte[4];
                    for (int j = 0; j < 4; j++)
                    {
                        byte3[j] = content[0x70 + i * 4 + j];
                    }*/
                        Items.Add(new ItemPacket(content.ToList().GetRange(2 + i * 7, 7).ToArray(),
                            content.ToList().GetRange(0x48 + i * 4, 4).ToArray(),
                            content.ToList().GetRange(0x70 + i * 4, 4).ToArray()));
                }
            }
        }
    }
}
