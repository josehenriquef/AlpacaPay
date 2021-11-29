using Microsoft.EntityFrameworkCore;
using AlpacaPay.Business.Interfaces;
using AlpacaPay.Business.Models;
using AlpacaPay.Data.Context;
using System;
using System.Threading.Tasks;

namespace AlpacaPay.Data.Repository
{
    public class EnderecoRepository : Repository<Endereco>, IEnderecoRepository
    {
        public EnderecoRepository(AlpacaDbContext context) : base(context)
        {

        }

        public async Task<Endereco> ObterEnderecoPorFornecedor(Guid fornecedorId)
        {
            return await Db.Enderecos.AsNoTracking()
                .FirstOrDefaultAsync(e => e.FornecedorId == fornecedorId);
        }
    }
}
