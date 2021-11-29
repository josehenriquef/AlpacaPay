using Microsoft.EntityFrameworkCore;
using AlpacaPay.Business.Interfaces;
using AlpacaPay.Business.Models;
using AlpacaPay.Data.Context;
using System;
using System.Threading.Tasks;

namespace AlpacaPay.Data.Repository
{
    public class FornecedorRepository : Repository<Fornecedor>, IFornecedorRepository
    {
        public FornecedorRepository(AlpacaDbContext context) : base(context)
        {

        }

        public async Task<Fornecedor> ObterFornecedorEndereco(Guid id)
        {
            return await Db.Fornecedores.AsNoTracking()
                .Include(f => f.Endereco)
                .FirstOrDefaultAsync(f => f.Id == id);
        }

        public async Task<Fornecedor> ObterFornecedorProdutosEndereco(Guid id)
        {
            return await Db.Fornecedores.AsNoTracking()
                .Include(f => f.Endereco)
                .Include(f => f.Produtos)
                .FirstOrDefaultAsync(f => f.Id == id);
        }
    }
}
