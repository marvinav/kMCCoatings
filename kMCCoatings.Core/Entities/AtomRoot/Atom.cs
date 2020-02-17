using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using kMCCoatings.Core.Entities.SiteRoot;
using kMCCoatings.Core.Extension;
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
        /// Формируем список переходов для атома
        /// </summary>
        public Transition CalculateTransion(Site targetSite)
        {
            var oldEnergy = Site.EnergyInSite(Element.Id);
            var difEnergy = oldEnergy - targetSite.EnergyInSite(Element.Id);
            return new Transition(this, targetSite, difEnergy);
        }

        public Atom(Element element, Site site)
        {
            Element = element;
            site.OccupiedAtom = this;
            Site = site;
        }
    }
}
