using System.Collections.Generic;
using MathNet.Spatial.Euclidean;

namespace kMCCoatings.Core.LatticeRoot
{
    /// <summary>
    /// Векторы трансляций, характеризующие связи
    /// </summary>
    public class Translation
    {

        /// <summary>
        /// Id атома в кристаллической рещётки
        /// </summary>
        public int AtomId { get; set; }

        /// <summary>
        /// Id атома в связи в кристаллической решётки
        /// </summary>
        public int BoundedAtomId { get; set; }

        /// <summary>
        /// Длина связи
        /// </summary>
        public double LengthRelToBasis { get; set; }

        /// <summary>
        /// Трансляции
        /// </summary>
        public Vector3D[] Translations { get; set; }

    }
}