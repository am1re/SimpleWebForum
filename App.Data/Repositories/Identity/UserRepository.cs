using App.Data.Contexts;
using App.Data.Entities.Identity;
using App.Data.Interfaces.Repositories;
using App.Data.Repositories.Base;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace App.Data.Repositories.Identity
{
    public class UserRepository : BaseRepository<User>, IUserRepository
    {
        private readonly DbContext _context;

        public UserRepository(ForumDbContext context) : base(context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public ValueTask<User> GetByIdAsync(string id)
        {
            return _context.Set<User>().FindAsync(id);
        }
    }
}
