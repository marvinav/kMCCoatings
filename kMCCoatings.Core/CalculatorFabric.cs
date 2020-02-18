using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using kMCCoatings.Core.Configuration;
using Newtonsoft;
using Newtonsoft.Json;

namespace kMCCoatings.Core
{
    /// <summary>
    /// Служба для создания калькулятора из настроек json
    /// </summary>
    public static class CalculatorFabric
    {
        public static Calculator CreateCalculator(string settingsDir)
        {
            var settings = Newtonsoft.Json.JsonConvert.DeserializeObject<Settings>(File.ReadAllText(settingsDir));
            ValidateConcentrationFlow(settings);
            return new Calculator(settings);
        }

        public static bool ValidateConcentrationFlow(Settings settings)
        {
            var elementIds = settings.Dimer.Lattices.Select(x => x.).ToList();

            foreach (var id in elementIds)
            {
                for (int step = 0; step < settings.De.ConcentrationFlow.Length; step++)
                {
                    var elementConcentration = settings.De.ConcentrationFlow[step];
                    if (Array.Find(elementConcentration, x => x.ElementId == id) == null)
                    {
                        throw new Exception($"У элемента {id} не задана концентрация на участке {step}");
                    }
                }
            }
            return true;
        }
    }
}
