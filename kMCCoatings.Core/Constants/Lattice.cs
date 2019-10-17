using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using kMCCoatings.Core.Entities;
using kMCCoatings.Core.Extension;

namespace kMCCoatings.Core.Constants
{
    ///<summary>
    ///Описание кристаллической решётки
    ///</summary>
    public class Lattice
    {
        ///<summary>
        ///Имя кристаллической решётки
        ///</summary>
        public string Name { get; set; }

        ///<summary>
        ///Список атомов кристаллической решётки
        ///</summary>
        private int[] Atoms { get; set; }

        ///<summary>
        ///Словарь возможных кристаллографических направлений в зависимости от пар атомов.
        ///</summary>
        private Dictionary<(int, int), Vector3> LatticeDirections { get; set; } = new Dictionary<(int, int), Vector3>();

        public Vector3 BasicVector { get; set; }

        /// <summary>
        /// Создать кристаллографическую решётку соединения
        /// </summary>
        /// <param name="atoms">Атомы кристаллической решётки</param>
        /// <param name="name">Название химического соединения</param>
        /// <param name="latticeDirections">Кристаллографическое направление, формируемыми двумя парами атомов</param>
        /// <param name="basicVector">Кристаллографическое направление, относительно которых сформирован список трансляций</param>
        /// <remarks>
        /// Список трансляций у basicVector - это не кристаллографические трансляция,
        /// а векторы к атомомам, с которыми формируется связь
        /// </remarks>
        public Lattice(string name, int[] atoms, Dictionary<(int, int), Vector3> latticeDirections, Vector3 basicVector)
        {
            Name = name;
            Atoms = atoms;
            LatticeDirections = latticeDirections;
            BasicVector = basicVector;
        }

        public Lattice() { }
        ///<summary>
        ///Положение химического элемента в кристаллической решётки
        ///</summary>
        private Dictionary<int, int> ElementsPosition { get; set; } = new Dictionary<int, int>();

        ///<summary>
        ///Добавить элемент в словарь кристаллической решётки
        ///</summary>
        ///<param name="atomTypeId">Тип атома относительно кристаллической решётки</param>
        ///<param name="elementId">Порядковый номер химического элемента</param>
        public void AddElementsToLattice(int elementId, int atomTypeId)
        {
            if (Atoms.Length >= atomTypeId)
            {
                ElementsPosition.Add(elementId, atomTypeId);
            }
            else
            {
                throw new System.Exception("Тип атома в кристаллической решётки при добавлении элемента в словарь превышает указанное число атомов.");
            }
        }

        ///<summary>
        /// Определяет, формируют ли два указанных атома кристаллическую решётку.
        ///</summary>
        public bool IsContains(int firstElementId, int secondElementId)
        {
            bool firstExist = false;
            bool secondExist = false;

            foreach (var item in ElementsPosition.Keys)
            {
                if (!firstExist)
                {
                    firstExist = item == firstElementId;
                }
                if (!secondExist)
                {
                    secondExist = item == secondElementId;
                }
            }

            return firstExist == true && firstExist == secondExist;
        }


        ///<summary>
        /// Возвращает вектор в глобальных координатах, соответствующий базовому кристаллографическому направлению
        ///</summary>
        public Vector3 GetBasicVectors(int firstElementId, int secondElementId, Vector3 vector)
        {
            var firstAtomType = Atoms[firstElementId];
            var secondAtomType = Atoms[secondElementId];
            var key = firstAtomType < secondAtomType ? (firstAtomType, secondAtomType) : (secondAtomType, firstAtomType);
            var latticeVector = LatticeDirections[key];

            if (latticeVector.Equals(BasicVector))
            {
                return vector;
            }
            else
            {
                var cos = BasicVector.Cosines(vector);
                var sin = (float)Math.Sqrt(1 - Math.Pow(cos, 2));

                return new Vector3()
                {
                    X = (vector.X * cos - vector.Y * sin),
                    Y = (vector.X * sin - vector.Y * cos),
                    Z = 0
                };
            }
        }
    }
}