namespace SystemElectric.TestTask.Domain.Services
{
    public abstract class TimeProvider
    {
        public abstract DateTime Now { get; }

        public abstract void Update();
    }
}
