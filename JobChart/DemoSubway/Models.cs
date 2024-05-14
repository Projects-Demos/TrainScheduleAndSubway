using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobChart.DemoSubway
{
	/// <summary>
	/// 深圳地铁模型
	/// </summary>
	public class ShenTie
	{
		[JsonProperty("s")]
		public string Name { get; set; }

		[JsonProperty("i")]
		public string Index { get; set; }

		[JsonProperty("l")]
		public SubwayLine[] SubwayLines { get; set; }

		[JsonProperty("o")]
		public string Obj { get; set; }
	}

	public class SubwayLine
	{
		[JsonProperty("st")]
		public St[] Sites { get; set; }

		[JsonProperty("ln")]
		public string LineNumber { get; set; }

		[JsonProperty("su")]
		public string Su { get; set; }

		[JsonProperty("kn")]
		public string KName { get; set; }

		[JsonProperty("c")]
		public string[] Circles { get; set; }

		[JsonProperty("lo")]
		public string Lo { get; set; }

		[JsonProperty("lp")]
		public string[] LinePosition { get; set; }

		[JsonProperty("ls")]
		public string Ls { get; set; }

		[JsonProperty("cl")]
		public string Color { get; set; }

		[JsonProperty("la")]
		public string La { get; set; }

		[JsonProperty("x")]
		public string X { get; set; }

		[JsonProperty("li")]
		public string Li { get; set; }
	}

	public class St
	{
		[JsonProperty("rs")]
		public string Rs { get; set; }

		[JsonProperty("udpx")]
		public string Udpx { get; set; }

		[JsonProperty("su")]
		public string Su { get; set; }

		[JsonProperty("udsu")]
		public string Udsu { get; set; }

		[JsonProperty("n")]
		public string Name { get; set; }

		[JsonProperty("en")]
		public string En { get; set; }

		[JsonProperty("sid")]
		public string Sid { get; set; }

		[JsonProperty("p")]
		public string Position { get; set; }

		[JsonProperty("r")]
		public string R { get; set; }

		[JsonProperty("udsi")]
		public string Udsi { get; set; }

		[JsonProperty("t")]
		public string T { get; set; }

		[JsonProperty("si")]
		public string Si { get; set; }

		[JsonProperty("sl")]
		public string Sl { get; set; }

		[JsonProperty("udli")]
		public string Udli { get; set; }

		[JsonProperty("poiid")]
		public string Poiid { get; set; }

		[JsonProperty("lg")]
		public string Lg { get; set; }

		[JsonProperty("sp")]
		public string Sp { get; set; }
	}
}