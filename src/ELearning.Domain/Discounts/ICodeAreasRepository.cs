using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ELearning.Domain.Shared.Specifications;

namespace ELearning.Domain.Discounts;
public interface ICodeAreasRepository
{
    Task<CodeAreas?> GetWithSpecAsync(IBaseSpecifications<CodeAreas> spec,  CancellationToken cancellationToken = default);
    void Add(CodeAreas codeAreas);
    void Update(CodeAreas codeAreas);
    void Delete(CodeAreas codeAreas);   
}
