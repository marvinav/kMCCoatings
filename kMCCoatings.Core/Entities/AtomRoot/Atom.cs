using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using kMCCoatings.Core.Entities.SiteRoot;
using kMCCoatings.Core.Extension;
using kMCCoatings.Core.LatticeRoot;
using MathNet.Spatial.Euclidean;

namespace kMCCoatings.Core.Entities.AtomRoot
{
    public class Atom
    {

        /// <summary>
        /// Номер атома в вычислениях
        /// </summary>
        public int AtomNumber { get; set; }

        /// <summary>
        /// Тип атома
        /// </summary>
        public Element Element { get; set; }

        /// <summary>
        /// Сайт, в котором находится атом
        /// </summary>
        public Site Site { get; set; }

        /// <summary>
        /// Список всех возможных переходов для указанного атома
        /// </summary>
        public List<Transition> Transitions { get; set; }

        /// <summary>
        /// Находит сайты между свободно-жиффундирующими атомомами, формирует эти сайты, изменяя сайты
        /// </summary>
        public void FindSiteBetweenAtomsBySites(IEnumerable<Site> targetSites)
        {
            var cals = Site.CalculatorSettings;
            //Связь с диффундирующим атомом определяется так, что строится прямая между двумя свободными сайтами
            //Из этого направления считается противоположный с росстоянием, равным справочнику и прибавляется к целевому атому
            foreach (var targetSite in targetSites)
            {
                var direction = Site.Coordinates - targetSite.Coordinates;
                var coordinates = targetSite.Coordinates + direction.ScaleBy(Element.InteractionRadius[targetSite.OccupiedAtom.Element.Id] / direction.Length);
                var site = new Site()
                {
                    Coordinates = targetSite.Coordinates + direction.ScaleBy(Element.InteractionRadius[targetSite.OccupiedAtom.Element.Id] / direction.Length),
                    SiteStatus = SiteStatus.Vacanted,
                    SiteType = SiteType.Dimer
                };
                var neigborhoods = Site.NeigborhoodsSite.Union(targetSite.NeigborhoodsSite).Except(new List<Site>(2) { targetSite, Site });
                if (neigborhoods.FirstOrDefault(x => x.SiteStatus == SiteStatus.Occupied && cals.Dimension.CalculateDistance(x.Coordinates, coordinates) < cals.ForbiddenRadius) == default(Site))
                {
                    // Если количество соседних сайтов с заданным радиусом, отвечающим контактному правилу меньше трёх (за вычетом текущих двух атомов)
                    if (neigborhoods.Count(x => x.SiteStatus == SiteStatus.Occupied && cals.Dimension.CalculateDistance(x.Coordinates, site.Coordinates) < cals.ContactRadius) < cals.ContactRule)
                    {
                        site.AddSitesWithReverse(neigborhoods.Union(new List<Site>(2) { Site, targetSite }));
                    }
                }
            }
        }

        /// <summary>
        /// Формируем список переходов для атома
        /// </summary>
        public List<Transition> CalculateTransion(IEnumerable<Site> targetSites)
        {
            var result = new List<Transition>();
            foreach (var targetSite in targetSites)
            {
                //TODO: реализовать обновление параметров сайтов и атомов, оказавшихся в области воздействия атома
                //NOTE: послать список приобритённых и список потерянных связей
                var oldEnergy = Site.EnergyInSite(Element.Id);
                var difEnergy = oldEnergy - targetSite.EnergyInSite(Element.Id);
                result.Add(new Transition(this, targetSite, difEnergy));
            }
            return result;
        }

        /// <summary>
        /// Формируем список переходов для атома и его связей
        /// </summary>
        public List<Transition> CalculateTransion()
        {
            var result = CalculateTransion(Site.NeigborhoodsSite.
                Where(x => x.SiteStatus == SiteStatus.Vacanted &&
                Site.CalculatorSettings.Dimension.CalculateDistance(Site.Coordinates, x.Coordinates) <= Site.CalculatorSettings.DiffusionRadius));
            return result;
        }
    }
}
