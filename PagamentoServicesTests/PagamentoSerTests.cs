using AutoMapper;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using NovaFormaProject.Application.Dtos;
using NovaFormaProject.Application.Services;
using NovaFormaProject.Domain.DatabaseEntities;
using NovaFormaProject.Domain.DatabaseEntities.Enums;
using NovaFormaProject.Domain.Interfaces.EntitiesRepositoryInterface;
using System.Linq.Expressions;


namespace PagamentoServicesTests;
public class PagamentoSerTests
{
    [Fact]
    public async Task AddPagamentoAsync_ShouldThrowException_WhenAlunoNotFound()
    {
        // Arrange
        var unitOfWorkMock = new Mock<IUnitOfWork>();
        var mapperMock = new Mock<IMapper>();
        var pagamentoServices = new PagamentoServices(unitOfWorkMock.Object, mapperMock.Object);
        var pagamentoDto = new PagamentoDto { AlunoID = 1 };

        unitOfWorkMock.Setup(u => u.AlunoRepository.GetByIdAsync(It.IsAny<int>())).ReturnsAsync((Aluno)null);

        // Act
        Func<Task> act = async () => await pagamentoServices.AddPagamentoAsync(pagamentoDto, 1);

        // Assert
        await act.Should().ThrowAsync<InvalidOperationException>().WithMessage("O pagamento só pode ser feito para alunos ativos.");
    }

