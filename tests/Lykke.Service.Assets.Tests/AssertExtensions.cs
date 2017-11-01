using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Lykke.Service.Assets.Tests
{
    public static class AssertExtensions
    {
        public static void IsActionResultOfType<T>(this Assert assert, IActionResult value, out T actionResult)
            where T : IActionResult
        {
            if (value is T t)
            {
                actionResult = t;
            }
            else
            {
                throw new AssertFailedException("Action result type does not match.");
            }
        }

        public static void IsInstanceOfType<T>(this Assert assert, object value, out T result)
        {
            if (value is T t)
            {
                result = t;
            }
            else
            {
                throw new AssertFailedException("Value type does not match.");
            }
        }
    }
}