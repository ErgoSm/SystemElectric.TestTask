namespace SystemElectric.TestTask.Domain.Services
{
    public sealed class ThreadWrapper
    {
        private readonly Thread _thread;

        public int Discreteness { get; init; }
        public bool IsEnabled { get; private set; } = false;

        public ThreadWrapper(Action action, ManualResetEvent resetEvent, int discreteness)
        {
            Discreteness = discreteness;

            _thread = new Thread(() =>
            {
                while (true)
                {
                    resetEvent.WaitOne();

                    action();

                    resetEvent.Reset();
                }
            });

            _thread.IsBackground = true;
        }

        public void Toggle()
        {
            if (_thread.ThreadState.HasFlag(ThreadState.Unstarted))
            {
                _thread.Start();
            }

            //Маркер, используемый для приостановки потока (в состоянии false Timer не переводит ManualResetEvent в сигнальное состояние)
            IsEnabled = !IsEnabled;
        }
    }
}
