using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using kMCCoatings.Core.LatticeRoot;
using MathNet.Spatial.Euclidean;
using MathNet.Spatial.Units;

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

        /// <summary>
        /// Преобразует крист.направление в вектор в глобальных координатах
        /// </summary>
        public static Vector3D ParseVectorInGlobal(this CoordinateSystem cs, string vector)
        {
            var vectors = vector.Split(" ");
            var coefX = Convert.ToDouble(vectors[1]);
            var coefY = Convert.ToDouble(vectors[2]);
            var coefZ = Convert.ToDouble(vectors[3]);
            return (coefX * cs.XAxis) + (coefY * cs.YAxis) + (coefZ * cs.ZAxis);
        }

        /// <summary>
        /// Вращение вектора относительно другого вектора с заданным шагом от нуля до 360
        /// </summary>
        public static Vector3D[] RotateVector(this Vector3D vector, Vector3D aroundAxis, int angle)
        {
            var result = new Vector3D[360 / angle];
            for (int i = 0; i < 360; i += angle)
            {
                result[i] = vector.Rotate(aroundAxis, Angle.FromDegrees(i));
            }
            return result;
        }

        /// <summary>
        /// Выполняет расчёт расстояния между указанными точками с учетом ограниченного пространства
        /// </summary>
        public static double CalculateDistance(this Point3D dimension, Point3D first, Point3D second)
        {
            double distance;
            if (Math.Abs(second.X - first.X) > dimension.X / 2
                || Math.Abs(second.Y - first.Y) > dimension.Y / 2)
            {
                double moreX, lessX, moreY, lessY;
                (moreX, lessX) = first.X > second.X
                    ? (first.X, second.X)
                    : (second.X, first.X);
                (moreY, lessY) = first.Y > second.Y
                                        ? (first.Y, second.Y)
                                        : (second.Y, first.Y);
                distance = new Vector3D(
                            dimension.X - moreX + lessX,
                            dimension.Y - moreY + lessY,
                            second.Z - first.Z).Length;
            }
            else
            {
                distance = first.DistanceTo(second);
            }
            return distance;
        }

        public static Point3D TranslatePointInDimension(this Point3D dimension, Point3D point)
        {
            double x, y, z;
            x = point.X > dimension.X ? point.X - dimension.X : point.X < 0 ? point.X + dimension.X : point.X;
            y = point.Y > dimension.Y ? point.Y - dimension.Y : point.Y < 0 ? point.Y + dimension.Y : point.Y;
            z = point.Z;
            return new Point3D(x, y, z);
        }

        public static Dictionary<int, List<InteractionEnergy>> GetInteractionEnergiesForElement(this List<InteractionEnergy> energies, int elementId)
        {
            var elementEnergies = energies.Where(e => e.Elements.Contains(elementId));
            //TODO: сделать формирование словаря энергий для каждого атома
            return null;

        }
    }
}