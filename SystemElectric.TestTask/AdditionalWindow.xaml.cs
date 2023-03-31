using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

        private readonly StorageQueryManager _queryManager;
        private readonly ThreadsManager _threadsManager;
        private readonly ILogger<AdditionalWindow> _logger;

        public AdditionalWindow(ThreadsManager threadsManager, StorageQueryManager queryManager, ILogger<AdditionalWindow> logger)
        {
            InitializeComponent();
            _queryManager = queryManager;
            _threadsManager = threadsManager;
            _logger = logger;

            CarDrivers = new ObservableCollection<CarDriverPair>();            
            lstTable.ItemsSource = CarDrivers;
        }

        private void AdditionalWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = true;
            this.Visibility = Visibility.Hidden;
        }

        private void UpdateTable(object? sender, TEntityArgs<IEnumerable<CarDriverPair>> e)
        {
            this.Dispatcher.Invoke(() =>
            {
                foreach(var pair in e.Entity)
                {
                    CarDrivers.Insert(0, pair);
                }
            });
        }

        private void Window_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if(this.Visibility == Visibility.Visible)
            {
                _threadsManager.OnTimerTick += _queryManager.GetEntries;
                _queryManager.OnAddedEntry += UpdateTable;

                _logger.LogInformation("Additional window has opened");
            }
            else
            {
                _threadsManager.OnTimerTick -= _queryManager.GetEntries;
                _queryManager.OnAddedEntry -= UpdateTable;

                _logger.LogInformation("Additional window has closed");
            }
        }
    }
}
