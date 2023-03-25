namespace SystemElectric.TestTask.Domain.Services
{
    public sealed class DataProvider
    {
        public static readonly string[] Cars = new string[] { "Мондео", "Крета", "Приус", "УАЗик", "Вольво", "Фокус", "Октавия", "Запорожец", "Ваз2102", "ПассатСС" };
        public static readonly string[] Drivers = new string[] { "Петр", "Василий", "Николай", "Марина", "Феодосий", "Карина", "Алевтина", "Карп", "Никанор", "Антип" };

        private byte _carCounter = 0;
        private byte _driverCounter = 0;

        public string GetNextCar()
        {
            if(++_carCounter > Cars.Length)
            {
                _carCounter = 1;
            }

            return Cars[_carCounter - 1];
        }

        public string GetNextDriver()
        {
            if (++_driverCounter > Drivers.Length)
            {
                _driverCounter = 1;
            }

            return Drivers[_driverCounter - 1];
        }
    }
}
