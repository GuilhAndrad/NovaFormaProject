using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace NovaFormaProject.Application.Services.PagamentoUseCase
{
    public class PagamentoUpdateService : IHostedService, IDisposable
    {
        private readonly IServiceProvider _services;
        private Timer _timer;

        public PagamentoUpdateService(IServiceProvider services)
        {
            _services = services;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            // Iniciar o serviço imediatamente e repetir a cada 24 horas (ou qualquer intervalo desejado)
            _timer = new Timer(DoWork, null, TimeSpan.Zero, TimeSpan.FromMinutes(2));// No caso ele tá repetindo a cada 2 min. Só alterar pro valor que quiser

            return Task.CompletedTask;
        }

        private void DoWork(object state)
        {
            using var scope = _services.CreateScope();
            var pagamentoServices = scope.ServiceProvider.GetRequiredService<PagamentoServices>();
            pagamentoServices.UpdateStatusPagamentosAtrasadosAsync().Wait(); // Aguarde a conclusão para evitar exceções
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _timer?.Change(Timeout.Infinite, 0);
            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer?.Dispose();
            GC.SuppressFinalize(this);
        }

    }
}
