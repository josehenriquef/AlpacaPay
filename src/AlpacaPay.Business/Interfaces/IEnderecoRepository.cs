using AlpacaPay.Business.Models;
using System;
using System.Threading.Tasks;

namespace AlpacaPay.Business.Interfaces
{
    public interface IEnderecoRepository : IRepository<Endereco>
    {
        Task<Endereco> ObterEnderecoPorFornecedor(Guid fornecedorId);
    }
}
