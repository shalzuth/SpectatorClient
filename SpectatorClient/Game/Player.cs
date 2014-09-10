using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SpectatorClient.Packets;
namespace SpectatorClient.Game
{
    class Player
    {
        public UInt32 NetId;
        public UInt32 Num;
        public String Name;
        public String Champ;
        public List<Item> Items;
        public Player(UInt32 NetId, String Name, String Champ, UInt32 Num)
        {
            this.NetId = NetId;
            this.Name = Name;
            this.Champ = Champ;
            this.Num = Num;
            Items = new List<Item>();
            for (int i = 0; i < 7; i++){
                Items.Add(new Item());
            }
        }
    }
}
