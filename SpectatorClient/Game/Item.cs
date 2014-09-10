using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SpectatorClient.Packets;

namespace SpectatorClient.Game
{
    class Item
    {
        public UInt32 ItemId { get; set; }
        public Byte ItemCount { get; set; }
        public Item(PurchaseItem item)
        {
            ItemId = item.ItemId;
            ItemCount = item.ItemCount;
        }
        public Item()
        {
            ItemId = 0;
            ItemCount = 0;
        }
    }
}
