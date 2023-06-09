//проект работает в связке с test_3.pro
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Modbus.Device;
using System.Net.Sockets;
using System.ComponentModel;
using System.Dynamic;
using System.Windows.Threading;
using System.Threading.Tasks;


namespace Stend4
{
	/// <summary>
	/// Логика взаимодействия для MainWindow.xaml
	/// </summary>
	/// 


	public partial class MainWindow : Window
	{
		private double a = 50;
		private ushort b = 50;
		private bool AutomatCh = false;
		private bool NasosCh = false;
		private object x;
		public int myReg0 = 0;
		public ushort myReg1 = 0;

		public MainWindow()
		{
			InitializeComponent();
			TagSource = new Depoller(Dispatcher);

			DispatcherTimer timer = new DispatcherTimer();
			timer.Interval = TimeSpan.FromSeconds(0.5);
			timer.Tick += timer_Tick;
			timer.Start();

			void timer_Tick(object sender, EventArgs e)
			{
			myBigTextbox.Text = myReg0.ToString();
				switch (myReg0)
				{
					case 0:
						Automat.IsChecked = true;
						break;
					case 1:
						Automat.IsChecked = false;
						break;
					case 2:
						Nasos.IsChecked = true;
						break;
					case 3:
						Nasos.IsChecked = false;
						break;
					case 4:
						CHP.IsChecked = true;
						break;
					case 5:
						CHP.IsChecked = false;
						break;
				}
				
			}
		}

		ModbusIpMaster mbMaster;
		Task masterLoop;

			private void Update()
		{
			//var myBinder = new Binding("Value");
			
			while (true)
			{
				ushort[] data = mbMaster.ReadHoldingRegisters(1, 0, 2);
				TagSource.Input(mbMaster.ReadHoldingRegisters(1, 0, 2));  //последнее число - количество считываемых регистров
				//TagSource.MyGetIntMember(out x);
				myReg0 = data[0];
				myReg1 = data[1];

				//myBigTextbox.Text= value.ToString();

			}
		}
		
		private void Grid_Loaded(object sender, RoutedEventArgs e)
		{
			mbMaster = ModbusIpMaster.CreateIp(new TcpClient("192.168.0.30", 502));
			masterLoop = Task.Factory.StartNew(new Action(Update));
		}

		public Depoller TagSource
		{
			get;
			private set;
		}
		
		//----------------------------------------------------------------------------------------
		private void Connect_but_Click(object sender, RoutedEventArgs e)
		{
			mbMaster.WriteSingleRegister(1, 0, 0);
		}
		
		private void Pusk_Click(object sender, RoutedEventArgs e)
		{
			mbMaster.WriteSingleRegister(1, 0, b);
		}

		private void progressBar_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
		{

		}
		
		private void Window_Loaded(object sender, RoutedEventArgs e)
		{
			/*if (myTextBox.Text!=null)
						{
				try
				{
					x = Convert.ToInt32(myTextBox.Text);
				}
				catch (Exception ex)
				{ }
							//	MessageBox.Show("Не то число " + ex.Message, "Exception Sample", MessageBoxButton.OK, MessageBoxImage.Warning);
							//}
							if (x == 1)
							{
								bpusk = true;
							}
							else bpusk = false;
						}
			Automat.IsChecked = bpusk;*/
		}
	}
}
