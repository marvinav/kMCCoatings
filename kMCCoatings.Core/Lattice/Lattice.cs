using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using MathNet.Spatial.Euclidean;
using MathNet.Spatial.Units;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace kMCCoatings.Core.LatticeRoot
{
    public class Lattice
    {
        /// <summary>
        /// Имя соединения
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Количество атомов в решётке
        /// </summary>
        public int NumberOfAtoms { get; set; }
        /// <summary>
        /// Положение хим.элементов решётки
        /// </summary>
        public ElementsPosition[] ElementsPosition { get; set; }
        /// <summary>
        /// Взаимное расположение атомов в решётки
        /// </summary>
        public AtomsRelation[] AtomsRelations { get; set; }

        /// <summary>
        /// Правило формирования диммера
        /// </summary>
        public DimerRule[] DimerRules { get; set; }

        /// <summary>
        /// Правила формирования сайтов путём трансляций
        /// </summary>
        public TranslationRule[] TranslationRules { get; set; }

        /// <summary>
        /// Получить список трансляция для димера в глобальных координатах
        /// </summary>
        /// <param name="cs"> Базис димера в глобальных координатах </param>
        public Translation[] GetTranslations(CoordinateSystem cs)
        {
            var translations = new List<Translation>();
            foreach (var transRule in TranslationRules)
            {
                var transSites = new List<Vector3D>();
                foreach (var tranRule in transRule.Rules)
                {
                    // Получение поворотов
                    var toTurn = ParseVector(tranRule.Turn, cs);
                    var axis = ParseVector(tranRule.Around, cs);
                    transSites.AddRange(RotateVector(toTurn, axis, tranRule.Angle));
                }
                translations.Add(new Translation()
                {
                    AtomId = transRule.AtomId,
                    BoundedAtomId = transRule.BoundedAtomId,
                    Translations = transSites.ToArray()
                });
            }
            return translations.ToArray();
        }

        /// <summary>
        /// Преобразует крист.направление в вектор в глобальных координатах
        /// </summary>
        public Vector3D ParseVector(string vector, CoordinateSystem cs)
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
        public Vector3D[] RotateVector(Vector3D vector, Vector3D aroundAxis, int angle)
        {
            var result = new Vector3D[360 / angle];
            for (int i = 0; i < 360; i += angle)
            {
                result[i] = vector.Rotate(aroundAxis, Angle.FromDegrees(i));
            }
            return result;
        }

        ///<summary>
        /// Определяет, формируют ли два указанных атома кристаллическую решётку.
        ///</summary>
        public bool IsContains(int firstElementId, int secondElementId)
        {
            var result = false;
            var firstId = ElementsPosition.Where(x => x.Elements.Contains(firstElementId)).FirstOrDefault()?.AtomId;
            var secondId = ElementsPosition.Where(x => x.Elements.Contains(secondElementId)).FirstOrDefault()?.AtomId;
            if (firstId != null && secondId != null)
            {
                int lessId;
                int moreId;
                if (firstId >= secondId)
                {
                    lessId = (int)secondId;
                    moreId = (int)firstId;
                }
                else
                {
                    lessId = (int)firstId;
                    moreId = (int)secondId;
                }
                foreach (var rule in DimerRules.Select(x => x.BoundedAtoms))
                {
                    if (rule[0] == lessId && rule[1] == moreId)
                    {
                        result = true;
                        break;
                    }
                }
            }
            return result;
        }
    }
}
