using Microsoft.EntityFrameworkCore;
using Ramsey.NET.Implementations;
using System;

namespace Ramsey.NET.Runner
{
    class Program
    {
        private static RamseyContext _ramseyContext;

        static void Main(string[] args)
        {
            DbContextOptionsBuilder<RamseyContext> dbContextOptions;

            if (Environment.GetEnvironmentVariable("ENVIRONMENT") == "Production")
            {
                dbContextOptions = new DbContextOptionsBuilder<RamseyContext>()
                    .UseInMemoryDatabase()
            }
            else
            {

            }

            _ramseyContext = new RamseyContext(dbContextOptions);
        }
    }
}
