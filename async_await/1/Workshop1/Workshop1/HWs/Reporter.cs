using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Workshop1.HWs
{
    class Reporter
    {
        #region Thread
        public static void Init(List<Tuple<string, string>> dataStorage)
        {
            new Thread(() => WriteReport(dataStorage)).Start();
        }
        #endregion

        #region Task
        public static void InitWithTask(List<Tuple<string, string>> dataStorage)
        {
            Task.Factory.StartNew(() => WriteReport(dataStorage)).Wait();
        }
        #endregion

        #region AsyncAwait
        public static async Task InitAsync(List<Tuple<string, string>> dataStorage)
        {
            await Task.Factory.StartNew(() => WriteReport(dataStorage), TaskCreationOptions.AttachedToParent);
        }
        #endregion

        #region Common
        private static void WriteReport(List<Tuple<string, string>> dataStorage)
        {
            while (dataStorage.Count < 10) { }

            lock (dataStorage)
            {
                foreach (var (path, data) in dataStorage)
                {
                    Console.WriteLine("\t" + path + ":");
                    Console.WriteLine(data);
                }
            }

            dataStorage.Clear();
        }
        #endregion
    }
}
