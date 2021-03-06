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
        /// <summary>
        /// Id элемента, соответствующий порядковому номеру в таблицы Менделева
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Энергия атома при осаждении
        /// </summary>
        public double Energy { get; set; }

        /// <summary>
        /// Энергия взаимодействия атомов
        /// </summary>
        public Dictionary<int, double> InteractionEnergy { get; set; }

        /// <summary>
        /// Радиус, на котором располагается атом элемента от другого атома элемента
        /// </summary>
        public Dictionary<int, double> InteractionRadius { get; set; }
    }
}