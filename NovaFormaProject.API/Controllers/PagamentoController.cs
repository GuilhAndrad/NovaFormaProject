using Microsoft.AspNetCore.Mvc;
using NovaFormaProject.Application.Dtos.Request;
using NovaFormaProject.Application.Dtos.Response;
using NovaFormaProject.Application.Services.PagamentoUseCase;

namespace NovaFormaProject.API.Controllers;

/// <summary>
/// Controller responsável por operações relacionadas aos pagamentos.
/// </summary>

[Route("api/[controller]")]
[ApiController]
public class PagamentoController : ControllerBase
{

    /// <summary>
    /// Obtém todos os pagamentos cadastrados.
    /// </summary>
    /// <returns>Lista de todos os pagamentos.</returns>
    /// <response code="200">Retorna todos os pagamentos cadastrados.</response>
    [HttpGet("todos-os-pagamentos")]
    [ProducesResponseType(typeof(IEnumerable<PagamentoRequestJson>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllPagamentos([FromServices] PagamentoServices pagamentoServices)
    {
        var pagamentos = await pagamentoServices.GetAllPagamentosAsync();
        return Ok(pagamentos);
    }

    /// <summary>
    /// Obtém um pagamento pelo ID.
    /// </summary>
    /// <param name="pagamentoServices"></param>
    /// <param name="id">ID do pagamento a ser obtido.</param>
    /// <returns>O pagamento correspondente ao ID.</returns>
    /// <response code="200">Retorna o pagamento correspondente ao ID.</response>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(PagamentoResponseJson), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetPagamentoById([FromServices] PagamentoServices pagamentoServices, [FromRoute] int id)
    {
        var pagamento = await pagamentoServices.GetPagamentoByIdAsync(id);
        return Ok(pagamento);
    }

    /// <summary>
    /// Obtém os pagamentos de um aluno pelo ID do aluno.
    /// </summary>
    /// <param name="pagamentoServices"></param>
    /// <param name="alunoId">ID do aluno.</param>
    /// <returns>Lista de pagamentos do aluno.</returns>
    /// <response code="200">Retorna a lista de pagamentos do aluno correspondente ao ID.</response>
    [HttpGet("pagamentos-aluno/{alunoId}")]
    [ProducesResponseType(typeof(IEnumerable<PagamentoRequestJson>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetPagamentosByAluno([FromServices] PagamentoServices pagamentoServices, [FromRoute] int alunoId)
    {
        var pagamentos = await pagamentoServices.GetPagamentosByAlunoAsync(alunoId);
        return Ok(pagamentos);
    }

    /// <summary>
    /// Obtém os pagamentos em atraso.
    /// </summary>
    /// <returns>Lista de pagamentos em atraso.</returns>
    /// <response code="200">Retorna a lista de pagamentos em atraso.</response>
    [HttpGet("pagamentos-atrasados")]
    [ProducesResponseType(typeof(IEnumerable<PagamentoRequestJson>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetPagamentosEmAtraso([FromServices] PagamentoServices pagamentoServices)
    {
        var pagamentosAtraso = await pagamentoServices.GetPagamentosEmAtrasoAsync();
        return Ok(pagamentosAtraso);
    }

    /// <summary>
    /// Adiciona um novo pagamento para um aluno.
    /// </summary>
    /// <param name="pagamentoServices"></param>
    /// <param name="pagamentoRequest">Dados do novo pagamento.</param>
    /// <param name="alunoId">ID do aluno associado ao pagamento.</param>
    /// <returns>O pagamento recém-adicionado.</returns>
    /// <response code="201">Retorna o pagamento recém-adicionado.</response>
    [HttpPost("{alunoId}")]
    [ProducesResponseType(typeof(PagamentoResponseJson), StatusCodes.Status201Created)]
    public async Task<IActionResult> AddPagamento([FromServices] PagamentoServices pagamentoServices, [FromBody] PagamentoRequestJson pagamentoRequest, [FromRoute] int alunoId)
    {
        await pagamentoServices.AddPagamentoAsync(pagamentoRequest, alunoId);
        return Created(string.Empty, pagamentoRequest);
    }

    /// <summary>
    /// Atualiza um pagamento existente.
    /// </summary>
    /// <param name="pagamentoServices"></param>
    /// <param name="pagamentoId">ID do pagamento a ser atualizado.</param>
    /// <param name="pagamentoRequest">Novos dados do pagamento.</param>
    /// <returns>O pagamento atualizado.</returns>
    /// <response code="200">Retorna o pagamento atualizado.</response>
    [HttpPatch("{pagamentoId}")]
    [ProducesResponseType(typeof(PagamentoResponseJson), StatusCodes.Status200OK)]
    public async Task<IActionResult> UpdatePagamento([FromServices] PagamentoServices pagamentoServices, [FromBody] PagamentoRequestJson pagamentoRequest, [FromRoute] int pagamentoId)
    {
        await pagamentoServices.UpdatePagamentoAsync(pagamentoId, pagamentoRequest);
        return Ok(pagamentoRequest);
    }

    /// <summary>
    /// Deleta um pagamento existente.
    /// </summary>
    /// <param name="pagamentoServices"></param>
    /// <param name="pagamentoId">ID do pagamento a ser deletado.</param>
    /// <returns>NoContent se o pagamento foi deletado com sucesso.</returns>
    /// <response code="204">Retorna NoContent se o pagamento foi deletado com sucesso.</response>
    [HttpDelete("{pagamentoId}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> DeletePagamento([FromServices] PagamentoServices pagamentoServices, [FromRoute] int pagamentoId)
    {
        await pagamentoServices.DeletePagamentoAsync(pagamentoId);
        return NoContent();
    }
}

