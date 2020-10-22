using System.Threading.Tasks;
using System.Collections.Generic;
using ClientApi.DTOs;
using ClientApi.Interfaces;
using System.Data.SQLite;

namespace ClientApi.Persistence
{
    public class FakeClientRepository : IClientRepository
    {
        //private readonly string _cs = @"URI=/users/cea/Desktop/OAF/payments/faker/ClientApi/clients.db";
        private readonly string _cs = @"URI=file:D:\Projects\OAF\P\payments\faker\ClientApi\clients.db";
        private readonly SQLiteConnection _con;
        public FakeClientRepository(){
            _con = new SQLiteConnection(_cs);
            _con.Open();
        }

        public void InitializeTable()
        {
            using var cmd = new SQLiteCommand(_con);
            cmd.CommandText = "DROP TABLE IF EXISTS clients";
            cmd.ExecuteNonQuery();
            cmd.CommandText = @"CREATE TABLE clients(
                accountnumber INT PRIMARY KEY,
                isactive TEXT,
                country TEXT
                )";
            cmd.ExecuteNonQuery();
        }

        public Task<FClient> GetClientByAccountNumber(int accountNumber, string country)
        {
            return Task.Run(()=>
            {
                using var cmd = new SQLiteCommand(_con);
                cmd.CommandText = @"SELECT * FROM clients WHERE  accountnumber = @accountnumber and country = @country";
                cmd.Parameters.AddWithValue("@accountnumber", accountNumber);
                cmd.Parameters.AddWithValue("@country", country);
                cmd.Prepare();
                cmd.ExecuteNonQuery();
                using SQLiteDataReader rdr = cmd.ExecuteReader();
                return new FClient
                        {
                            AccountNumber  = rdr.GetValue(0).ToString(),
                            IsActive = rdr.GetValue(1).ToString(),
                            Country = rdr.GetValue(2).ToString()
                        };
            });
        }

        public Task<List<FClient>> GetAllClients(string country)
        {
            List<FClient> fakeClients = new List<FClient>();
            return Task.Run(()=>
            {
                using var cmd = new SQLiteCommand(_con);
                cmd.CommandText = @"SELECT * FROM clients where country = @country";
                cmd.Parameters.AddWithValue("@country", country);
                cmd.Prepare();
                cmd.ExecuteNonQuery();
                using SQLiteDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    fakeClients.Add
                    (
                        new FClient {
                            AccountNumber  = rdr.GetValue(0).ToString(),
                            IsActive = rdr.GetValue(1).ToString(),
                            Country = rdr.GetValue(2).ToString()
                        }
                    );
                }
                return fakeClients;
            });
        }

        async public Task<FakeClient> InsertClient(FakeClient client)
        {
            using var cmd = new SQLiteCommand(_con);
            cmd.CommandText = @"INSERT INTO clients(
                    accountnumber,
                    isactive,
                    country
                ) VALUES(
                    @accountnumber,
                    @isactive,
                    @country)";
            cmd.Parameters.AddWithValue("@accountnumber", client.AccountNumber);
            cmd.Parameters.AddWithValue("@isactive", client.IsActive);
            cmd.Parameters.AddWithValue("@country", client.Country);
            cmd.Prepare();
            cmd.ExecuteNonQuery();
            return await Task.FromResult(client);
        }

    }
}