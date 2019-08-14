using System.Threading.Tasks;
using Shared.Models;

namespace Shared
{
    public interface IStorage
    {
        Task Save(Order order);
    }
}
