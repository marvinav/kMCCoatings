using System;
using System.Collections.Generic;
using System.Linq;
using kMCCoatings.Core.Configuration;
using kMCCoatings.Core.Entities.AtomRoot;
using kMCCoatings.Core.Entities.SiteRoot;
using kMCCoatings.Core.Extension;
using MathNet.Spatial.Euclidean;

namespace kMCCoatings.Core
{
    public class SiteService
    {
        public Dictionary<Point3D, List<Site>> SitesByCells { get; set; }
        public CalculatorSettings CalculatorSettings { get; set; }
        public int HighestNotEmptyCell { get; set; }

        public SiteService(CalculatorSettings calcSet)
        {
            CalculatorSettings = calcSet;
            SitesByCells = new Dictionary<Point3D, List<Site>>();
        }

        public List<Site> GetSites(Point3D coord)
        {
            return GetSites(coord, (int)CalculatorSettings.CrossRadius);
        }

        /// <summary>
        /// Получить сайт вокруг указанной координаты с заданным радиусом
        /// </summary>
        public List<Site> GetSites(Point3D coord, int radius)
        {
            var pX = (int)Math.Floor(coord.X);
            var pY = (int)Math.Floor(coord.Y);
            var pZ = (int)Math.Floor(coord.Z);
            var zLeft = pZ - radius < 0 ? 0 : pZ - radius;
            var zRight = pZ + radius > (int)CalculatorSettings.Dimension.Z ? (int)CalculatorSettings.Dimension.Z : pZ + radius;

            var result = new List<Site>();
            for (int x = pX - radius; x <= pX + radius; x++)
            {
                for (int y = pY - radius; y <= pY + radius; y++)
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

        // Проверяет, принадлежат ли сайты к одной клетки пространства
        public bool IsOneCell(Site firstPoint, Site secondPoint)
        {
            return (Math.Floor(firstPoint.Coordinates.X) - Math.Floor(secondPoint.Coordinates.X)) == 0 &&
                (Math.Floor(firstPoint.Coordinates.Y) - Math.Floor(secondPoint.Coordinates.Y)) == 0 &&
                (Math.Floor(firstPoint.Coordinates.Z) - Math.Floor(firstPoint.Coordinates.Z)) == 0;
        }

        /// <summary>
        /// Находит сайты между свободно-диффундирующий атомомами, формирует эти сайты, изменяя сайты
        /// </summary>
        public List<Site> FindSitesBetweenAtoms(Atom firstAtom, Atom secondAtom)
        {
            List<Site> result = new List<Site>();
            // Связь с диффундирующим атомом определяется так, что строится прямая между двумя свободными сайтами
            // Из этого направления считается противоположный с росстоянием, равным справочнику и прибавляется к целевому атому
            var direction = secondAtom.Site.Coordinates - firstAtom.Site.Coordinates;
            var scaledDirection = direction.ScaleBy(firstAtom.Element.InteractionRadius[secondAtom.Element.Id] / direction.Length).Round();

            // Если вектор диффузии превышает максимальный радиус диффузии, то атомы не формируют сайты
            if (direction.Length - scaledDirection.Length <= CalculatorSettings.DiffusionRadius)
            {
                var firstToSecond = firstAtom.Site.Coordinates + scaledDirection;
                var secondToFirst = secondAtom.Site.Coordinates - scaledDirection;
                // Находим соседей этих двух сайтов
                var neighborhoods = GetSites(firstAtom.Site.Coordinates + (direction / 2));

                var fSite = CheckDimerSiteRule(neighborhoods, firstToSecond, secondAtom);
                var sSite = CheckDimerSiteRule(neighborhoods, secondToFirst, firstAtom);
                result.Add(fSite);
                result.Add(sSite);
                // Если количество соседних сайтов с заданным радиусом, отвечающим контактному правилу меньше трёх (за вычетом текущих двух атомов)

            }
            return result;
        }

        private Site CheckDimerSiteRule(List<Site> neighborhoods, Point3D coor, Atom dimerAtom)
        {
            var site = new Site()
            {
                Coordinates = coor,
                SiteType = SiteType.Dimer,
                DimerAtom = dimerAtom
            };
            var forbRadius = false;
            var numberOfContact = 0;
            foreach (var nSite in neighborhoods.Where(x => x.SiteStatus == SiteStatus.Occupied))
            {
                var dist = CalculatorSettings.Dimension.CalculateDistance(nSite.Coordinates, coor);
                if (dist <= CalculatorSettings.ForbiddenRadius && nSite.OccupiedAtom != dimerAtom)
                {
                    forbRadius = true;
                }
                if (dist <= CalculatorSettings.ContactRadius)
                {
                    numberOfContact++;
                }
                if (dist <= CalculatorSettings.InteractionRadius)
                {
                    site.AddAtomToInteractionField(nSite.OccupiedAtom);
                }
            }
            var prohibitedReason = forbRadius ? ProhibitedReason.ForbiddenRadius : ProhibitedReason.None;
            if (numberOfContact < CalculatorSettings.ContactRule)
            {
                prohibitedReason = prohibitedReason == ProhibitedReason.None ? ProhibitedReason.ContactRule : ProhibitedReason.All;
            }

            site.ProhibitedReason = prohibitedReason;
            site.SiteStatus = prohibitedReason != ProhibitedReason.None ? SiteStatus.Prohibited : SiteStatus.Vacanted;

            return site;
        }

        /// <summary>
        /// Получить список доступной поверхности
        /// </summary>
        public Point3D[] GetCellOnSurface(int density)
        {
            var availableCells = new Point3D[density];
            int generated = 0;
            var rnd = new Random();
            while (generated < density)
            {
                var x = rnd.Next((int)CalculatorSettings.Dimension.X);
                var y = rnd.Next((int)CalculatorSettings.Dimension.Y);
                var point = new Point3D(x, y, HighestNotEmptyCell);
                var isEmpty = true;
                while (isEmpty)
                {
                    var sitesInPoint = GetSites(point, 1);
                    if (sitesInPoint.Any(x => x.SiteStatus == SiteStatus.Occupied))
                    {
                        point = new Point3D(x, y, point.Z + 1);
                        isEmpty = false;
                    }
                    else
                    {
                        point = new Point3D(x, y, point.Z - 1);
                    }
                }
                availableCells[generated] = point;
                generated++;
            }
            return availableCells;
        }
    }
}
