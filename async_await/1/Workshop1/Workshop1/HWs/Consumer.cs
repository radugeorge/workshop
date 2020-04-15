using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Workshop1.HWs
{
    class Consumer
    {
        #region Thread
        private static volatile bool consumerParentIsCancelled;

        public static void Init(BlockingCollection<string> fileQueue, List<Tuple<string, string>> dataStorage)
        {
            consumerParentIsCancelled = false;
            new Thread(() => RunConsumer(fileQueue, dataStorage)).Start();
        }

        private static void RunConsumer(BlockingCollection<string> fileQueue, List<Tuple<string, string>> dataStorage)
        {
            while (!consumerParentIsCancelled)
            {
                var file = fileQueue.Take();

                if (file != null)
                {
                    ThreadPool.QueueUserWorkItem(state => ProcessFile(fileQueue, file, dataStorage));
                }
            }

            Console.WriteLine("\nConsumer parent is cancelled!!!\n");
        }

        internal static void Deinit(BlockingCollection<string> fileQueue)
        {
            consumerParentIsCancelled = true;
            fileQueue.Add(null);
        }
        #endregion

        #region Task

        private static CancellationTokenSource consumerParentTokenSource;

        public static void InitWithTask(BlockingCollection<string> fileQueue, List<Tuple<string, string>> dataStorage)
        {
            consumerParentTokenSource = new CancellationTokenSource();
            var token = consumerParentTokenSource.Token;

            Task.Factory.StartNew(() => RunConsumerTask(fileQueue, dataStorage, token), token);
        }

        private static void RunConsumerTask(BlockingCollection<string> fileQueue, List<Tuple<string, string>> dataStorage, CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                var file = fileQueue.Take();

                if (file != null)
                {
                    Task.Factory.StartNew(() => ProcessFile(fileQueue, file, dataStorage), TaskCreationOptions.AttachedToParent);
                }
            }

            Console.WriteLine("\nConsumer parent is cancelled!!!\n");
        }

        internal static void DeinitWithTask(BlockingCollection<string> fileQueue)
        {
            consumerParentTokenSource.Cancel();
            fileQueue.Add(null);
        }
        #endregion

        #region AsyncAwait
        public static async Task InitAsync(BlockingCollection<string> fileQueue, List<Tuple<string, string>> dataStorage)
        {
            consumerParentTokenSource = new CancellationTokenSource();
            var token = consumerParentTokenSource.Token;

            await Task.Factory.StartNew(() => RunConsumerAsync(fileQueue, dataStorage, token), token);
        }

        private static async Task RunConsumerAsync(BlockingCollection<string> fileQueue, List<Tuple<string, string>> dataStorage, CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                var file = fileQueue.Take();

                if(file != null)
                {
                    await Task.Factory.StartNew(() => ProcessFile(fileQueue, file, dataStorage), TaskCreationOptions.AttachedToParent);
                }
            }

            Console.WriteLine("\nConsumer parent is cancelled!!!\n");
        }
        #endregion

        #region Common
        private static void ProcessFile(BlockingCollection<string> fileQueue, string file, List<Tuple<string, string>> dataStorage)
        {
            try
            {
                var data = File.ReadAllText(file);
                lock (dataStorage)
                {
                    dataStorage.Add(new Tuple<string, string>(file, data));
                }
            }
            catch (IOException)
            {
                fileQueue.Add(file);
            }
        }
        #endregion
    }
}
