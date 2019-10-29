using System.Collections.Generic;
using System.Linq;
using kMCCoatings.Core.Configuration;
using kMCCoatings.Core.Entities.AtomRoot;
using kMCCoatings.Core.Extension;
using MathNet.Spatial.Euclidean;

namespace kMCCoatings.Core.Entities.SiteRoot
{
    public class Site
    {

        public CalculatorSettings CalculatorSettings { get; set; }
        /// <summary>
        /// Координаты сайта.
        /// </summary>
        public Point3D Coordinates { get; set; }

        /// <summary>
        /// Состояние сайта
        /// </summary>
        public SiteStatus SiteStatus { get; set; }

        /// <summary>
        /// Тип сайта
        /// </summary>
        public SiteType SiteType { get; set; }

        /// <summary>
        /// Атом, размещённый в сайте
        /// </summary>
        public Atom OccupiedAtom { get; set; }

        /// <summary>
        /// Список всех элементов, которые могут окупировать этот сайт
        /// </summary>
        public int[] AtomTypeIds { get; set; }

        /// <summary>
        /// Список соседних атомов у сайта
        /// </summary>
        public List<Site> CrossSites { get; set; } = new List<Site>();

        /// <summary>
        /// Список сайтов возле димеров, где ключ - сайт сосед, а значение - диммерный сайт
        /// </summary>
        public Dictionary<Site, Site> DimerSites { get; set; } = new Dictionary<Site, Site>();

        public List<Atom> PossibleToDiffuse
        {
            get
            {
                return CrossSites.Where(site => CalculatorSettings.Dimension.
                    CalculateDistance(site.Coordinates, Coordinates) <= CalculatorSettings.PossibleToDifuseRadius).
                    Select(x => x.OccupiedAtom).ToList();
            }
        }
        /// <summary>
        /// Словарь, который определяет энергия (value) атома заданного типа (key)
        /// </summary>
        public Dictionary<int, double> Energies { get; set; } = new Dictionary<int, double>();

        /// <summary>
        /// Добавить атома в поле взаимодействия
        /// </summary>
        public void AddSite(Site site)
        {
            if (!CrossSites.Contains(site))
            {
                CrossSites.Add(site);
                // Энергию сайта обновляем только в том случае, если добавляемый сайт занят атомом и расстояние менее радиуса взаимодействия
                if (site.SiteStatus == SiteStatus.Occupied && CalculatorSettings.Dimension.CalculateDistance(site.Coordinates, Coordinates) <= CalculatorSettings.InteractionRadius)
                {
                    foreach (var elementId in site.OccupiedAtom.Element.InteractionEnergy)
                    {
                        if (Energies.ContainsKey(elementId.Key))
                        {
                            Energies[elementId.Key] += site.OccupiedAtom.Element.InteractionEnergy[elementId.Key];
                        }
                        else
                        {
                            Energies.Add(elementId.Key, site.OccupiedAtom.Element.InteractionEnergy[elementId.Key]);
                        }
                    }
                    // Если оба сайта заняты атомом, то при соблюдении дистанции между ними должна быть связь диммера
                    if (SiteStatus == SiteStatus.Occupied)
                    {
                        var newDimerSites = new Dictionary<Site, Site>();
                        foreach (var neigborhoodSite in CrossSites.Where(x => CalculatorSettings.Dimension.CalculateDistance(Coordinates, x.Coordinates) <= CalculatorSettings.PossibleToDifuseRadius))
                        {
                            if (DimerSites.TryGetValue(neigborhoodSite, out var dimerSite))
                            {
                                //TODO: Проверить состояние сайта после добавления атома
                                if (CalculatorSettings.Dimension.CalculateDistance(dimerSite.Coordinates, site.Coordinates) < CalculatorSettings.ForbiddenRadius)
                                {
                                    dimerSite.SiteStatus = SiteStatus.Prohibited;
                                }
                            }
                            else
                            {
                                // Находим сайт между текущим атомом и соседом
                                dimerSite = FindSiteBetweenAtomsBySites(neigborhoodSite);
                                if (dimerSite != null)
                                {
                                    newDimerSites.Add(neigborhoodSite, dimerSite);
                                }
                            }
                        }
                        foreach (var item in newDimerSites)
                        {
                            DimerSites.Add(item.Key, item.Value);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Добавить список атомов в поле взаимодействия
        /// </summary>
        public void AddSitesWithReverse(IEnumerable<Site> sites)
        {
            foreach (var site in sites)
            {
                if (CalculatorSettings.Dimension.CalculateDistance(site.Coordinates, Coordinates) <= CalculatorSettings.ForbiddenRadius)
                {
                    if (site.SiteStatus != SiteStatus.Occupied)
                    {
                        /// Если в результате добавления атома в соседи, расстояние до сайта меньше или равно минимальному радиусу, то стату сайта меням на запрещён
                        site.SiteStatus = SiteStatus.Prohibited;
                    }
                    else if (SiteStatus == SiteStatus.Occupied)
                    {
                        throw new System.Exception("Атомы находятся ближе предельного допустимого радиуса");
                    }

                }
                site.AddSite(this);
                this.AddSite(site);
            }

        }

        /// <summary>
        /// Получить энергию взаимодействия атома в данном сайте
        /// </summary>
        public double EnergyInSite(int elementId)
        {
            return Energies[elementId];
        }

        /// <summary>
        /// Находит сайты между свободно-жиффундирующими атомомами, формирует эти сайты, изменяя сайты
        /// </summary>
        public Site FindSiteBetweenAtomsBySites(Site targetSite)
        {
            Site result = null;
            var cals = CalculatorSettings;
            // Связь с диффундирующим атомом определяется так, что строится прямая между двумя свободными сайтами
            // Из этого направления считается противоположный с росстоянием, равным справочнику и прибавляется к целевому атому

            var direction = Coordinates - targetSite.Coordinates;
            var scaledDirection = direction.ScaleBy(OccupiedAtom.Element.InteractionRadius[targetSite.OccupiedAtom.Element.Id] / direction.Length);
            var coordinates = targetSite.Coordinates + scaledDirection;
            var reversedCoordinates = targetSite.Coordinates - scaledDirection;

            var neigborhoods = CrossSites.Union(targetSite.CrossSites).Except(new List<Site>(2) { targetSite, this });

            // Если количество соседних сайтов с заданным радиусом, отвечающим контактному правилу меньше трёх (за вычетом текущих двух атомов)
            if (neigborhoods.Count(x => x.SiteStatus == SiteStatus.Occupied && cals.Dimension.CalculateDistance(x.Coordinates, coordinates) < cals.ContactRadius) > cals.ContactRule)
            {
                var site = new Site()
                {
                    Coordinates = coordinates,
                    SiteStatus = SiteStatus.Vacanted,
                    SiteType = SiteType.Dimer,
                    CalculatorSettings = CalculatorSettings
                };
                site.AddSitesWithReverse(neigborhoods.Union(new List<Site>(2) { this, targetSite }));
            }

            return result;
        }
    }
}