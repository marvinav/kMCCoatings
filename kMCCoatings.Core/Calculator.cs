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

        public CalculatorSettings CalculatorSettings { get; set; }

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

        public Dictionary<Atom, List<Transition>> Transitions { get; set; }


        /// <summary>
        /// Список свободных сайтов
        /// </summary>
        public List<Site> VacantedSites { get; set; }

        /// <summary>
        /// Список запрещённых сайтов
        /// </summary>
        public List<Site> ForbiddenSites { get; set; }
        public DimerSettings DimerSettings { get; set; }

        /// <summary>
        /// Служба управления сайтами
        /// </summary>
        public SiteService SiteService { get; set; }
        public Calculator()
        {
        }
        public Calculator(CalculatorSettings calculatorSettings)
        {
            CalculatorSettings = calculatorSettings;
            SiteService = new SiteService(CalculatorSettings);
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
            // Получаем сайты в радиусе воздействия атома (CrossRadius)

            var sites = SiteService.GetSites(coord).Where(site => CalculatorSettings.Dimension.CalculateDistance(site.Coordinates, coord) < CalculatorSettings.CrossRadius).ToList();
            var atom = new Atom()
            {
                Element = element,
                // Формируем сайт, в котором сидит атом
                Site = new Site()
                {
                    Coordinates = coord,
                    SiteType = SiteType.Free,
                    SiteStatus = SiteStatus.Occupied,
                    CalculatorSettings = CalculatorSettings
                }
            };

            atom.Site.OccupiedAtom = atom;
            // Находим блуждающие атомы и рассчитываем для них связи
            //NOTE: для димерных и кристаллических атомов пересчёт структур производиться не будет в предположении, что дифундирующий атом на может выбить на себя атом из решётки
            atom.FindSiteBetweenAtomsBySites(sites.Where(x => x.SiteStatus == SiteStatus.Occupied && CalculatorSettings.Dimension.CalculateDistance(x.Coordinates, coord) < CalculatorSettings.ContactRadius));
            // Формируем список соседних сайтов
            SiteService.AddRange(atom.Site.NeigborhoodsSite);
            atom.Site.AddSitesWithReverse(sites);
            //TODO: после добавления атома необходимо обновить сайты и переходы
            Atoms.Add(atom);
            SiteService.Add(atom.Site);
        }


    }
}
