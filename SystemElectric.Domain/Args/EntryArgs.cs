using SystemElectric.TestTask.Domain.Entities;

namespace SystemElectric.TestTask.Domain.Args
{
    public class EntryArgs : EventArgs
    {
        public Entry Entry { get; init; }

        public EntryArgs(Entry entry)
        {
            Entry = entry;
        }
    }
}