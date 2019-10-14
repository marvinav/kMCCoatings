using System.Numerics;

namespace kMCCoatings.Core.Extension
{
    public static class VectorExtension
    {
        /// <summary>
        /// Return cosines angle between two vectors
        /// </summary>
        public static double Cosines(this Vector3 basicVector, Vector3 vector)
        {
            return ((basicVector.X * vector.X) + (basicVector.Y * vector.Y) + (basicVector.Z * vector.Z)) / basicVector.Length() / vector.Length();
        }
    }
}