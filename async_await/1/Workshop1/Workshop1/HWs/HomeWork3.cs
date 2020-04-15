using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;
using Workshop1.HWs;

namespace Workshop1
{
    class HomeWork3
    {
        public static void Run()
        {
            Console.WriteLine("Starting 3...");

            BlockingCollection<string> fileQueue = new BlockingCollection<string>(4);
            List<Tuple<string, string>> dataStorage = new List<Tuple<string, string>>();

            Producer.Init(fileQueue);
            Consumer.InitAsync(fileQueue, dataStorage).Wait();
            Reporter.InitAsync(dataStorage).Wait();

            Console.ReadLine();

            Producer.Deinit();
            Consumer.DeinitWithTask(fileQueue);
        }
    }
}
