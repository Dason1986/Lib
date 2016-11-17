using NLog.Revicer.Listeners;
using NLog.Revicer.Models;
using NLog.Revicer.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Windows.Forms;

namespace NLog.Revicer
{
    public partial class MainSheetView : Form, ILogViwer
    {
        public MainSheetView()
        {
            InitializeComponent();
            dataGridView1.AutoGenerateColumns =
                dataGridView2.AutoGenerateColumns =
                dataGridView5.AutoGenerateColumns = false;
            listener = new UDPLogListener();
            listener.SetConfig( config,logic);
            listener.NewLog += Listener_NewLog;
        }

        private void Listener_NewLog(object sender, NewLogEventArgs e)
        {
            var method = System.Reflection.MethodBase.GetCurrentMethod();
            Console.WriteLine(method);
            Action de = () =>
            {
                try
                {

                    

                    var sourcelog = e.Log;

                    string filterlog = txtLogger.Text;
                    if (!string.IsNullOrWhiteSpace(filterlog) && !string.IsNullOrWhiteSpace(sourcelog.Message) && sourcelog.Message.IndexOf(filterlog, StringComparison.OrdinalIgnoreCase) == -1)
                    {
                        return;
                    }
                    var index = logtypes.IndexOf(sourcelog.LogType);
                    if (comboBox1.SelectedIndex != -1 && comboBox1.SelectedIndex > index) return;
                    var lines = (int)NudLines.Value - listView1.Items.Count;
                    if (lines < 0)
                    {

                        listView1.Items.Clear();

                    }
                    var item = new ListViewItem(new string[] { sourcelog .LogType, sourcelog.Time.ToString(), sourcelog.Logger, sourcelog.Message, sourcelog.Error!=null? sourcelog.Error.Message:sourcelog.Context } ,sourcelog.LogType);
                   
                    if (index > 0)
                    {

                        switch (sourcelog.LogType)
                        {
                            case "ERROR":

                            case "FATAL":


                            case "CLOSE":

                                item.BackColor = Color.Silver;
                                item.ForeColor = Color.Red;
                                break;
                            case "WARN":
                                item.BackColor = Color.Silver;
                                item.ForeColor = Color.Yellow;

                                break;
                        }
                    }

                    listView1.Items.Insert(0, item);
                }
                catch (Exception)
                {

                }
            };
            this.Invoke(de);
        }

     
        private void button1_Click(object sender, EventArgs e)
        {

            numericUpDown1.Enabled = button1.Enabled = listener.IsRunning;
            button2.Enabled = !button1.Enabled;

            if (!button1.Enabled)
            {

             config.Port = (int)numericUpDown1.Value;
                ThreadPool.QueueUserWorkItem(n =>
                            {
                                UDP();
                            });
            }
            else
            {
                listener.Stop();
            }


        }
        LogListener listener;

        private void UDP()
        {
            try
            {
                listener.Start();
            }
            catch (Exception)
            {

                throw;
            }


        }
        static IList<string> logtypes = new List<string> { "All", "TRACE", "DEBUG", "INFO", "WARN", "ERROR", "FATAL", "CLOSE" };

        ReprotLogLogic logic = new ReprotLogLogic();
        LogViewConfig config = new LogViewConfig();
        private void MainSheetView_Load(object sender, EventArgs e)
        {

            this.numericUpDown1.DataBindings.Add(new Binding("Value", config, "Port"));
            comboBox1.SelectedIndex = 0;
            dataGridView1.DataSource = logic.AmountLogers;
            dataGridView2.DataSource = logic.AmountLogers2;
            dataGridView5.DataSource = logic.AmountLogers3;
            //  System.ComponentModel.TypeDescriptor.AddProvider((new MyTypeDescriptionProvider<HTime>()), typeof(ReportLog));
            button1_Click(sender, e);
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
  

 
}
