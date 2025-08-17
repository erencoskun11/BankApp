using BankApp.Application.DTOs;
using BankApp.Application.DTOs.CustomerDto;
using BankApp.Application.Interfaces;
using BankApp.Application.Services;
using BankApp.Infrastructure.Data;
using BankAppDomain.Views;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BankApp.API2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PersonalFinancialInfoController : ControllerBase
    {
        // Controllerda veya serviste dbContext kullanılmaz.
        // Burda önce servis oluşturulcak.
        //Sonra da repo oluşturulup o servise bağlanacak.
        //Data çekme işini repo yapacak.
        //İstek sırası: Controller->Servis->Repo-Veritabanı
        //Metod doğrudan entity veya viewın tamamını dönmesin.Dto ya maplenerek verilsin.
        //Bu ve diğer controllerların tamamında IActionResult dönüş tipi kaldırılacak.Dto veya type (bool,string,int vs.) dönmeli.

        private readonly IPersonalFinancialInfoViewService _service;

        public PersonalFinancialInfoController(IPersonalFinancialInfoViewService service)
        {
            _service = service;
        }
        [HttpGet]
        public async Task<List<PersonalFinancialInfoDto>> GetAll()
        {
            // Repo içinde bu sorgu yazılırken dapper kullanılsın.
            var data = await _service.ToListAsync();
            return data;
        }
    }
}
