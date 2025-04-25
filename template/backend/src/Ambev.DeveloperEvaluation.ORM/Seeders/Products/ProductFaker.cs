using Ambev.DeveloperEvaluation.Domain.Entities;
using Bogus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ambev.DeveloperEvaluation.ORM.Seeders.Products
{
    public class ProductFaker : Faker<Product>
    {
        public ProductFaker()
        {
            RuleFor(p => p.Name, f => f.Commerce.ProductName());
            RuleFor(p => p.Price, f => f.Finance.Amount(10, 1000));
            RuleFor(p => p.Description, f => f.Lorem.Sentence(10));
            RuleFor(p => p.Rating, f => new Rating
            {
                Rate = f.Random.Double(1, 5),
                Count = f.Random.Int(0, 1000)
            });
        }
    }
}
