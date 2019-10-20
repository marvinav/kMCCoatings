using System.Linq;
using kMCCoatings.Core.Configuration;
using kMCCoatings.Core.Entities.AtomRoot;
using kMCCoatings.Core.LatticeRoot;

namespace kMCCoatings.Core.Entities.DimerRoot
{
    /// <summary>
    /// Связь двух атомов в пространстве
    /// </summary>
    public class BoundedAtoms
    {
        public Lattice Lattice { get; set; }

        public Atom[] Atoms { get; set; }

        public static int BoundedAtomsCounter = 0;

        /// <summary>
        /// Формирирование связи
        /// </summary>
        public BoundedAtoms(Atom firstAtom, Atom secondAtom, DimerSettings dimerSettings)
        {
            BoundedAtomsCounter++;
            firstAtom.Site.BoundedAtomsCounter = BoundedAtomsCounter;
            secondAtom.Site.BoundedAtomsCounter = BoundedAtomsCounter;
            Atoms = new Atom[2]
            {
                firstAtom, secondAtom
            };
            // Ищем подходящую решётку
            Lattice = (from Lattice lattice in dimerSettings.Lattices
                       where lattice.IsContains(firstAtom.ElementId, secondAtom.ElementId)
                       select lattice).FirstOrDefault();
        }
    }
}