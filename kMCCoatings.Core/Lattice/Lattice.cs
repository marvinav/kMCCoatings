using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using kMCCoatings.Core.Entities.DimerRoot;
using kMCCoatings.Core.Extension;
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
        /// Энергия связи хим.эл-ов
        /// </summary>
        public InteractionEnergy[] ElementsEnergies { get; set; }

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
                    var toTurn = cs.ParseVectorInGlobal(tranRule.Turn);
                    var axis = cs.ParseVectorInGlobal(tranRule.Around);
                    transSites.AddRange(toTurn.RotateVector(axis, tranRule.Angle));
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
        /// Определяет, формируют ли два указанных атома кристаллическую решётку.
        /// </summary>
        public bool IsContains(int firstElementId, int secondElementId)
        {
            var result = false;
            var firstId = Array.Find(ElementsPosition, x => x.Elements.Contains(firstElementId))?.AtomId;
            var secondId = Array.Find(ElementsPosition, x => x.Elements.Contains(secondElementId))?.AtomId;
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
