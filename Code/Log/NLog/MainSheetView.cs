using NLog.Revicer.Logic;
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
    public partial class MainSheetView : Form
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
                    var item = new ListViewItem(sourcelog.SourceItems);
                    item.ImageKey = sourcelog.LogType;
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
        UDPLogListener listener;

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
    public class MyTypeDescriptionProvider<T> : TypeDescriptionProvider
    {
        private ICustomTypeDescriptor td;
        public MyTypeDescriptionProvider()
            : this(TypeDescriptor.GetProvider(typeof(ReportLog)))
        {
        }
        public MyTypeDescriptionProvider(TypeDescriptionProvider parent)
            : base(parent)
        {
        }
        public override ICustomTypeDescriptor GetTypeDescriptor(Type objectType, object instance)
        {
            if (td == null)
            {
                td = base.GetTypeDescriptor(objectType, instance);
                td = new MyCustomTypeDescriptor(td, typeof(T));
            }
            return td;
        }
    }

    public class SubPropertyDescriptor : PropertyDescriptor
    {
        private PropertyDescriptor _subPD;
        private PropertyDescriptor _parentPD;

        public SubPropertyDescriptor(PropertyDescriptor parentPD, PropertyDescriptor subPD, string pdname)
            : base(pdname, null)
        {
            _subPD = subPD;
            _parentPD = parentPD;
        }

        public override bool IsReadOnly { get { return false; } }
        public override void ResetValue(object component) { }
        public override bool CanResetValue(object component) { return false; }
        public override bool ShouldSerializeValue(object component)
        {
            return true;
        }

        public override Type ComponentType
        {
            get { return _parentPD.ComponentType; }
        }
        public override Type PropertyType { get { return _subPD.PropertyType; } }

        public override object GetValue(object component)
        {
            return _subPD.GetValue(_parentPD.GetValue(component));
        }

        public override void SetValue(object component, object value)
        {
            _subPD.SetValue(_parentPD.GetValue(component), value);
            OnValueChanged(component, EventArgs.Empty);
        }
    }

    public class MyCustomTypeDescriptor : CustomTypeDescriptor
    {
        Type typeProperty;
        public MyCustomTypeDescriptor(ICustomTypeDescriptor parent, Type type)
            : base(parent)
        {
            typeProperty = type;
        }
        public override PropertyDescriptorCollection GetProperties(Attribute[] attributes)
        {
            PropertyDescriptorCollection cols = base.GetProperties(attributes);

            string propertyName = "";
            foreach (PropertyDescriptor col in cols)
            {
                if (col.PropertyType.Name == typeProperty.Name)
                    propertyName = col.Name;
            }
            PropertyDescriptor pd = cols[propertyName];
            PropertyDescriptorCollection children = pd.GetChildProperties();
            PropertyDescriptor[] array = new PropertyDescriptor[cols.Count + children.Count];
            int count = cols.Count;
            cols.CopyTo(array, 0);

            foreach (PropertyDescriptor cpd in children)
            {
                array[count] = new SubPropertyDescriptor(pd, cpd, pd.Name + "_" + cpd.Name);
                count++;
            }

            PropertyDescriptorCollection newcols = new PropertyDescriptorCollection(array);
            return newcols;
        }
    }
}
