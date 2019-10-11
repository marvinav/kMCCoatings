using System;
using System.Collections.Generic;
using System.Text;
using kMCCoatings.Core.Entities.SiteRoot;

namespace kMCCoatings.Core.Entities.CellRoot
{
    public class Cell
    {
        /// <summary>
        /// Координаты клетки.
        /// </summary>
        public GlobalCoordinates Coordinates { get; set; }

        /// <summary>
        /// Список всех сайтов 
        /// </summary>
        public List<Site> Sites { get; set; }

        /// <summary>
        /// Сайт, в котором находится атом
        /// </summary>
        public Site OccupiedSite { get; set; }

        /// <summary>
        /// Состояние клетки
        /// </summary>
        public CellStatus Status { get; set; }

        /// <summary>
        /// Координаты соседних клеток
        /// </summary>
        public List<GlobalCoordinates> Neighbors { get; set; }


    }
}
