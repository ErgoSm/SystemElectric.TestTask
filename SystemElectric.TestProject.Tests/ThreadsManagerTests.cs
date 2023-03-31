using Microsoft.Extensions.Logging;
using Moq;
using System.Reflection.Metadata.Ecma335;
using SystemElectric.TestTask.Domain.Services;

namespace SystemElectric.TestTask.Tests
{
    public class ThreadsManagerTests
    {
        private readonly ThreadsManager _threadsManager;
        private readonly DataReader _reader;

        public ThreadsManagerTests()
        {
            var timeProvider = new DefaultTimeProvider();

            _reader = new DataReader(new DataProvider(), new DefaultTimeProvider());
            var logger = new Mock<ILogger<ThreadsManager>>();

            _threadsManager = new ThreadsManager(_reader, timeProvider, logger.Object);
        }

        [Theory]
        [InlineData(4000)]
        [InlineData(6000)]
        public async void Test_number_of_generated_entities(int executionTimeInMs)
        {
            _threadsManager.Toggle(1, null, null); 
            _threadsManager.Toggle(2, null, null);

            int carsCounter = 0;
            int driversCounter = 0;

            _reader.OnCarRead += (sender, e) => carsCounter++;
            _reader.OnDriverRead += (sender, e) => driversCounter++;

            await Task.Delay(executionTimeInMs);


            Assert.Equal(executionTimeInMs / ParametersHelper.FirstThreadDiscreteness / 1000, carsCounter);
            Assert.Equal(executionTimeInMs / ParametersHelper.SecondThreadDiscreteness / 1000, driversCounter);
        }
    }
}
