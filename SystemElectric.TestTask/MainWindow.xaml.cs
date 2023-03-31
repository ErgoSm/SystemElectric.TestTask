using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using System.Collections.ObjectModel;
using System.Threading;
using System.Windows;
using SystemElectric.TestTask.Domain.Args;
using SystemElectric.TestTask.Domain.Entities;
using SystemElectric.TestTask.Domain.Services;
using SystemElectric.TestTask.Hubs;

namespace SystemElectric.TestTask
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private ObservableCollection<CarDriverPair> CarDrivers { get; set; } = new ObservableCollection<CarDriverPair>();

        private readonly ThreadsManager _container;
        private readonly StorageCommandManager _dbPresenter;
        private readonly DataReader _reader;

        private readonly AdditionalWindow _additionalWindow;
        private readonly ILogger<MainWindow> _logger;
        private readonly IHubContext<MainHub> _hubContext;

        public MainWindow(
            DataReader reader,
            ThreadsManager container,
            AdditionalWindow additionalWindow,
            StorageCommandManager dbPresenter,
            IHubContext<MainHub> hubContext,
            ILogger<MainWindow> logger)
        {
            InitializeComponent();

            _additionalWindow = additionalWindow;
            _container = container;
            _logger = logger;
            _reader = reader;
            _hubContext = hubContext;
            _dbPresenter = dbPresenter;
            lstTable.ItemsSource = CarDrivers;

            container.OnSimultaneousRead += Reader_OnSimultaneousRead;
        }

        private void Reader_OnSimultaneousRead(object? sender, ReadArgs e)
        {
            this.Dispatcher.Invoke(() => CarDrivers.Add(e.Pair));
        }

        private async void Reader_OnCarRead(object? sender, EntryArgs e)
        {
            await _dbPresenter.Store(e.Entry as CarEntry, CancellationToken.None);

            await _hubContext.Clients.All.SendAsync("EntryAdded", e.Entry);
        }

        private async void Reader_OnDriverRead(object? sender, EntryArgs e)
        {
            await _dbPresenter.Store(e.Entry as DriverEntry, CancellationToken.None);

            await _hubContext.Clients.All.SendAsync("EntryAdded", e.Entry);
        }

        private void btnRun1_Click(object sender, RoutedEventArgs e)
        {
            _container.Toggle(1, () => _reader.OnCarRead += Reader_OnCarRead, () => _reader.OnCarRead -= Reader_OnCarRead);
            _logger.LogInformation("Start thread 1");
        }

        private void btnRun2_Click(object sender, RoutedEventArgs e)
        {
            _container.Toggle(2, () => _reader.OnDriverRead += Reader_OnDriverRead, () => _reader.OnDriverRead -= Reader_OnDriverRead);
            _logger.LogInformation("Start thread 2");
        }

        private void btnWin2_Click(object sender, RoutedEventArgs e)
        {
            _additionalWindow.Visibility = Visibility.Visible;
        }

        private async void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            _logger.LogInformation("Application has closed");
            Application.Current.Shutdown();
        }
    }
}
