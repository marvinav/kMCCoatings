using kMCCoatings.Core.Entities;
using MathNet.Spatial.Euclidean;
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
        public Point3D Dimension { get; set; }

        /// <summary>
        /// Радиус сферы вокруг атома, которая зависит от положения атома
        /// </summary>
        public double CrossRadius { get; set; }

        /// <summary>
        /// Радиус сферы вокруг атома, куда может диффунзировать атом
        /// </summary>
        public double DiffusionRadius { get; set; }

        /// <summary>
        /// Минимальное расстояние между двумя атомами
        /// </summary>
        public double ForbiddenRadius { get; set; }

        /// <summary>
        /// Радиус, при котором связь считается при подсчёте контакта
        /// </summary>
        public double ContactRadius { get; set; }

        /// <summary>
        /// Радиус взаимодействия атомов
        /// </summary>
        public double InteractionRadius { get; set; }

        /// <summary>
        /// Максимальный радиус связп межжду двумя атомами, при котором они могут сформировать димер
        /// </summary>
        public double PossibleToDifuseRadius => DiffusionRadius * 2;
        public int ContactRule { get; set; }

    }
}
