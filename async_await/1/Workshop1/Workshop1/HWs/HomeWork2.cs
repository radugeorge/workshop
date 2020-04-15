using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using Workshop1.HWs;

namespace Workshop1
{
    class HomeWork2
    {
        public static void Run()
        {
            Console.WriteLine("Starting 2...");

            BlockingCollection<string> fileQueue = new BlockingCollection<string>(4);
            List<Tuple<string, string>> dataStorage = new List<Tuple<string, string>>();

            Producer.Init(fileQueue);

            Consumer.InitWithTask(fileQueue, dataStorage);
            Reporter.InitWithTask(dataStorage);

            Console.ReadLine();

            Producer.Deinit();
            Consumer.DeinitWithTask(fileQueue);
        }
    }
}
