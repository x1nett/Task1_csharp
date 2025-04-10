using System;
using System.Threading;
using System.Collections.Generic;

namespace ThreadSequenceSum
{
    class Program
    {
        private static int threadCount = 2; // можна змінювати кількість потоків
        private static List<Worker> workers = new List<Worker>();

        static void Main(string[] args)
        {
            for (int i = 0; i < threadCount; i++)
            {
                var worker = new Worker(i + 1, step: i + 1); // крок збільшується для кожного потоку
                workers.Add(worker);
                Thread thread = new Thread(worker.Calculate);
                thread.Start();

                Thread stopperThread = new Thread(() => StopWorker(worker, (i + 1) * 5000));
                stopperThread.Start();
            }
        }

        static void StopWorker(Worker worker, int delay)
        {
            Thread.Sleep(delay);
            worker.RequestStop();
        }
    }

    class Worker
    {
        private volatile bool shouldStop = false;
        private readonly int threadNumber;
        private readonly int step;
        private long sum = 0;
        private long count = 0;

        public Worker(int threadNumber, int step)
        {
            this.threadNumber = threadNumber;
            this.step = step;
        }

        public void Calculate()
        {
            long current = 0;
            while (!shouldStop)
            {
                sum += current;
                current += step;
                count++;
            }

            Console.WriteLine($"Потік #{threadNumber}: Сума = {sum}, Кількість доданків = {count}");
        }

        public void RequestStop()
        {
            shouldStop = true;
        }
    }
}
