using kMCCoatings.Core.Configuration;
using kMCCoatings.Core.Entities;
using kMCCoatings.Core.Entities.Atom;
using System;
using System.Collections.Generic;
using System.Text;

namespace kMCCoatings.Core
{
    public class Calculator
    {
        /// <summary>
        /// Пространство интеграции
        /// </summary>
        public static Dimension Dimension { get; set; }

        /// <summary>
        /// Общее время интегрирования
        /// </summary>
        public double CalculationTime { get; set; }

        public Calculator(CalculatorSettings calculatorSettings)
        {
            Dimension = new Dimension(calculatorSettings.Lx, calculatorSettings.Ly, calculatorSettings.Lz);
        }



        public void FindPossibleSitesForAtom(Atom atom)
        {
            // Получаем список клетки, в которой находится атом
            var cell = Dimension.Cells[atom.Site.X, atom.Site.Y, atom.Site.Z];
            var 
        }

    }
}
