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
        /// Id элемента, соответсвующий порядковому номеру в таблицы Менделева
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Список всех возможных решёток для данного элемента
        /// </summary>
        public Lattice[] Lattices { get; set; }
    }
}