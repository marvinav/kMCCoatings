using System;
using System.Collections.Generic;
using System.Text;
using kMCCoatings.Core.Entities.AtomRoot;
using kMCCoatings.Core.Entities.SiteRoot;

namespace kMCCoatings.Core.Entities
{
    /// <summary>
    /// Описание перехода
    /// </summary>
    public class Transition
    {
        /// <summary>
        /// Энергия активации перехода
        /// </summary>
        public int ActivationEnergy { get; set; }

        /// <summary>
        /// Целевой сайт
        /// </summary>
        public Site TargetSite { get; set; }

        /// <summary>
        /// Атом, который осуществляет переход
        /// </summary>
        public Atom Atom { get; set; }
    }
}
