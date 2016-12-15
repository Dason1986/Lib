using System;
using System.Net;
using System.Net.Sockets;

namespace NLog.Revicer.Listeners
{

    public class TCPLogListener : LogListener
    {

        protected override void OnInit()
        {
            try
            {
                TcpListener server = new TcpListener(IPAddress.Any, Config.Port);
                server.Start();


                Byte[] bytes = new Byte[Config.BuffSize];
             

                while (IsRunning)
                {
                    try
                    {
                        TcpClient client = server.AcceptTcpClient();
                     

                 

                     
                        NetworkStream stream = client.GetStream();

                        int i;
                        stream.Read(bytes, 0, bytes.Length);
                       
                          

                        // Shutdown and end connection
                        client.Close();
                        ReceiveNewLog(bytes,null);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex);

                    }
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine("ERROR: {0}", ex.ToString());
                Console.ReadLine();
            }
        }


    }
}