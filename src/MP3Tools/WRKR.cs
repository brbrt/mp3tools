using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Concurrent;
using System.Threading;
using System.IO;

namespace MP3Tools
{
    public static class WRKR
    {
        private static ConcurrentQueue<FileItem> queue;
        private static Thread workerThread;
        private static volatile bool isRunning;

        static WRKR()
        {
            queue = new ConcurrentQueue<FileItem>();

            workerThread = new Thread(DoWork);
            workerThread.IsBackground = true;
            workerThread.Name = "Dolgozó";
            isRunning = true;
            workerThread.Start();
        }

        public static void AddItem(FileItem fi)
        {
            queue.Enqueue(fi);
        }

        public static void Stop()
        {
            isRunning = false;
        }

        private static void DoWork()
        {
            StringBuilder logMessage = new StringBuilder();

            while (isRunning)
            {
                if (queue.IsEmpty)
                {
                    if (logMessage.Length > 0)
                    {
                        SaveLog(logMessage.ToString());
                        logMessage.Clear();
                    }

                    Thread.Sleep(25);
                }
                else
                {
                    FileItem fi;
                    if (queue.TryDequeue(out fi))
                    {
                        try
                        {
                            ProcessMP3.Process(fi);
                        }
                        catch (Exception ex)
                        {
                            fi.Processed = ProcessState.Error;
                            fi.NewName = "ERROR: " + ex.Message;
                        }


                        //LOG
                        logMessage.Append("Original name: " + fi.FileName + "\n");
                        logMessage.Append("New name     : " + fi.NewName + "\n");
                        logMessage.Append("Status: " + fi.Processed + "\n");
                        logMessage.Append("Path: " + fi.Path + "\n");
                        logMessage.Append("@" + DateTime.Now.ToString() + "\n");
                        logMessage.Append("------------------------------------------------------------\n");
                    }

                }
            }
        }

        private static void SaveLog(string message)
        {
            using (StreamWriter logger = File.AppendText("log.txt"))
            {
                logger.Write(message);
            }
        }
    }
}
