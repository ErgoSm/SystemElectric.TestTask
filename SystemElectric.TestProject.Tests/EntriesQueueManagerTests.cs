using SystemElectric.TestTask.Domain.Entities;
using SystemElectric.TestTask.Domain.Services;

namespace SystemElectric.TestTask.Tests
{
    public class EntriesQueueManagerTests
    {
        private readonly EntriesQueueManager queue = new EntriesQueueManager();
        private readonly DateTime baseTimestamp = DateTime.Parse("2023-03-24 00:00:00");

        public EntriesQueueManagerTests()
        {
            queue.Enqueue(new CarDriverPair(baseTimestamp, "Вольво", "Пётр"));
        }

        [Fact]
        public void Contains_test_Sucseed()
        {
            Assert.True(queue.Contains(baseTimestamp));
        }

        [Fact]
        public void Not_contains_test_Sucseed()
        {
            Assert.False(queue.Contains(baseTimestamp.AddSeconds(1)));
        }

        [Fact]
        public void Enquque_test_Sucseed()
        {
            queue.Enqueue(new CarDriverPair(baseTimestamp.AddSeconds(1), "Вольво", "Пётр"));

            Assert.True(queue.Contains(baseTimestamp.AddSeconds(1)));
        }

        [Fact]
        public void Enquque_test_Failed()
        {
            Assert.Throws<InvalidOperationException>(() => queue.Enqueue(new CarDriverPair(baseTimestamp, "Вольво", "Пётр")));
        }

        [Fact]
        public void Update_test_Sucseed()
        {
            queue.Update(baseTimestamp, new CarDriverPair(baseTimestamp, "Мерседес", "Глеб"));

            var newEntity = queue.GetEntity(baseTimestamp);

            Assert.Equal("Мерседес", newEntity.Car);
            Assert.Equal("Глеб", newEntity.Driver);
        }

        [Fact]
        public void TryDequeue_test_Sucseed()
        {
            Assert.True(queue.TryDequeue(out var timestamp, out var entity));
            Assert.Equal(baseTimestamp, timestamp);
            Assert.Equal(new CarDriverPair(baseTimestamp, "Вольво", "Пётр"), entity);
        }

        [Fact]
        public void TryDequeue_test_Failed()
        {
            Assert.True(queue.TryDequeue(out var timestamp, out var entity));
            Assert.False(queue.TryDequeue(out timestamp, out entity));
        }

        [Fact]
        public void GetEntity_test_Sucseed()
        {
            var entity = queue.GetEntity(baseTimestamp);

            Assert.Equal("Вольво", entity.Car);
            Assert.Equal("Пётр", entity.Driver);
        }
    }
}
