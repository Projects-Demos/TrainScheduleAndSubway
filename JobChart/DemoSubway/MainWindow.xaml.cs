using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
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

namespace JobChart.DemoSubway
{
	/// <summary>
	/// MainWindow.xaml 的交互逻辑
	/// </summary>
	public partial class MainWindow : Window
	{
		public MainWindow()
		{
			InitializeComponent();
		}

		private void Window_Loaded(object sender, RoutedEventArgs e)
		{
			string jsonFile = @"DemoSubway/shentie.json";
			var shentie = JsonHelper.DeserializeFromFile<ShenTie>(jsonFile);
			this.tbTitle.Text = shentie.Name;
			List<string> lstSites = new List<string>();
			for (int i = 0; i < shentie.SubwayLines?.Length; i++)
			{
				var subwayLine = shentie.SubwayLines[i];
				if (subwayLine != null)
				{
					//地铁线路
					var color = ColorTranslator.FromHtml($"#{subwayLine.Color}");//线路颜色
					var circles = subwayLine.Circles;//线路节点
					Path line = new Path();
					PathFigureCollection lineFigures = new PathFigureCollection();
					PathFigure lineFigure = new PathFigure();
					lineFigure.IsClosed = false;
					var start = circles?[0].Split(' ');//线路起始位置
					lineFigure.StartPoint = new System.Windows.Point(int.Parse(start[0]), int.Parse(start[1]));

					for (int j = 0; j < circles?.Length; j++)
					{
						var circle = circles[j].Split(' ');
						LineSegment lineSegment = new LineSegment(new System.Windows.Point(int.Parse(circle[0]), int.Parse(circle[1])), true);
						lineFigure.Segments.Add(lineSegment);
					}
					lineFigures.Add(lineFigure);
					line.Data = new PathGeometry(lineFigures, FillRule.Nonzero, null);
					line.Stroke = new SolidColorBrush(System.Windows.Media.Color.FromArgb(color.A, color.R, color.G, color.B));
					line.StrokeThickness = 4;
					this.subwayBox.Children.Add(line);
					//地铁站点
					for (int j = 0; j < subwayLine.Sites?.Length; j++)
					{
						var site = subwayLine.Sites[j];
						if (site != null)
						{

							//站点标识，圆圈
							Path siteCirclePath = new Path();
							var sitePosition = site?.Position?.Split(' ');
							EllipseGeometry ellipse = new EllipseGeometry();
							ellipse.Center = new System.Windows.Point(int.Parse(sitePosition[0]), int.Parse(sitePosition[1]));
							ellipse.RadiusX = 4;
							ellipse.RadiusY = 4;
							siteCirclePath.Data = ellipse;
							siteCirclePath.Fill = System.Windows.Media.Brushes.White;
							siteCirclePath.Stroke = new SolidColorBrush(System.Windows.Media.Color.FromArgb(color.A, color.R, color.G, color.B));
							siteCirclePath.Cursor = Cursors.Hand;
							siteCirclePath.Focusable = true;
							siteCirclePath.Tag = site?.Name;
							siteCirclePath.MouseDown += SiteCirclePath_MouseDown;
							this.subwayBox.Children.Add(siteCirclePath);
							//站点名字
							if (lstSites.Contains(site?.Name))
							{
								continue;//对于交汇站点，只绘制一次
							}
							//站点名称
							Path siteTextPath = new Path();
							FormattedText siteContent = new FormattedText(site?.Name, CultureInfo.CurrentCulture, FlowDirection.LeftToRight, new Typeface("Arial"), 14, System.Windows.Media.Brushes.Black, 1.25);
							var x = int.Parse(sitePosition[0]);
							var y = int.Parse(sitePosition[1]);
							if (j + 1 < subwayLine.Sites?.Length)
							{
								//站点位置适当偏移
								var next = subwayLine.Sites[j + 1]?.Position?.Split(' ');
								var nextx = int.Parse(next[0]);
								var nexty = int.Parse(next[1]);
								if (x == nextx)
								{
									x = x + 6;
								}
								else if (y == nexty)
								{
									y = y + 6;
								}
								else
								{
									x = x + 1;
									y = y + 1;
								}
							}
							Geometry geometry = siteContent.BuildGeometry(new System.Windows.Point(x, y));
							siteTextPath.Data = geometry;
							siteTextPath.Stroke = System.Windows.Media.Brushes.Black;
							siteTextPath.Focusable = true;
							siteTextPath.Cursor = Cursors.Hand;
							siteTextPath.MouseDown += SiteTextPath_MouseDown;
							siteTextPath.Tag = site?.Name;
							this.subwayBox.Children.Add(siteTextPath);
							lstSites.Add(site?.Name);
						}
					}

					var kName = subwayLine.KName;//线路名称
					var linePosition = subwayLine.LinePosition?[0].Split(' ');
					if (kName != null)
					{
						Path lineNamePath = new Path();
						FormattedText lineNameText = new FormattedText(kName, CultureInfo.CurrentCulture, FlowDirection.LeftToRight, new Typeface("Arial"), 16, System.Windows.Media.Brushes.Black, 1.25);
						var lineX = int.Parse(linePosition[0]);
						var lineY = int.Parse(linePosition[1]);
						if (subwayLine.LineNumber == "1")
						{
							lineX = lineX - 10;
							lineY = lineY + 20;
						}
						Geometry geometry = lineNameText.BuildGeometry(new System.Windows.Point(lineX, lineY));
						lineNamePath.Data = geometry;
						lineNamePath.Stroke = new SolidColorBrush(System.Windows.Media.Color.FromArgb(color.A, color.R, color.G, color.B));
						this.subwayBox.Children.Add(lineNamePath);
					}
				}
			}
		}

		private void SiteCirclePath_MouseDown(object sender, MouseButtonEventArgs e)
		{
			throw new NotImplementedException();
		}

		private void SiteTextPath_MouseDown(object sender, MouseButtonEventArgs e)
		{
			throw new NotImplementedException();
		}
	}
}
