using Ambev.DeveloperEvaluation.Application.Products.GetAllProduct;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ambev.DeveloperEvaluation.Application.Branchs.GetAllBranchs
{
    public class GetAllBranchsHandler : IRequestHandler<GetAllBranchsQuery, GetAllBranchsResult>
    {
        private readonly IBranchRepository _branchRepository;
        private readonly IMapper _mapper;

        public GetAllBranchsHandler(IBranchRepository branchRepository, IMapper mapper)
        {
            _branchRepository = branchRepository;
            _mapper = mapper;
        }

        public async Task<GetAllBranchsResult> Handle(GetAllBranchsQuery request, CancellationToken cancellationToken)
        {
            var result = await _branchRepository.GetAllAsync(cancellationToken);

            return new GetAllBranchsResult { Data = result };
        }
    }
}
