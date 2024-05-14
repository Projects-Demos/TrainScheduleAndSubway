using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobChart.TrainScheduleGraph
{
	public class TrainScheduleViewModel
	{
		public ObservableCollection<TrainStop> Stops { get; } = new ObservableCollection<TrainStop>();

		public TrainScheduleViewModel()
		{
			// 填充模拟数据
			var random = new Random();
			var currentTime = TimeSpan.FromHours(6); // 假设从早上6点开始
			for (int i = 0; i < 20; i++) // 创建20个站点的数据
			{
				Stops.Add(new TrainStop
				{
					StationName = $"Station {i + 1}",
					ArrivalTime = currentTime
				});
				// 假设每个站点的停留时间是随机的，介于5到15分钟之间
				currentTime = currentTime.Add(TimeSpan.FromMinutes(5 + random.Next(10)));
			}
		}
	}


}
