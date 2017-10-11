using System.Collections.Generic;

namespace Lykke.Service.Assets.Models
{
    public class ListOf<T>
    {
        public IEnumerable<T> Items { get; set; }
    }
}
