using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
//using System.Threading;

//using System.Diagnostics.Process;
namespace BT2 {


    public partial class Service1 : ServiceBase
    {
        
        Timer timer = new Timer();
        public Service1()
        {
            InitializeComponent();
        }
        //Start service 
        protected override void OnStart(string[] args)
        {           
            WriteToFile("Service is started at " + DateTime.Now);
                             
            DateTime time1 = new DateTime(2021, 3, 29, 20, 59, 59); //khởi tạo ngày để check 
            // start proceess
            var process = new Process
            {
                StartInfo = new ProcessStartInfo 
                {
                    FileName = "Notepad.exe"
                }

            };
            if (DateTime.Now < time1) // lên lịch (check điều kiện nếu ngày hiện tại bé hơn ngày điều kiện thì start process, ngược lại thì kill process )
            {
                
                WriteToFile("Start Process " + DateTime.Now);
                process.Start();
            }
            else
            {
                process.Kill();
                WriteToFile("Stop Process " + DateTime.Now); //stop process
            }                
            
            timer.Elapsed += new ElapsedEventHandler(OnElapsedTime);
            timer.Enabled = true;

        }
        private void OnElapsedTime(object source, ElapsedEventArgs e)
        {
            ServiceController service = new ServiceController("YourServiceName");
            service.Start();

        }
        //stop service
        protected override void OnStop()
        {
            WriteToFile("Service is stopped at " + DateTime.Now);
            timer.Elapsed += new ElapsedEventHandler(OffElapsedTime);
            // timer.Interval = 5000; //number in milisecinds 

            timer.Enabled = true;
        }
        private void OffElapsedTime(object source, ElapsedEventArgs e)
        {
            ServiceController service = new ServiceController("YourServiceName");
            service.Stop();
    
        }

        // Tạo file log
        public void WriteToFile(string Message)
        {
            string path = AppDomain.CurrentDomain.BaseDirectory + "\\Logs";
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            string filepath = AppDomain.CurrentDomain.BaseDirectory + "\\Logs\\ServiceLog_" + DateTime.Now.Date.ToShortDateString().Replace('/', '_') + ".txt";
            if (!File.Exists(filepath))
            {
                // Create a file to write to. 
                using (StreamWriter sw = File.CreateText(filepath))
                {
                    sw.WriteLine(Message);
                }
            }
            else
            {
                using (StreamWriter sw = File.AppendText(filepath))
                {
                    sw.WriteLine(Message);
                }
            }
        }

    }
}

