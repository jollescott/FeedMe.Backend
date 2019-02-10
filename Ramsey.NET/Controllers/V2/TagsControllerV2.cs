using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Ramsey.NET.Controllers.Interfaces.V2;
using Ramsey.NET.Interfaces;
using Ramsey.Shared.Dto.V2;
using Ramsey.Shared.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ramsey.NET.Controllers.V2
{
    [Route("v2/tags")]
    public class TagsControllerV2 : Controller, ITagsControllerV2
    {
        private IRamseyContext _ramseyContext;

        public TagsControllerV2(IRamseyContext ramseyContext)
        {
            _ramseyContext = ramseyContext;
        }

        [Route("suggest")]
        [HttpPost]
        public IActionResult Suggest(RamseyLocale locale = RamseyLocale.Swedish)
        {
            var tags = _ramseyContext.Tags
                .AsNoTracking()
                .Where(x => x.Locale == locale)
                .Include(x => x.RecipeTags)
                .ThenInclude(x => x.Recipe)
                .OrderBy(x => Guid.NewGuid())
                .Take(10);

            List<TagDto> dtos = new List<TagDto>();

            foreach(var tag in tags)
            {
                var headerRecipe = tag.RecipeTags.OrderBy(x => Guid.NewGuid()).FirstOrDefault()?.Recipe;

                if(headerRecipe != null)
                {
                    dtos.Add(new TagDto
                    {
                        Name = tag.Name,
                        PreviewImage = headerRecipe.Image,
                        TagId = tag.TagId
                    });
                }
            }

            return Json(dtos);
        }

        public IActionResult Suggest(int tagid, int start = 0)
        {
            var recipes = _ramseyContext.RecipeTags
                .Where(x => x.TagId == tagid)
                .Include(x => x.Recipe)
                .Select(x => x.Recipe)
                .OrderBy(x => x.Name)
                .Skip(start)
                .Take(25);

            var dtos = new List<RecipeMetaDtoV2>();

            foreach(var recipe in recipes)
            {
                var dto = new RecipeMetaDtoV2();
                dto.Image = recipe.Image;
                dto.Name = recipe.Name;
                dto.Source = recipe.Source;
                dto.OwnerLogo = recipe.OwnerLogo;
                dto.Owner = recipe.Owner;

                dtos.Add(dto);
            }

            return Json(dtos);
        }
    }
}
