using HospitalData.DTOs;
using HospitalData.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace HospitalData.Services
{
    public class AuthService : IAuthService
    {
        private readonly HospitalDbContext _context;
        public AuthService(HospitalDbContext context)
        {
            _context = context;
        }

        public async Task<AuthenticatedUser?> LoginAsync(string username, string password)
        {
            var result = await _context.AuthenticatedUsers
                .FromSqlInterpolated($"EXEC SP_UserLogin @Username={username}, @Password={password}")
                .ToListAsync();

            return result.FirstOrDefault();
        }
    }
}