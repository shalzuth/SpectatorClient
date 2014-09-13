using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using SpectatorClient.Packets;

namespace SpectatorClient.Game
{
    public class Item
    {
        public UInt32 ItemId { get; set; }
        public Byte Quantity { get; set; }
        public PictureBox ItemIcon = new PictureBox();
        public Item(BuyItem item)
        {
            ItemId = item.ItemId;
            Quantity = item.Quantity;
        }
        public Item(InventoryUpdate.ItemPacket item)
        {
            ItemId = item.ItemId;
            Quantity = item.Quantity;
        }
        public Item()
        {
            ItemId = 0;
            Quantity = 0;
        }
    }
}
