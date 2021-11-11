﻿using System.Threading.Tasks;
using DAL.Model;

namespace Business.Interfaces
{
    public interface IJwtGenerator
    {
        Task<string> GenerateTokenAsync(User user);
    }
}