using System;
using System.Collections.Generic;
using System.Text;

namespace kMCCoatings.Core.Entities.Atom
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
        public Site.Site Site { get; set; }

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
    }
}
