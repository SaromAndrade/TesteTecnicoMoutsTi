using Ambev.DeveloperEvaluation.WebApi.Common;
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
    }
}
