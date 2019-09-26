using System.Collections.Generic;

namespace kMCCoatings.Core.Entities.Site
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
        public int X { get; set; }

        /// <summary>
        /// Нижняя граница клетки
        /// </summary>
        public int Y { get; set; }

        /// <summary>
        /// Положение клетки на оси Z
        /// </summary>
        public int Z { get; set; }

        /// <summary>
        /// Состояние сайта
        /// </summary>
        public SiteStatus SiteStatus { get; set; }

        /// <summary>
        /// Атом, размещённый в сайте
        /// </summary>
        public Atom.Atom OccupiedAtom { get; set; }

        /// <summary>
        /// Список всех типов возможных атомов, который могут окупировать этот сайт
        /// </summary>
        public int[] AtomTypeIds { get; set; }

    }
}