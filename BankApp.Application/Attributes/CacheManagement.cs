using BankApp.Application.Attributes.Helpers;
using BankApp.Application.Enums;
using BankApp.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Reflection;
using System.Threading.Tasks;
using BankApp.Application.Attributes.Interfaces;

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
            if (cacheService == null)
            {
                await next();
                return;
            }

            if (!(Activator.CreateInstance(_entityType) is ICacheKeyProvider provider))
            {
                await next();
                return;
            }

            var listKey = provider.GetCacheKey();

            if (_operationType == CacheOperationType.Read)
            {
                var cached = await cacheService.GetAsync<object>(listKey);
                if (cached != null)
                {

                    context.Result = new OkObjectResult(cached);
                    return;
                }

                var executedContext = await next();

                if (executedContext.Exception == null && executedContext.Result is ObjectResult objectResult)
                {
                    await cacheService.SetAsync(listKey, objectResult.Value);
                }
                return;
            }

            var resultContext = await next();
            if (resultContext.Exception != null) return;

            // Liste cache'i sil
            await cacheService.RemoveAsync(listKey);

            // Tekil entity cache'i
            object maybeEntity = null;
            if (resultContext.Result is ObjectResult orRes && orRes.Value != null)
                maybeEntity = orRes.Value;

            int? id = null;

            if (maybeEntity != null)
                id = ExtractId(maybeEntity);

            if (id == null && context.ActionArguments.TryGetValue("id", out var idObj))
            {
                if (int.TryParse(idObj?.ToString(), out var parsed))
                    id = parsed;
            }

            if (id != null && maybeEntity != null)
            {
                var singleKey = provider.GetSingleKey(id.Value);
                await cacheService.SetAsync(singleKey, maybeEntity);
            }
        }

        private int? ExtractId(object obj)
        {
            var t = obj.GetType();
            var prop = t.GetProperty("Id", BindingFlags.Public | BindingFlags.Instance)
                       ?? t.GetProperty("id", BindingFlags.Public | BindingFlags.Instance);

            if (prop == null) return null;
            var val = prop.GetValue(obj);
            if (val == null) return null;
            if (int.TryParse(val.ToString(), out var id)) return id;
            return null;
        }
    }
}
