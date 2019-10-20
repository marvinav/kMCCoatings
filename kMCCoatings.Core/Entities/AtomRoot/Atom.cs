using System;
using System.Collections.Generic;
using System.Text;
using kMCCoatings.Core.Entities.SiteRoot;

namespace kMCCoatings.Core.Entities.AtomRoot
{
    public class Atom
    {

        /// <summary>
        /// Номер атома в вычислениях
        /// </summary>
        public int AtomNumber { get; set; }

        public static int AtomCounter { get; set; } = 0;
        /// <summary>
        /// Тип атома
        /// </summary>
        public int ElementId { get; set; }

        /// <summary>
        /// Сайт, в котором находится атом
        /// </summary>
        public Site Site { get; set; }

        /// <summary>
        /// Список всех возможных переходов для указанного атома
        /// </summary>
        public List<Transition> Transitions { get; set; }

        /// <summary>
        /// Рассчитать все возможные переходы
        /// </summary>
        public static void CalculateTransitions(Atom atom)
        {

        }

        public static int CalculateDistance(Atom first, Atom second)
        {
            return (int)Math.Sqrt(Math.Pow(first.Site.Coordinates.X - second.Site.Coordinates.X, 2.0) +
                Math.Pow(first.Site.Coordinates.Y - second.Site.Coordinates.Y, 2.0) +
                Math.Pow(first.Site.Coordinates.Z - second.Site.Coordinates.Z, 2.0));
        }

        public Atom()
        {
            AtomNumber = AtomCounter;
            AtomCounter++;
        }
    }
}
