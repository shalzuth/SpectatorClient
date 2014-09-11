using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using SpectatorClient.Packets;

namespace SpectatorClient.Game
{
    public class Player
    {
        public UInt32 NetId;
        public UInt32 Num;
        public String Name;
        public String Champ;
        public List<Item> Items = new List<Item>();
        public Player()
        {
            for (int i = 0; i < 7; i++)
            {
                Items.Add(new Item());
            }
        }
        public void Update(UInt32 NetId, String Name, String Champ, UInt32 Num)
        {
            this.NetId = NetId;
            this.Name = Name;
            this.Champ = Champ;
            this.Num = Num;
        }
    }
}
