using System;
using System.Collections;
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
            List<DictionaryEntry> list = new List<DictionaryEntry>();
            list.AddRange(CalendarInfo.LunarHolidays.Select(holiday => new DictionaryEntry(holiday.HolidayName, holiday)));
            list.AddRange(CalendarInfo.WeekHolidays.Select(holiday => new DictionaryEntry(holiday.HolidayName, holiday)));
            list.AddRange(CalendarInfo.SolarHolidays.Select(holiday => new DictionaryEntry(holiday.HolidayName, holiday)));
            listBox1.DataSource = list;
            listBox1.DisplayMember = "Key";
            listBox1.ValueMember = "Value";

        }



        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            grid.SelectedObject = new ChineseDateTime(this.dateTimePicker1.Value);
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            IHoliday selectedValue = listBox1.SelectedValue as IHoliday;
            if (selectedValue == null) return;
            var holiday = selectedValue;
            dateTimePicker1.Value = holiday.ConvertDateTime(dateTimePicker2.Value.Year);
        }
    }
}
