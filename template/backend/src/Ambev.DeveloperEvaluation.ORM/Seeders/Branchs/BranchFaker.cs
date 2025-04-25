using Ambev.DeveloperEvaluation.Domain.Entities;
using Bogus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ambev.DeveloperEvaluation.ORM.Seeders.Branchs
{
    public class BranchFaker : Faker<Branch>
    {
        public BranchFaker()
        {
            RuleFor(b => b.Name, f => $"{f.Company.CompanyName()} {f.Commerce.Ean8()}");
            RuleFor(b => b.Location, f => $"{f.Address.City()}, {f.Address.StateAbbr()}, {f.Address.Country()}");
        }
    }
}
