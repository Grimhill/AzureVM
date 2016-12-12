using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Blob;
using System.Configuration;
using System.IO;

using Newtonsoft.Json;
using System.Web;
using Microsoft.WindowsAzure.Storage.Queue;
using System.Collections.Specialized;

using System.Collections.Concurrent;
using System.Threading;
using QueueLogic;
using CreateVM;
using TCPClient;
using ADWrite;

namespace WindowsServ
{
    public partial class Service1 : ServiceBase
    {
        //параллельный поток где проверяемстатус машины
        public static ConcurrentQueue<Data> CheckVM = new ConcurrentQueue<Data>();
        public Service1()
        {
            InitializeComponent();
        }

        //Для работы сервиса в нормальном режиме
        protected override void OnStart(string[] args)
        {
            /*
            //поток проверки очерели
            Thread thredi = new Thread(CheckQue);
            thredi.IsBackground = true;
            thredi.Start();
 
            //создание потока для мониторинга состояния машины
            Thread thred = new Thread(CheckVMQueue);
            thred.IsBackground = true;
            thred.Start();
            */
        }

        public void CheckQue()
        {
            //извлечение сообщения из очереди
            string connstring = ConfigurationManager.ConnectionStrings["AzureStorageConn"].ConnectionString;
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(connstring);

            //queue client
            CloudQueueClient client = storageAccount.CreateCloudQueueClient();

            //queue
            CloudQueue queue = client.GetQueueReference("queue");

            //мониторинг очереди на новые собщения
            while (true)
            {
                using (UserDBContext db = new UserDBContext())
                {
                    try
                    {
                        var cloudQueueMes = queue.GetMessage();
                        if (cloudQueueMes != null)
                        {
                            System.IO.File.AppendAllText(@"C:\log\log.txt", "i am going to read message" + "\n");
                            
                            //достаем содержимое сообщения и записываем в базу, само сообщение удаляем
                            string parameters = cloudQueueMes.AsString;
                            Data que = new System.Web.Script.Serialization.JavaScriptSerializer().Deserialize<Data>(parameters);
                            System.IO.File.AppendAllText(@"C:\log\log.txt", "I am read the mes" + "\n");
                            db.SaveData(que);
                            System.IO.File.AppendAllText(@"C:\log\log.txt", "data saved to BD" + "\n");
                            queue.DeleteMessage(cloudQueueMes);
                            System.IO.File.AppendAllText(@"C:\log\log.txt", "Message from queue deleted" + "\n");

                            //вызов создания нового объекта класса где создается виртуальная машина
                            CreateVM.VMManager VMcreater = new CreateVM.VMManager();
                            VMcreater.QuickCreateVM(que);
                            System.IO.File.AppendAllText(@"C:\log\log.txt", "VM has started to create" + "\n");

                            //write user, pasword, group and user to group in AD
                            ADWrite.ADWriter WriteToAD = new ADWrite.ADWriter();
                            WriteToAD.ADtoWrite(que);
                            System.IO.File.AppendAllText(@"C:\log\log.txt", "User added to domain" + "\n");
                            //вызов создания параллельного потока мониторинга статуса машины
                            CheckVM.Enqueue(que);

                        }
                        else
                        {
                            System.Threading.Thread.Sleep(1000);
                        }
                    }
                    catch (Exception e)
                    {
                        System.IO.File.AppendAllText(@"C:\log\log.txt", e.ToString() + "\n");
                    }
                }
            }
        }

        //Мониторинг состояния создаваемой машины в другом потоке
        public void CheckVMQueue()
        {
            while(true)
            {
                Data forCheck;
                List<Data> backtoqueue = new List<Data>(); 
                while (CheckVM.TryDequeue(out forCheck))
                {
                    CreateVM.VMManager VMcreater = new CreateVM.VMManager();
                    if (VMcreater.VMStatus(forCheck))
                    {
                        System.IO.File.AppendAllText(@"C:\log\log.txt", "VM created" + "\n");                     
                    }
                    else
                    {
                        backtoqueue.Add(forCheck);
                        System.IO.File.AppendAllText(@"C:\log\log.txt", "VM still creating" + "\n");
                        //CheckVM.Enqueue(forCheck);
                        //Thread.Sleep(30000);
                    }
                }
                
                    Thread.Sleep(10000);
                    foreach (var que in backtoqueue) //ложим обратно в очередь
                    {
                        CheckVM.Enqueue(que);
                    }
            }
        }

        //В режиме отладчика
        public void Process()
        {
            //поток проверки очерели
            Thread thredi = new Thread(CheckQue);
            thredi.IsBackground = false;
            thredi.Start();
            
            //создание потока для мониторинга состояния машины
            Thread thred = new Thread(CheckVMQueue);
            thred.IsBackground = false;
            thred.Start();            
        }

        protected override void OnStop()
        {
        }
    }
}
