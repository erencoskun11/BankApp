using BankApp.Application.DTOs;
using BankApp.Application.Interfaces;
using BankApp.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[ApiController]
[Route("api/[controller]")]
public class CustomerAccountCardController : ControllerBase
{
    private readonly ICustomerAccountCardViewService _service;

    public CustomerAccountCardController(ICustomerAccountCardViewService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IEnumerable<CustomerAccountCardViewDto>> GetAll()
    {
        return await _service.GetAllAsync();
    }
}

