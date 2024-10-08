using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class BalanceCommand : IRequest<BalanceResponse>
    {
        public string Token { get; set; }
    }
    public class BalanceResponse
    {
        public decimal Balance { get; set; }
    }
}
