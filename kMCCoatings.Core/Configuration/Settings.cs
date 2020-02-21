using System;
using System.Collections.Generic;
using System.Text;
using kMCCoatings.Core.Entities.AtomRoot;

namespace kMCCoatings.Core.Configuration
{
    public class Settings
    {
        public Element[] Elements { get; set; }

        public DepositionSettings Deposition { get; set; }

        public CalculatorSettings Calculator { get; set; }

        public DimerSettings Dimer { get; set; }

        #region Alias
        /// <summary>
        /// Alias for deposition
        /// </summary>
        public DepositionSettings De => Deposition;

        /// <summary>
        /// Alias for Calculator
        /// </summary>
        public CalculatorSettings Calc => Calculator;

        /// <summary>
        /// Alias for Dimer
        /// </summary>
        public DimerSettings Di => Dimer;
        #endregion Alias
    }
}
