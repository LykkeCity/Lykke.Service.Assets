using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Lykke.Service.Assets.Models
{
    public class Error
    {
        public Dictionary<string, List<string>> ErrorMessages { get; } = new Dictionary<string, List<string>>();

        public static Error Create(ModelStateDictionary modelState)
        {
            var response = new Error();

            foreach (var state in modelState)
            {
                var messages = state.Value.Errors
                    .Where(e => !string.IsNullOrWhiteSpace(e.ErrorMessage))
                    .Select(e => e.ErrorMessage)
                    .Concat(state.Value.Errors
                        .Where(e => string.IsNullOrWhiteSpace(e.ErrorMessage))
                        .Select(e => e.Exception.Message))
                    .ToList();

                response.ErrorMessages.Add(state.Key, messages);
            }

            return response;
        }

        public static Error Create(string message)
        {
            var response = new Error();

            response.ErrorMessages.Add(string.Empty, new List<string> { message });

            return response;
        }

        public static Error Create(string field, string message)
        {
            var response = new Error();

            response.ErrorMessages.Add(field, new List<string> { message });

            return response;
        }
    }
}