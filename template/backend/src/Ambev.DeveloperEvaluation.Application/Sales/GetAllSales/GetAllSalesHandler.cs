using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.GetAllSales
{
    public class GetAllSalesHandler : IRequestHandler<GetAllSalesQuery, GetAllSalesResult>
    {
        private readonly ISaleRepository _saleRepository;
        private readonly IMapper _mapper;

        public GetAllSalesHandler(ISaleRepository saleRepository, IMapper mapper)
        {
            _saleRepository = saleRepository;
            _mapper = mapper;
        }

        public async Task<GetAllSalesResult> Handle(GetAllSalesQuery request, CancellationToken cancellationToken)
        {
            var (paginatedSales, totalItems) = await _saleRepository.GetAllAsync(request.Page, request.Size, request.Order, cancellationToken);
            var paginatedSalesResult = _mapper.Map<List<Sale>>(paginatedSales);

            return new GetAllSalesResult { Sales = paginatedSalesResult, TotalItems = totalItems, };
        }
    }
}
