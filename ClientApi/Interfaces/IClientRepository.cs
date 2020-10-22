using System.Collections.Generic;
using System.Threading.Tasks;
using ClientApi.DTOs;

namespace ClientApi.Interfaces
{
    public interface IClientRepository
    {
        Task<FClient> GetClientByAccountNumber(int accountNumber, string country);
        Task<FakeClient> InsertClient(FakeClient client);
        Task<List<FClient>> GetAllClients(string country);
        void InitializeTable();
    }
}