using System.Collections.Generic;
using kMCCoatings.Core.Entities.AtomRoot;

namespace kMCCoatings.Core.Entities.SiteRoot
{
    public class Site
    {
        /// <summary>
        /// Координаты сайта.
        /// </summary>
        public GlobalCoordinates Coordinates { get; set; }

        /// <summary>
        /// Левая граница клетки
        /// </summary>
        public float X { get; set; }

        /// <summary>
        /// Нижняя граница клетки
        /// </summary>
        public float Y { get; set; }

        /// <summary>
        /// Положение клетки на оси Z
        /// </summary>
        public float Z { get; set; }

        /// <summary>
        /// Состояние сайта
        /// </summary>
        public SiteStatus SiteStatus { get; set; }

        /// <summary>
        /// Атом, размещённый в сайте
        /// </summary>
        public Atom OccupiedAtom { get; set; }

        /// <summary>
        /// Список всех типов возможных атомов, который могут окупировать этот сайт
        /// </summary>
        public int[] AtomTypeIds { get; set; }

        /// <summary>
        /// Идентификатор Id диммера, к которому относится атом
        /// </summary>
        public int DimerId { get; set; }

    }
}