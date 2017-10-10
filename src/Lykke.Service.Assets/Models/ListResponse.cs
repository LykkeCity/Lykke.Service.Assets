using System;
using System.Collections.Generic;
using System.Text;

namespace Lykke.Service.Assets.Models
{
    public class ListResponse<T>
    {
        public IEnumerable<T> List;
    }
}
