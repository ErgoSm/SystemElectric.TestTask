using System.Collections.ObjectModel;
using System.Threading;
using SystemElectric.TestTask.Domain.Args;
using SystemElectric.TestTask.Domain.Entities;
using SystemElectric.TestTask.Domain.Repositories;

namespace SystemElectric.TestTask.Domain.Services
{
    public sealed class StorageCommandManager
    {
        private readonly IGenericRepository<CarEntry> _carsRepository;
        private readonly IGenericRepository<DriverEntry> _driversRepository;
        private readonly TimeProvider _timeProvider;

        private DateTime? _lastReadTimestamp;

        public StorageCommandManager(IGenericRepository<CarEntry> carsRepository, IGenericRepository<DriverEntry> driversRepository, TimeProvider timeProvider)
        {
            _carsRepository = carsRepository;
            _driversRepository = driversRepository;
            _timeProvider = timeProvider;
        }

        public Task Store(CarEntry entry, CancellationToken cancellationToken)
        {
            return _carsRepository.AddEntry(entry, cancellationToken);
        }

        public Task Store(DriverEntry entry, CancellationToken cancellationToken)
        {
            return _driversRepository.AddEntry(entry, cancellationToken);
        }

        /*public async Task<IEnumerable<CarDriverPair>> GetCarDriverPairs(CancellationToken cancellationToken)
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

            _lastReadTimestamp = carsDrivers.Values.Max(x => x.Timestamp);

            return carsDrivers.Values.OrderByDescending(x => x.Timestamp);
        }*/
    }
}
