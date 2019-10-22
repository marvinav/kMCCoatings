using kMCCoatings.Core.Configuration;
using kMCCoatings.Core.Entities;
using kMCCoatings.Core.Entities.AtomRoot;
using kMCCoatings.Core.Entities.SiteRoot;
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
        /// Список возможных сайтов
        /// </summary>
        public List<Site> Sites { get; set; }

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

        /// <summary>
        /// П
        /// </summary>
        public void ScanCrossField(Atom affectedAtom)
        {
            var energy = affectedAtom.Site.EnergyInSite(affectedAtom.Element.Id);
            var transtions = new List<Transition>();
            Parallel.ForEach(Sites, (Action<Site>)(site =>
            {
                if (site.AtomTypeIds.Contains(affectedAtom.Element.Id) && site.SiteStatus == SiteStatus.Vacanted)
                {
                    transtions.Add(new Transition(affectedAtom, site, (double)(site.Energies[affectedAtom.Element.Id] - energy)));
                }
            }));
        }
    }
}
