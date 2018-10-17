using System;

namespace LTest.Runner.Interfaces
{
	public class TestEndHandlerArgs
	{
		public TestEndHandlerArgs()
		{
			IsSuccess = true;
		}
		public string TestName { get; set; }
		public TimeSpan TimeForSetup { get; set; }
		public TimeSpan TimeForTest { get; set; }
		public TimeSpan TimeForCleanup { get; set; }
	public Exception Execption { get; set; }

		public bool IsSuccess { get; set; }
	}
}