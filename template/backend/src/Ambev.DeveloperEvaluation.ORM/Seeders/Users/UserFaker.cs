using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Enums;
using Bogus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ambev.DeveloperEvaluation.ORM.Seeders.Users
{
    public class UserFaker : Faker<User>
    {
        public UserFaker()
        {
            RuleFor(u => u.Username, f => f.Internet.UserName(f.Person.FirstName, f.Person.LastName));
            RuleFor(u => u.Email, (f, u) => f.Internet.Email(u.Username));
            RuleFor(u => u.Phone, f => GenerateBrazilianPhoneNumber(f));
            RuleFor(u => u.Password, f => BCrypt.Net.BCrypt.HashPassword("Senha@123"));
            RuleFor(u => u.Role, f => f.PickRandom<UserRole>());
            RuleFor(u => u.Status, f => f.PickRandom<UserStatus>());
            RuleFor(u => u.CreatedAt, f => f.Date.Past(2).ToUniversalTime());
            RuleFor(u => u.UpdatedAt, f => f.Date.Recent(30).OrNull(f, 0.3f)?.ToUniversalTime());
        }
        private string GenerateBrazilianPhoneNumber(Faker f)
        {
            // Gera um número no formato (XX) XXXXX-XXXX
            var ddd = f.Random.Int(11, 99).ToString("00");
            var firstPart = f.Random.Int(1000, 9999).ToString("0000");
            var secondPart = f.Random.Int(1000, 9999).ToString("0000");
            return $"({ddd}) {firstPart}-{secondPart}";
        }
    }
}
