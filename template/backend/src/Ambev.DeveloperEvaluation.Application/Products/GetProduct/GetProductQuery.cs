using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ambev.DeveloperEvaluation.Application.Products.GetProduct
{
    public class GetProductQuery : IRequest<GetProductResult>
    {
        public Guid Id { get; set; }
    }
}
