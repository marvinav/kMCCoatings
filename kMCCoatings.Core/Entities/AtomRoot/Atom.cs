using System;
using System.Collections.Generic;
using System.Text;
using kMCCoatings.Core.Entities.SiteRoot;
using kMCCoatings.Core.LatticeRoot;
using MathNet.Spatial.Euclidean;

namespace kMCCoatings.Core.Entities.AtomRoot
{
    public class Atom
    {

        /// <summary>
        /// Номер атома в вычислениях
        /// </summary>
        public int AtomNumber { get; set; }

        /// <summary>
        /// Тип атома
        /// </summary>
        public Element Element { get; set; }

        /// <summary>
        /// Сайт, в котором находится атом
        /// </summary>
        public Site Site { get; set; }

        /// <summary>
        /// Список всех возможных переходов для указанного атома
        /// </summary>
        public List<Transition> Transitions { get; set; }

        /// <summary>
        /// Список соседних атомов
        /// </summary>
        public List<Atom> Neigborhoods { get; set; }


    }
}
