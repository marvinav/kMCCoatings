using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;
using kMCCoatings.Core.Configuration;
using kMCCoatings.Core.Entities.AtomRoot;

namespace kMCCoatings.Core.Entities.DimerRoot
{
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
        /// Ориентация диммера в пространстве
        /// </summary>
        public DimerOrientation Orientation { get; set; }

        /// <summary>
        /// Атомы диммера
        /// </summary>
        public List<Atom> Atoms { get; set; }

        /// <summary>
        /// Список трансляций в глобальных системах координат
        /// </summary>
        public List<(double, double, double)> Translations { get; set; }

        /// <summary>
        /// Формируем димер при связывании двух атомов
        /// </summary>
        /// <param name="firstAtom"></param>
        /// <param name="secondAtom"></param>
        public Dimer(Atom firstAtom, Atom secondAtom, DimerSettings dimerSettings)
        {
            // Инициализация димера            
            //Получение порядкового номера димера и присваивание его атомам
            DimerCounter++;
            Id = DimerCounter;
            firstAtom.Site.DimerId = Id;
            secondAtom.Site.DimerId = Id;
            Atoms = new List<Atom>()
            {
                firstAtom, secondAtom
            };
            var dimerOrientation = GlobalCoordinates.GetRotationOfVector(firstAtom.Site.Coordinates, secondAtom.Site.Coordinates);
            //Расчёт ориентации диммера в пространстве
            Orientation = new DimerOrientation()
            {
                CosAlfa = dimerOrientation.Item1, 
                CosBetta = dimerOrientation.Item2, 
                CosGamma = dimerOrientation.Item3 
            };
            // Расчёт трансляций димера
            Translations = CalculateTranslations(Orientation, dimerSettings);
        }

        /// <summary>
        /// Расчёт трансляций диммера в глобальные системы координат
        /// </summary>
        /// <param name="dimer"></param>
        /// <param name="dimerSettings"></param>
        private static List<(double, double, double)> CalculateTranslations(DimerOrientation dimerOrientation, DimerSettings dimerSettings)
        {

            var m1 = dimerOrientation;
            throw new NotImplementedException();
        }
    }
}
