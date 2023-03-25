using SystemElectric.TestTask.Domain.Services;

namespace SystemElectric.TestTask.Tests
{
    public class ThreadWripperTests
    {
        private readonly ThreadWrapper wripper = new ThreadWrapper(() => { return; },
            new ManualResetEvent(false), 1);

        [Fact]
        public void Toggle_first_call_Succeed()
        {
            Assert.False(wripper.IsEnabled);

            wripper.Toggle();
            Assert.True(wripper.IsEnabled);

            wripper.Toggle();
            Assert.False(wripper.IsEnabled);
            
            wripper.Toggle();
            Assert.True(wripper.IsEnabled);
        }
    }
}
