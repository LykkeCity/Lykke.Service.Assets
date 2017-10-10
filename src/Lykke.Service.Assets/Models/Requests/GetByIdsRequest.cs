using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lykke.Service.Assets.Models.Requests
{
    public class GetByIdsRequest
    {
        public IEnumerable<string> Ids { get; set; }
    }
}
