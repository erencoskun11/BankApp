namespace BankAppDomain.Models.ManagersModels
{
    public class CustomerUpdateModel
    {
        public string FullName { get; set; }
        public string NationalId { get; set; }
        public string BirthPlace { get; set; }
        public DateTime BirthDate { get; set; }
        public decimal RiskLimit { get; set; }
        public bool IsActive { get; set; }
    }
}