using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel.Core;
using Windows.Devices.Bluetooth;
using Windows.Devices.Enumeration;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace owon_multimeter
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
			CoreApplication.GetCurrentView().TitleBar.ExtendViewIntoTitleBar = true;
			var titleBar = ApplicationView.GetForCurrentView().TitleBar;
			titleBar.ButtonBackgroundColor = Colors.Transparent;
			titleBar.ButtonInactiveBackgroundColor = Colors.Transparent;

			//DevicePicker picker = new DevicePicker();
			//picker.Filter.SupportedDeviceSelectors.Add(BluetoothLEDevice.GetDeviceSelectorFromPairingState(false));
			//picker.Filter.SupportedDeviceSelectors.Add(BluetoothLEDevice.GetDeviceSelectorFromPairingState(true));
			//picker.Show(new Rect(0, 0, 100, 200));
		}

		DeviceWatcher deviceWatcher;
		protected async override void OnNavigatedTo(NavigationEventArgs e)
		{
			deviceWatcher = DeviceInformation.CreateWatcher(
				"System.ItemNameDisplay:~~\"BlueNRG\"",
				new string[]
				{
					"System.Devices.Aep.DeviceAddress",
					"System.Devices.Aep.IsConnected"
				},
				DeviceInformationKind.AssociationEndpoint);
			deviceWatcher.Added += DeviceWatcher_Added;
			deviceWatcher.Removed += DeviceWatcher_Removed;
			deviceWatcher.Start();
			base.OnNavigatedTo(e);
		}

		protected override void OnNavigatedFrom(NavigationEventArgs e)
		{
			deviceWatcher.Stop();
			base.OnNavigatedFrom(e);
		}
		private void DeviceWatcher_Removed(DeviceWatcher sender, DeviceInformationUpdate args)
		{
			//throw new NotImplementedException();
		}
		private async void DeviceWatcher_Added(DeviceWatcher sender, DeviceInformation args)
		{
			var device = await BluetoothLEDevice.FromIdAsync(args.Id);
			var services = await device.GetGattServicesAsync();
			foreach (var service in services.Services)
			{
				Debug.WriteLine($"Service: {service.Uuid}");
				var characteristics = await service.GetCharacteristicsAsync();
				foreach (var character in characteristics.Characteristics)
				{
					Debug.WriteLine($"Characteristic: {character.Uuid}");
				}
			}
		}

	}
}
