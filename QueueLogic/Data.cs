using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Newtonsoft.Json;

namespace QueueLogic
{
    //данные принятые с формы
    public class Data
    {
        public bool FirstSoft { get; set; }
        public bool SecondSoft { get; set; }
        public bool ThirdSoft { get; set; }
   
        public string VirtualMachineSelected { get; set; }
        public string RegionSelected { get; set; }
        public string VMSize { get; set; }

        public string VirtualMachineName { get; set; }
        public string AdminUser { get; set; }
        public string Password { get; set; }
        public string CloudServiceName { get; set; }

        //для записи в базу в одну ячейку какой софт устанавливать
        public string Soft { get { 
            var result = string.Empty;
            if(FirstSoft == true)
            {
               var Frst = "skype ";
               result += Frst;
            }
            if(SecondSoft == true)
            {
               var Scnd = "winrar ";
               result += Scnd;
            }
            if(ThirdSoft == true)
            {
               var Trd = "notepad ";
               result += Trd;
            } return result;
        } }

        
        //параметры, передаваемые tcpclient сообщить что нужно установить
        public string GetParams()
        {
            string result = string.Empty;           

            result = string.Format("{0} {1} {2}", Convert.ToInt32(FirstSoft).ToString(),
                Convert.ToInt32(SecondSoft).ToString(), Convert.ToInt32(ThirdSoft).ToString());
                        
            return result;
        }
            
    }
}
