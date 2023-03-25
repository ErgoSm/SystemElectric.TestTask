using SystemElectric.TestTask.Domain.Services;

namespace SystemElectric.TestProject.Tests
{
    public class DataProviderTests
    {
        private readonly DataProvider _dataProvider;

        public DataProviderTests()
        {
            _dataProvider = new DataProvider();
        }

        [Fact]
        public void Test_car_data_provided_Succeed()
        {
            for(int i = 0; i < 21; i++)
            {
                Assert.Equal(DataProvider.Cars[i % 10], _dataProvider.GetNextCar());
            }
        }

        [Fact]
        public void Test_car_data_provided_Failed()
        {
            Assert.NotEqual(DataProvider.Cars[1], _dataProvider.GetNextCar());
        }

        [Fact]
        public void Test_driver_data_provided_Succeed()
        {
            for (int i = 0; i < 21; i++)
            {
                Assert.Equal(DataProvider.Drivers[i % 10], _dataProvider.GetNextDriver());
            }
        }

        [Fact]
        public void Test_driver_data_provided_Failed()
        {
            Assert.NotEqual(DataProvider.Drivers[1], _dataProvider.GetNextDriver());
        }
    }
}