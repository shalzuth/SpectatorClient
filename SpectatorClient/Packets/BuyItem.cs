﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpectatorClient.Packets
{
    public class BuyItem : Packet
    {
        public UInt32 NetId { get { return param; } }
        public UInt32 ItemId { get { return BitConverter.ToUInt32(content, 0); } }
        public Byte SlotId { get { return content[4]; } set { content[4] = value; } }
        public Byte Quantity { get { return content[5]; } }
        public BuyItem(Packet p)
            : base(p.param, p.header, p.time, p.content)
        {
        }
    }
}
