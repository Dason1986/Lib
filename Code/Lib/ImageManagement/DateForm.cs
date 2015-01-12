using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Library.Date;

namespace ImageManagement
{
    public partial class DateForm : Form
    {
        public DateForm()
        {
            InitializeComponent();
            dateTimePicker1_ValueChanged(this,EventArgs.Empty);
        }

      

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            grid.SelectedObject = new ChineseCalendar(this.dateTimePicker1.Value);
        }
    }
}
