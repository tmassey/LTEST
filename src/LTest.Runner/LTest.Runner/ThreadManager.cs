using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using LTest.Runner.Interfaces;

namespace LTest.Runner
{
	public class ThreadManager
	{
		private static readonly object _monitor = new object();
		private readonly List<Thread> _activeThreads = new List<Thread>();
		private readonly int _maxThreads = Environment.ProcessorCount * 2;
		private readonly Queue<Thread> _threads = new Queue<Thread>();
		private readonly int _timesPerWorker;
		private readonly int _delay;
		private int _totalThreads;

		public ThreadManager(int timesPerWorker, int delay)
		{
			_timesPerWorker = timesPerWorker;
			_delay = delay;
		}

		public event ProgressChanged OnProgressChanged;
		public event TestEndHandler OnTestEnd;

		public void AddWorker(string type)
		{
			for (var x = 0; x < _timesPerWorker; x++)
				lock (_monitor)
				{
					var worker = new Worker(type);
					worker.OnTestEnd += Worker_OnTestEnd;
					_threads.Enqueue(new Thread(worker.RunTest) {Name = worker.Name});
				}
		}

		private void Worker_OnTestEnd(object sender, TestEndHandlerArgs args)
		{
			InvokeOnTestEnd(args);
		}

		public void RunWorkers()
		{
			_totalThreads = _threads.Count;
			while (_threads.Count != 0)
			{
				StartNextThread();
				Thread.Sleep(_delay);
				while (_activeThreads.Count == _maxThreads - 1)
				{
					foreach (var thread in _activeThreads.Where(x => x.ThreadState == ThreadState.Stopped).ToList())
						_activeThreads.Remove(thread);					
				}
			}
		}

		private void StartNextThread()
		{
			var thread = _threads.Dequeue();
			_activeThreads.Add(thread);
			thread.Start();
			OnOnProgressChanged(new ProgressChangedArgs
			{
				Progress = 1 - (float) ((decimal) _threads.Count / _totalThreads),
				ProgressMessage = thread.Name
			});
		}

		protected virtual void OnOnProgressChanged(ProgressChangedArgs args)
		{
			OnProgressChanged?.Invoke(this, args);
		}

		protected virtual void InvokeOnTestEnd(TestEndHandlerArgs args)
		{
			OnTestEnd?.Invoke(this, args);
		}
	}
}