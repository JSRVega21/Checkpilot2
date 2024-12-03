using CheckPilot.Server.Service;
using Newtonsoft.Json.Linq;
using System.Threading.Tasks;

namespace CheckPilot.Server.Repository
{
    public class InvoiceRepository : IInvoiceRepository
    {
        private readonly SapService _sapService;

        public InvoiceRepository(SapService sapService)
        {
            _sapService = sapService;
        }

        public async Task<string> GetInvoicesAsync(string filter, string sessionId)
        {
            return await _sapService.GetInvoicesAsync(filter, sessionId);
        }

        public async Task<string> GetInvoicesTimeAsync(string filter, string sessionId)
        {
            return await _sapService.GetInvoicesTimeAsync(filter, sessionId);
        }

        public async Task<bool> UpdateInvoiceAsync(int docEntry, JObject updateData, string sessionId)
        {
            return await _sapService.UpdateInvoiceAsync(docEntry, updateData, sessionId);
        }
    }
}
