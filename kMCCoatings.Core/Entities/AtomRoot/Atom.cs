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
        /// Список соседних атомов
        /// </summary>
        public List<Atom> Neigborhoods { get; set; }

        public List<Site> FindSiteBetweenAtoms(double crossRadius, double forbiddenRaduis, double contactRadius, Point3D dimension, IEnumerable<Atom> targetAtoms)
        {
            var result = new List<Site>();
            //Связь с диффундирующим атомом определяется так, что строится прямая между двумя свободными сайтами
            //Из этого направления считается противоположный с росстоянием, равным справочнику и прибавляется к целевому атому
            foreach (var targetAtom in targetAtoms)
            {
                var direction = Site.Coordinates - targetAtom.Site.Coordinates;
                var coordinates = targetAtom.Site.Coordinates + direction.ScaleBy(Element.InteractionRadius[targetAtom.Element.Id] / direction.Length);
                var site = new Site()
                {
                    Coordinates = targetAtom.Site.Coordinates + direction.ScaleBy(Element.InteractionRadius[targetAtom.Element.Id] / direction.Length),
                    SiteStatus = SiteStatus.Vacanted,
                    OccupiedAtom = targetAtom,
                    SiteType = SiteType.Dimer
                };
                if (site.NeigborhoodsAtom.FirstOrDefault(x => dimension.CalculateDistance(x.Site.Coordinates, coordinates) < forbiddenRaduis) != default(Atom))
                {
                    site.SiteStatus = SiteStatus.Prohibited;
                }
                site.NeigborhoodsSites = targetAtom.Site.NeigborhoodsSites.Union(Site.NeigborhoodsSites).Where(x => dimension.CalculateDistance(coordinates, x.Coordinates) < crossRadius).ToList();
                site.AddAtomsToInteractionField(targetAtom.Site.NeigborhoodsAtom.
                    Union(Site.NeigborhoodsAtom).
                    Where(x => dimension.CalculateDistance(site.Coordinates, x.Site.Coordinates) < crossRadius));
                // Если количество соседних сайтов с заданным радиусом, отвечающим контактному правилу меньше трёх (за вычетом текущих двух атомов)
                if (site.NeigborhoodsAtom.Count(x => dimension.CalculateDistance(x.Site.Coordinates, site.Coordinates) < contactRadius) < 3)
                {
                    result.Add(site);
                    targetAtom.Site = site;
                }
            }
            return result;
        }
    }
}
