using Ambev.DeveloperEvaluation.Domain.DTOs;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Domain.Services;
using AutoMapper;
using FluentValidation;
using MediatR;
using System.Net.Http.Headers;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Ambev.DeveloperEvaluation.Application.Sales.UpdateSale
{
    public class UpdateSaleHandler : IRequestHandler<UpdateSaleCommand, UpdateSaleResult>
    {
        private readonly ISaleRepository _saleRepository;
        private readonly ISaleValidatorService _validator;
        private readonly ISaleFactoryService _factory;
        private readonly IMapper _mapper;

        public UpdateSaleHandler(ISaleRepository saleRepository, ISaleValidatorService validator, ISaleFactoryService factory, IMapper mapper)
        {
            _saleRepository = saleRepository;
            _validator = validator;
            _factory = factory;
            _mapper = mapper;
        }

        public async Task<UpdateSaleResult> Handle(UpdateSaleCommand command, CancellationToken cancellationToken)
        {
            var validator = new UpdateSaleCommandValidator();
            var validationResult = await validator.ValidateAsync(command, cancellationToken);

            if (!validationResult.IsValid)
                throw new ValidationException(validationResult.Errors);

            var updateSaleDto = _mapper.Map<SaleDto>(command);
            await _validator.ValidateAsync(updateSaleDto, cancellationToken);
            var sale = await _factory.CreateSaleAsync(updateSaleDto, cancellationToken);
            var updateSale = await _saleRepository.UpdateAsync(command.Id, sale, cancellationToken);
            return _mapper.Map<UpdateSaleResult>(updateSale);
        }
    }
}
