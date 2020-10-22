using System;
using System.Collections.Generic;
using ClientApi.Enums;

namespace ClientApi.Services
{
    public class ResponseStatus
    {
        public static string GetResponse(string id, BeyonicValidationResponses response)
        {
            if ((int)response == 1)
            {
                return $"{BeyonicValidationStatus.ACCEPT} {id}";
            }
            return $"{BeyonicValidationStatus.REJECT} {id} {GetMessage(response)}";
        }

        public static string GetMessage(BeyonicValidationResponses response)
        {
            var mapper = new Dictionary<BeyonicValidationResponses, string>()
            {
                { BeyonicValidationResponses.InvalidClient, "Client Not Found" },
                { BeyonicValidationResponses.InactiveClient, "Client Not Active" },
                { BeyonicValidationResponses.PhoneCurrencyMismatch, "PhoneNumber CurrencyCode Mismatch" },
                { BeyonicValidationResponses.InvalidAccountNumber,"Invalid Account Number" },
                { BeyonicValidationResponses.InvalidCountry, "Invalid CountryCode From PhoneNumber"},
                { BeyonicValidationResponses.InvalidPhoneNumber, "Invalid Phone Number"},
                { BeyonicValidationResponses.InvalidTransactionId, "Invalid Transaction Id"},
                { BeyonicValidationResponses.InvalidAmount, "Invalid Amount" },
                { BeyonicValidationResponses.DuplicateAccount, "Account Not Unique" },
                { BeyonicValidationResponses.TransactionAlreadyProcessed, "Transaction with this Id Already Processed"}
            };

            if (mapper.ContainsKey(response))
            {
                return mapper[response];
            }

            throw new ArgumentException();
        }
    }
}