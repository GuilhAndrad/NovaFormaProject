using AutoMapper;
using NovaFormaProject.Application.Dtos.Request;
using NovaFormaProject.Application.Dtos.Response;
using NovaFormaProject.Application.ExceptionsBase;
using NovaFormaProject.Application.Validations;
using NovaFormaProject.Application.Validations.ResourcesMensagesError;
using NovaFormaProject.Domain.DatabaseEntities;
using NovaFormaProject.Domain.DatabaseEntities.Enums;
using NovaFormaProject.Domain.Interfaces.EntitiesRepositoryInterface;

namespace NovaFormaProject.Application.Services.PagamentoUseCase;
public class PagamentoServices
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPagamentoRepository _pagamentoRepository;
    private readonly IAlunoRepository _alunoRepository;
    private readonly IMapper _mapper;

    public PagamentoServices(IUnitOfWork unitOfWork, IPagamentoRepository pagamentoRepository, IMapper mapper, IAlunoRepository alunoRepository)
    {
        _unitOfWork = unitOfWork;
        _pagamentoRepository = pagamentoRepository;
        _mapper = mapper;
        _alunoRepository = alunoRepository;
    }

    public async Task<IEnumerable<PagamentoRequestJson>> GetAllPagamentosAsync()
    {
        var pagamentos = await _pagamentoRepository.GetAllAsync();
        return _mapper.Map<IEnumerable<PagamentoRequestJson>>(pagamentos);
    }

    public async Task<IEnumerable<PagamentoRequestJson>> GetPagamentosByAlunoAsync(int alunoId)
    {
        var aluno = await _alunoRepository.GetByIdAsync(alunoId);
        ValidarAlunoNotFound(aluno);

        var pagamentos = await _pagamentoRepository.GetPagamentosByAlunoAsync(alunoId);

        return _mapper.Map<IEnumerable<PagamentoRequestJson>>(pagamentos);
    }

    public async Task<IEnumerable<PagamentoRequestJson>> GetPagamentosEmAtrasoAsync()
    {
        var pagamentos = await _pagamentoRepository.FindAsync(p => p.DueDate < DateTime.UtcNow && p.PagamentoStatus != PagamentoStatus.Pago);
        return _mapper.Map<IEnumerable<PagamentoRequestJson>>(pagamentos);
    }

    public async Task<PagamentoResponseJson> GetPagamentoByIdAsync(int pagamentoId)
    {
        var pagamento = await _pagamentoRepository.GetByIdAsync(pagamentoId);
        ValidarPagamentoNotFound(pagamento);
        return _mapper.Map<PagamentoResponseJson>(pagamento);
    }

    public async Task<PagamentoResponseJson> AddPagamentoAsync(PagamentoRequestJson pagamentoRequest, int alunoId)
    {
        var aluno = await _alunoRepository.GetByIdAsync(alunoId);

        ValidarAlunoNotFound(aluno);
        ValidarStatusDoAluno(aluno);

        var pagamento = _mapper.Map<Pagamento>(pagamentoRequest);
        pagamento.Aluno = aluno;

        Validar(pagamentoRequest);

        await _pagamentoRepository.AddAsync(pagamento);
        await _unitOfWork.Commit();

        return _mapper.Map<PagamentoResponseJson>(pagamento);
    }

    public async Task<PagamentoResponseJson> UpdatePagamentoAsync(int pagamentoId, PagamentoRequestJson pagamentoRequest)
    {
        var pagamento = await _pagamentoRepository.GetByIdAsync(pagamentoId);

        ValidarPagamentoNotFound(pagamento);

        _mapper.Map(pagamentoRequest, pagamento);

        var aluno = await _alunoRepository.GetByIdAsync(pagamentoRequest.AlunoID);

        ValidarAlunoNotFound(aluno);

        Validar(pagamentoRequest);

        await _pagamentoRepository.UpdateAsync(pagamento);
        await _unitOfWork.Commit();

        return _mapper.Map<PagamentoResponseJson>(pagamento);
    }


    public async Task DeletePagamentoAsync(int pagamentoId)
    {
        var pagamento = await _pagamentoRepository.GetByIdAsync(pagamentoId);

        ValidarPagamentoNotFound(pagamento);

        await _pagamentoRepository.DeleteAsync(pagamento);
        await _unitOfWork.Commit();
    }

    public async Task UpdateStatusPagamentosAtrasadosAsync()
    {
        var pagamentosAtrasados = await _pagamentoRepository.FindAsync(p => p.DueDate < DateTime.UtcNow && p.PagamentoStatus != PagamentoStatus.Pago);

        foreach (var pagamento in pagamentosAtrasados)
        {
            pagamento.PagamentoStatus = PagamentoStatus.Atrasado;
            await _pagamentoRepository.UpdateAsync(pagamento);
        }

        await _unitOfWork.Commit();
    }

    private static void Validar(PagamentoRequestJson pagamentoRequest)
    {
        var validator = new PagamentoValidator();
        var result = validator.Validate(pagamentoRequest);

        if (!result.IsValid)
        {
            var mensagensDeErro = result.Errors.Select(c => c.ErrorMessage).ToList();
            throw new ValidationsErrorException(mensagensDeErro);
        }
    }

    private static void ValidarPagamentoNotFound(Pagamento pagamento)
    {
        if (pagamento is null)
        {
            throw new ValidationsErrorException(new List<string> { PagamentoMensagesError.PAGAMENTO_NOT_FOUND });
        }
    }

    private static void ValidarStatusDoAluno(Aluno aluno)
    {
        if (aluno.Status != AlunoStatus.Ativo)
        {
            throw new ValidationsErrorException(new List<string> { PagamentoMensagesError.ALUNO_INATIVO });
        }
    }

    private static void ValidarAlunoNotFound(Aluno aluno)
    {
        if (aluno is null)
        {
            throw new ValidationsErrorException(new List<string> { AlunoMensagesError.ALUNO_NOT_FOUND });
        }
    }
}