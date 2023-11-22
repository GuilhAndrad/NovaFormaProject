using Microsoft.AspNetCore.Mvc;
using NovaFormaProject.Application.Dtos;
using NovaFormaProject.Application.Services;

namespace NovaFormaProject.API.Controllers;

/// <summary>
/// Controller responsável por operações relacionadas aos alunos.
/// </summary>
[Route("api/[controller]")]
[ApiController]
public class AlunoController : ControllerBase
{
    /// <summary>
    /// Obtém todos os alunos cadastrados.
    /// </summary>
    /// <returns>Lista de todos os alunos.</returns>
    /// <response code="200">Retorna todos os alunos cadastrados.</response>
    [HttpGet("todos-os-alunos")]
    [ProducesResponseType(typeof(IEnumerable<AlunoRequestJson>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllAlunos([FromServices] AlunoServices alunoServices)
    {
        var alunos = await alunoServices.GetAllAlunosAsync();
        return Ok(alunos);
    }

    /// <summary>
    /// Obtém um aluno pelo ID.
    /// </summary>
    /// <param name="alunoServices"></param>
    /// <param name="id">ID do aluno a ser obtido.</param>
    /// <returns>O aluno correspondente ao ID.</returns>
    /// <response code="200">Retorna o aluno correspondente ao ID.</response>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(AlunoRequestJson), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAlunoById([FromServices] AlunoServices alunoServices, [FromRoute] int id)
    {
        var aluno = await alunoServices.GetAlunoByIdAsync(id);
        return Ok(aluno);
    }

    /// <summary>
    /// Procura alunos pelo nome.
    /// </summary>
    /// <param name="alunoServices"></param>
    /// <param name="name">Nome do aluno a ser procurado.</param>
    /// <returns>Lista de alunos correspondentes ao nome.</returns>
    /// <response code="200">Retorna a lista de alunos correspondentes ao nome.</response>
    [HttpGet("search")]
    [ProducesResponseType(typeof(IEnumerable<AlunoRequestJson>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAlunosByName([FromServices] AlunoServices alunoServices, [FromQuery] string name)
    {
        var alunos = await alunoServices.GetAlunosByNameAsync(name);
        return Ok(alunos);
    }

    /// <summary>
    /// Adiciona um novo aluno.
    /// </summary>
    /// <param name="alunoServices"></param>
    /// <param name="alunoRequest">Dados do novo aluno a ser adicionado.</param>
    /// <returns>O aluno recém-adicionado.</returns>
    /// <response code="201">Retorna o aluno recém-adicionado.</response>
    [HttpPost]
    [ProducesResponseType(typeof(AlunoResponseJson), StatusCodes.Status201Created)]
    public async Task<IActionResult> AddAluno([FromServices] AlunoServices alunoServices, [FromBody] AlunoRequestJson alunoRequest)
    {
        var alunoResponse = await alunoServices.AddAlunoAsync(alunoRequest);

        return Created(string.Empty, alunoResponse);
    }

    /// <summary>
    /// Atualiza um aluno existente.
    /// </summary>
    /// <param name="alunoServices"></param>
    /// <param name="id">ID do aluno a ser atualizado.</param>
    /// <param name="alunoRequest">Novos dados do aluno.</param>
    /// <returns>O aluno atualizado.</returns>
    /// <response code="200">Retorna o aluno atualizado.</response>
    [HttpPatch]
    [Route("{id}")]
    [ProducesResponseType(typeof(AlunoResponseJson), StatusCodes.Status204NoContent)]
    public async Task<IActionResult> AtualizarAluno([FromServices] AlunoServices alunoServices, [FromBody] AlunoRequestJson alunoRequest, [FromRoute] int id)
    {

        var alunoResponse = await alunoServices.UpdateAlunoAsync(id, alunoRequest);
        return Ok(alunoResponse);
    }

    /// <summary>
    /// Deleta um aluno existente.
    /// </summary>
    /// <param name="alunoServices"></param>
    /// <param name="id">ID do aluno a ser deletado.</param>
    /// <returns>NoContent se o aluno foi deletado com sucesso.</returns>
    /// <response code="204">Retorna NoContent se o aluno foi deletado com sucesso.</response>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> DeleteAluno([FromServices] AlunoServices alunoServices, [FromRoute] int id)
    {
        await alunoServices.DeleteAlunoAsync(id);
        return NoContent();
    }
}