namespace BankApp.Application.DTOs.CardDtos;

public class CardGetDto
{
    public int Id { get; set; }
    public string CardNumber { get; set; }
    public int ExpiryMonth { get; set; }
    public int ExpiryYear { get; set; }
    public bool IsActive { get; set; }
    public int AccountId { get; set; }
    public int CardTypeId { get; set; }
}

