using Ambev.DeveloperEvaluation.ORM.Seeders.Branchs;
using Ambev.DeveloperEvaluation.ORM.Seeders.Products;
using Ambev.DeveloperEvaluation.ORM.Seeders.Users;
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
            if (!context.Branchs.Any())
            {
                var faker = new BranchFaker();
                var branchs = faker.Generate(5);
                await context.Branchs.AddRangeAsync(branchs);
            }
            if (!context.Users.Any())
            {
                var faker = new UserFaker();
                var users = faker.Generate(2);
                var userSpecific = SpecificCustomerCreator.CreateSpecificCustomer();
                users.Add(userSpecific);
                await context.Users.AddRangeAsync(users);
            }
            await context.SaveChangesAsync();
        }
    }
}
