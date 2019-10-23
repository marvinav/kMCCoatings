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
        public List<Atom> Atoms { get; set; }

        /// <summary>
        /// Список атомов, чьи энергии необходимо пересчитать
        /// </summary>
        public List<Atom> AffectedAtoms { get; set; }

        /// <summary>
        /// Список переходов, которые будут осуществляться в этой иттерации.
        /// </summary>        
        public List<Transition> Transitions { get; set; }

        /// <summary>
        /// Список всех сайтов
        /// </summary>
        public List<Site> Sites { get; set; }

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
        public Calculator(CalculatorSettings calculatorSettings)
        {
            CrossRadius = calculatorSettings.CrossRadius;
        }

        public void AddAtoms(IEnumerable<Atom> atoms)
        {
            Atoms.AddRange(atoms);
            AffectedAtoms = Atoms;
        }

        /// <summary>
        /// Обновляем состоянием атомов
        /// </summary>
        public void UpdateAtomsState()
        {
            Parallel.ForEach(AffectedAtoms, afAtoms =>
            {
                afAtoms.Neigborhoods = Atoms.Where(at => at.CalculateDistance(afAtoms) <= CrossRadius).ToList();
                afAtoms.Site = new Site()
                {
                    Coordinates = afAtoms.Coordinate,
                    OccupiedAtom = afAtoms,
                    SiteStatus = SiteStatus.Occupied
                };
            });
        }

        /// <summary>
        /// Начать процесс интеграции
        /// </summary>
        public void Start()
        {

        }

        /// Добавить атом в вычисления
        public void AddAtom(Point3D coord, Element element)
        {
            var atom = new Atom()
            {
                Element = element,
                Site = new Site()
                {
                    Coordinates = coord,
                    SiteType = SiteType.Free,
                    SiteStatus = SiteStatus.Occupied,
                    NeigborhoodsAtom = Atoms.Where(atom => Dimension.CalculateDistance(atom.Site.Coordinates, coord) < CrossRadius).ToList()
                }
            };

            Atoms.Add(atom);
        }

        /// <summary>
        /// Формируем список переходов
        /// </summary>
        public void CalculateTransion(Atom movedAtom, Site targetSite)
        {
            // Получаем у перемещённого атома список старых связей
            var oldNeigborhoods = movedAtom.Site.NeigborhoodsAtom;
            // Получаем новые списки связей: их необходимо обновить
            var newNeigborhoods = targetSite.NeigborhoodsAtom.Concat(oldNeigborhoods);
            var lostNeigborhoods = oldNeigborhoods.Concat(newNeigborhoods).ToList();


            //TODO: реализовать обновление параметров сайтов и атомов, оказавшихся в области воздействия атома
            // NOTE: послать список приобритённых и список потерянных связей

            var oldSite = movedAtom.Site;
            var oldEnergy = oldSite.EnergyInSite(movedAtom.Element.Id);
            var difEnergy = oldEnergy - targetSite.EnergyInSite(movedAtom.Element.Id);


            // var transtions = new List<Transition>();
            // var vacSites = VacantedSites.Where(vs => movedAtom.CalculateDistance(vs.Coordinates, Dimension) < CrossRadius);
            // // Получаем сайты уже сформированных диммеров
            // Parallel.ForEach(vacSites, site =>
            // {
            //     if (site.AtomTypeIds.Contains(movedAtom.Element.Id) && site.SiteStatus == SiteStatus.Vacanted)
            //     {
            //         transtions.Add(new Transition(movedAtom, site, site.EnergyInSite(movedAtom.Element.Id) - energy));
            //     }
            // });
            // // Ищем свободные атомы
            // Parallel.ForEach(movedAtom.Site.NeigborhoodsAtom.Where(na => na.Site.SiteType != SiteType.Lattice), source =>
            // {
            //     source.Site.Si
            // });
        }
    }
}
