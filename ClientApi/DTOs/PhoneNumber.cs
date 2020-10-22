using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using ClientApi.Enums;
using ClientApi.Services;

namespace ClientApi.DTOs
{
    public class PhoneNumber
    {
        public string Number { get; set; }
        public CountryCode Country { get; set; }

        public static PhoneNumber FromString(string rawNumber)
        {
            if (!rawNumber.StartsWith("+"))
            {
                throw new InvalidPhoneNumberException();
            }

            rawNumber = Regex.Replace(rawNumber, @"(?<!^)\+|[^\d+]+", "");

            if (rawNumber.Length > 8)
            {
                return new PhoneNumber()
                {
                    Number = $"0{rawNumber.Substring(4)}",
                    Country = GetCountryCodeFromExtension(rawNumber.Substring(0, 4)),
                };
            }

            throw new InvalidPhoneNumberException();
        }
        public static CountryCode GetCountryCodeFromExtension(string phoneExtension)
        {
            var mapper = new Dictionary<string, CountryCode>()
            {
                { "+234", CountryCode.NG },
                { "+250", CountryCode.RW },
                { "+254", CountryCode.KE },
                { "+255", CountryCode.TZ },
                { "+256", CountryCode.UG },
                { "+257", CountryCode.BI },
                { "+260", CountryCode.ZM },
                { "+265", CountryCode.MW }
            };
            if (mapper.ContainsKey(phoneExtension))
            {
                return mapper[phoneExtension];
            }

            throw new ArgumentException($"{ResponseStatus.GetMessage(BeyonicValidationResponses.InvalidPhoneNumber)}");
        }
    }
}