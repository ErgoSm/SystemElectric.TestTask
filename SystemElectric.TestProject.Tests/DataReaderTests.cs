using SystemElectric.TestTask.Domain.Args;
using SystemElectric.TestTask.Domain.Services;

namespace SystemElectric.TestTask.Tests
{
    public class DataReaderTests
    {
        private readonly DataReader dataReader;

        public DataReaderTests()
        {
            dataReader = new DataReader(new DataProvider(), new DefaultTimeProvider());
        }

        [Fact]
        public void ReadCars_Test_Sucseed()
        {
            var ev = Assert.Raises<EntryArgs>(
                h => dataReader.OnCarRead += h,
                h => dataReader.OnCarRead -= h,
                () => dataReader.ReadCar());


            Assert.NotNull(ev);
            Assert.Equal(dataReader, ev.Sender);
            Assert.NotNull(ev.Arguments);
        }

        [Fact]
        public void ReadDrivers_Test_Sucseed()
        {
            var ev = Assert.Raises<EntryArgs>(
                h => dataReader.OnDriverRead += h,
                h => dataReader.OnDriverRead -= h,
                () => dataReader.ReadDriver());


            Assert.NotNull(ev);
            Assert.Equal(dataReader, ev.Sender);
            Assert.NotNull(ev.Arguments);
        }
    }
}
