namespace SystemElectric.TestTask.Domain.Services
{
    public sealed class ThreadWrapper
    {
        private Thread _thread;
        private readonly ManualResetEvent _resetEvent;
        private readonly Action _action;

        private CancellationTokenSource _cancellationTokenSource;

        public int Discreteness { get; init; }
        public bool IsEnabled { get; private set; } = false;

        public ThreadWrapper(Action action, ManualResetEvent resetEvent, int discreteness)
        {
            Discreteness = discreteness;
            _resetEvent = resetEvent;
            _action = action;

            Initialize();
        }

        private void Initialize()
        {
            _cancellationTokenSource = new CancellationTokenSource();

            _thread = new Thread(() =>
            {
                try
                {
                    while (true)
                    {
                        _resetEvent.WaitOne();

                        _cancellationTokenSource.Token.ThrowIfCancellationRequested();

                        _action();

                        _resetEvent.Reset();
                    }
                }
                catch (Exception ex)
                {
                }
            });

            _thread.IsBackground = true;
        }

        public void Start()
        {
            Initialize();

            _thread.Start();
            IsEnabled = true;
        }

        public void Stop()
        {
            _cancellationTokenSource.Cancel();
            _resetEvent.Set();
            IsEnabled = false;
        }


        public void Toggle()
        {
            if (!IsEnabled)
            {
                Start();
            }
            else
            {
                Stop();
            }
        }
    }
}
