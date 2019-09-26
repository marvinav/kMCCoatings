using System;
using System.Collections.Generic;
using System.Text;

namespace kMCCoatings.Core.Entities
{
    public class GlobalCoordinates
    {
        public int X { get; set; }

        public int Y { get; set; }

        public int Z { get; set; }
        
        public GlobalCoordinates()
        {

        }

        public GlobalCoordinates(int x, int y, int z, int xMax, int yMax)
        {
            if (z >= 0)
            {
                if (x >= xMax - 1)
                {
                    x = 0;
                }
                else if (x < 0)
                {
                    x = xMax - 1;
                }
                if (y >= yMax - 1)
                {
                    y = 0;
                }
                else if (y < 0)
                {
                    y = yMax - 1;
                }
                X = x;
                Y = y;
                Z = z;
            }
            else
            {
                throw new ArgumentOutOfRangeException("Координата клетки Z < 0");
            }
        }
    }
}
