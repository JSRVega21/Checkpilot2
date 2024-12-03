using CheckPilot.Models;
using CheckPilot.Server.Data;
using CheckPilot.Server.Repository;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace CheckPilot.Server.Repository
{
    public class LoginRepository : ILoginRepository
    {
        private readonly ApplicationDbContext _context;

        public LoginRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<User?> AuthenticateUserAsync(string identifier, string password)
        {
            var user = await _context.UserCheckPilot
                .Where(u => u.UserName == identifier || u.UserEmail == identifier || u.UserPhone == identifier)
                .FirstOrDefaultAsync();

            if (user == null)
            {
                Console.WriteLine("Usuario no encontrado");
                return null;
            }

            if (!BCrypt.Net.BCrypt.Verify(password, user.UserPassword))
            {
                Console.WriteLine("Contraseña incorrecta");
                return null;
            }

            Console.WriteLine("Usuario autenticado correctamente");
            return user;
        }


    }
}
