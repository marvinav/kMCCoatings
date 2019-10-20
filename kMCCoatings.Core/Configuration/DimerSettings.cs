using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;
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

        public DimerSettings()
        {

        }


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
    }
}
