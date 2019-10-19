using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;
using kMCCoatings.Core.Constants;
using kMCCoatings.Core.Extension;
namespace kMCCoatings.Core.Configuration
{
    public class DimerSettings
    {
        /// <summary>
        /// Список кристаллических решёток
        ///</summary>
        public List<Lattice> Lattices { get; set; }


        public DimerSettings()
        {

        }

        /// <summary>
        /// Углы между базовым вектором и атомами в связи Me-Me
        /// </summary>
        public static List<(double, double)> AnglesOfMeMeBondInFCC()
        {
            return new List<(double, double)>
            {
                (0, 0),
                (90, 0),
                (180, 0),
                (270, 0),
                (0, 90),
                (0, -90)
            };
        }
        /// <summary>
        /// Угле между базовыми векторами и атомами в связи Me - N
        /// </summary>
        public static List<(double, double)> AngelsOfMeNBondInFCC()
        {
            return new List<(double, double)>
            {
                (45,0),
                (135,0),
                (225, 0),
                (315,0),
                (0,45),
                (0, -45),
                (90, 45),
                (90, -45),
                (180, 45),
                (180, -45),
                (270, 45),
                (270, -45)
            };
        }

        /// <summary>
        /// Угле между базовыми векторами и атомами в связи N - Me
        /// </summary>
        public static List<(double, double)> AngelsOfNMeBondInFCC()
        {
            return new List<(double, double)>
            {
                (45,0),
                (135,0),
                (225, 0),
                (315,0)
            };
        }

        /// <summary>
        /// Угле между базовыми векторами и атомами в связи N - N
        /// </summary>
        public static List<(double, double)> AngelsOfNNBondInFCC()
        {
            return new List<(double, double)>
            {
                (0,45),
                (0,-45),
                (90, 45),
                (90, -45),
                (180, 45),
                (180, -45),
                (270, 45),
                (270,-45)
            };
        }
    }
}
