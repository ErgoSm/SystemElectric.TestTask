using SystemElectric.TestTask.Domain.Entities;
using SystemElectric.TestTask.Domain.Services;

namespace SystemElectric.TestTask.Domain.Args
{
    public class ReadArgs : EventArgs
    {
        public CarDriverPair Pair { get; init; }

        public ReadArgs(CarDriverPair pair)
        {
            Pair = pair;
        }

    }
}
