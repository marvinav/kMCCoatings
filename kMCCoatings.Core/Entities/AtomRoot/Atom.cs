using System;
using System.Collections.Generic;
using System.Text;
using kMCCoatings.Core.Entities.SiteRoot;
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

        /// <summary>
        /// Координаты атома
        /// </summary>
        public Point3D Coordinate { get; set; }

        /// <summary>
        /// Рассчитать все возможные переходы
        /// </summary>
        public static void CalculateTransitions(Atom atom)
        {

        }

        /// <summary>
        /// Выполняет расчёт расстояния до указанного атома
        /// </summary>
        public double CalculateDistance(Atom second)
        {
            return second.Coordinate.DistanceTo(Coordinate);
        }

        /// <summary>
        /// Выполняет расчёт расстояния до указанного атома
        /// </summary>
        public double CalculateDistance(Atom second, Point3D dimension)
        {
            double distance;
            if (Math.Abs(second.Coordinate.X - Coordinate.X) > dimension.X / 2
                || Math.Abs(second.Coordinate.Y - Coordinate.Y) > dimension.Y / 2)
            {
                double moreX, lessX, moreY, lessY;
                (moreX, lessX) = Coordinate.X > second.Coordinate.X
                    ? (Coordinate.X, second.Coordinate.X)
                    : (second.Coordinate.X, Coordinate.X);
                (moreY, lessY) = Coordinate.Y > second.Coordinate.Y
                                        ? (Coordinate.Y, second.Coordinate.Y)
                                        : (second.Coordinate.Y, Coordinate.Y);
                distance = (new Vector3D(
                            dimension.X - moreX + lessX,
                            dimension.Y - moreY + lessY,
                            second.Coordinate.Z - Coordinate.Z)).Length;
            }
            else
            {
                distance = Coordinate.DistanceTo(second.Coordinate);
            }
            return distance;
        }
    }
}
