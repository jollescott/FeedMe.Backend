using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using Ramsey.NET.Implementations;
using Ramsey.NET.Interfaces;
using System.Text;
using System.Threading.Tasks;
using Ramsey.NET.Extensions;
using Ramsey.NET.Shared.Interfaces;

namespace Ramsey.NET.Tests.Misc
{
    [TestFixture]
    public class RecipeManagerTests
    {
        private ICrawlerService _crawlerService;

        private IRecipeManager _recipeManager;
        private RamseyContext _context;

        [SetUp]
        public void SetUp()
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            
            var options = new DbContextOptionsBuilder<RamseyContext>()
                .ConnectRamseyTestServer(null, true)
                .Options;

            _context = new RamseyContext(options);
            _context.Database.EnsureCreated();
            
            _recipeManager = new SqlRecipeManager(_context);
            
            _crawlerService = new CrawlerService(_context, _recipeManager);
        }

        [Test]
        public async Task CrawlerServiceUpdateAsync()
        {
            await _crawlerService.UpdateIndexAsync();
        }
    }
}
