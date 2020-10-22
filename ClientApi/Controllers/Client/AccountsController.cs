using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using ClientApi.Interfaces;
using System.Collections.Generic;
using ClientApi.DTOs;
using System;

namespace ClientApi.Controllers.Client
{
    [Route("/api/countries/{country}/[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private readonly IClientRepository _fakeClientRepository;
        public AccountsController(IClientRepository fakeClientRepository)
        {
            _fakeClientRepository = fakeClientRepository;
        }

        // POST: /api/clients/FakeClient/
        [HttpPost]
        public async Task<ActionResult<string>> PostFakeClient(FakeClient client)
        {
            var response = await _fakeClientRepository.InsertClient(client);
            return Ok($"ACCEPTED {response.AccountNumber}");
        }

        // GET: /api/countries/mw/accounts/87663148
        [HttpGet("{accountnumber}")]
        public async Task<ActionResult<FClient>> GetFakeClient(int accountnumber, string country)
        {
            var res = await _fakeClientRepository.GetClientByAccountNumber(accountnumber, country);
            return res;
        }

        // GET: /api/countries/mw/accounts/
        [HttpGet()]
        public async Task<ActionResult<List<FClient>>> GetFakeClients(string country)
        {
            var res = await _fakeClientRepository.GetAllClients(country);
            Console.WriteLine(res);
            return res;
        }
    }
}