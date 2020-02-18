using MathNet.Numerics.LinearAlgebra;
using MathNet.Spatial.Euclidean;

namespace kMCCoatings.Core.LatticeRoot
{
    public class ElementsPosition
    {
        public int AtomId { get; set; }

        public int[] ElementIds { get; set; }

        public Point3D Coordinates { get; set; }
    }
}