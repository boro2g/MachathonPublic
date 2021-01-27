using System.Collections.Generic;

namespace HomeAssistantToContenful
{
    public class TemperatureEntry
    {
        public string Temperature { get; set; }
        public Dictionary<string, double> TemperatureBreakdown { get; set; }
    }
}



