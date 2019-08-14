using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessLayer.Models;
using Shared;

namespace BusinessLayer
{
    public class StreamManager : IStreamManager
    {
        private readonly IEnumerable<IStorage> _storages;

        public StreamManager(IEnumerable<IStorage> storages)
        {
            _storages = storages;
        }

        public async Task ProcessStream(Stream stream)
        {
            using (var streamReader = new StreamReader(stream, Encoding.UTF8, true, 1024, true))
            {
                var headers = await streamReader.ReadLineAsync(); // skipping headers
                while (streamReader.Peek() >= 0)
                {
                    var values = await streamReader.ReadLineAsync();
                    var order = new OrderBl(values.Split(','));

                    await Task.WhenAll(_storages.Select(storage => storage.Save(order.ToEntity())).ToArray());
                }
            }
        }
    }
}
