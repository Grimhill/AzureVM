using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Management.Automation;
using System.Management.Automation.Runspaces;
using System.Diagnostics;

using QueueLogic;


//Для выполнения скрипта нужно включить разрешение выполнения сценариев

//gpedit.msc
//Административные шаблоны\Компоненты Windows\Windows PowerShell\включить выполнение сценариев

namespace UsePowerShellScr
{
    public class CallScript
    {
        //static void Main(string[] args)
        //{
        //    powershellscpript();
        //}
        //param([string] $UserName, [string] $SubscriptionName, [string] $ServiceName, [string] $Name, [bool] $FirstSoft, [bool] $SecondSoft, [bool] $ThirdSoft)
        /*private void ExtractParameters(Data que, CommandParameterCollection someparam)
        {
            someparam.Add("UserName", que.AdminUser);
            someparam.Add("SubscriptionName", "BizSpark");
            someparam.Add("ServiceName", que.CloudServiceName);
            someparam.Add("Name", que.VirtualMachineName);
            someparam.Add("FirstSoft", que.FirstSoft);
            someparam.Add("SecondSoft", que.SecondSoft);
            someparam.Add("ThirdSoft", que.ThirdSoft);

        }
        //scriptParameters передавать по одному нужные?
        public void PowershellScript(Data que)
        {
            string scriptpath = @"C:\Users\Алексей\Documents\Powershell\firstscript.ps1";
            //string scriptfile = "some path";
            RunspaceConfiguration runspaceConfiguration = RunspaceConfiguration.Create();

            Runspace runspace = RunspaceFactory.CreateRunspace(runspaceConfiguration);
            runspace.Open();

            RunspaceInvoke scriptInvoker = new RunspaceInvoke(runspace);

            Pipeline pipeline = runspace.CreatePipeline();

            //Here's how you add a new script with arguments
            Command myCommand = new Command(scriptpath);
            ExtractParameters(que, myCommand.Parameters);
            //myCommand.Parameters.Add("FirstSoft", "$true");
            
            //CommandParameter testParam = new CommandParameter("UserName", "value");
            //myCommand.Parameters.Add(testParam);

            pipeline.Commands.Add(myCommand);

            // Execute PowerShell script
            var results = pipeline.Invoke();
        }*/
        
        public void PowershellScript(Data que)
        {
            string Params = string.Empty;
            Params = que.GetParams();

            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.FileName = @"powershell.exe"; //если что посмотреть на виртуалке расположение
            //скрипт для работы с вм
            //startInfo.Arguments = @"& 'C:\Users\Алексей\Documents\Powershell\testscript.ps1' " + Params;
            //пробный маленький скрипт
            startInfo.Arguments = @"& 'C:\Users\Алексей\Documents\Powershell\firstscript.ps1' 1";// +Params;
            startInfo.RedirectStandardOutput = true;
            startInfo.RedirectStandardError = true;
            startInfo.UseShellExecute = false;
            startInfo.CreateNoWindow = true;
            Process process = new Process();
            process.StartInfo = startInfo;
            process.Start();

            string output = process.StandardOutput.ReadToEnd();
            //Assert.IsTrue(output.Contains("StringToBeVerifiedInAUnitTest"));

            string errors = process.StandardError.ReadToEnd();
            //Assert.IsTrue(string.IsNullOrEmpty(errors));
            //Console.WriteLine("Norma");
        }
    }
}
