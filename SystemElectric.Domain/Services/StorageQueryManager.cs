using Microsoft.Extensions.DependencyInjection;
using SystemElectric.TestTask.Domain.Args;
using SystemElectric.TestTask.Domain.Entities;
using SystemElectric.TestTask.Domain.Repositories;


namespace SystemElectric.TestTask.Domain.Services
{
    public sealed class StorageQueryManager
    {
        public event EventHandler<TEntityArgs<IEnumerable<CarDriverPair>>>? OnAddedEntry;

        private readonly IServiceProvider _serviceProvider;
        private DateTime? _lastReadTimestamp;

        public StorageQueryManager(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async void GetEntries(object? sender, EventArgs e)
        {
            var carsDrivers = new Dictionary<DateTime, CarDriverPair>();

            await Task.Delay(ParametersHelper.CombinationWaitingDelay);

            using (var carsRepository = _serviceProvider.GetService<IGenericRepository<CarEntry>>())
            {
                var cars = await carsRepository.GetEntries(x => _lastReadTimestamp == null || x.Timestamp > _lastReadTimestamp, CancellationToken.None);

                foreach (var car in cars)
                {
                    if (!carsDrivers.ContainsKey(car.Timestamp))
                    {
                        carsDrivers.Add(car.Timestamp, new CarDriverPair(car.Timestamp, car.Name, ""));
                    }
                    else
                    {
                        carsDrivers[car.Timestamp] = carsDrivers[car.Timestamp] with { Car = car.Name };
                    }
                }
            }

            using (var driversRepository = _serviceProvider.GetService<IGenericRepository<DriverEntry>>())
            {
                var drivers = await driversRepository.GetEntries(x => _lastReadTimestamp == null || x.Timestamp > _lastReadTimestamp, CancellationToken.None);

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
            }

            if (carsDrivers.Any())
            {
                _lastReadTimestamp = carsDrivers.Values.Max(x => x.Timestamp);
            }
                

            OnAddedEntry?.Invoke(this, new TEntityArgs<IEnumerable<CarDriverPair>>(carsDrivers.Values.OrderBy(x => x.Timestamp)));
        }
    }
}
