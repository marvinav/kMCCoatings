using System.Collections.Generic;
using System.Linq;
using kMCCoatings.Core.Configuration;
using kMCCoatings.Core.Entities.AtomRoot;
using kMCCoatings.Core.Extension;
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
        /// Причина запрёщенности сайта
        /// </summary>
        public ProhibitedReason ProhibitedReason { get; set; }

        /// <summary>
        /// Счётчик запрещённых атомов
        /// </summary>
        public int ForbiddenRAtoms { get; set; }

        /// <summary>
        /// Тип сайта
        /// </summary>
        public SiteType SiteType { get; set; }

        /// <summary>
        /// Атом, размещённый в сайте
        /// </summary>
        public Atom OccupiedAtom { get; set; }

        /// <summary>
        /// В случае, есть сайт диммерный, то он определяется атомомом, которым сформирован сайт
        /// </summary>
        public Atom DimerAtom { get; set; }

        /// <summary>
        /// Список всех элементов, которые могут окупировать этот сайт
        /// </summary>
        public List<int> ElementIds { get; set; } = new List<int>();

        /// <summary>
        /// Словарь, который определяет энергия (value) атома заданного типа (key)
        /// </summary>
        public Dictionary<int, double> Energies { get; set; } = new Dictionary<int, double>();

        /// <summary>
        /// Добавить атома в поле взаимодействия
        /// </summary>
        public void AddAtomToInteractionField(Atom atom)
        {
            foreach (var elementId in atom.Element.InteractionEnergy)
            {
                if (Energies.ContainsKey(elementId.Key))
                {
                    Energies[elementId.Key] += atom.Element.InteractionEnergy[elementId.Key];
                }
                else
                {
                    Energies.Add(elementId.Key, atom.Element.InteractionEnergy[elementId.Key]);
                }
            }
        }

        /// <summary>
        /// Убрать атом из поля взаимодействия
        /// </summary>
        public void RemoveAtomFromInteractionField(Atom atom)
        {
            foreach (var elementId in atom.Element.InteractionEnergy)
            {
                if (Energies.ContainsKey(elementId.Key))
                {
                    Energies[elementId.Key] -= atom.Element.InteractionEnergy[elementId.Key];
                }
            }
        }

        /// <summary>
        /// Получить энергию взаимодействия атома в данном сайте
        /// </summary>
        public double EnergyInSite(int elementId)
        {
            return Energies[elementId];
        }

        public void AddProhibitedReason(ProhibitedReason prohibitedReason)
        {
            if (ProhibitedReason == ProhibitedReason.None)
            {
                ProhibitedReason = prohibitedReason;
            }
            else if (ProhibitedReason != prohibitedReason)
            {
                ProhibitedReason = ProhibitedReason.All;
            }
            SiteStatus = SiteStatus.Prohibited;
        }

        public void RemoveProhibitedReason(ProhibitedReason prohibitedReason)
        {
            if (ProhibitedReason == ProhibitedReason.All)
            {
                ProhibitedReason = prohibitedReason == ProhibitedReason.ContactRule ? ProhibitedReason.ForbiddenRadius : ProhibitedReason.ContactRule;
            }
            else
            {
                ProhibitedReason = ProhibitedReason.None;
                SiteStatus = SiteStatus.Vacanted;
            }
        }

        public bool IsContactRuleProhibited()
        {
            return ProhibitedReason == ProhibitedReason.ContactRule || ProhibitedReason == ProhibitedReason.All;
        }
    }
}