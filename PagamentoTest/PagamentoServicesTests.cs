using AutoMapper;
using Moq;
using NovaFormaProject.Application.Dtos;
using NovaFormaProject.Application.Services;
using NovaFormaProject.Domain.DatabaseEntities;
using NovaFormaProject.Domain.DatabaseEntities.Enums;
using NovaFormaProject.Domain.Interfaces.EntitiesRepositoryInterface;

namespace PagamentoTest;

public class PagamentoServicesTests
{
    [Fact]
    public async Task AddPagamentoAsync_InvalidAluno_ThrowsException()
    {
        // Arrange
        var alunoId = 3;
        var unitOfWorkMock = new Mock<IUnitOfWork>();
        var alunoRepositoryMock = new Mock<IAlunoRepository>();
        var pagamentoRepositoryMock = new Mock<IPagamentoRepository>();
        var mapperMock = new Mock<IMapper>();

        unitOfWorkMock.Setup(u => u.AlunoRepository).Returns(alunoRepositoryMock.Object);

        var pagamentoDto = new PagamentoDto { /* Preencha os detalhes do pagamento DTO aqui */ };
        var aluno = new Aluno { Status = AlunoStatus.Inativo };

        alunoRepositoryMock.Setup(r => r.GetByIdAsync(alunoId)).ReturnsAsync(aluno);

        var pagamentoServices = new PagamentoServices(unitOfWorkMock.Object, mapperMock.Object);

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() => pagamentoServices.AddPagamentoAsync(pagamentoDto, alunoId));
    }
}
