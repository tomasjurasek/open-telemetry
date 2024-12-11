using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eshop.Contracts.Logistics
{
    public record OrderDeliveredEvent(Guid OrderId);
}
