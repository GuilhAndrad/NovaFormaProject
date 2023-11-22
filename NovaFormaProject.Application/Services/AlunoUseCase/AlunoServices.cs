using AutoMapper;
using NovaFormaProject.Application.Dtos.Request;
using NovaFormaProject.Application.Dtos.Response;
using NovaFormaProject.Application.ExceptionsBase;
using NovaFormaProject.Application.Validations;
using NovaFormaProject.Application.Validations.ResourcesMensagesError;
using NovaFormaProject.Domain.DatabaseEntities;
using NovaFormaProject.Domain.Interfaces.EntitiesRepositoryInterface;

namespace NovaFormaProject.Application.Services.AlunoUseCase;
public class AlunoServices
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IAlunoRepository _alunoRepository;
    private readonly IMapper _mapper;

    public AlunoServices(IUnitOfWork unitOfWork, IAlunoRepository alunoRepository, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _alunoRepository = alunoRepository;
        _mapper = mapper;

    }

    public async Task<IEnumerable<AlunoRequestJson>> GetAllAlunosAsync()
    {
        var alunos = await _alunoRepository.GetAllAsync();
        return _mapper.Map<IEnumerable<AlunoRequestJson>>(alunos);
    }

    public async Task<AlunoRequestJson> GetAlunoByIdAsync(int alunoId)
    {
        var aluno = await _alunoRepository.GetByIdAsync(alunoId);
        ValidarAlunoNotFound(aluno);
        return _mapper.Map<AlunoRequestJson>(aluno);
    }

    public async Task<IEnumerable<AlunoRequestJson>> GetAlunosByNameAsync(string name)
    {
        var alunos = await _alunoRepository.GetAlunosByNameAsync(name);
        return _mapper.Map<IEnumerable<AlunoRequestJson>>(alunos);
    }

    public async Task<AlunoResponseJson> AddAlunoAsync(AlunoRequestJson alunoRequest)
    {
        Validar(alunoRequest);
        var aluno = _mapper.Map<Aluno>(alunoRequest);
        await _alunoRepository.AddAsync(aluno);
        await _unitOfWork.Commit();

        return _mapper.Map<AlunoResponseJson>(aluno);
    }

    public async Task<AlunoResponseJson> UpdateAlunoAsync(int alunoId, AlunoRequestJson alunoRequest)
    {
        var aluno = await _alunoRepository.GetByIdAsync(alunoId);
        //validando se o aluno existe
        ValidarAlunoNotFound(aluno);
        //executando as demais validações
        Validar(alunoRequest);

        _mapper.Map(alunoRequest, aluno);

        await _alunoRepository.UpdateAsync(aluno);
        await _unitOfWork.Commit();

        return _mapper.Map<AlunoResponseJson>(aluno);
    }

    public async Task DeleteAlunoAsync(int alunoId)
    {
        var aluno = await _alunoRepository.GetByIdAsync(alunoId);

        ValidarAlunoNotFound(aluno);

        await _alunoRepository.DeleteAsync(aluno);

        await _unitOfWork.Commit();
    }

    private static void Validar(AlunoRequestJson alunoRequest)
    {
        var validator = new AlunoValidator();
        var result = validator.Validate(alunoRequest);

        if (!result.IsValid)
        {
            var mensagesDeErro = result.Errors.Select(c => c.ErrorMessage).ToList();
            throw new ValidationsErrorException(mensagesDeErro);
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