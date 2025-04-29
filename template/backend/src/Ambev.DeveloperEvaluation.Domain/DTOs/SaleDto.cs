using Ambev.DeveloperEvaluation.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ambev.DeveloperEvaluation.Domain.DTOs
{
    public class SaleDto
    {
        public Guid CustomerId { get; set; }
        public string CustomerEmail { get; set; }
        public Guid BranchId { get; set; }
        public List<SaleItemDto> Items { get; set; } = new();
    }
}
