using System;
using System.Threading;
using System.Threading.Tasks;
using NLog;

namespace LogRollingTest
{
	class Program
	{
		static void Main(string[] args)
		{
			var requestLog = LogManager.GetLogger("FileTarget");
		
			var cts = new CancellationTokenSource();
			var task = Task.Run(() =>
			{
				while (!cts.IsCancellationRequested)
				{
					requestLog.Info("Hi there! 1234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890");
					cts.Token.WaitHandle.WaitOne(5);
				}
			}, cts.Token);

			Console.ReadLine();
			cts.Cancel();
			task.Wait();
		}
	}
}
