using System.Collections.Generic;
using kMCCoatings.Core.Entities.AtomRoot;
using MathNet.Spatial.Euclidean;

namespace kMCCoatings.Core.Entities.SiteRoot
{
    public class Site
    {
        /// <summary>
        /// Координаты сайта.
        /// </summary>
        public Point3D Coordinates { get; set; }

        /// <summary>
        /// Состояние сайта
        /// </summary>
        public SiteStatus SiteStatus { get; set; }

        /// <summary>
        /// Тип сайта
        /// </summary>
        public SiteType SiteType { get; set; }

        /// <summary>
        /// Атом, размещённый в сайте
        /// </summary>
        public Atom OccupiedAtom { get; set; }

        /// <summary>
        /// Список всех элементов, которые могут окупировать этот сайт
        /// </summary>
        public int[] AtomTypeIds { get; set; }

        /// <summary>
        /// Список соседних атомов у сайта
        /// </summary>
        public List<Atom> NeigborhoodsAtom { get; set; }

        public Dictionary<int, double> Energies { get; set; }

        public void AddAtomToInteractionField(Atom atom)
        {
            NeigborhoodsAtom.Add(atom);
            foreach (var elementId in AtomTypeIds)
            {
                Energies[elementId] += atom.Element.InteractionEnergy[elementId];
            }
        }

        /// <summary>
        /// Получить энергию взаимодействия атома в данном сайте
        /// </summary>
        public double EnergyInSite(int elementId)
        {
            return Energies[elementId];
        }
    }
}