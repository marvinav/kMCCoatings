using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using kMCCoatings.Core.Entities.AtomRoot;
using kMCCoatings.Core.Entities.DimerRoot;
using kMCCoatings.Core.Extension;
using kMCCoatings.Core.LatticeRoot;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace kMCCoatings.Core.Configuration
{
    public class DimerSettings
    {
        /// <summary>
        /// Список кристаллических решёток
        ///</summary>
        public Lattice[] Lattices { get; set; }


        public List<InteractionEnergy> InteractionEnergies { get; set; }

        /// <summary>
        /// Создает связь между атомами, если это разрешено
        /// </summary>
        public BoundedAtoms CreateBoundedAtoms(Atom fAtom, Atom sAtom)
        {
            BoundedAtoms result = null;
            // Ищем подходящую решётку
            for (int i = 0; i < Lattices.Length; i++)
            {
                if (Lattices[i].IsContains(fAtom.ElementId, sAtom.ElementId))
                {
                    result = new BoundedAtoms(fAtom, sAtom, Lattices[i]);
                    break;
                }
            }
            return result;
        }

        /// <summary>
        /// Получить энергию взаимодействия двух атомов
        /// </summary>
        public double InteractionEnergy(Atom fAtom, Atom sAtom)
        {
            double result;
            if (fAtom.ElementId == sAtom.ElementId)
            {
                result = InteractionEnergies.First(x => x.Elements.Count(el => el == fAtom.ElementId) == 2).Energy;
            }
            else
            {
                result = InteractionEnergies.First(x => x.Elements.Contains(fAtom.ElementId) && x.Elements.Contains(sAtom.ElementId)).Energy;
            }
            return result;
        }


        #region Инфраструктура
        public static Lattice[] GetLatticeFromJson(string json) => JsonConvert.DeserializeObject<DimerSettings>(json, GetJsonConvert()).Lattices;

        public static JsonSerializerSettings GetJsonConvert()
        {
            DefaultContractResolver contractResolver = new DefaultContractResolver
            {
                NamingStrategy = new CamelCaseNamingStrategy()
            };
            return new JsonSerializerSettings()
            {
                ContractResolver = contractResolver
            };
        }

        /// <summary>
        /// Формирование настроек диммера из файла конфигурации
        /// </summary>
        public static DimerSettings CreateDimerSettings(string json) => JsonConvert.DeserializeObject<DimerSettings>(json, GetJsonConvert());
        #endregion Инфраструктура
    }
}
