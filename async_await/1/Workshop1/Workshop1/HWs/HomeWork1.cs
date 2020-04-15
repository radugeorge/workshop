using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using Workshop1.HWs;

namespace Workshop1
{
    class HomeWork1
    {
        public static void Run()
        {
            Console.WriteLine("Starting 1...");

            var fileQueue = new BlockingCollection<string>(4);
            var dataStorage = new List<Tuple<string, string>>();

            Producer.Init(fileQueue);
            Consumer.Init(fileQueue, dataStorage);
            Reporter.Init(dataStorage);

            Console.ReadLine();

            Producer.Deinit();
            Consumer.Deinit(fileQueue);
        }
    }
}
