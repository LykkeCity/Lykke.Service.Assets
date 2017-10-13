using System.Collections.Generic;

namespace Lykke.Service.Assets.Responses.V2
{
    public class ListOf<T>
    {
        public IEnumerable<T> Items { get; set; }
    }
}
