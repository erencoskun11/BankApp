using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BankApp.Application.DTOs.CustomerDto;
using BankApp.Application.Interfaces;
using BankAppDomain.Constants;
using Nest;

namespace BankApp.Application.Services
{
    public class ElasticSearchService : IElasticSearchService
    {
        private readonly IElasticClient _elasticClient;
        public ElasticSearchService(IElasticClient elasticClient){_elasticClient = elasticClient;}
        public async Task IndexAsync<T>(T document, string indexName) where T : class
        {
            var response = await _elasticClient.IndexAsync(document, idx => idx.Index(indexName));
        }
        public async Task<IEnumerable<T>> SearchAsync<T>(string indexName, string searchText) where T : class
        {
            var response = await _elasticClient.SearchAsync<T>(s => s
                .Index(indexName)
                .Query(q => q
                    .QueryString(d => d
                        .Query($"*{searchText}*"))));
            return response.Documents;}
        public async Task DeleteAsync<T>(string indexName, string id) where T : class
        {
            var response = await _elasticClient.DeleteAsync<T>(id, d => d.Index(indexName));

            if (!response.IsValid)
            {
                if (response.ApiCall.HttpStatusCode == 404)
                {
                    Console.WriteLine($"[ElasticSearch] Belge zaten yok (index: {indexName}, id: {id})");return;
                }
                throw new Exception($"Elasticsearch deletion failed: {response.DebugInformation}");
            }}
        public async Task IndexCustomerAsync(CustomerDto dto)
        {
            await _elasticClient.IndexAsync(dto, i=> i
            .Index(ElasticSearchConstants.Customer.IndexName)
            .Id(dto.Id));
        }

    }
}