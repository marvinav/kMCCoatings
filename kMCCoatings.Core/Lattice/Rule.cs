using MathNet.Spatial.Euclidean;

namespace kMCCoatings.Core.LatticeRoot
{
    public class Rule
    {
        public Vector3D Turn { get; set; }

        public Vector3D Around { get; set; }

        public int Angle { get; set; }
    }
}