using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobChart.TrainScheduleGraph
{
	public class TrainStop
	{
		public string StationName { get; set; }
		public TimeSpan ArrivalTime { get; set; }
	}
}
