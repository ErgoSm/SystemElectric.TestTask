using Moq;
using SystemElectric.TestTask.Domain.Args;
using SystemElectric.TestTask.Domain.Entities;
using SystemElectric.TestTask.Domain.Repositories;
using SystemElectric.TestTask.Domain.Services;

namespace SystemElectric.TestTask.Tests
{
    public class StorageInteractorTests
    {
        private readonly StorageInteractor interactor;
        private readonly Mock<IGenericRepository<CarEntry>> carsRepoMock;
        private readonly Mock<IGenericRepository<DriverEntry>> driverRepoMock;

        private readonly DateTime baseTimestamp = DateTime.Parse("2023-03-24 00:00:00");

        public StorageInteractorTests()
        {
            carsRepoMock = new Mock<IGenericRepository<CarEntry>>();
            driverRepoMock = new Mock<IGenericRepository<DriverEntry>>();

            interactor = new StorageInteractor(new EntriesQueueManager(),
                carsRepoMock.Object, driverRepoMock.Object);
        }

        [Fact]
        public void OnCarAdded_Test_Succeed()
        {
            var car = new CarEntry { Timestamp = baseTimestamp, Name = "Газ3102" };
            carsRepoMock.Setup(x => x.AddEntry(car, CancellationToken.None))
                .Returns(async () => { return; })
                .Raises(m => m.OnAddedEntry += null, new TEntityArgs<CarEntry>(car));

            var ev = Assert.Raises<TEntityArgs<CarDriverPair>>(
                h => interactor.OnAddedEntry += h,
                h => interactor.OnAddedEntry -= h,
                () => Execute<CarEntry>(carsRepoMock.Object, car, 300));

            Assert.NotNull(ev);
            Assert.Equal(interactor, ev.Sender);
            Assert.Equal(baseTimestamp, ev.Arguments.Entity.Timestamp);
            Assert.Equal("Газ3102", ev.Arguments.Entity.Car);
            Assert.Equal("", ev.Arguments.Entity.Driver);
        }

        [Fact]
        public void OnDriverAdded_Test_Succeed()
        {
            var driver = new DriverEntry { Timestamp = baseTimestamp, Name = "Яков" };
            driverRepoMock.Setup(x => x.AddEntry(driver, CancellationToken.None))
                .Returns(async () => { return; })
                .Raises(m => m.OnAddedEntry += null, new TEntityArgs<DriverEntry>(driver));

            var ev = Assert.Raises<TEntityArgs<CarDriverPair>>(
                h => interactor.OnAddedEntry += h,
                h => interactor.OnAddedEntry -= h,
                () => Execute<DriverEntry>(driverRepoMock.Object, driver, 300));


            Assert.NotNull(ev);
            Assert.Equal(interactor, ev.Sender);
            Assert.Equal(baseTimestamp, ev.Arguments.Entity.Timestamp);
            Assert.Equal("Яков", ev.Arguments.Entity.Driver);
            Assert.Equal("", ev.Arguments.Entity.Car);
        }

        [Fact]
        public void OnSimultaneouslyAdded_Test_Succeed()
        {
            var car = new CarEntry { Timestamp = baseTimestamp, Name = "Газ3102" };
            carsRepoMock.Setup(x => x.AddEntry(car, CancellationToken.None))
                .Returns(async () => { return; })
                .Raises(m => m.OnAddedEntry += null, new TEntityArgs<CarEntry>(car));

            var driver = new DriverEntry { Timestamp = baseTimestamp, Name = "Яков" };
            driverRepoMock.Setup(x => x.AddEntry(driver, CancellationToken.None))
                .Returns(async () => { return; })
                .Raises(m => m.OnAddedEntry += null, new TEntityArgs<DriverEntry>(driver));

            var ev = Assert.Raises<TEntityArgs<CarDriverPair>>(
                h => interactor.OnAddedEntry += h,
                h => interactor.OnAddedEntry -= h,
                () => {
                    Execute<CarEntry>(carsRepoMock.Object, car, 10);
                    Execute<DriverEntry>(driverRepoMock.Object, driver, 300);
                });


            Assert.NotNull(ev);
            Assert.Equal(interactor, ev.Sender);
            Assert.Equal(baseTimestamp, ev.Arguments.Entity.Timestamp);
            Assert.Equal("Яков", ev.Arguments.Entity.Driver);
            Assert.Equal("Газ3102", ev.Arguments.Entity.Car);
        }

        private void Execute<T>(IGenericRepository<T> repo, T entry, int delay) where T : class
        {
            repo.AddEntry(entry, CancellationToken.None).GetAwaiter().GetResult();
            Task.Delay(delay).Wait();
        }
    }
}
