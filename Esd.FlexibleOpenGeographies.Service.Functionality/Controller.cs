using System.Threading;

namespace Esd.FlexibleOpenGeographies.Service.Functionality
{
    public class Controller
    {
        private Thread _worker;
        private readonly AutoResetEvent _stop = new AutoResetEvent(false);

        public void Start()
        {
            
            _worker = new Thread(DoWork);
            _worker.Start();
        }

        private void DoWork()
        {
            var factory = new ContextFactory();
            var queryFactory = new QueryFactory(factory);
            var workFactory = new UnitOfWorkFactory(factory, null, null);

            while (true)
            {
                if (_stop.WaitOne(60000)) return;

                UploadProcessor.Process(queryFactory.CreateUploadBasicSingleQuery().Find(), workFactory, queryFactory);
            }
        }

        public void Stop()
        {
            _stop.Set();
            _worker.Join();
        }
    }
}
