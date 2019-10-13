using System;

namespace kMCCoatings.Core.Entities.DimerRoot
{
    /// <summary>
    /// Ориентация направление диммера относительно начала координат (0,0,0).
    /// Направляющие косинусов. Вектор лежит в плоскости Oxy. 
    /// </summary>
    public struct DimerOrientation
    {
        /// <summary>
        /// Направляющий косинус вдоль X
        /// </summary>
        public double CosAlfa { get; set; }

        /// <summary>
        /// Направляющий косинус вдоль Y
        /// </summary>
        public double CosBetta { get; set; }

        public double SinAlfa()
        {
            return Math.Sqrt(1 - Math.Pow(CosAlfa, 2));
        }
        
        public double SinBetta()
        {
            return Math.Sqrt(1 - Math.Pow(CosBetta, 2));
        }
    }
}
