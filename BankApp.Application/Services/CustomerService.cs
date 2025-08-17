using BankApp.Domain.Entities;
using BankApp.Application.Interfaces;
using BankApp.Application.DTOs.CustomerDto;
using AutoMapper;
using BankAppDomain.Models.ViewModels;
using BankAppDomain.Managers;
using BankAppDomain.Constants;
using BankApp.Application.Events;
using BankAppDomain.Events;

namespace BankApp.Application.Services
{
    public class CustomerService :  ICustomerService
    {

        private readonly ICustomerRepository _customerRepository;
        private readonly CustomerManager _customerManager;
        private readonly IMapper _mapper;
        private readonly ICacheService _cacheService;
        private readonly IElasticSearchService _elasticSearchService;
        private readonly IEventPublisher<CustomerCreateEto> _customerCreateEventPublisher;
        private readonly IEventPublisher<CustomerDeleteEto> _customerDeletedEventPublisher;


        public CustomerService(ICustomerRepository customerRepository, IMapper mapper, CustomerManager customerManager,ICacheService cacheService, IElasticSearchService elasticSearchService,IEventPublisher<CustomerCreateEto> customerCreateEventPublisher, IEventPublisher<CustomerDeleteEto> customerDeletedEventPublisher)
        {
            _customerRepository = customerRepository;
            _mapper = mapper;
            _customerManager = customerManager;
            _cacheService = cacheService;
            _elasticSearchService = elasticSearchService;
            _customerCreateEventPublisher = customerCreateEventPublisher;
            _customerDeletedEventPublisher = customerDeletedEventPublisher;

        }

        public async Task<List<CustomerDto>> GetAllCustomersAsync()
        {
            string cacheKey = "customer_list";

            var cachedCustomers = await _cacheService.GetAsync<List<CustomerDto>>(cacheKey);
            if (cachedCustomers != null)
            {
                return cachedCustomers; 
            }

            // 2️⃣ Cache yoksa veritabanından çek
            var customers = await _customerRepository.GetAllAsync();
            var customerDtos = _mapper.Map<List<CustomerDto>>(customers);

            // 10 dakika sürecek
            await _cacheService.SetAsync(cacheKey, customerDtos, TimeSpan.FromMinutes(10));

            return customerDtos;
        }

        public async Task<CustomerDto?> GetCustomerByIdAsync(int id)
        {
            var customer = await _customerRepository.GetByIdAsync(id);
            if (customer == null) return null;
            return _mapper.Map<CustomerDto>(customer);
        }

        public async Task<bool> CreateCustomerAsync(CustomerCreateDto dto)
        {
            try
            {
                bool exists = await _customerRepository.ExistsByNationalIdAsync(dto.NationalId);
                if (exists)
                    throw new InvalidOperationException($"NationalId '{dto.NationalId}' zaten kayıtlı.");

                var customerCreateModel = _mapper.Map<CustomerCreateModel>(dto);
                var customer = _customerManager.Create(customerCreateModel);
                

                await _customerRepository.AddAsync(customer);
                await _customerRepository.SaveChangesAsync();
                await _elasticSearchService.IndexAsync(customer, ElasticSearchConstants.Customer.IndexName);

                var @event = new CustomerCreateEto
                {
                    Id = customer.Id,
                    FullName = customer.FullName,
                    NationalId = customer.NationalId,
                    BirthDate = customer.BirthDate,
                    BirthPlace = customer.BirthPlace,
                    RiskLimit = customer.RiskLimit
                };

                await _customerCreateEventPublisher.PublishAsync(@event, QueueNameConstant.CustomerCreated);
                return true;
            }
            catch (Exception ex) 
            {
                Console.WriteLine($"[CreateCustomerAsync] HATA: {ex.Message}");

                return false;}
        }

        public async Task<bool> UpdateCustomerAsync(CustomerUpdateDto dto)
        {
            var existing = await _customerRepository.GetByIdAsync(dto.Id);
            if (existing == null)
                return false;

            _mapper.Map(dto, existing);

            await _customerRepository.UpdateAsync(existing);
            await _customerRepository.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeleteCustomerAsync(int id)
        {
            var existing = await _customerRepository.GetByIdAsync(id);
            if (existing == null)
                return false;

            await _customerRepository.DeleteAsync(id);
            await _customerRepository.SaveChangesAsync();

            await _elasticSearchService.DeleteAsync<Customer>("customers", id.ToString());

            var @event = new CustomerDeleteEto
            {
                CustomerId = id,
                DeletedAt = DateTime.UtcNow
            };

            await _customerDeletedEventPublisher.PublishAsync(@event, QueueNameConstant.CustomerDeleted);

            // ✅ Cache'i temizlemeyi unutmusum swaggerda karsialstigim hatanin sebebi buydu
            await _cacheService.RemoveAsync("customer_list");

            return true;
        }

        public async Task<bool> ExistsByNationalIdAsync(string nationalId)
        {
            return await _customerRepository.ExistsByNationalIdAsync(nationalId);
        }

        public async Task<List<CustomerDto>>SearchCustomerAsync(string searchText)
        {
            var customers = await _elasticSearchService.SearchAsync<Customer>("customers", searchText);
            var customerDtos = _mapper.Map <List< CustomerDto >> (customers);

            return customerDtos;
        }
    }
}

