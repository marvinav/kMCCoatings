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
        public List<Site> NeigborhoodsSite { get; set; } = new List<Site>();

        /// <summary>
        /// Словарь, который определяет энергия (value) атома заданного типа (key)
        /// </summary>
        public Dictionary<int, double> Energies { get; set; } = new Dictionary<int, double>();

        /// <summary>
        /// Добавить атома в поле взаимодействия
        /// </summary>
        public void AddSite(Site site)
        {
            if (!NeigborhoodsSite.Contains(site))
            {
                NeigborhoodsSite.Add(site);
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
                }
            }
        }

        /// <summary>
        /// Добавить список атомов в поле взаимодействия
        /// </summary>
        public void AddSitesWithReverse(IEnumerable<Site> sites)
        {
            if (sites != null)
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
                        else
                        {
                            throw new System.Exception("Атомы находятся ближе предельного допустимого радиуса");
                        }

                    }
                    site.AddSite(this);
                    this.AddSite(site);
                }
            }
        }

        /// <summary>
        /// Получить энергию взаимодействия атома в данном сайте
        /// </summary>
        public double EnergyInSite(int elementId)
        {
            return Energies[elementId];
        }
    }
}