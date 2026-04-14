using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ELearning.Infastructure.Outbox;
public interface IProcessOutboxMessageJob
{
    Task ExecuteAsync();
}
