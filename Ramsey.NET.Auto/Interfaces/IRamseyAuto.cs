using Ramsey.NET.Crawlers.Interfaces;
using Ramsey.Shared.Dto.V2;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Ramsey.NET.Auto.Interfaces
{
    public interface IRamseyAuto : IRecipeCrawler
    {
        void Init(IAutoConfig config);
    }
}
