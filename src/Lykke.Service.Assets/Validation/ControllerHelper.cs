using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Mvc;

namespace Lykke.Service.Assets.Controllers.V2
{
    public static class ControllerHelper
    {
        public static bool ValidateKey(this Controller controller, string keyParam)
        {
            const string pattern = "^[A-Za-z][A-Za-z0-9\\s]{2,62}$";
            return Regex.IsMatch(keyParam, pattern);
        }
    }
}
