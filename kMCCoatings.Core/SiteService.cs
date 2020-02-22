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
        public Settings Settings { get; set; }
        public double HighestNotEmptyCell { get; set; }

        public SiteService(Settings set)
        {
            Settings = set;
            SitesByCells = new Dictionary<Point3D, List<Site>>();
        }

        public List<Site> GetSites(Point3D coord)
        {
            return GetSites(coord, (int)Settings.Calc.CrossRadius);
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
            var zRight = pZ + radius > (int)Settings.Calc.Dimension.Z ? (int)Settings.Calc.Dimension.Z : pZ + radius;

            var result = new List<Site>();
            for (int x = pX - radius; x <= pX + radius; x++)
            {
                for (int y = pY - radius; y <= pY + radius; y++)
                {
                    for (int z = zLeft; z <= zRight; z++)
                    {
                        if (SitesByCells.TryGetValue(Settings.Calc.Dimension.TranslatePointInDimension(new Point3D(x, y, z)), out var sites))
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
                if (oldSites.Contains(site)) throw new Exception("Попытка добавить существующий сайт");
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
            if (direction.Length - scaledDirection.Length <= Settings.Calc.DiffusionRadius)
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
                var dist = Settings.Calc.Dimension.CalculateDistance(nSite.Coordinates, coor);
                if (dist <= Settings.Calc.ForbiddenRadius && nSite.OccupiedAtom != dimerAtom)
                {
                    forbRadius = true;
                }
                if (dist <= Settings.Calc.ContactRadius)
                {
                    numberOfContact++;
                }
                if (dist <= Settings.Calc.InteractionRadius)
                {
                    site.AddAtomToInteractionField(nSite.OccupiedAtom);
                }
            }
            var prohibitedReason = forbRadius ? ProhibitedReason.ForbiddenRadius : ProhibitedReason.None;
            if (numberOfContact < Settings.Calc.ContactRule)
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
            var generated = new List<Point3D>(density);
            int count = 0, valid = 0;
            while (valid <= density - 1)
            {
                int x, y;
                do
                {
                    x = Settings.Rnd.Next((int)Settings.Calc.Dimension.X);
                    y = Settings.Rnd.Next((int)Settings.Calc.Dimension.Y);
                }
                while (generated.Any(p => p.X == x && p.Y == y));
                generated.Add(new Point3D(x, y, HighestNotEmptyCell));
                var isCellFree = true;
                while (isCellFree)
                {
                    isCellFree = !GetSites(generated[count], 1).Any(x => x.SiteStatus == SiteStatus.Occupied);
                    if (generated[count].Z < 0)
                    {
                        generated[count] = new Point3D(x, y, 0);
                        break;
                    }
                    else if (!isCellFree)
                    {
                        generated[count] = new Point3D(x, y, generated[count].Z + 1);
                        while (GetSites(generated[count], 1).Any(x => x.SiteStatus == SiteStatus.Occupied))
                        {
                            generated[count] = new Point3D(x, y, generated[count].Z + 1);
                        }
                        break;
                    }
                    else
                    {
                        generated[count] = new Point3D(x, y, generated[count].Z - 1);
                    }
                }
                availableCells[valid] = generated[count];
                valid++;
                HighestNotEmptyCell = HighestNotEmptyCell <= generated[count].Z ? generated[count].Z + 1 : HighestNotEmptyCell;
                count++;
            }
            return availableCells;
        }
    }
}
