using AutoMapper;
using DevIO.Api.ViewModels;
using DevIO.Business.Intefaces;
using DevIO.Business.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace DevIO.Api.Controllers
{
    [Authorize]
    [Route("api/fornecedores")]
    public class FornecedoresController : MainController
    {
        private readonly IFornecedorRepository _fornecedorRepository;
        private readonly IFornecedorService _fornecedorService;
        private readonly IEnderecoRepository _enderecoRepository;
        private readonly IMapper _mapper;

        public FornecedoresController(IFornecedorRepository fornecedorRepository,
                                      IMapper mapper,
                                      IFornecedorService fornecedorService,
                                      IEnderecoRepository enderecoRepository,
                                      INotificador notificador) : base(notificador)
        {
            _mapper = mapper;
            _fornecedorRepository = fornecedorRepository;
            _enderecoRepository = enderecoRepository;
            _fornecedorService = fornecedorService;
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<FornecedorViewModel>>> Get()
        {
            var fornecedoresViewModel = _mapper.Map<IEnumerable<FornecedorViewModel>>(await _fornecedorRepository.ObterTodos());

            return CustomResponse(fornecedoresViewModel);
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<IEnumerable<FornecedorViewModel>>> Get(Guid id)
        {
            var fornecedorViewModel = await ObterFornecedorProdutosEndereco(id);

            if (fornecedorViewModel == null) return NotFound();

            return CustomResponse(fornecedorViewModel);
            
        }

        [HttpPost]
        public async Task<ActionResult<FornecedorViewModel>> Post([FromBody] FornecedorViewModel fornecedorViewModel)
        {
            if(!ModelState.IsValid) return CustomResponse(ModelState);

            var fornecedor = _mapper.Map<Fornecedor>(fornecedorViewModel);

            await _fornecedorService.Adicionar(fornecedor);

            return CustomResponse(fornecedorViewModel);
        }

        [HttpPut("{id:guid}")]
        public async Task<ActionResult<FornecedorViewModel>> Put(Guid id, [FromBody] FornecedorViewModel fornecedorViewModel)
        {
            if(id != fornecedorViewModel.Id) 
            {
                NotificarErro("O id informado não é o mesmo que foi passado na query.");
                return CustomResponse(fornecedorViewModel);
            }

            if (!ModelState.IsValid) return CustomResponse(ModelState);

            var fornecedor = _mapper.Map<Fornecedor>(fornecedorViewModel);

            await _fornecedorService.Atualizar(fornecedor);

            return CustomResponse(fornecedorViewModel);
        }

        [HttpDelete("{id:guid}")]
        public async Task<ActionResult<FornecedorViewModel>> Delete(Guid id)
        {
            var fornecedorViewModel = ObterFornecedorEndereco(id);

            if (fornecedorViewModel == null) return NotFound();

            await _fornecedorService.Remover(id);

            return CustomResponse(fornecedorViewModel);
        }

        [HttpGet("obter-endereco/{id:guid}")]
        public async Task<EnderecoViewModel> ObterEnderecoPorId(Guid id)
        {
            return _mapper.Map<EnderecoViewModel>(await _enderecoRepository.ObterPorId(id));
        }

        [HttpPut("atualizar-endereco/{id:guid}")]
        public async Task<IActionResult> AtualizarEdereco(Guid id, [FromBody] EnderecoViewModel enderecoViewModel)
        {
            if(id != enderecoViewModel.Id) return BadRequest();

            if(!ModelState.IsValid) return CustomResponse(ModelState);

            var endereco = _mapper.Map<Endereco>(enderecoViewModel);
            await _fornecedorService.AtualizarEndereco(endereco);

            return CustomResponse(enderecoViewModel);
        }

        private async Task<FornecedorViewModel> ObterFornecedorProdutosEndereco(Guid id)
        {
            return _mapper.Map<FornecedorViewModel>(await _fornecedorRepository.ObterFornecedorProdutosEndereco(id));
        }

        private async Task<FornecedorViewModel> ObterFornecedorEndereco(Guid id)
        {
            return _mapper.Map<FornecedorViewModel>(await _fornecedorRepository.ObterFornecedorEndereco(id));
        }
    }
}
