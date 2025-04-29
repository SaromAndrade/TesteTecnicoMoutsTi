using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Enums;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Ambev.DeveloperEvaluation.ORM.Repositories
{
    public class SaleRepository : ISaleRepository
    {
        private readonly DefaultContext _context;

        public SaleRepository(DefaultContext context)
        {
            _context = context;
        }

        public async Task<Sale> CreateAsync(Sale sale, CancellationToken cancellationToken)
        {
            await _context.Sales.AddAsync(sale, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
            return sale;
        }

        public async Task<(List<Sale> sales, int TotalItems)> GetAllAsync(int page, int size, string order, CancellationToken cancellationToken = default)
        {  
            page = page < 1 ? 1 : page;
            size = size < 1 ? 10 : size;

            var query = _context.Sales
                .Include(s => s.Customer)
                .Include(s => s.Branch)
                .Include(s => s.Items)
                .AsQueryable();
  
            query = order.ToLower() switch
            {
                "date" => query.OrderBy(s => s.Date),
                "date_desc" => query.OrderByDescending(s => s.Date),
                "number" => query.OrderBy(s => s.SaleNumber),
                "number_desc" => query.OrderByDescending(s => s.SaleNumber),
                "amount" => query.OrderBy(s => s.FinalAmount),
                "amount_desc" => query.OrderByDescending(s => s.FinalAmount),
                _ => query.OrderByDescending(s => s.Date) 
            };
            var totalItems = await query.CountAsync(cancellationToken);

            var sales = await query
                .Skip((page - 1) * size)
                .Take(size)
                .ToListAsync(cancellationToken);

            return (sales, totalItems);
        }

        public async Task<Sale?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            return await _context.Sales.FirstOrDefaultAsync(p => p.Id == id, cancellationToken);
        }

        public async Task<Sale> UpdateAsync(Guid id, Sale updatedSale, CancellationToken cancellationToken)
        {
            var existingSale = await _context.Sales
                .Include(s => s.Customer)
                .Include(s => s.Branch)
                .Include(s => s.Items)
                    .ThenInclude(i => i.Product)
                .FirstOrDefaultAsync(s => s.Id == id, cancellationToken);

            if (existingSale == null)
            {
                throw new KeyNotFoundException($"Sale with ID {id} not found");
            }

            if (existingSale.Status == SaleStatus.Cancelled && updatedSale.Status != SaleStatus.Cancelled)
            {
                throw new DomainException("Cannot modify a cancelled sale");
            }

            existingSale.SaleNumber = updatedSale.SaleNumber;
            existingSale.Date = updatedSale.Date;

            await UpdateSaleItems(existingSale, updatedSale.Items, cancellationToken);

            if (updatedSale.Status == SaleStatus.Cancelled && existingSale.Status != SaleStatus.Cancelled)
            {
                existingSale.Cancel();
            }

            await _context.SaveChangesAsync(cancellationToken);
            return existingSale;
        }
        private async Task UpdateSaleItems(Sale existingSale, List<SaleItem> updatedItems, CancellationToken cancellationToken)
        {
            var itemsToRemove = existingSale.Items
                .Where(existingItem => !updatedItems.Any(updatedItem => updatedItem.Id == existingItem.Id))
                .ToList();

            foreach (var item in itemsToRemove)
            {
                existingSale.Items.Remove(item);
                _context.SaleItems.Remove(item);
            }

            foreach (var updatedItem in updatedItems)
            {
                var existingItem = existingSale.Items.FirstOrDefault(i => i.Id == updatedItem.Id);

                if (existingItem != null)
                {
                    await UpdateExistingItem(existingItem, updatedItem, cancellationToken);
                }
                else
                {
                    await AddNewItem(existingSale, updatedItem, cancellationToken);
                }
            }

            existingSale.RecalculateTotals();
        }
        private async Task UpdateExistingItem(SaleItem existingItem, SaleItem updatedItem, CancellationToken cancellationToken)
        {
            if (existingItem.Product.Id != updatedItem.Product.Id)
            {
                existingItem.Product = await _context.Products.FindAsync(new object[] { updatedItem.Product.Id }, cancellationToken);
            }

            var quantityDifference = updatedItem.Quantity - existingItem.Quantity;
            if (quantityDifference != 0)
            {
                existingItem.IncreaseQuantity(quantityDifference);
            }
        }
        private async Task AddNewItem(Sale sale, SaleItem newItem, CancellationToken cancellationToken)
        {
            var product = await _context.Products.FindAsync(new object[] { newItem.Product.Id }, cancellationToken);
            sale.AddItem(product, newItem.Quantity);

            var addedItem = sale.Items.Last();
            if (newItem.Quantity != addedItem.Quantity)
            {
                addedItem.IncreaseQuantity(newItem.Quantity - addedItem.Quantity);
            }
        }

        public async Task<bool> CancelAsync(int SaleNumber, CancellationToken cancellationToken)
        {
            var sale = await _context.Sales.FirstOrDefaultAsync(p => p.SaleNumber == SaleNumber, cancellationToken);

            if (sale == null)
                throw new KeyNotFoundException($"Sale with SaleNumber {SaleNumber} not found.");

            if (sale.Status == SaleStatus.Cancelled)
                throw new InvalidOperationException("Sale is already cancelled.");

            sale.Cancel();

            // Atualiza apenas o campo IsCancelled no banco de dados
            _context.Sales.Update(sale);
            await _context.SaveChangesAsync(cancellationToken);
            return true;
        }
        public async Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var sale = await GetByIdAsync(id, cancellationToken);
            if (sale == null)
                return false;

            _context.Sales.Remove(sale);
            await _context.SaveChangesAsync(cancellationToken);
            return true;
        }
    }
}
