using System;
using System.Text;

namespace RegisterErc20Tokens
{
    public static class ExceptionExtensions
    {
        public static string ToOperationResult(this Exception e)
        {
            var sb = new StringBuilder();

            sb.AppendLine("exception caught");
            sb.AppendLine("-----");
            sb.AppendLine();
            sb.AppendLine(e.ToString());
            sb.AppendLine();
            sb.AppendLine("-----");

            return sb.ToString();
        }
    }
}