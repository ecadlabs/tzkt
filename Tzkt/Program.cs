using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Tzkt.Api;
using Tzkt.Sync;

namespace Tzkt
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Host.CreateDefaultBuilder(args)
                .ConfigureIndexer()
                .ConfigureApi()
                .Build()
                .Init()
                .Run();
        }
    }
}
