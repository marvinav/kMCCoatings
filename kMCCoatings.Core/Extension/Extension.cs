using System;
using System.Collections.Generic;
using System.Numerics;

namespace kMCCoatings.Core.Extension
{
    public static class VectorExtension
    {
        /// <summary>
        /// Return cosines angle between two vectors
        /// </summary>
        public static float Cosines(this Vector3 basicVector, Vector3 vector)
        {
            return ((basicVector.X * vector.X) + (basicVector.Y * vector.Y) + (basicVector.Z * vector.Z)) / basicVector.Length() / vector.Length();
        }


        /// <summary>
        /// Возвращает повернутые векторы по указанной схеме в плоскости XY.
        /// </summary>
        /// <param name="schemes">
        /// Схема поврота, которая состоит уз положительного угла в градусах (>0) 
        /// Угол поднятия по оси z
        /// </param>
        /// <param name="precision"> Точность округления </param>
        public static List<Vector3> RotatesInXYPlane(this Vector3 basicVector, List<(double, double)> schemes, int precision = 5)
        {
            var result = new List<Vector3>();
            var lenght = basicVector.Length();
            foreach (var scheme in schemes)
            {
                var angleInRadXY = Math.PI * scheme.Item1 / 180.0000f;
                var sinInZ = Math.Sin(Math.PI * scheme.Item2 / 180.0000f);
                var cosInZ = Math.Cos(Math.PI * scheme.Item2 / 180.0000f);
                result.Add(new Vector3()
                {
                    X = (float)Math.Round(cosInZ *
                        (basicVector.X * Math.Cos(angleInRadXY) - basicVector.Y * Math.Sin(angleInRadXY)),
                        precision),
                    Y = (float)Math.Round(cosInZ *
                        (basicVector.X * Math.Sin(angleInRadXY) + basicVector.Y * Math.Cos(angleInRadXY)),
                        precision),
                    Z = (float)Math.Round(lenght * sinInZ, precision)
                });
            }
            return result;
        }

        /// <summary>
        /// Возвращает повернутые векторы по указанной схеме в плоскости XY.
        /// </summary>
        /// <param name="schemes">
        /// Схема поврота, которая состоит уз положительного угла в градусах (>0) 
        /// Угол поднятия по оси z, и коэффициента уменьшения длины вектора
        /// </param>
        /// <param name="precision"> Точность округления </param>
        public static List<Vector3> RotatesInXYPlaneAndShrink(this Vector3 basicVector, List<(double, double, double)> schemes, int precision = 5)
        {
            var result = new List<Vector3>();
            var lenght = basicVector.Length();
            foreach (var scheme in schemes)
            {
                var angleInRadXY = Math.PI * scheme.Item1 / 180.0000f;
                var sinInZ = Math.Sin(Math.PI * scheme.Item2 / 180.0000f);
                var cosInZ = Math.Cos(Math.PI * scheme.Item2 / 180.0000f);
                result.Add(new Vector3()
                {
                    X = (float)Math.Round(cosInZ *
                        (basicVector.X * Math.Cos(angleInRadXY) - basicVector.Y * Math.Sin(angleInRadXY)),
                        precision),
                    Y = (float)Math.Round(cosInZ *
                        (basicVector.X * Math.Sin(angleInRadXY) + basicVector.Y * Math.Cos(angleInRadXY)),
                        precision),
                    Z = (float)Math.Round(lenght * sinInZ, precision)
                });
            }
            return result;         
        }
    }
}