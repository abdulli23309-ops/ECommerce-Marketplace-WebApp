using ECommerce.Application.Interfaces;

namespace ECommerce.Infrastructure.Services
{
    public class BCryptPasswordHasherService : IPasswordHasherService
    {
        private const int WorkFactor = 12; // can be increased in the future

        public string Hash(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password, workFactor: WorkFactor);
        }

        public bool Verify(string password, string passwordHash)
        {
            return BCrypt.Net.BCrypt.Verify(password, passwordHash);
        }
    }
}