using SystemElectric.TestTask.Domain.Entities;

namespace SystemElectric.TestTask.Domain.Services
{
    //Класс, инкапсулирующий логику очереди записей <машина-водитель> 
    public sealed class EntriesQueueManager
    {
        private Dictionary<DateTime, CarDriverPair> _entriesDictionary = new Dictionary<DateTime, CarDriverPair>();
        private Queue<DateTime> _entriesTimestampQueue = new Queue<DateTime>();

        public bool Contains(DateTime date)
        {
            return _entriesDictionary.ContainsKey(date);
        }

        public void Enqueue(CarDriverPair entry)
        {
            if (Contains(entry.Timestamp))
            {
                throw new InvalidOperationException("The queue already contains entry with specified timestamp!");
            }

            _entriesTimestampQueue.Enqueue(entry.Timestamp);
            _entriesDictionary.Add(entry.Timestamp, entry);
        }

        public void Update(DateTime timestamp, CarDriverPair entity)
        {
            if(Contains(timestamp))
            {
                _entriesDictionary[timestamp] = entity;
            }
        }

        public bool TryDequeue(out DateTime timestamp, out CarDriverPair? entry)
        {
            var result = false;
            entry = null;

            if (result = _entriesTimestampQueue.TryDequeue(out timestamp))
            {
                _entriesDictionary.TryGetValue(timestamp, out entry);
                _entriesDictionary.Remove(timestamp);
            }

            return result;
        }

        public CarDriverPair? GetEntity(DateTime timestamp)
        {
            if(!Contains(timestamp))
            {
                return null;
            }

            return _entriesDictionary[timestamp];
        }
    }
}
