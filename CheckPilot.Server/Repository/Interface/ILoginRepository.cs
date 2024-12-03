using CheckPilot.Server.Data;
using CheckPilot.Models;
using System.Threading.Tasks;

namespace CheckPilot.Server.Repository
{
    public interface ILoginRepository
    {
        Task<User?> AuthenticateUserAsync(string userName, string password);
    }
}
