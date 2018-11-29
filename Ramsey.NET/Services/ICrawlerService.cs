using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ramsey.NET.Services
{
    public interface ICrawlerService
    {
        Task UpdateIndexAsync();
    }
}
