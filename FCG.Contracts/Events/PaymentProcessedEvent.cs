using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FCG.Contracts.Events
{
    public record PaymentProcessedEvent
    (
        int UserId,
        int GameId,
        string Status
    );
}
