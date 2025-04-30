using Ambev.DeveloperEvaluation.Domain.Entities;

namespace Ambev.DeveloperEvaluation.Unit.Domain.Entities.TestData
{
    public static class SaleItemTestData
    {
        public static SaleItem GenerateValidSaleItem(int quantity = 1)
        {
            var product = ProductTestData.GenerateValidProduct();
            return new SaleItem(product, quantity);
        }
        public static SaleItem GenerateSaleItemWithMaxDiscount() 
        {
            var product = ProductTestData.GenerateValidProduct();
            return new SaleItem(product, 20);
        }
        public static SaleItem GenerateSaleItemWithMediumDiscount()
        {
            var product = ProductTestData.GenerateValidProduct();
            return new SaleItem(product, 5);
        }
        public static SaleItem GenerateSaleItemWithoutDiscount()
        {
            var product = ProductTestData.GenerateValidProduct();
            return new SaleItem(product, 1);
        }

        public static SaleItem GenerateSaleItemAtLimitQuantity()
        {
            var product = ProductTestData.GenerateValidProduct();
            return new SaleItem(product, 20);
        }

        public static SaleItem GenerateSaleItemBelowLimitQuantity()
        {
            var product = ProductTestData.GenerateValidProduct();
            return new SaleItem(product, 19);
        }

        public static Product GenerateProductWithPrice(decimal price)
        {
            var product = ProductTestData.GenerateValidProduct();
            product.Price = price;
            return product;
        }
    }
}
