using System;
using System.Collections.Generic;
using System.Text;
using kMCCoatings.Core.Entities.Site;

namespace kMCCoatings.Core.Entities.Cell
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
        public List<Site.Site> Sites { get; set; }

        /// <summary>
        /// Сайт, в котором находится атом
        /// </summary>
        public Site.Site OccupiedSite { get; set; }

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
