using Ambev.DeveloperEvaluation.ORM.Seeders.Products;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ambev.DeveloperEvaluation.ORM.Seeders
{
    public class DatabaseSeeder
    {
        public static async Task SeedAsync(DefaultContext context)
        {
            context.Database.AutoTransactionBehavior = AutoTransactionBehavior.Never;

            if (!context.Products.Any())
            {
                var faker = new ProductFaker();
                var products = faker.Generate(10);

                await context.Products.AddRangeAsync(products);
            }
            await context.SaveChangesAsync();
        }
    }
}
