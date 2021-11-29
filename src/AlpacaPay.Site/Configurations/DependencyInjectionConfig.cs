using Microsoft.AspNetCore.Mvc.DataAnnotations;
using Microsoft.Extensions.DependencyInjection;
using AlpacaPay.Business.Interfaces;
using AlpacaPay.Data.Context;
using AlpacaPay.Data.Repository;
using AlpacaPay.Site.Extensions;

namespace AlpacaPay.Site.Configurations
{
    public static class DependencyInjectionConfig
    {
        public static IServiceCollection ResolverDependencies(this IServiceCollection services)
        {
            services.AddScoped<AlpacaDbContext>();
            services.AddScoped<IProdutoRepository, ProdutoRepository>();
            services.AddScoped<IFornecedorRepository, FornecedorRepository>();
            services.AddScoped<IEnderecoRepository, EnderecoRepository>();
            services.AddSingleton<IValidationAttributeAdapterProvider, MoedaValidationAttributeAdapterProvider>();

            return services;
        }
    }
}
