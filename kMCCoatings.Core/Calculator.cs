using kMCCoatings.Core.Configuration;
using kMCCoatings.Core.Entities;
using kMCCoatings.Core.Entities.AtomRoot;
using kMCCoatings.Core.Entities.DepositionRoot;
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
        /// Список всех атомов покрытия
        /// </summary>
        public List<Atom> Atoms { get; set; } = new List<Atom>();

        /// <summary>
        /// Список атомов, чьи энергии необходимо пересчитать
        /// </summary>
        public List<Atom> AffectedAtoms { get; set; } = new List<Atom>();

        /// <summary>
        /// Список переходов, которые будут осуществляться в этой итерации.
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

        public Settings Settings { get; set; }

        /// <summary>
        /// Служба управления сайтами
        /// </summary>
        public SiteService SiteService { get; set; }

        public Deposition Deposition { get; set; }

        public Calculator(Settings settings)
        {
            Settings = settings;
            Deposition = new Deposition(settings.Deposition.ConcentrationFlow);
            SiteService = new SiteService(settings.Calc);
        }

        /// <summary>
        /// Начать процесс интеграции
        /// </summary>
        public void Start()
        {

        }

        public void Deposit()
        {
            var flows = Deposition.MakeStep();
            var elements = new List<Element>();
            foreach (var element in flows)
            {
                for (int i = 0; i < element.Concentration; i++)
                {
                    elements.Add(element.Element);
                }
            }
            var availableCells = SiteService.GetCellOnSurface(elements.Count);
            for (int i = 0; i < availableCells.Length; i++)
            {
                // AddAtom(availableCells[i], elements[i]);
            }
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
                var dist = Settings.Calc.Dimension.CalculateDistance(coord, afSite.Coordinates);
                if (dist <= Settings.Calc.ForbiddenRadius) // Если в запрещённой области вокруг атома есть сайты, то они становятся запрещёнными
                {
                    afSite.AddProhibitedReason(ProhibitedReason.ForbiddenRadius);
                    atom.Site.AddProhibitedReason(ProhibitedReason.ForbiddenRadius);
                }
                else if (dist <= Settings.Calc.PossibleToDifuseRadius && afSite.SiteType == SiteType.Free) // Смотрим возможные димеры
                {
                    newSites.AddRange(SiteService.FindSitesBetweenAtoms(atom, afSite.OccupiedAtom));
                }

                if (dist <= Settings.Calc.ContactRadius && afSite.IsContactRuleProhibited())
                {
                    var contacts = affectedSites.Count(x => x.SiteStatus == SiteStatus.Occupied && Settings.Calc.Dimension.CalculateDistance(x.Coordinates, afSite.Coordinates) <= Settings.Calc.ContactRadius);
                    if (contacts >= Settings.Calc.ContactRule - 1) // Учтён контакт с добавляемым атомом
                    {
                        afSite.RemoveProhibitedReason(ProhibitedReason.ContactRule);
                    }
                }
                // Обновляем энергии для сайтов
                if (dist <= Settings.Calc.InteractionRadius)
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
            var neighborhoods = SiteService.GetSites(atom.Site.Coordinates, (int)Settings.Calc.DiffusionRadius);
            var transitions = new List<Transition>();
            foreach (var neighbor in neighborhoods)
            {
                // Сразу проверяем, что сайт не оккупирован атомом
                if (neighbor.SiteStatus != SiteStatus.Occupied
                    && Settings.Calc.Dimension.CalculateDistance(atom.Site.Coordinates, neighbor.Coordinates) <= Settings.Calc.DiffusionRadius)
                {
                    if (neighbor.ElementIds.Contains(atom.Element.Id) && neighbor.SiteStatus != SiteStatus.Prohibited)
                    {
                        transitions.Add(atom.CalculateTransion(neighbor));
                    }
                    else if (neighbor.DimerAtom == atom && neighbor.ProhibitedReason != ProhibitedReason.ContactRule)
                    {
                        var forbiddenRadiusCounter = neighborhoods.Count(x => x.SiteStatus == SiteStatus.Occupied && Settings.Calc.Dimension.CalculateDistance(neighbor.Coordinates, x.Coordinates) <= Settings.Calc.ForbiddenRadius);
                        if (forbiddenRadiusCounter <= 1)
                        {
                            transitions.Add(atom.CalculateTransion(neighbor));
                        }
                    }
                }
            }
            return transitions;
        }
    }
}
