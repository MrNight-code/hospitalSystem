using HospitalData.DTOs;

namespace HospitalData.Services
{
    public interface IAuthService
    {
        Task<AuthenticatedUser?> LoginAsync(string username, string password);
    }
}