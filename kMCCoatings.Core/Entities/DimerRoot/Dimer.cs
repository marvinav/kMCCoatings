using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;
using kMCCoatings.Core.Configuration;
using kMCCoatings.Core.Constants;
using kMCCoatings.Core.Entities.AtomRoot;
using System.Linq;

namespace kMCCoatings.Core.Entities.DimerRoot
{
    /// <summary>
    /// Зерно с определенной химической решёткой, ориентацией в пространстве и атомами.
    /// </summary>
    public struct Dimer
    {
        /// <summary>
        /// Счётчик димеров
        /// </summary>
        public static int DimerCounter { get; set; }

        /// <summary>
        /// Идентификатор диммера
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Атомы диммера
        /// </summary>
        public List<Atom> Atoms { get; set; }

        ///<summary>
        /// Кристаллическая решётка диммера
        ///</summary>
        public Lattice Lattice { get; set; }

        /// <summary>
        /// Основной кристаллографический вектор в глобальных координатах
        /// </summary>
        public Vector3 BasicVector { get; set; }

        /// <summary>
        /// Список трансляций в глобальных системах координат
        /// </summary>
        public Dictionary<int, (int, Vector3)> Translations { get; set; }

        /// <summary>
        /// Формируем димер при связывании двух атомов
        /// </summary>
        /// <param name="firstAtom"></param>
        /// <param name="secondAtom"></param>
        public Dimer(Atom firstAtom, Atom secondAtom, DimerSettings dimerSettings)
        {
            // Инициализация димера            
            // Получение порядкового номера димера и присваивание его атомам
            DimerCounter++;
            Id = DimerCounter;
            firstAtom.Site.DimerId = Id;
            secondAtom.Site.DimerId = Id;
            Atoms = new List<Atom>()
            {
                firstAtom, secondAtom
            };
            // Ищем подходящую решётку
            Lattice = (from Lattice lattice in dimerSettings.Lattices
                       where lattice.IsContains(firstAtom.AtomTypeId, secondAtom.AtomTypeId)
                       select lattice).FirstOrDefault();

            // Получаем кристаллографическое направление в глобальных координатах, сформировавшееся в диммере
            var globalVector = new Vector3(firstAtom.Site.X - secondAtom.Site.Y, firstAtom.Site.Y - secondAtom.Site.Y, firstAtom.Site.Z - firstAtom.Site.Z);
            BasicVector = Lattice.GetBasicVectors(firstAtom.AtomTypeId, secondAtom.AtomTypeId, globalVector);

            // Расчёт трансляций димера
            Translations = null;
        }
        /// <summary>
        /// Расчёт трансляций диммера в глобальные системы координат
        /// </summary>
        /// <param name="dimer"></param>
        /// <param name="dimerSettings"></param>
        private static List<Vector3> CalculateTranslations(Vector3 basicVector, DimerSettings dimerSettings)
        {
            // TODO: Получить список трансляций для димера в глобальных системах координат
            var translations = dimerSettings.Translations; // Список  трансляций в кристаллографических координатах
            var m1 = basicVector;
            throw new NotImplementedException();
        }
    }
}