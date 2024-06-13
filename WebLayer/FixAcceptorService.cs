using QuickFix;

namespace SimulatorLD.WebLayer
{
    public class FixAcceptorService: IHostedService, IDisposable
    {
        private IAcceptor _acceptor;

        public FixAcceptorService()
        {
            SessionSettings settings = new SessionSettings("simpleacc.cfg");
            IApplication app = new SimpleAcceptorApp();
            IMessageStoreFactory storeFactory = new FileStoreFactory(settings);
            ILogFactory logFactory = new FileLogFactory(settings);
            _acceptor = new ThreadedSocketAcceptor(app, storeFactory, settings, logFactory);
        }

        public Task StartAsync(CancellationToken stoppingToken)
        {
            return Task.Factory.StartNew(() => _acceptor.Start(), stoppingToken);
        }

        public Task StopAsync(CancellationToken stoppingToken)
        {
            _acceptor.Stop();
            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _acceptor?.Dispose();
        }
    }
}
    

