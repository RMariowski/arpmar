using System;
using System.ServiceProcess;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using ArpmarCore.Domain;

namespace ArpmarApp.Views
{
    public partial class ServiceManagementView : UserControl
    {
        private const int UpdateStatusTime = 500;

        private readonly ServiceController _serviceController;
        private Timer _serviceTimer;

        public ServiceManagementView()
        {
            InitializeComponent();

            StartButton.IsEnabled = false;
            StopButton.IsEnabled = false;

            try
            {
                _serviceController = new ServiceController(Service.Name);
                CreateServiceTimer();
                _serviceTimer.Start();
            }
            catch (Exception e)
            {
                App.ShowMessageBox(e.Message);
            }
        }

        private void CreateServiceTimer()
        {
            _serviceTimer = new Timer(UpdateStatusTime) {AutoReset = true};
            _serviceTimer.Elapsed += (sender, args) =>
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    _serviceController.Refresh();
                    UpdateStatusControls();
                });
            };
        }

        private void UpdateStatusControls()
        {
            bool isServiceOn = IsServiceOn();
            if (isServiceOn)
            {
                StatusText.Text = "ON";
                StatusText.Foreground = Brushes.DarkGreen;

                StartButton.IsEnabled = false;
                StopButton.IsEnabled = true;
            }
            else
            {
                StatusText.Text = "OFF";
                StatusText.Foreground = Brushes.DarkRed;

                StartButton.IsEnabled = true;
                StopButton.IsEnabled = false;
            }
        }

        private bool IsServiceOn()
        {
            switch (_serviceController.Status)
            {
                case ServiceControllerStatus.PausePending:
                case ServiceControllerStatus.Paused:
                case ServiceControllerStatus.StartPending:
                case ServiceControllerStatus.Running:
                case ServiceControllerStatus.ContinuePending:
                    return true;

                case ServiceControllerStatus.Stopped:
                case ServiceControllerStatus.StopPending:
                    return false;

                default:
                    return false;
            }
        }

        private void StartButton_OnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!Service.IsAdministrator())
                    throw new Exception("Administrator permissions required");

                _serviceController.Start();
            }
            catch (Exception ex)
            {
                App.ShowMessageBox(ex.Message);
            }
        }

        private void StopButton_OnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!Service.IsAdministrator())
                    throw new Exception("Administrator permissions required");

                _serviceController.Stop();
            }
            catch (Exception ex)
            {
                App.ShowMessageBox(ex.Message);
            }
        }
    }
}