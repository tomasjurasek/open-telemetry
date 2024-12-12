using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eshop.Contracts.Payment
{
    public record OrderPaidEvent(Guid OrderId);
}
