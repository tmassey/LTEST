using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using LTest.Runner.Interfaces;

namespace LTest.Runner
{
    public class ThreadManager
    {
        private readonly int _maxThreads = Environment.ProcessorCount*2;
        private readonly int _timesPerWorker;
        private int _delay=0;
        private readonly Queue<Thread> _threads=new Queue<Thread>();
        private readonly List<Thread> _activeThreads = new List<Thread>();
        private static readonly object _monitor = new object();
        public ThreadManager(int timesPerWorker, int delay)
        {
            _timesPerWorker = timesPerWorker;
            _delay = delay;
        }

        public void AddWorker(string type)
        {
            for (var x = 0; x < _timesPerWorker; x++)
            {
                lock (_monitor)
                {
                    var worker = new Worker(type);                    
                    _threads.Enqueue(new Thread(worker.RunTest));
                }
            }
        }

        public void RunWorkers()
        {
            while (_threads.Count != 0)
            {
                StartNextThread();
                Thread.Sleep(_delay);
                while (_activeThreads.Count == _maxThreads-1)
                {
                    foreach (var thread in _activeThreads.Where(x=>x.ThreadState==ThreadState.Stopped).ToList())
                    {
                        _activeThreads.Remove(thread);
                    }
                }
            }

        }

        private void StartNextThread()
        {
            var thread = _threads.Dequeue();
            _activeThreads.Add(thread);
            thread.Start();
        }
    }
}