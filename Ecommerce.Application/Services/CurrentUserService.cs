﻿using Ecommerce.Application.Abstractions.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Application.Services
{
    public class CurrentUserService : ICurrentUserService
    {
        public Guid UserGuid => Guid.Parse("564660d1-7970-4895-91a3-b81bb95a8d03");
        public string FirstName => "Bleron";
        public string LastName => "Qorri";
    }
}
