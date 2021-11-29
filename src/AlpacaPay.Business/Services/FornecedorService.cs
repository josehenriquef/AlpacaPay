using AlpacaPay.Business.Interfaces;
using AlpacaPay.Business.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace AlpacaPay.Business.Services
{
    public class FornecedorService : IFornecedorService
    {
        private readonly IFornecedorRepository _fornecedorRepository;
        private readonly IEnderecoRepository _enderecoRepository;

        public FornecedorService(IFornecedorRepository fornecedorRepository, IEnderecoRepository enderecoRepository)
        {
            _fornecedorRepository = fornecedorRepository;
            _enderecoRepository = enderecoRepository;
        }

        public async Task Adicionar(Fornecedor fornecedor)
        {
            // TODO: Validar

            if (_fornecedorRepository.Buscar(f => f.Documento == fornecedor.Documento).Result.Any())
            {
                // TODO: Notificar Erro
                return;
            }

            await _fornecedorRepository.Adicionar(fornecedor);
        }

        public async Task Atualizar(Fornecedor fornecedor)
        {
            // TODO: Validar

            if (_fornecedorRepository.Buscar(f => f.Documento == fornecedor.Documento && f.Id != fornecedor.Id).Result.Any())
            {
                // TODO: Notificar Erro
                return;
            }

            await _fornecedorRepository.Atualizar(fornecedor);
        }

        public async Task AtualizarEndereco(Endereco endereco)
        {
            // TODO: Validar

            await _enderecoRepository.Atualizar(endereco);
        }

        public async Task Remover(Guid id)
        {
            if (_fornecedorRepository.ObterFornecedorProdutosEndereco(id).Result.Produtos.Any())
            {
                // TODO: Notificar Erro
                return;
            }

            var endereco = await _enderecoRepository.ObterEnderecoPorFornecedor(id);
            if (endereco != null)
            {
                await _enderecoRepository.Remover(endereco.Id);
            }

            await _fornecedorRepository.Remover(id);
        }

        public void Dispose()
        {
            _fornecedorRepository?.Dispose();
            _enderecoRepository?.Dispose();
        }
    }
}
