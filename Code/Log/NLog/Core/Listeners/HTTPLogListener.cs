using System;
using System.Net;

namespace NLog.Revicer.Listeners
{

    public class HTTPLogListener : LogListener
    {
        protected override void OnInit()
        {
            HttpWeb();
        }
        System.Net.HttpListener listener = new System.Net.HttpListener();
        protected override void OnStop()
        {
            base.OnStop();
            listener.Stop();
        }
        private void HttpWeb()
        {
            try
            {



                //else
                {
                    listener.Prefixes.Add(string.Format("http://localhost:{0}/", Config.Port));
                }
                listener.Start();

                while (IsRunning)
                {
                    try
                    {
                        HttpListenerContext context = listener.GetContext();
                        var request = context.Request;
                        var st = request.InputStream;
                        byte[] data = new byte[request.ContentLength64];
                        st.Read(data, 0, data.Length);
                        ReceiveNewLog(data);
                    }
                    catch (Exception e)
                    {

                        Console.WriteLine("ERROR: {0}", e.ToString());
                        Console.ReadLine();
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