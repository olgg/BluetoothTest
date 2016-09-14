using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using InTheHand.Net;
using InTheHand.Net.Bluetooth;
using InTheHand.Net.Sockets;

namespace BluetoothTest
{
	class Program
	{
		static void Main(string[] args)
		{
			var cli = new BluetoothClient();
			Console.WriteLine("Поиск устройств...");
			BluetoothDeviceInfo[] peers = cli.DiscoverDevices();
			if (peers.Any())
			{
				Console.WriteLine("Найдено {0} bluetooth устройств:", peers.Count());
				int index = -1;
				for (int i = 0; i < peers.Length; i++)
				{
					var peer = peers[i];
					if (peer.DeviceName == "iPhone (Oleg)")
						index = i;
					Console.WriteLine(peer.DeviceName);
				}

				if (index >= 0)
				{
					Console.WriteLine("Устройство-метка есть в списке");
					bool old = false;
					while (true)
					{
						bool inRange;
						BluetoothDeviceInfo device = peers[index];
						try
						{
							ServiceRecord[] records = device.GetServiceRecords(device.InstalledServices.First());				
							inRange = true;
						}
						catch (SocketException)
						{
							inRange = false;
						}

						if (old != inRange)
						{
							if (inRange)
								Console.WriteLine("Устройство обнаружено в {0}", DateTime.Now);
							else
								Console.WriteLine("Устройство НЕ обнаружено в {0}", DateTime.Now);
							
							old = inRange;
						}

						Thread.Sleep(60000);
					}
				}
			}
			else
				Console.WriteLine("Bluetooth устройств не обнаружено.");

			Console.ReadLine();
		}
	}
}
