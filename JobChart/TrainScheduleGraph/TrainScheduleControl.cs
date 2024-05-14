using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace JobChart.TrainScheduleGraph
{
	public class TrainScheduleControl : Control
	{
		static TrainScheduleControl()
		{
			DefaultStyleKeyProperty.OverrideMetadata(typeof(TrainScheduleControl), new FrameworkPropertyMetadata(typeof(TrainScheduleControl)));
		}

		public static readonly DependencyProperty ScheduleProperty = DependencyProperty.Register(
			"Schedule",
			typeof(TrainScheduleViewModel),
			typeof(TrainScheduleControl),
			new FrameworkPropertyMetadata(null, OnScheduleChanged));

		public TrainScheduleViewModel Schedule
		{
			get => (TrainScheduleViewModel)GetValue(ScheduleProperty);
			set => SetValue(ScheduleProperty, value);
		}

		private static void OnScheduleChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			((TrainScheduleControl)d).InvalidateVisual();
		}

		protected override void OnRender(DrawingContext drawingContext)
		{
			base.OnRender(drawingContext);

			if (Schedule == null || Schedule.Stops.Count == 0)
				return;

			double width = ActualWidth;
			double height = ActualHeight;

			// 假设所有列车都在同一天运行，找出最早和最晚的时间
			TimeSpan startTime = TimeSpan.FromHours(24);
			TimeSpan endTime = TimeSpan.Zero;
			foreach (var stop in Schedule.Stops)
			{
				if (stop.ArrivalTime < startTime) startTime = stop.ArrivalTime;
				if (stop.ArrivalTime > endTime) endTime = stop.ArrivalTime;
			}

			double timeRange = (endTime - startTime).TotalMinutes;
			double yInterval = height / Schedule.Stops.Count;

			// 绘制每个站点的时间和线
			for (int i = 0; i < Schedule.Stops.Count; i++)
			{
				var stop = Schedule.Stops[i];
				double yPosition = yInterval * i;
				double xPosition = ((stop.ArrivalTime - startTime).TotalMinutes / timeRange) * width;

				// 绘制时间文本
				FormattedText formattedText = new FormattedText(
					stop.ArrivalTime.ToString(@"hh\:mm"),
					CultureInfo.CurrentCulture,
					FlowDirection.LeftToRight,
					new Typeface("Segoe UI"),
					12,
					Brushes.Black);

				drawingContext.DrawText(formattedText, new Point(xPosition - formattedText.Width / 2, yPosition));

				// 绘制线
				if (i > 0)
				{
					var previousStop = Schedule.Stops[i - 1];
					double previousXPosition = ((previousStop.ArrivalTime - startTime).TotalMinutes / timeRange) * width;
					double previousYPosition = yInterval * (i - 1);

					drawingContext.DrawLine(new Pen(Brushes.Black, 1), new Point(previousXPosition, previousYPosition), new Point(xPosition, yPosition));
				}
			}
		}
	}
}

