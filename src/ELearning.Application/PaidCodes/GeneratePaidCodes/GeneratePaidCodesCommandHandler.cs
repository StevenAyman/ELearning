using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ELearning.Application.Abstractions.Clock;
using ELearning.Application.Abstractions.Messaging;
using ELearning.Application.PaidCodes.DTOs;
using ELearning.Domain.Purchases;
using ELearning.Domain.Shared;

namespace ELearning.Application.PaidCodes.GeneratePaidCodes;
internal sealed class GeneratePaidCodesCommandHandler(
    IPaidCodeRepository paidCodeRepository,
    IUnitOfWork unitOfWork,
    CodeGenerationDomainService codeGenerationDomainService,
    IDateTimeProvider dateTimeProvider) : ICommandHandler<GeneratePaidCodesCommand, IEnumerable<PaidCodeResponse>>
{
    private readonly IPaidCodeRepository _paidCodeRepository = paidCodeRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly CodeGenerationDomainService _codeGenerationDomainService = codeGenerationDomainService;
    private readonly IDateTimeProvider _dateTimeProvider = dateTimeProvider;

    public async Task<Result<IEnumerable<PaidCodeResponse>>> Handle(GeneratePaidCodesCommand request, CancellationToken cancellationToken)
    {
        List<PaidCodeResponse> codes = new();
        for (int i = 1; i <= request.Count; i++)
        {
            var generatedCode = _codeGenerationDomainService.GenerateCode();
            var paidCode = new PaidCode(
                $"pc_{Guid.CreateVersion7()}",
                generatedCode,
                new Money(request.Balance),
                _dateTimeProvider.UtcNow);

            _paidCodeRepository.Add(paidCode);

            var dto = new PaidCodeResponse
            {
                Id = paidCode.Id,
                Code = generatedCode,
                Status = "Available",
                Amount = request.Balance
            };
            codes.Add(dto);
        }

        var result = await _unitOfWork.SaveChangesAsync(cancellationToken);

        if (result <= 0)
        {
            return Errors.DatabaseError;
        }

        return codes;
    }
}
