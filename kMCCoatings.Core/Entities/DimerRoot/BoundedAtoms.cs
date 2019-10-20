using System.Collections.Generic;
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
        public int BoundedAtomsNumber;
        public Lattice Lattice { get; set; }

        public Atom[] Atoms { get; set; }

        public static int BoundedAtomsCounter = 0;

        /// <summary>
        /// Формирирование связи двух атомов
        /// </summary>
        public BoundedAtoms(Atom firstAtom, Atom secondAtom, Lattice lattice)
        {
            BoundedAtomsNumber = BoundedAtomsCounter;
            BoundedAtomsCounter++;
            Atoms = new Atom[2]
            {
                firstAtom, secondAtom
            };
            Lattice = lattice;
        }

        public List<Transition> CalculateTransitions()
        {
            //TODO: Рассчитать переход и учесть адгезию с близлежайщими атомами
            return new List<Transition>()
            {
                new Transition()
                {
                    Atom = Atoms[0],
                    TargetSite = new SiteRoot.Site()
                    {
                        SiteType = SiteRoot.SiteType.Free
                    }
                }
            };
        }
    }
}
