using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;
using System.Net;
using System.Net.Sockets;

using QueueLogic;
using CreateVM;
namespace TCPClient
{
    public class TCPClass
    {
        public void TCPclie(Data qui, VMManager publicAdr)
        {
            //сообщение для скрипта что устанавливать
            string parameters = string.Empty;
            parameters = qui.GetParams();
            //внешний общедоступный адресс
            string pubaddr = string.Empty;
            pubaddr = publicAdr.VMPublicIP(qui);

            Connect(pubaddr, parameters);//с деплоймента можно вытянуть локальный IP            
        }

        public void Connect(String server, String message)
        {
            try
            {
                // Создаём TcpClient.
                // Для созданного в предыдущем проекте TcpListener 
                // Настраиваем его на IP нашего сервера и тот же порт.

                Int32 port = 5000;
                TcpClient client = new TcpClient(server, port);

                // Переводим наше сообщение в ASCII, а затем в массив Byte.
                Byte[] data = System.Text.Encoding.ASCII.GetBytes(message);

                // Получаем поток для чтения и записи данных.
                NetworkStream stream = client.GetStream();

                // Отправляем сообщение нашему серверу. 
                stream.Write(data, 0, data.Length);
                Console.WriteLine("Sent: {0}", message);

                // Получаем ответ от сервера.
                //Например, при переходе на странице Create мониторить состояние - пришел ответ или нет
                //Если пришел то выводим Ready
                // Буфер для хранения принятого массива bytes.
                data = new Byte[256];

                // Строка для хранения полученных ASCII данных.
                String responseData = String.Empty;

                // Читаем первый пакет ответа сервера. 
                // Можно читать всё сообщение.
                // Для этого надо организовать чтение в цикле как на сервере.
                Int32 bytes = stream.Read(data, 0, data.Length);
                responseData = System.Text.Encoding.ASCII.GetString(data, 0, bytes);
                Console.WriteLine("Received: {0}", responseData);

                // Закрываем всё.
                stream.Close();
                client.Close();
            }
            catch (ArgumentNullException e)
            {
                Console.WriteLine("ArgumentNullException: {0}", e);
            }
            catch (SocketException e)
            {
                Console.WriteLine("SocketException: {0}", e);
            }
        }
    }
}
