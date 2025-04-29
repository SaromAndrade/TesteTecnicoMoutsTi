using Ambev.DeveloperEvaluation.Domain.DTOs;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Domain.Services;
using AutoMapper;
using FluentValidation;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.CreateSale
{
    public class CreateSaleHandler : IRequestHandler<CreateSaleCommand, CreateSaleResult>
    {
        private readonly ISaleRepository _saleRepository;
        private readonly ISaleValidatorService _validator;
        private readonly ISaleFactoryService _factory;
        private readonly IMapper _mapper;

        public CreateSaleHandler(ISaleRepository saleRepository, ISaleValidatorService validator, ISaleFactoryService factory, IMapper mapper)
        {
            _saleRepository = saleRepository;
            _validator = validator;
            _factory = factory;
            _mapper = mapper;
        }

        public async Task<CreateSaleResult> Handle(CreateSaleCommand command, CancellationToken cancellationToken)
        {
            var validator = new CreateSaleCommandValidator();
            var validationResult = await validator.ValidateAsync(command, cancellationToken);

            if (!validationResult.IsValid)
                throw new ValidationException(validationResult.Errors);

            var creationDto = _mapper.Map<SaleDto>(command);
            await _validator.ValidateAsync(creationDto, cancellationToken);
            var sale = await _factory.CreateSaleAsync(creationDto, cancellationToken);

            var createdSale = await _saleRepository.CreateAsync(sale, cancellationToken);
            return _mapper.Map<CreateSaleResult>(createdSale);
        }
    }
}
