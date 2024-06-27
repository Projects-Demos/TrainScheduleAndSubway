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

namespace JobChart
{
	/// <summary>
	/// MainWindow.xaml 的交互逻辑
	/// </summary>
	public partial class MainWindow : Window
	{
		public MainWindow()
		{
			InitializeComponent();
			Closing += (s, e) =>
			{
				Application.Current.Shutdown();
			};
		}


		TrainScheduleGraph.MainWindow trainScheduleGraph = new TrainScheduleGraph.MainWindow();
		private void TrainShceduleGraph_Click(object sender, RoutedEventArgs e)
		{
			trainScheduleGraph = new TrainScheduleGraph.MainWindow();
			trainScheduleGraph.Show();
		}

		DemoSubway.MainWindow demoSubway = new DemoSubway.MainWindow();
		private void DemoSubway_Click(object sender, RoutedEventArgs e)
		{
			demoSubway = new DemoSubway.MainWindow();
			demoSubway.Show();
		}

		TrainSchedule.MainWindow TrainSchedule = new TrainSchedule.MainWindow();
		private void TrainShcedule_Click(object sender, RoutedEventArgs e)
		{
			TrainSchedule = new TrainSchedule.MainWindow();
			TrainSchedule.Show();
		}
    }
}
