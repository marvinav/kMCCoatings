using kMCCoatings.Core.Configuration;
using kMCCoatings.Core.Entities;
using kMCCoatings.Core.Entities.AtomRoot;
using kMCCoatings.Core.Entities.SiteRoot;
using kMCCoatings.Core.Extension;
using MathNet.Spatial.Euclidean;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kMCCoatings.Core
{
    public class Calculator
    {

        /// <summary>
        /// Общее время интегрирования
        /// </summary>
        public double CalculationTime { get; set; }

        /// <summary>
        /// Пространство расчётов
        /// </summary>
        public Point3D Dimension { get; set; }

        /// <summary>
        /// Список всех атомов покрытия
        /// </summary>
        public List<Atom> Atoms { get; set; } = new List<Atom>();

        /// <summary>
        /// Список атомов, чьи энергии необходимо пересчитать
        /// </summary>
        public List<Atom> AffectedAtoms { get; set; } = new List<Atom>();

        /// <summary>
        /// Список переходов, которые будут осуществляться в этой иттерации.
        /// </summary>

        public Dictionary<int, Transition> Transitions { get; set; }

        /// <summary>
        /// Список всех сайтов
        /// </summary>
        public List<Site> Sites { get; set; } = new List<Site>();

        /// <summary>
        /// Список свободных сайтов
        /// </summary>
        public List<Site> VacantedSites { get; set; }

        /// <summary>
        /// Список запрещённых сайтов
        /// </summary>
        public List<Site> ForbiddenSites { get; set; }
        public DimerSettings DimerSettings { get; set; }

        public double CrossRadius { get; set; }

        public double DiffusionRadius { get; set; }

        public double ForbiddenRadius { get; set; }

        public double ContactRadius { get; set; }
        public Calculator()
        {
        }

        /// <summary>
        /// Обновляем состоянием атомов
        /// </summary>
        public void UpdateAtomsState()
        {
            // Parallel.ForEach(AffectedAtoms, afAtoms =>
            // {
            //     afAtoms.Neigborhoods = Atoms.Where(at => at.CalculateDistance(afAtoms) <= CrossRadius).ToList();
            //     afAtoms.Site = new Site()
            //     {
            //         Coordinates = afAtoms.Coordinate,
            //         OccupiedAtom = afAtoms,
            //         SiteStatus = SiteStatus.Occupied
            //     };
            // });
        }

        /// <summary>
        /// Начать процесс интеграции
        /// </summary>
        public void Start()
        {

        }

        /// <summary>
        /// Добавить атом к вычислениям
        /// <summary>
        public void AddAtom(Point3D coord, Element element)
        {
            var sites = Sites.Where(site => Dimension.CalculateDistance(site.Coordinates, coord) < CrossRadius).ToList();
            var atom = new Atom()
            {
                Element = element,
                Site = new Site()
                {
                    Coordinates = coord,
                    SiteType = SiteType.Free,
                    SiteStatus = SiteStatus.Occupied,
                    NeigborhoodsSites = sites
                }
            };
            atom.Site.OccupiedAtom = atom;
            atom.Site.AddAtomsToInteractionField(sites.Where(s => s.OccupiedAtom != null).Select(s => s.OccupiedAtom).ToList());
            //TODO: после инициализации атома, нужно просканировать окружение на неянвные сайты (наличие рядом св. атома)
            var sitesBetweenAtoms = atom.FindSiteBetweenAtoms(CrossRadius, ForbiddenRadius, ContactRadius, Dimension,
                atom.Site.NeigborhoodsAtom.Where(x => x.Site.SiteType == SiteType.Free));
            atom.Site.NeigborhoodsSites.AddRange(sitesBetweenAtoms);

            Atoms.Add(atom);
            Sites.Add(atom.Site);
            Sites.AddRange(sitesBetweenAtoms);
        }

        /// <summary>
        /// Формируем список переходов для атома
        /// </summary>
        public List<Transition> CalculateTransion(Atom movedAtom, params Site[] targetSites)
        {
            var result = new List<Transition>();
            foreach (var targetSite in targetSites)
            {
                // Получаем у перемещённого атома список старых связей
                var oldNeigborhoods = movedAtom.Site.NeigborhoodsAtom;
                // Получаем новые списки связей: их необходимо обновить
                var newNeigborhoods = targetSite.NeigborhoodsAtom.Concat(oldNeigborhoods);
                var lostNeigborhoods = oldNeigborhoods.Concat(newNeigborhoods).ToList();
                //TODO: реализовать обновление параметров сайтов и атомов, оказавшихся в области воздействия атома
                //NOTE: послать список приобритённых и список потерянных связей
                var oldSite = movedAtom.Site;
                var oldEnergy = oldSite.EnergyInSite(movedAtom.Element.Id);
                var difEnergy = oldEnergy - targetSite.EnergyInSite(movedAtom.Element.Id);

                result.Add(new Transition(movedAtom, targetSite, difEnergy));
            }
            return result;
        }
    }
}