    [Fact]
    public async Task AddPagamentoAsync_ShouldAddPagamentoWithStatusPago_WhenPaymentDateIsProvided()
    {
        // Arrange
        var aluno = new Aluno { ID = 1, Status = AlunoStatus.Ativo, Pagamentos = new List<Pagamento>() };
        var unitOfWorkMock = new Mock<IUnitOfWork>();
        var mapperMock = new Mock<IMapper>();
        var pagamentoServices = new PagamentoServices(unitOfWorkMock.Object, mapperMock.Object);
        var pagamentoDto = new PagamentoDto { AlunoID = 1, PaymentDate = DateTime.UtcNow };

        unitOfWorkMock.Setup(u => u.AlunoRepository.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(aluno);
        unitOfWorkMock.Setup(u => u.PagamentoRepository.AddAsync(It.IsAny<Pagamento>())).Callback((Pagamento p) => aluno.Pagamentos.Add(p));
        unitOfWorkMock.Setup(u => u.Commit());

        // Act
        await pagamentoServices.AddPagamentoAsync(pagamentoDto, 1);

        // Assert
        unitOfWorkMock.Verify(u => u.PagamentoRepository.AddAsync(It.Is<Pagamento>(p => p.PagamentoStatus == PagamentoStatus.Pago)), Times.Once);
        unitOfWorkMock.Verify(u => u.Commit(), Times.Once);
        Assert.Single(aluno.Pagamentos); // Verifica se o pagamento foi adicionado à lista de pagamentos do aluno
    }



    [Fact]
    public async Task UpdatePagamentoAsync_ShouldUpdateStatusToPago_WhenPaymentDateIsProvided()
    {
        // Arrange
        var existingPagamento = new Pagamento { ID = 1, Aluno = new Aluno { Status = AlunoStatus.Ativo } };
        var unitOfWorkMock = new Mock<IUnitOfWork>();
        var mapperMock = new Mock<IMapper>();
        var pagamentoServices = new PagamentoServices(unitOfWorkMock.Object, mapperMock.Object);
        var pagamentoDto = new PagamentoDto { ID = 1, PaymentDate = DateTime.UtcNow };

        unitOfWorkMock.Setup(u => u.PagamentoRepository.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(existingPagamento);
        unitOfWorkMock.Setup(u => u.PagamentoRepository.UpdateAsync(It.IsAny<Pagamento>())).Callback((Pagamento p) => existingPagamento.PagamentoStatus = p.PagamentoStatus);
        unitOfWorkMock.Setup(u => u.Commit());

        // Act
        await pagamentoServices.UpdatePagamentoAsync(1, pagamentoDto);

        // Assert
        unitOfWorkMock.Verify(u => u.PagamentoRepository.UpdateAsync(It.Is<Pagamento>(p => p.PagamentoStatus == PagamentoStatus.Pago)), Times.Once);
        unitOfWorkMock.Verify(u => u.Commit(), Times.Once);
    }


    [Fact]
    public async Task UpdateStatusPagamentosAtrasadosAsyncShouldUpdateStatusToAtrasadoWhenDueDateIsPastAndNotPaid()
    {
        // Arrange
        var unitOfWorkMock = new Mock<IUnitOfWork>();
        var mapperMock = new Mock<IMapper>();
        var pagamentoServices = new PagamentoServices(unitOfWorkMock.Object, mapperMock.Object);

        var pagamento1 = new Pagamento { ID = 1, DueDate = DateTime.UtcNow.AddDays(-1), PagamentoStatus = PagamentoStatus.Pendente };
        var pagamento2 = new Pagamento { ID = 2, DueDate = DateTime.UtcNow.AddDays(-2), PagamentoStatus = PagamentoStatus.Pendente };
        var pagamentosAtrasados = new List<Pagamento> { pagamento1, pagamento2 };

        unitOfWorkMock.Setup(u => u.PagamentoRepository.FindAsync(It.IsAny<Expression<Func<Pagamento, bool>>>())).ReturnsAsync(pagamentosAtrasados);
        unitOfWorkMock.Setup(u => u.PagamentoRepository.UpdateAsync(It.IsAny<Pagamento>()));
        unitOfWorkMock.Setup(u => u.Commit());

        // Act
        await pagamentoServices.UpdateStatusPagamentosAtrasadosAsync();

        // Assert
        unitOfWorkMock.Verify(u => u.PagamentoRepository.UpdateAsync(It.Is<Pagamento>(p => p.PagamentoStatus == PagamentoStatus.Atrasado)), Times.Exactly(2));
        unitOfWorkMock.Verify(u => u.Commit(), Times.Once);
    }

    [Fact]
    public void PagamentoUpdateServiceShouldStartAndStopWithoutException()
    {
        // Arrange
        var services = new ServiceCollection();
        services.AddTransient<PagamentoServices>();
        services.AddSingleton<IUnitOfWork>(new Mock<IUnitOfWork>().Object);

        var serviceProvider = services.BuildServiceProvider();

        using (var service = new PagamentoUpdateService(serviceProvider))
        {
            // Act
            Func<Task> startAsync = async () => await service.StartAsync(default);
            Func<Task> stopAsync = async () => await service.StopAsync(default);

            // Assert
            startAsync.Should().NotThrowAsync();
            stopAsync.Should().NotThrowAsync();
        }
    }
    [Fact]
    public async Task UpdateStatusPagamentosAtrasadosAsync_ShouldUpdateStatusToAtrasado_WhenDueDateIsPastAndNotPaid()
    {
        // Arrange
        var unitOfWorkMock = new Mock<IUnitOfWork>();
        var mapperMock = new Mock<IMapper>();
        var pagamentoServices = new PagamentoServices(unitOfWorkMock.Object, mapperMock.Object);

        var pagamento1 = new Pagamento { ID = 1, DueDate = DateTime.UtcNow.AddDays(-1), PagamentoStatus = PagamentoStatus.Pendente };
        var pagamento2 = new Pagamento { ID = 2, DueDate = DateTime.UtcNow.AddDays(-2), PagamentoStatus = PagamentoStatus.Pendente };
        var pagamentosAtrasados = new List<Pagamento> { pagamento1, pagamento2 };

        unitOfWorkMock.Setup(u => u.PagamentoRepository.FindAsync(It.IsAny<Expression<Func<Pagamento, bool>>>())).ReturnsAsync(pagamentosAtrasados);
        unitOfWorkMock.Setup(u => u.PagamentoRepository.UpdateAsync(It.IsAny<Pagamento>()));
        unitOfWorkMock.Setup(u => u.Commit());

        // Act
        await pagamentoServices.UpdateStatusPagamentosAtrasadosAsync();

        // Assert
        unitOfWorkMock.Verify(u => u.PagamentoRepository.UpdateAsync(It.Is<Pagamento>(p => p.PagamentoStatus == PagamentoStatus.Atrasado)), Times.Exactly(2));
        unitOfWorkMock.Verify(u => u.Commit(), Times.Once);
    }

    [Fact]
    public void PagamentoUpdateService_ShouldStartAndStopWithoutException()
    {
        // Arrange
        var services = new ServiceCollection();
        services.AddTransient<PagamentoServices>();
        services.AddSingleton<IUnitOfWork>(new Mock<IUnitOfWork>().Object);

        var serviceProvider = services.BuildServiceProvider();

        using (var service = new PagamentoUpdateService(serviceProvider))
        {
            // Act
            Func<Task> startAsync = async () => await service.StartAsync(default);
            Func<Task> stopAsync = async () => await service.StopAsync(default);

            // Assert
            startAsync.Should().NotThrowAsync();
            stopAsync.Should().NotThrowAsync();
        }
    }
}
