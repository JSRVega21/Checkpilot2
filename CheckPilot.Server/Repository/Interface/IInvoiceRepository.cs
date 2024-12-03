using System.Threading.Tasks;

namespace CheckPilot.Server.Repository
{

    public interface IInvoiceRepository
    {
        Task<string> GetInvoicesAsync(string filter, string sessionId);
        Task<string> GetInvoicesTimeAsync(string filter, string sessionId);
    }

}
