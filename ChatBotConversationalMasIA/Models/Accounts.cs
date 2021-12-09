namespace Models
{
    public class AccountDetails
    {
        public Header header { get; set; }
        public Data Data { get; set; }
    }
    
    public class Data
    {
        public AccountUser[] Account { get; set; }
    }

    public class AccountUser
    {
        public string Status { get; set; }
        public string Currency { get; set; }
        public string AccountType { get; set; }
        public string AccountSubType { get; set; }
        public string Nickname { get; set; }
        public AccountBase[] Account { get; set; }
        public Balance[] Balance { get; set; }
    }
    
    public class AccountBase
    {
        public string SchemeName { get; set; }
        public string Identification { get; set; }
        public string SecondaryIdentification { get; set; }
    }

    public class Balance
    {
        public string Type { get; set; }
        public AmountBase Amount { get; set; }
    }
    public class AmountBase
    {
        public float Amount { get; set; }
        public string Currency { get; set; }
    }
}