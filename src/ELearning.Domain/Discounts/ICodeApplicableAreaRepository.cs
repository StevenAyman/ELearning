using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ELearning.Domain.Discounts;
public interface ICodeApplicableAreaRepository
{
    Task<CodeApplicableArea?> GetByIdAsync(string id, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<CodeApplicableArea>> GetAllAsync(CancellationToken cancellationToken = default);
    void Add(CodeApplicableArea codeApplicableArea);
    void Update(CodeApplicableArea codeApplicableArea);
    void Delete(CodeApplicableArea codeApplicableArea);
}
