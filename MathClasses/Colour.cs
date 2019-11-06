using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathClasses
{
    public class Colour
    {
        public UInt32 colour;

        public Colour()
        {
            colour = 0x00000000;
        }
        public Colour(UInt32 r, UInt32 g, UInt32 b,UInt32 a)
        {
            r = (r >> 32);
            g = (g >> 32);
            b = (b >> 32);
            colour = (r<<24)+(g<<16)+(b<<8)+(a>>32);
        }
        public void SetRed(UInt32 r)
        {
            colour =(r >>32);
            colour = (colour<<24);
        }
        public void SetGreen(UInt32 g)
        {
            colour = (g >> 32);
            colour = (colour << 16);
        }
        public void SetBlue(UInt32 b)
        {
            colour = (b >> 32);
            colour = (colour << 8);
        }
        public void SetAlpha(UInt32 a)
        {
            colour = (a >> 32);
        }
        public byte GetAlpha()
        {
            UInt32 other =(colour << 24);
            return Convert.ToByte(other>>24);
        }
        public byte GetBlue()
        {
            UInt32 other =(colour << 16);
            return Convert.ToByte(other >> 24);
        }
        public byte GetGreen()
        {
            UInt32 other =(colour << 8);
            return Convert.ToByte(other >> 24);
        }
        public byte GetRed()
        {
            return Convert.ToByte(colour >> 24);
        }
    }
}
