using System;
using System.Collections.Generic;
using System.Text;

namespace kMCCoatings.Core.Entities
{
    public struct GlobalCoordinates
    {
        public int X { get; set; }

        public int Y { get; set; }

        public int Z { get; set; }

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
    
        /// <summary>
        /// Получить вектор по двум точкам
        /// </summary>
        /// <return>Возвращает кортеж вектора</return>
        public static (double, double, double) GetRotationOfVector(GlobalCoordinates firstCor, GlobalCoordinates secondCor)
        {
            var x = firstCor.X - secondCor.X;
            var y = firstCor.Y - secondCor.Y;
            var z = firstCor.Z - secondCor.Z;
            var length = Math.Sqrt(Math.Pow(x, 2) + Math.Pow(y, 2) + Math.Pow(z, 2));
            return (x / length, y / length, z / length);
        }
    }
}
