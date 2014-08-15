using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Dilemma.Data.Models;

namespace Dilemma.Data.EntityFramework.Initialization
{
    internal static class CategoryInitialization
    {
        internal static void Seed(DilemmaContext context)
        {
            var categories = new[] { "Relationship", "Teen", "Work", "Family", "Personal", "Sex" };

            foreach (var category in categories.Where(category => !context.Categories.Any(x => x.Name == category)))
            {
                context.Categories.Add(new Category { Name = category });
            }
        }
    }
}
