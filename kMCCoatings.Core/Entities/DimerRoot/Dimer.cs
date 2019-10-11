using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;
using kMCCoatings.Core.Configuration;
using kMCCoatings.Core.Entities.AtomRoot;

namespace kMCCoatings.Core.Entities.DimerRoot
{
    public class Dimer
    {
        /// <summary>
        /// Идентификатор диммера
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Формируем димер при связывании двух атомов
        /// </summary>
        /// <param name="firstAtom"></param>
        /// <param name="secondAtom"></param>
        public Dimer(Atom firstAtom, Atom secondAtom, DimerSettings dimerSettings)
        {
            
        }

        public static void CalculateVectors(GlobalCoordinates firstAtom, GlobalCoordinates secondAtom, Dimer dimer)
        {
            
        }
    }
}
