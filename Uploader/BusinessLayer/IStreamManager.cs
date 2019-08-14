using System.IO;
using System.Threading.Tasks;

namespace BusinessLayer
{
    public interface IStreamManager
    {
        Task ProcessStream(Stream stream);
    }
}