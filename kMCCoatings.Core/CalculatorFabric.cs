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
            try
            {
                foreach (var flows in settings.Deposition.ConcentrationFlow)
                {
                    foreach (var flow in flows)
                    {
                        flow.Element = settings.Elements.First(x => x.Id == flow.ElementId);
                    }
                }
            }
            catch (Exception ex)
            {
                if (ex is NullReferenceException)
                {
                    throw new Exception($"Обнаружен не объявленный элемент");
                }
            }

            ConcentrationFlow(settings);
            return new Calculator(settings);
        }

        /// <summary>
        /// Проверяет целостность концентрационного потока
        /// </summary>
        public static bool ConcentrationFlow(Settings settings)
        {
            var flow = settings.Deposition.ConcentrationFlow;
            for (int i = 1; i < flow.Length; i++)
            {
                var prevDensity = flow[i - 1].Sum(x => x.Concentration);
                var curDensity = flow[i].Sum(x => x.Concentration);
                curDensity = prevDensity > curDensity ? prevDensity : curDensity;
                if (curDensity > settings.Calc.Dimension.X * settings.Calc.Dimension.Y)
                {
                    throw new Exception($"На участке {i} плотность потока превышает площадь области расчёта.");
                }
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
