using System.Collections.Generic;
using MathNet.Spatial.Euclidean;

namespace kMCCoatings.Core.Constants
{
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
        /// Трансляции
        /// </summary>
        public Vector3D[] Translations { get; set; }

    }
}