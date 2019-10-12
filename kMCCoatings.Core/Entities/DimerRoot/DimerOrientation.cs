using System;

namespace kMCCoatings.Core.Entities.DimerRoot
{
    /// <summary>
    /// Ориентация направление диммера относительно начала координат (0,0,0).
    /// Направлающие косинусов. Косинусы углов Эйлера.
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

        /// <summary>
        /// Направляющий косинус вдоль Z
        /// </summary>
        public double CosGamma { get; set; }

        public double SinAlfa()
        { 
            return Math.Sqrt(1 - Math.Pow(CosAlfa, 2)); 
        }
        public double SinBetta()
        {
            return Math.Sqrt(1 - Math.Pow(CosBetta, 2));
        }
        public double SinGamma()
        {
            return Math.Sqrt(1 - Math.Pow(CosGamma, 2));
        }
    }
}
