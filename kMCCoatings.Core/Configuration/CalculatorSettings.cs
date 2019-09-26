using kMCCoatings.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace kMCCoatings.Core.Configuration
{
    /// <summary>
    /// Settings of one calculation process.
    /// </summary>
    public class CalculatorSettings
    {
        /// <summary>
        /// The size of calculation
        /// </summary>
        public int Lx { get; set; }

        public int Ly { get; set; }

        public int Lz { get; set; }


    }
}
