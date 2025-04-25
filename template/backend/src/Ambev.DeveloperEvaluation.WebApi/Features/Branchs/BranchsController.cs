using Ambev.DeveloperEvaluation.Application.Branchs.GetAllBranchs;
using Ambev.DeveloperEvaluation.Application.Products.GetAllProduct;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.WebApi.Common;
using Ambev.DeveloperEvaluation.WebApi.Features.Branchs.GetAllBranchs;
using Ambev.DeveloperEvaluation.WebApi.Features.Products.GetAllProduct;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Branchs
{
    [Route("api/[controller]")]
    [ApiController]
    public class BranchsController : BaseController
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public BranchsController(IMediator mediator, IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(typeof(ApiResponseWithData<GetAllBranchsResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetAllBranchs(CancellationToken cancellationToken)
        {
            var query = new GetAllBranchsQuery();
            var result = await _mediator.Send(query, cancellationToken);
            var response = _mapper.Map<GetAllBranchsResponse>(result);

            var pagedList = new PaginatedList<Branch>(response.Data, response.Data.Count(), 1, 10);

            return OkPaginated<Branch>(pagedList);
        }
    }
}
