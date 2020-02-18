using MathNet.Spatial.Euclidean;

namespace kMCCoatings.Core.LatticeRoot
{
    public class AtomsRelation
    {
        public int FirstAtomId { get; set; }

        public int SecondAtomId { get; set; }

        public Vector3D Direction { get; set; }
    }
}