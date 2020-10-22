namespace ClientApi.Enums
{
    public enum BeyonicValidationResponses
    {
        ValidTransaction =1,
        InvalidClient,
        PhoneCurrencyMismatch,
        InvalidAccountNumber,
        InvalidCountry,
        InvalidPhoneNumber,
        InvalidTransactionId,
        InvalidAmount,
        InactiveClient,
        DuplicateAccount,
        TransactionAlreadyProcessed
    }
}