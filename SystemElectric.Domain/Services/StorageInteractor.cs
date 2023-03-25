using System.Collections.ObjectModel;
using SystemElectric.TestTask.Domain.Args;
using SystemElectric.TestTask.Domain.Entities;
using SystemElectric.TestTask.Domain.Repositories;

namespace SystemElectric.TestTask.Domain.Services
{
    public sealed class StorageInteractor
    {
        public event EventHandler<TEntityArgs<CarDriverPair>>? OnAddedEntry;

        private readonly EntriesQueueManager _queueManager;
        private readonly IGenericRepository<CarEntry> _carsRepository;
        private readonly IGenericRepository<DriverEntry> _driversRepository;

        public StorageInteractor(EntriesQueueManager queueManager, IGenericRepository<CarEntry> carsRepository, IGenericRepository<DriverEntry> driversRepository)
        {
            _queueManager = queueManager;
            _carsRepository = carsRepository;
            _driversRepository = driversRepository;

            _carsRepository.OnAddedEntry += async (object? sender, TEntityArgs<CarEntry> e) =>
            {
                var isEnqued = false;
                lock (_queueManager)
                {
                    if (!_queueManager.Contains(e.Entity.Timestamp))
                    {
                        isEnqued = true;
                        _queueManager.Enqueue(new CarDriverPair(e.Entity.Timestamp, e.Entity.Name, ""));
                    }
                    else
                    {
                        _queueManager.Update(e.Entity.Timestamp, _queueManager.GetEntity(e.Entity.Timestamp) with { Car = e.Entity.Name });
                    }
                }

                if (isEnqued)
                {
                    await WaitAndNotify();
                }
            };

            _driversRepository.OnAddedEntry += async (object? sender, TEntityArgs<DriverEntry> e) =>
            {
                var isEnqued = false;
                lock (_queueManager)
                {
                    if (!_queueManager.Contains(e.Entity.Timestamp))
                    {
                        isEnqued = true;
                        _queueManager.Enqueue(new CarDriverPair(e.Entity.Timestamp, "", e.Entity.Name));
                    }
                    else
                    {
                        _queueManager.Update(e.Entity.Timestamp, _queueManager.GetEntity(e.Entity.Timestamp) with { Driver = e.Entity.Name });
                    }
                }

                if (isEnqued)
                {
                    await WaitAndNotify();
                }
            };
        }

        //Метод выполняет ожидание второй из двух записей с одинаковой меткой времени для объединения перед отправкой
        public async Task WaitAndNotify()
        {
            await Task.Delay(ParametersHelper.CombinationWaitingDelay);

            lock (_queueManager)
            {
                if (_queueManager.TryDequeue(out var timestamp, out var entity))
                {
                    OnAddedEntry?.Invoke(this, new TEntityArgs<CarDriverPair>(entity));
                }
            }
        }

        public Task Store(CarEntry entry, CancellationToken cancellationToken)
        {
            return _carsRepository.AddEntry(entry, cancellationToken);
        }

        public Task Store(DriverEntry entry, CancellationToken cancellationToken)
        {
            return _driversRepository.AddEntry(entry, cancellationToken);
        }

        public async Task<IEnumerable<CarDriverPair>> GetCarDriverPairs(CancellationToken cancellationToken)
        {
            var carsDrivers = (await _carsRepository.GetEntries(_ => true, cancellationToken)).ToDictionary(x => x.Timestamp, x => new CarDriverPair(x.Timestamp, x.Name, ""));

            var drivers = await _driversRepository.GetEntries(_ => true, cancellationToken);

            foreach (var driver in drivers)
            {
                if (!carsDrivers.ContainsKey(driver.Timestamp))
                {
                    carsDrivers.Add(driver.Timestamp, new CarDriverPair(driver.Timestamp, "", driver.Name));
                }
                else
                {
                    carsDrivers[driver.Timestamp] = carsDrivers[driver.Timestamp] with { Driver = driver.Name };
                }
            }

            return carsDrivers.Values.OrderByDescending(x => x.Timestamp);
        }
    }
}
