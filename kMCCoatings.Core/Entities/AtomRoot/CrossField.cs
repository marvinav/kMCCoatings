using System.Collections.Generic;
using MathNet.Spatial.Euclidean;

namespace kMCCoatings.Core.Entities.AtomRoot
{

    /// <summary>
    /// Запрещённая область вокруг атома
    /// </summary>
    public struct CrossField
    {
        /// <summary>
        /// Координаты атома
        /// </summary>
        public Point3D AtomCor { get; set; }

        public CrossField(Point3D atomCoor)
        {
            AtomCor = atomCoor;
        }

        // public Point3D GetEmptySpace(IEnumerable<Point3D> points, double crosRad, Point3D leftBound, Point3D rightBound)
        // {

        //     return true;
        // }
    }
}