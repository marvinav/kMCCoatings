using System;
using System.Collections.Generic;
using System.Text;
using kMCCoatings.Core.Constants;

namespace kMCCoatings.Core.Configuration
{
    public class DimerSettings
    {

        /// <summary>
        /// Список возможных трансляций для каждой пары атомов.
        ///</summary>
        public Dictionary<(short, short), List<(double, double, double)>> Translations { get; set; } = new Dictionary<(short, short), List<(double, double, double)>>();

        /// <summary>
        /// Список кристаллических решёток
        ///</summary>
        public List<Lattice> Lattices { get; set; }
        public DimerSettings()
        {
            var key = ((short)1, (short)1);
            var length = 2.0 * Math.Sqrt(2.0);
            // Ti - Ti (Ti - Cr, Cr - Cr) in fcc
            var translations = new List<(double, double, double)>()
            {
                (-length, 0, 0),
                (length, 0, 0),
                (0, length, 0),
                (0, -length, 0),
                (0, 0, length),
                (0, 0, -length)
            };
            Translations.Add(key, translations);
            // Ti - N in fcc
            key = (1, 2);
            translations = new List<(double, double, double)>()
            {
                (-1, -1, 0),
                (-1, 1, 0),
                (1, 1, 0),
                (1, -1, 0),
                (0, -1, -1),
                (0, -1, 1),
                (0, 1, -1),
                (0, 1, 1),
                (-1, 0, -1),
                (-1, 0, 1),
                (1, 0, -1),
                (1, 0, 1)
            };            
            // N - N interaction not now

            Translations.Add(key, translations);

        }
}
}
