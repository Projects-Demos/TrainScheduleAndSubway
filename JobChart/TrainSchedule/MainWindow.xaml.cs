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
using System.Windows.Shapes;

namespace JobChart.TrainSchedule
{
	/// <summary>
	/// MainWindow.xaml 的交互逻辑
	/// </summary>
	public partial class MainWindow : Window
	{
		// 构造函数初始化窗口组件，并调用绘图方法
		public MainWindow()
		{
			InitializeComponent();
			DrawGrid();
			DrawStations();
			DrawTrains();
		}

		// 定义站点及其位置
		private Dictionary<string, Point> stationPositions = new Dictionary<string, Point>
		{
			{"Station A", new Point(100, 50)},
			{"Station B", new Point(200, 120)},
			{"Station C", new Point(300, 200)},
			{"Station D", new Point(400, 280)},
			{"Station E", new Point(500, 360)}
		};

		// 定义列车线路及其途径的站点
		private List<List<string>> trainRoutes = new List<List<string>>
		{
			new List<string> { "Station A", "Station B", "Station D" },
			new List<string> { "Station A", "Station C", "Station E" },
			new List<string> { "Station B", "Station C", "Station D" },
            // ...可以根据实际情况添加更多线路...
        };


		// DrawGrid 方法用于在 Canvas 上绘制网格背景
		private void DrawGrid()
		{
			int cellSize = 50; // 网格单元格大小
							   // 循环绘制垂直线条
			for (int i = 0; i < trainScheduleCanvas.ActualWidth; i += cellSize)
			{
				var line = new Line
				{
					X1 = i,
					Y1 = 0,
					X2 = i,
					Y2 = trainScheduleCanvas.ActualHeight,
					Stroke = Brushes.LightGray
				};
				trainScheduleCanvas.Children.Add(line); // 将线条添加到 Canvas
			}
			// 循环绘制水平线条
			for (int j = 0; j < trainScheduleCanvas.ActualHeight; j += cellSize)
			{
				var line = new Line
				{
					X1 = 0,
					Y1 = j,
					X2 = trainScheduleCanvas.ActualWidth,
					Y2 = j,
					Stroke = Brushes.LightGray
				};
				trainScheduleCanvas.Children.Add(line); // 将线条添加到 Canvas
			}
		}


		// 绘制站点标签
		private void DrawStations()
		{
			// 绘制站点标签
			foreach (var station in stationPositions)
			{
				var stationLabel = new Label
				{
					Content = station.Key,
					Foreground = Brushes.Black,
					Background = Brushes.White,
					FontSize = 10,
					Width = 60,
					Height = 20,
					Padding = new Thickness(1),
					Margin = new Thickness(station.Value.X, station.Value.Y, 0, 0)
				};
				trainScheduleCanvas.Children.Add(stationLabel);
			}
		}

		// 绘制列车线路
		private void DrawTrains()
		{
			Random random = new Random();

			// 绘制列车线路
			foreach (var route in trainRoutes)
			{
				var trainPath = new Polyline
				{
					Stroke = RandomBrush(),
					StrokeThickness = 2,
					Points = new PointCollection()
				};

				// 按线路途径的站点顺序添加站点到路径
				foreach (var stationName in route)
				{
					if (stationPositions.TryGetValue(stationName, out Point position))
					{
						// 这里可以根据实际情况调整时间和位置的关系
						trainPath.Points.Add(position);
					}
				}

				// 将列车路径添加到 Canvas
				trainScheduleCanvas.Children.Add(trainPath);
			}
		}

		private Brush RandomBrush()
		{
			Random random = new Random();
			byte[] colorBytes = new byte[3];
			random.NextBytes(colorBytes);
			Color color = Color.FromRgb(colorBytes[0], colorBytes[1], colorBytes[2]);
			return new SolidColorBrush(color);
		}
	}
}