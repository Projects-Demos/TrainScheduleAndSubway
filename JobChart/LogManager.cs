using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobChart
{
	internal class LogManager
	{
		static LogManager()
		{
			////每次启动新建一个日志文件
			//string currTime = DateTime.Now.ToString("yyyyMMddHHmmss");
			//Trace.Listeners.Add(new TextWriterTraceListener("Log" + currTime + ".log", "myListener"));
			//Trace.Listeners.Add(new TextWriterTraceListener(System.Console.Out));
			//Trace.Listeners.Add(new EventLogTraceListener());


		}

		internal static void Error(string v)
		{
			//Debug.WriteLine(v);

			//Trace.TraceInformation(v); //记录日志
			//退出的时候清空
			//Trace.Flush();

			TraceSourceLog.LogInformation(v);
		}
	}
}
