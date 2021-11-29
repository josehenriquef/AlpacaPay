using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AlpacaPay.Business.Interfaces;
using AlpacaPay.Business.Models;
using AlpacaPay.Site.ViewModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AlpacaPay.Site.Controllers
{
    public class FornecedoresController : Controller
    {
        private readonly IFornecedorRepository _fornecedorRepository;
        private readonly IEnderecoRepository _enderecoRepository;
        //private readonly IFornecedorService _fornecedorService;
        private readonly IMapper _mapper;

        public FornecedoresController(IFornecedorRepository fornecedorRepository, IEnderecoRepository enderecoRepository,
             IMapper mapper)
        {
            _fornecedorRepository = fornecedorRepository;
            _enderecoRepository = enderecoRepository;
            //_fornecedorService = fornecedorService;
            _mapper = mapper;
        }

        public async Task<IActionResult> Index()
        {
            return View(_mapper.Map<List<FornecedorViewModel>>(await _fornecedorRepository.ObterTodos()));
        }

        public async Task<IActionResult> Details(Guid id)
        {
            var fornecedorViewModel = _mapper.Map<FornecedorViewModel>(await _fornecedorRepository.ObterFornecedorEndereco(id));

            if (fornecedorViewModel == null)
            {
                return NotFound();
            }

            return View(fornecedorViewModel);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(FornecedorViewModel fornecedorViewModel)
        {
            if (ModelState.IsValid)
            {
                fornecedorViewModel.Id = Guid.NewGuid();

                // Vai mudar
                await _fornecedorRepository.Adicionar(_mapper.Map<Fornecedor>(fornecedorViewModel));

                return RedirectToAction(nameof(Index));
            }
            return View(fornecedorViewModel);
        }

        public async Task<IActionResult> Edit(Guid id)
        {
            var fornecedorViewModel = _mapper.Map<FornecedorViewModel>(await _fornecedorRepository.ObterFornecedorProdutosEndereco(id));

            if (fornecedorViewModel == null)
            {
                return NotFound();
            }

            return View(fornecedorViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, FornecedorViewModel fornecedorViewModel)
        {
            if (id != fornecedorViewModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _fornecedorRepository.Atualizar(_mapper.Map<Fornecedor>(fornecedorViewModel));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await FornecedorExists(fornecedorViewModel.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(fornecedorViewModel);
        }

        public async Task<IActionResult> Delete(Guid id)
        {
            var fornecedorViewModel = _mapper.Map<FornecedorViewModel>(await _fornecedorRepository.ObterFornecedorEndereco(id));

            if (fornecedorViewModel == null)
            {
                return NotFound();
            }

            return View(fornecedorViewModel);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            await _fornecedorRepository.Remover(id);

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> ObterEndereco(Guid id)
        {
            var fornecedor = _mapper.Map<FornecedorViewModel>(await _fornecedorRepository.ObterFornecedorEndereco(id));
            if (fornecedor == null)
            {
                return NotFound();
            }

            return PartialView("_DetalhesEndereco", fornecedor);
        }

        public async Task<IActionResult> AtualizarEndereco(Guid id)
        {
            var fornecedor = _mapper.Map<FornecedorViewModel>(await _fornecedorRepository.ObterFornecedorEndereco(id));

            if (fornecedor == null)
            {
                return NotFound();
            }

            return PartialView("_AtualizarEndereco", new FornecedorViewModel { Endereco = fornecedor.Endereco });
        }

        [HttpPost]
        public async Task<IActionResult> AtualizarEndereco(FornecedorViewModel fornecedorViewModel)
        {
            ModelState.Remove("Nome");
            ModelState.Remove("Documento");

            if (!ModelState.IsValid) return PartialView("_AtualizarEndereco", fornecedorViewModel);

            await _enderecoRepository.Atualizar(_mapper.Map<Endereco>(fornecedorViewModel.Endereco));

            var url = Url.Action("ObterEndereco", "Fornecedores", new { id = fornecedorViewModel.Endereco.FornecedorId });
            return Json(new { success = true, url });
        }

        private async Task<bool> FornecedorExists(Guid id)
        {
            return await _fornecedorRepository.ObterPorId(id) != null;
        }
    }
}
