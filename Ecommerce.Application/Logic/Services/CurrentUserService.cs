using Ecommerce.Application.Abstractions.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Application.Logic.Services
{
    public class CurrentUserService : ICurrentUserService
    {
        public string UserId => "TestUser";
    }
}
