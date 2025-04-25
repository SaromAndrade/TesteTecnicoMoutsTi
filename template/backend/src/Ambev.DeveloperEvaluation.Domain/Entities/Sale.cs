using Ambev.DeveloperEvaluation.Domain.Common;
using Ambev.DeveloperEvaluation.Domain.Enums;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ambev.DeveloperEvaluation.Domain.Entities
{
    public class Sale : BaseEntity
    {
        public Sale()
        {
        }

        public Sale(int saleNumber, User customer, Branch branch)
        {
            SaleNumber = saleNumber;
            Customer = customer;
            Branch = branch;
        }

        public int SaleNumber { get; set; }
        public DateTime Date { get; set; }
        public User Customer { get; set; }
        public Branch Branch { get; private set; }
        private readonly List<SaleItem> _items = new();
        public IReadOnlyCollection<SaleItem> Items => _items.AsReadOnly();
        public SaleStatus Status { get; private set; }
        public decimal TotalAmount { get; private set; }
        public decimal TotalDiscount { get; private set; }
        public decimal FinalAmount { get; private set; }
        public void AddItem(Product product, int quantity, decimal unitPrice)
        {
            if (Status != SaleStatus.Created)
                throw new DomainException("Cannot add items to a completed or cancelled sale");

            var item = new SaleItem(product, quantity, unitPrice);
            _items.Add(item);
            RecalculateTotals();
        }
        private void RecalculateTotals()
        {
            TotalAmount = _items.Sum(i => i.TotalPrice);
            TotalDiscount = _items.Sum(i => i.DiscountAmount);
            FinalAmount = TotalAmount - TotalDiscount;
        }
        public void Cancel()
        {
            if (Status == SaleStatus.Cancelled)
                throw new DomainException("Sale is already cancelled");

            Status = SaleStatus.Cancelled;
            // DomainEvents.Add(new SaleCancelledEvent(this));
        }
    }
}
