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
        #endregion Инфраструктура
    }
}
