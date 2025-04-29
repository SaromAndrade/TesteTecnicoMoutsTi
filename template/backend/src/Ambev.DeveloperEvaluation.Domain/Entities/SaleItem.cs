using Ambev.DeveloperEvaluation.Domain.Common;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ambev.DeveloperEvaluation.Domain.Entities
{
    public class SaleItem : BaseEntity
    {
        public SaleItem()
        {
        }

        public SaleItem(Product product, int quantity)
        {
            if (quantity > 20)
                throw new DomainException("Quantity cannot exceed 20 items for the same product");

            Product = product;
            Quantity = quantity;
            CalculateDiscount();
        }

        public Product Product { get; set; }
        public int Quantity { get; private set; }
        public decimal TotalPrice => Quantity * Product.Price;
        public decimal DiscountAmount { get; private set; }
        public decimal FinalPrice => TotalPrice - DiscountAmount;

        public void IncreaseQuantity(int additionalQuantity)
        {
            if (additionalQuantity <= 0)
                throw new DomainException("Additional quantity must be positive");

            var newQuantity = Quantity + additionalQuantity;
            if (newQuantity > 20)
                throw new DomainException("Cannot have more than 20 items of the same product");

            Quantity = newQuantity;
            CalculateDiscount();
        }

        private void CalculateDiscount()
        {
            if (Quantity < 4)
            {
                DiscountAmount = 0;
                return;
            }

            var discountPercentage = Quantity switch
            {
                >= 10 and <= 20 => 0.2m,
                >= 4 => 0.1m,
                _ => 0m
            };

            DiscountAmount = TotalPrice * discountPercentage;
        }
    }
}
