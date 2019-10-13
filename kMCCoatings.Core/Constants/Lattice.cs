using System.Collections.Generic;
using System.Linq;

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
        ///Словарь координат атомов
        ///</summary>
        private Dictionary<int, List<(double, double, double)>> Coordinates { get; set; } = new Dictionary<int, List<(double, double, double)>>();

        public Lattice(string name, int[] atoms, Dictionary<int, List<(double, double, double)>> coordinates)
        {
            Name = name;
            Atoms = atoms;
            Coordinates = coordinates;
        }
        public Lattice()
        {

        }

        ///<summary>
        ///Положение химического элемента в кристаллической решётки
        ///</summary>
        private List<(int, int)> ElementsPosition { get; set; } = new List<(int, int)>();

        ///<summary>
        ///Добавить элемент в словарь кристаллической решётки
        ///</summary>
        ///<param name="atomTypeId">Тип атома относительно кристаллической решётки</param>
        ///<param name="elementId">Порядковый номер химического элемента</param>
        public void AddElementsToLattice(int elementId, int atomTypeId)
        {
            if (Atoms.Length >= atomTypeId)
            {
                ElementsPosition.Add((elementId, atomTypeId));
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
            foreach(var item in ElementsPosition)
            {
                if(!firstExist)
                {
                    firstExist = item.Item1 == firstElementId;
                }
                if(!secondExist)
                {
                    secondExist = item.Item1 == secondElementId;
                }
            }
            return firstExist == true && firstExist == secondExist;
        }
    }
}