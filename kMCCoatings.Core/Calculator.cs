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

        public Dictionary<Atom, List<Transition>> Transitions { get; set; } = new Dictionary<Atom, List<Transition>>();

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
            // Формируем сайт
            var site = new Site
            {
                Coordinates = coord,
                SiteType = SiteType.Free,
                SiteStatus = SiteStatus.Occupied
            };
            var atom = new Atom(element, site);
            Atoms.Add(atom);
            SiteService.Add(atom.Site);
            // Получаем список сайтов и атомов, изменившихся после добавления нового атома
            // 1 Нужно для каждого соседнего атома посчитать наличие возможных сайтов
            // 2 Для всех соседних сайтов нужно проверить их запрещённость
            var affectedSites = SiteService.GetSites(coord).Except(new List<Site> { atom.Site });
            var affectedAtoms = new List<Site>();
            var newSites = new List<Site>();

            foreach (var afSite in affectedSites)
            {
                var dist = CalculatorSettings.Dimension.CalculateDistance(coord, afSite.Coordinates);
                if (dist <= CalculatorSettings.ForbiddenRadius) // Если в запрещённой области вокруг атома есть сайты, то они становятся запрещёнными
                {
                    afSite.AddProhibitedReason(ProhibitedReason.ForbiddenRadius);
                    atom.Site.AddProhibitedReason(ProhibitedReason.ForbiddenRadius);
                }
                else if (dist <= CalculatorSettings.PossibleToDifuseRadius && afSite.SiteType == SiteType.Free) // Смотрим возможные димеры
                {
                    newSites.AddRange(SiteService.FindSitesBetweenAtoms(atom, afSite.OccupiedAtom));
                }

                if (dist <= CalculatorSettings.ContactRadius && afSite.IsContactRuleProhibited())
                {
                    var contacts = affectedSites.Count(x => x.SiteStatus == SiteStatus.Occupied && CalculatorSettings.Dimension.CalculateDistance(x.Coordinates, afSite.Coordinates) <= CalculatorSettings.ContactRadius);
                    if (contacts >= CalculatorSettings.ContactRule - 1) // Учтён контакт с добавляемым атомом
                    {
                        afSite.RemoveProhibitedReason(ProhibitedReason.ContactRule);
                    }
                }
                // Обновляем энергии для сайтов
                if (dist <= CalculatorSettings.InteractionRadius)
                {
                    afSite.AddAtomToInteractionField(atom);
                    if (afSite.SiteStatus == SiteStatus.Occupied)
                    {
                        atom.Site.AddAtomToInteractionField(afSite.OccupiedAtom);
                    }
                }
                if (afSite.OccupiedAtom != null)
                {
                    affectedAtoms.Add(afSite);
                }
            }
            SiteService.AddRange(newSites);

            foreach (var afAtom in affectedAtoms.Union(new List<Site>() { atom.Site }))
            {
                var newTrans = CalculateTransition(afAtom.OccupiedAtom);
                if (Transitions.TryGetValue(afAtom.OccupiedAtom, out var oldTransitions))
                {
                    Transitions[afAtom.OccupiedAtom] = newTrans;
                }
                else
                {
                    Transitions.Add(afAtom.OccupiedAtom, newTrans);
                }
            }
        }

        public List<Transition> CalculateTransition(Atom atom)
        {
            var neigborhoods = SiteService.GetSites(atom.Site.Coordinates, (int)CalculatorSettings.DiffusionRadius);
            var transitions = new List<Transition>();
            foreach (var neigbore in neigborhoods)
            {
                // Сразу проверяем, что сайт не окупирован амомом
                if (neigbore.SiteStatus != SiteStatus.Occupied
                    && CalculatorSettings.Dimension.CalculateDistance(atom.Site.Coordinates, neigbore.Coordinates) <= CalculatorSettings.DiffusionRadius)
                {
                    if (neigbore.ElementIds.Contains(atom.Element.Id) && neigbore.SiteStatus != SiteStatus.Prohibited)
                    {
                        transitions.Add(atom.CalculateTransion(neigbore));
                    }
                    else if (neigbore.DimerAtom == atom && neigbore.ProhibitedReason != ProhibitedReason.ContactRule)
                    {
                        var forbiddenRadiusCounter = neigborhoods.Count(x => x.SiteStatus == SiteStatus.Occupied && CalculatorSettings.Dimension.CalculateDistance(neigbore.Coordinates, x.Coordinates) <= CalculatorSettings.ForbiddenRadius);
                        if (forbiddenRadiusCounter <= 1)
                        {
                            transitions.Add(atom.CalculateTransion(neigbore));
                        }
                    }
                }
            }
            return transitions;
        }

        public void MakeTransition(Transition transition)
        {
            throw new NotImplementedException();
        }
    }
}
