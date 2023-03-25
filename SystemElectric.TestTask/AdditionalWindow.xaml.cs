using Microsoft.Extensions.Logging;
using System.Collections.ObjectModel;
using System.Threading;
using System.Windows;
using SystemElectric.TestTask.Domain.Args;
using SystemElectric.TestTask.Domain.Entities;
using SystemElectric.TestTask.Domain.Services;

namespace SystemElectric.TestTask
{
    /// <summary>
    /// Логика взаимодействия для AdditionalWindow.xaml
    /// </summary>
    public partial class AdditionalWindow : Window
    {
        private ObservableCollection<CarDriverPair> CarDrivers { get; set; } = new ObservableCollection<CarDriverPair>();

        private readonly StorageInteractor _dbPresenter;
        private readonly ILogger<AdditionalWindow> _logger;

        public AdditionalWindow(StorageInteractor dbPresenter, ILogger<AdditionalWindow> logger)
        {
            InitializeComponent();
            _dbPresenter = dbPresenter;
            _logger = logger;

            _dbPresenter.OnAddedEntry += (object? sender, TEntityArgs<CarDriverPair> e) =>
            {
                this.Dispatcher.Invoke(() =>
                {
                    CarDrivers.Insert(0, e.Entity);
                });
            };

            var pairs = _dbPresenter.GetCarDriverPairs(CancellationToken.None).GetAwaiter().GetResult();

            CarDrivers = new ObservableCollection<CarDriverPair>(pairs);
            
            lstTable.ItemsSource = CarDrivers;
        }

        private void AdditionalWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = true;
            this.Visibility = Visibility.Hidden;

            _logger.LogInformation("Additional window has closed");
        }
    }
}
