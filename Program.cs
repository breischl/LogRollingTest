using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Common.Logging;

namespace LogRollingTest
{
	class Program
	{
		static void Main(string[] args)
		{
			var log = LogManager.GetLogger<Program>();
			var requestLog = LogManager.GetLogger("FileTarget");
			log.InfoFormat("Starting test...");

			var cts = new CancellationTokenSource();
			var task = Task.Run(() =>
			{
				while (!cts.IsCancellationRequested)
				{
					requestLog.InfoFormat("Hi there! 1234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890");
					cts.Token.WaitHandle.WaitOne(5);
				}
			}, cts.Token);

			Console.ReadLine();
			cts.Cancel();
			task.Wait();
		}
	}
}
