﻿using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public interface IJwtTokenService
    {
        string GenerateToken(User user);
        bool ValidateToken(string token);
    }
}
