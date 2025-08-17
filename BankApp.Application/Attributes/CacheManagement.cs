using BankApp.Application.Attributes.Helpers;
using BankApp.Application.Enums;
using BankApp.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;

namespace BankApp.Application.Attributes
{
    public class CacheManagement : ActionFilterAttribute
    {
        private readonly Type _entityType;
        private readonly CacheOperationType _operationType;

        public CacheManagement(Type entityType, CacheOperationType operationType)
        {
            _entityType = entityType;
            _operationType = operationType;
        }

        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var cacheService = context.HttpContext.RequestServices.GetService<ICacheService>();
            var cacheKey = CacheKeyHelper.GetCacheKey(_entityType);

            if (_operationType == CacheOperationType.Read)
            {
                // 1. Önce cache'ten oku
                var cachedData = await cacheService.GetAsync<object>(cacheKey);
                if (cachedData != null)
                {
                    context.Result = new OkObjectResult(cachedData);
                    return;
                }

                // 2. Yoksa action'ı çalıştır
                var executedContext = await next();

                // 3. Sonuç başarılıysa cache'e yaz
                if (executedContext.Exception == null && executedContext.Result is ObjectResult objectResult)
                {
                    await cacheService.SetAsync(cacheKey, objectResult.Value);
                }

                return;
            }

            // Write veya Refresh işlemleri için: önce action'ı çalıştır
            var result = await next();

            // Başarılıysa cache'i sil
            if ((_operationType == CacheOperationType.Refresh || _operationType == CacheOperationType.Write)
                && result.Exception == null)
            {
                await cacheService.RemoveAsync(cacheKey);
            }
        }
    }
}

