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

        /// <summary>
        /// Проверяет целостность концентрационного потока
        /// </summary>
        public static bool ValidateConcentrationFlow(Settings settings)
        {
            var flow = settings.Deposition.ConcentrationFlow;
            for (int i = 1; i < flow.Length; i++)
            {
                var prev = flow[i - 1].OrderBy(x => x.ElementId).Select(x => x.ElementId);
                var current = flow[i].OrderBy(x => x.ElementId).Select(x => x.ElementId);
                if (!prev.Intersect(current).Any())
                {
                    throw new Exception($"На участке {i} нарушена концентрационная зависимость.");
                }
            }
            return true;
        }
    }
}
