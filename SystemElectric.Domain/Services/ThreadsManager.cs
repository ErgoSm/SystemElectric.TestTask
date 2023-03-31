using Microsoft.Extensions.Logging;
using SystemElectric.TestTask.Domain.Args;
using SystemElectric.TestTask.Domain.Entities;

namespace SystemElectric.TestTask.Domain.Services
{
    public sealed class ThreadsManager
    {
        public event EventHandler<ReadArgs>? OnSimultaneousRead;
        public event EventHandler<EventArgs>? OnTimerTick;

        private readonly ManualResetEvent firstResetEvent = new ManualResetEvent(false);
        private readonly ManualResetEvent secondResetEvent = new ManualResetEvent(false);

        private ThreadWrapper firstThread;
        private ThreadWrapper secondThread;

        private readonly Timer timer;

        private readonly DataReader _dataReader;

        private int counter = 0;

        private CarEntry? _lastCarEntry;
        private DriverEntry? _lastDriverEntry;

        private readonly TimeProvider _timeProvider;
        private readonly ILogger<ThreadsManager> _logger;

        public ThreadsManager(DataReader dataReader, TimeProvider timeProvider, ILogger<ThreadsManager> logger)
        {
            _dataReader = dataReader;
            _timeProvider = timeProvider;
            _logger = logger;
            timer = new Timer(Handle, null, 0, 1000);

            firstThread = new ThreadWrapper(_dataReader.ReadCar, firstResetEvent, ParametersHelper.FirstThreadDiscreteness);
            secondThread = new ThreadWrapper(_dataReader.ReadDriver, secondResetEvent, ParametersHelper.SecondThreadDiscreteness);

            dataReader.OnDriverRead += DataReader_OnRead;
            dataReader.OnCarRead += DataReader_OnRead;
        }

        private void DataReader_OnRead(object? sender, EntryArgs e)
        {
            if(e.Entry is CarEntry)
            {
                _lastCarEntry = (CarEntry)e.Entry;
            }
            else if(e.Entry is DriverEntry)
            {
                _lastDriverEntry = (DriverEntry)e.Entry;
            }

            //Только при совпадении меток инициируется добавление в список основного окна
            if(_lastDriverEntry != null && _lastDriverEntry?.Timestamp == _lastCarEntry?.Timestamp)
            {
                OnSimultaneousRead?.Invoke(this, new ReadArgs(new CarDriverPair(_lastCarEntry.Timestamp, _lastCarEntry.Name, _lastDriverEntry.Name)));
            }
        }

        private void Handle(object? state)
        {
            OnTimerTick?.Invoke(this, new EventArgs());

            counter++;

            _timeProvider.Update();

            if(firstThread.IsEnabled && counter % firstThread.Discreteness == 0)
            {
                firstResetEvent.Set();
            }

            if (secondThread.IsEnabled && counter % secondThread.Discreteness == 0)
            {
                secondResetEvent.Set();
            }

            //Сброс счётчика (чтобы не ломал дискретность при переполнении типа)
            if(counter == firstThread.Discreteness * secondThread.Discreteness)
            {
                counter = 0;
            }
        }

        public void Toggle(int number, Action activation, Action deactivation)
        {
            string status;

            if (number == 1)
            {
                firstThread.Toggle();

                if (firstThread.IsEnabled)
                {
                    activation();
                    status = "enabled";
                }
                else
                {
                    deactivation();
                    status = "enabled";
                }

                _logger.LogInformation($"Thread 1 is {status}");
            }
            else if(number == 2)
            {
                secondThread.Toggle();

                if (secondThread.IsEnabled)
                {
                    activation();
                    status = "enabled";
                }
                else
                {
                    deactivation();
                    status = "enabled";
                }

                _logger.LogInformation($"Thread 2 is {status}");
            }
        }
    }
}
