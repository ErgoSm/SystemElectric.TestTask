using SystemElectric.TestTask.Domain.Args;
using SystemElectric.TestTask.Domain.Entities;

namespace SystemElectric.TestTask.Domain.Services
{
    public sealed class DataReader
    {
        public event EventHandler<EntryArgs>? OnCarRead;
        public event EventHandler<EntryArgs>? OnDriverRead;

        private readonly DataProvider _provider;
        private readonly TimeProvider _timeProvider;


        public DataReader(DataProvider provider, TimeProvider timeProvider)
        {
            _provider = provider;
            _timeProvider = timeProvider;
        }

        public void ReadCar()
        {
            var entry = new CarEntry { Timestamp = _timeProvider.Now, Name = _provider.GetNextCar() };

            OnCarRead?.Invoke(this, new EntryArgs(entry));
        }

        public void ReadDriver()
        {
            var entry = new DriverEntry { Timestamp = _timeProvider.Now, Name = _provider.GetNextDriver() };

            OnDriverRead?.Invoke(this, new EntryArgs(entry));
        }
    }
}
