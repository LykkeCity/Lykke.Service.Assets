using System.Threading.Tasks;

namespace TokenExporter.Commands
{
    public interface ICommand
    {
        Task<int> ExecuteAsync();
    }
}
