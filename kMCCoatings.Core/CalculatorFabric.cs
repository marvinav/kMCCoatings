using System;
using System.Collections.Generic;
using System.IO;
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
            var settings = File.ReadAllText(settingsDir);
            var a = Newtonsoft.Json.JsonConvert.DeserializeObject<Settings>(settings);
            return new Calculator();
        }
    }
}
