using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DAL.Interfaces;
using DAL.Model;
using DAL.UserContext;
using Microsoft.AspNetCore.Identity;

namespace DAL.Repositories
{
    public sealed class UserRepository : IRepository<User>, IDisposable
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<User> _userManager;

        public UserRepository(ApplicationDbContext context, UserManager<User> userManager)
        {
            _db = context;
            _userManager = userManager;
        }

        public IEnumerable<User> GetAllUsers()
        {
            return _userManager.Users;
        }

        public void Dispose()
        {
            _db.Dispose();
        }
    }
}
