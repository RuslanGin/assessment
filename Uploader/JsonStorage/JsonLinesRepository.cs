using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Shared;
using Shared.Models;

namespace JsonStorage
{
    public class JsonLinesRepository: IStorage
    {
        private readonly object _locker = new Object();
        private readonly string _path;

        public JsonLinesRepository(IConfiguration configuration)
        {
            _path = $"{AppDomain.CurrentDomain.BaseDirectory}/{configuration["FileStoragePath"]}";
        }

        public async Task Save(Order order)
        {
            await Task.Run(() =>
            {
                lock (_locker)
                {
                    using (var writer = new StreamWriter(_path, true, Encoding.UTF8, 1024))
                    {
                        writer.WriteLine(JsonConvert.SerializeObject(order));
                    }
                }
            });
        }
    }
}