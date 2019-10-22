using System;
using System.Collections.Generic;
using System.Text;
using kMCCoatings.Core.Entities.SiteRoot;
using kMCCoatings.Core.LatticeRoot;
using MathNet.Spatial.Euclidean;

namespace kMCCoatings.Core.Entities.AtomRoot
{
    public class Element
    {
        public int Id { get; set; }

        public Dictionary<int, double> InteractionEnergy { get; set; }
    }
}