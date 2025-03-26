using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Application.Abstractions.Infrastructure
{
    public interface ICurrentUserService
    {
        public Guid UserGuid { get; }
        public string FirstName { get; }
        public string LastName { get; }
    }
}
