namespace BankAppDomain.Views
{
    public class PersonalFinancialInfoView
    {
        public int CustomerId { get; set; }
        public string? FullName { get; set; }
        public string? MaskedNationalId { get; set; }
        public int? AccountCount { get; set; }
        public string? MaskedAccountNumbers { get; set; }
    }

}
