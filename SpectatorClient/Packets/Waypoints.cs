using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpectatorClient.Packets
{
    public class Vector
    {
        public Int16 x;
        public Int16 y;
        public Vector(Int16 x, Int16 y)
        {
            this.x = x;
            this.y = y;
        }
    }
    public class Unit
    {
        public Byte numCoords;
        public UInt32 netId;
        public List<Vector> destCoords = new List<Vector>();
    }
    public class Waypoints : Packet
    {
        public Single waypointTime { get { return BitConverter.ToSingle(content, 0); } }
        public Byte numUnits { get { return content[4]; } }
        public List<Unit> units = new List<Unit>();
        public Waypoints(Packet p)
            : base(p.param, p.header, p.time, p.content)
        {
            int offset = 6;
            for (int j = 0; j < numUnits; j++)
            {
                Unit unit = new Unit();
                unit.numCoords = content[offset];
                unit.netId = BitConverter.ToUInt32(content, offset + 1);
                if (content[offset + 4] != 64) continue;

                int maskOffset = offset + 5;
                Byte bitMask = content[maskOffset];
                UInt32 nPos = (UInt32)Math.Ceiling(((Double)(unit.numCoords) - 2) / 8);
                int vectorCount = unit.numCoords / 2;
                if (unit.numCoords == 3)
                    unit.numCoords--;
                int posoffset = 0;
                if (unit.numCoords % 2 == 1)
                {
                    if (content[maskOffset] == 0x00)
                    {
                        content[maskOffset] = 0xff;
                        posoffset = unit.numCoords % 8;
                    }
                }
                Vector lastCoord = new Vector(0, 0);
                for (int i = 0; i < vectorCount; i++)
                {
                    int pos = (i - 1) * 2;
                    if (pos >= 0 && ((1 & content[maskOffset + (pos + posoffset) / 8] >> ((pos + posoffset) % 8)) != 0))
                        lastCoord.x += content[maskOffset + nPos++];
                    else
                    {
                        Int16 upper = (Int16)((Int16)content[maskOffset + nPos + 1] << (Int16)8);
                        Byte lower = content[maskOffset + nPos];
                        lastCoord.x = (short)(upper | (Int16)lower);
                        nPos += 2;
                    }
                    pos = (i - 1) * 2 + 1;
                    if (pos >= 0 && ((1 & content[maskOffset + (pos + posoffset) / 8] >> ((pos + posoffset) % 8)) != 0))
                        lastCoord.y += content[maskOffset + nPos++];
                    else
                    {
                        if (maskOffset + nPos + 1 > content.Length)
                            return;
                        Int16 upper = (Int16)((Int16)content[maskOffset + nPos + 1] << (Int16)8);
                        Byte lower = content[maskOffset + nPos];
                        lastCoord.y = (short)(upper | (Int16)lower);
                        nPos += 2;
                    }
                    unit.destCoords.Add(new Vector(lastCoord.x, lastCoord.y));
                }
                if (unit.numCoords % 2 == 1)
                {
                    if (((int)1 & content[maskOffset + vectorCount / 8] >> (vectorCount % 8)) != 0)
                        nPos++;
                    else
                        nPos += 2;
                }
                offset = maskOffset + (int)nPos;
                Vector s = unit.destCoords[0];
                units.Add(unit);
            }
        }
    }
}
