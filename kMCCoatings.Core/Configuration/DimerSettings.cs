using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;
using kMCCoatings.Core.Constants;

namespace kMCCoatings.Core.Configuration
{
    public class DimerSettings
    {

        /// <summary>
        /// Список возможных трансляций для каждой пары атомов определённой кристаллической решётки. ГЦК
        ///</summary>
        public Dictionary<int, (int, List<Vector3>)> Translations { get; set; } = new Dictionary<int, (int, List<Vector3>)>();

        /// <summary>
        /// Список кристаллических решёток
        ///</summary>
        public List<Lattice> Lattices { get; set; }
        public DimerSettings()
        {
            var length = 2.0f * (float)Math.Sqrt(2.0);
            // Ti - Ti (Ti - Cr, Cr - Cr) in fcc
            var translations = new List<Vector3>()
            {
                new Vector3(length, 0, 0),
                new Vector3(-length, 0, 0),
                new Vector3(0, length, 0),
                new Vector3(0, -length, 0),
                new Vector3(0, 0, length),
                new Vector3(0, 0, -length)
            };

            Translations.Add(0, (0, translations));

            // Ti - N in fcc            
            var translationOfTiN = new List<Vector3>()
            {
                new Vector3(-1, -1, 0),
                new Vector3(-1, 1, 0),
                new Vector3(1, 1, 0),
                new Vector3(1, -1, 0),
                new Vector3(0, -1, -1),
                new Vector3(0, -1, 1),
                new Vector3(0, 1, -1),
                new Vector3(0, 1, 1),
                new Vector3(-1, 0, -1),
                new Vector3(-1, 0, 1),
                new Vector3(1, 0, -1),
                new Vector3(1, 0, 1)
            };
            // NOTE: N - N interaction not now
            Translations.Add(0, (1, translationOfTiN));

        }
    }
}
