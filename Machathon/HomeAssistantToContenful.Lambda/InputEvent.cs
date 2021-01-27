using System.Collections.Generic;


namespace HomeAssistantToContenful.Lambda
{
    public class InputEvent
    {
        public string Message { get; set; }
        public DataEvent Data { get; set; }
    }

    public class DataEvent
    {
        public Dictionary<string, double> Temperatures { get; set; }
        public string Action { get; set; }
        public string Username { get; set; }
        public string Usermood { get; set; }
        public string UsermoodOutcome { get; set; }
    }
}


