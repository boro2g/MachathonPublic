using System.Collections.Generic;

namespace HomeAssistantToContenful.Models.GraphQl
{
    public abstract class ItemsGraphQl<T>
    {
        public IEnumerable<T> Items { get; set; }
    }
}



