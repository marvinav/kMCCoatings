using System;
using System.Collections.Generic;
using System.Text;
using kMCCoatings.Core.Entities.AtomRoot;

namespace kMCCoatings.Core.Entities.DepositionRoot
{
    /// <summary>
    /// Концентрация элемента на i-ом шаге итерации
    /// </summary>
    public class ElementConcentration
    {
        /// <summary>
        /// Концентрация в абс.ед., типа Интенсивности, т.е. сумма концентраций всех элементов может быть не равна 100
        /// </summary>
        public int Concentration { get; set; }

        /// <summary>
        /// Id элемента
        /// </summary>
        public int ElementId { get; set; }

        /// <summary>
        /// Элемент
        /// </summary>
        public Element Element { get; set; }
    }
}
