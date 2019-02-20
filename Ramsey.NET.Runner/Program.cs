using Microsoft.EntityFrameworkCore;
using Ramsey.Core;
using Ramsey.NET.Auto.Implementations;
using Ramsey.NET.Implementations;
using Ramsey.NET.Interfaces;
using Ramsey.NET.Shared.Interfaces;
using Ramsey.Shared.Enums;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Ramsey.NET.Runner
{
    class Program
    {
        private static RamseyContext _ramseyContext;
        private static IRecipeManager _recipeManager;
        private static IWordRemover _wordRemover;
        private static ICrawlerService _crawlerService;

        static async Task Main(string[] args)
        {
            DbContextOptionsBuilder<RamseyContext> dbContextOptions;

            if (Environment.GetEnvironmentVariable("ENVIRONMENT") == "Production")
            {
                dbContextOptions = new DbContextOptionsBuilder<RamseyContext>()
                    .UseSqlServer("Server=tcp:159.89.5.112,1433;Database=feedmedb;User ID=jarvis;Password=@JagGillarJulmust;Connection Timeout=30;");
            }
            else
            {
                dbContextOptions = new DbContextOptionsBuilder<RamseyContext>()
                    .UseInMemoryDatabase("feedme");
            }

            _ramseyContext = new RamseyContext(dbContextOptions.Options);
            _recipeManager = new SqlRecipeManager(_ramseyContext);
            _wordRemover = new BasicWordRemover(_ramseyContext);
            _crawlerService = new CrawlerService(_ramseyContext, _recipeManager, _wordRemover);

            Enum.TryParse(args.FirstOrDefault(), out RecipeProvider recipeProvider);

            await _crawlerService.ReindexProviderAsync(recipeProvider);
        }
    }
}
