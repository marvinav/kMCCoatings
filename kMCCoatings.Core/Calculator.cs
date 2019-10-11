using kMCCoatings.Core.Configuration;
using kMCCoatings.Core.Entities;
using kMCCoatings.Core.Entities.AtomRoot;
using kMCCoatings.Core.Entities.SiteRoot;
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
        /// Пространство интеграции
        /// </summary>
        public static Dimension Dimension { get; set; }

        /// <summary>
        /// Общее время интегрирования
        /// </summary>
        public double CalculationTime { get; set; }


        public ConcurrentDictionary<int, ConcurrentBag<Transition>> Transitions { get; set; }

        public Calculator(CalculatorSettings calculatorSettings)
        {
            Dimension = new Dimension(calculatorSettings.Lx, calculatorSettings.Ly, calculatorSettings.Lz);
        }


        /// <summary>
        /// Найти возможные сайты для атома
        /// </summary>
        /// <param name="atom"></param>
        public void FindPossibleSitesForAtom(Atom atom)
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <remarks>
        /// 1 Смотрим, что атом не формирует димер с другим атомом. Проверка выполняются по поиску сайтов в незанятых клетках.
        /// 2 Если димеров нет, то смотрим наличие атомов в соседних клетках и формируем новый димер, при наличие хотя бы одного с наименьшим расстоянием.
        /// 3 Если в соседних клетках нет атомов, то добавляем переход - свободная диффузия по подложке.
        /// </remarks>
        public void FirstDiffusionAfterDeposition()
        {
            Parallel.ForEach(Transitions, trans =>
            {

            });
            // Перебираем каждый атом после первого напыления
            foreach (var cell in Dimension.Cells.Values.Where(c => c.OccupiedSite != null))
            {
                var distanceToAtoms = new Dictionary<GlobalCoordinates, double>();

                foreach (var neighborCell in cell.Neighbors)
                {
                    var nCell = Dimension.Cells[neighborCell];
                    if(nCell.OccupiedSite == null && nCell.Sites != null) // 1
                    {
                        // Рассчитываем переходы к сайтам
                    }
                    else if(nCell.OccupiedSite != null) // 2
                    {
                        distanceToAtoms.Add(nCell.Coordinates, Atom.CalculateDistance(cell.OccupiedSite.OccupiedAtom, nCell.OccupiedSite.OccupiedAtom));
                    }                    
                }
                if(distanceToAtoms != null)
                {
                    // Формируем димер с атомом с наименьшим расстоянием, рассчитываем сайты и переходы
                }
                else if(cell.OccupiedSite.OccupiedAtom.Transitions == null) // 3
                {
                    // Рассчитываем переходы по подложке
                }
            }
        }

        /// <summary>
        /// Осуществляем переход
        /// </summary>
        public void MakeTransition()
        {

        }

        /// <summary>
        /// Рассчитать переходы
        /// </summary>
        /// <param name="occupiedSite"></param>
        /// <param name="sites"></param>
        /// <returns></returns>
        public List<Transition> CalculateTransitions(Site occupiedSite, List<Site> sites)
        {
            var transtions = new List<Transition>();

            if(occupiedSite.OccupiedAtom != null)
            {

            }
            else
            {
                throw new Exception("Рассчёт перехода невозможен для сайта без атома.");
            }
            return transtions;
        }

    }
}
