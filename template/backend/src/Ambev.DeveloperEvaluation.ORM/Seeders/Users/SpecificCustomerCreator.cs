using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ambev.DeveloperEvaluation.ORM.Seeders.Users
{
    public static class SpecificCustomerCreator
    {
        public static User CreateSpecificCustomer()
        {
            return new User
            {
                Id = Guid.NewGuid(),
                Username = "customer.user",
                Email = "customer.user@example.com",
                Phone = "(11) 98765-4321",
                Password = BCrypt.Net.BCrypt.HashPassword("Customer@123"),
                Role = UserRole.Customer,
                Status = UserStatus.Active,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = null
            };
        }
    }
}
