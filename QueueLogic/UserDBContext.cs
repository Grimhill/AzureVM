using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Configuration;
using System.Data;
using System.Data.SqlClient;


namespace QueueLogic
{
    //работа с azure SQL
    public class UserDBContext: IDisposable
    {
        public string connectionString;
        public UserDBContext ()
        {
            connectionString = ConfigurationManager.ConnectionStrings["UsersDB"].ConnectionString;
        }
        public void SaveData(Data data)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                
                connection.Open();
                //команд текст присвоения данных
                var commandText = string.Format("insert into UsersDB (adminUser, UserPassword, VMName, VMtype, VMSize, Region, Soft, CloudServiceName, Date) values ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}', '{7}', @Date)", 
                    data.AdminUser, data.Password, data.VirtualMachineName, data.VirtualMachineSelected, data.VMSize, data.RegionSelected, data.Soft, data.CloudServiceName);
                //для столца времени произведения записи
                using (SqlCommand command = new SqlCommand(commandText,connection)) 
                {
                    command.Parameters.Add(new SqlParameter()
                        {
                            Value = DateTime.Now,
                            ParameterName = "@Date"
                        });
                    command.ExecuteNonQuery();
                }
            }
        }
        public void Dispose ()
        {

        }
    }
}
