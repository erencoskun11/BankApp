using AutoMapper;
using BankApp.Application.Attributes.Interfaces;
using BankApp.Application.Interfaces;
using BankAppDomain;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.DependencyInjection;
public class CacheRefresh : ActionFilterAttribute
{
    private readonly Type[] _entityTypes;

    public CacheRefresh(params Type[] entityTypes)
    {
        _entityTypes = entityTypes;
    }

    public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        var executed = await next();
        if (executed.Exception != null) return;

        var cacheService = context.HttpContext.RequestServices.GetService<ICacheService>();
        var mapper = context.HttpContext.RequestServices.GetService<IMapper>();

        if (cacheService == null || mapper == null) return;

        foreach (var type in _entityTypes)
        {
            if (!(Activator.CreateInstance(type) is ICacheKeyProvider provider)) continue;

            var listKey = provider.GetCacheKey();
            await cacheService.RemoveAsync(listKey);

            if (context.ActionArguments.TryGetValue("id", out var idObj))
            {
                var id = Convert.ToInt32(idObj);


                var repoType = typeof(IRepository<>).MakeGenericType(type);
                dynamic repo = context.HttpContext.RequestServices.GetService(repoType);
                if (repo != null)
                {
                    var entity = await repo.GetByIdAsync(id);
                    if (entity != null)
                    {
                        var dto = mapper.Map(entity, type, provider.CacheValueType);
                        var singleKey = provider.GetSingleKey(id);
                        await cacheService.SetAsync(singleKey, dto);
                    }
                }
            }
        }
    }
}
