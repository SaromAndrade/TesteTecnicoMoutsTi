using Ambev.DeveloperEvaluation.Domain.Entities;
using Bogus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ambev.DeveloperEvaluation.Unit.Domain.Entities.TestData
{
    public static class BranchTestData
    {
        private static readonly Faker<Branch> BranchFaker = new Faker<Branch>()
           .CustomInstantiator(f => new Branch())
           .RuleFor(b => b.Name, f => $"{f.Company.CompanyName()} {f.Commerce.Ean8()}")
           .RuleFor(b => b.Location, f => $"{f.Address.City()}, {f.Address.StateAbbr()}, {f.Address.Country()}");

        /// <summary>
        /// Generates a valid Branch instance with realistic values.
        /// </summary>
        public static Branch GenerateValidBranch()
        {
            return BranchFaker.Generate();
        }
    }
}
