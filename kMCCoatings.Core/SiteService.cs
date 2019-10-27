using System;
using System.Collections.Generic;
using kMCCoatings.Core.Configuration;
using kMCCoatings.Core.Entities.SiteRoot;
using kMCCoatings.Core.Extension;
using MathNet.Spatial.Euclidean;

namespace kMCCoatings.Core
{
    public class SiteService
    {
        public Dictionary<Point3D, List<Site>> SitesByCells { get; set; }
        public CalculatorSettings CalculatorSettings { get; set; }

        public SiteService(CalculatorSettings calcSet)
        {
            CalculatorSettings = calcSet;
            SitesByCells = new Dictionary<Point3D, List<Site>>();
        }

        public List<Site> GetSites(Point3D coord)
        {
            var pX = (int)Math.Floor(coord.X);
            var pY = (int)Math.Floor(coord.Y);
            var pZ = (int)Math.Floor(coord.Z);
            var crossR = (int)CalculatorSettings.CrossRadius;
            var zLeft = pZ - crossR < 0 ? 0 : pZ - crossR;
            var zRight = pZ + crossR > (int)CalculatorSettings.Dimension.Z ? (int)CalculatorSettings.Dimension.Z : pZ + crossR;

            var result = new List<Site>();
            for (int x = pX - crossR; x <= pX + crossR; x++)
            {
                for (int y = pY - crossR; y <= pY + crossR; y++)
                {
                    for (int z = zLeft; z <= zRight; z++)
                    {
                        if (SitesByCells.TryGetValue(CalculatorSettings.Dimension.TranslatePointInDimension(new Point3D(x, y, z)), out var sites))
                        {
                            result.AddRange(sites);
                        }
                    }
                }
            }
            return result;
        }

        public void AddRange(List<Site> sites)
        {
            foreach (var site in sites)
            {
                Add(site);
            }
        }

        public void Add(Site site)
        {
            var pX = (int)Math.Floor(site.Coordinates.X);
            var pY = (int)Math.Floor(site.Coordinates.Y);
            var pZ = (int)Math.Floor(site.Coordinates.Z);
            var key = new Point3D(pX, pY, pZ);
            if (SitesByCells.TryGetValue(key, out var oldSites))
            {
                if (oldSites.Contains(site)) throw new Exception("ПОпытка добавить существующий сайт");
                oldSites.Add(site);
            }
            else
            {
                SitesByCells.Add(key, new List<Site>() { site });
            }

        }

    }
}
