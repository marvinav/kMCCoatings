using System;
using System.Collections.Generic;
using System.Text;
using kMCCoatings.Core.Entities.SiteRoot;

namespace kMCCoatings.Core.Entities.AtomRoot
{
    public class Atom
    {
        /// <summary>
        /// Тип атома
        /// </summary>
        public int AtomTypeId { get; set; }

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
    }
}
