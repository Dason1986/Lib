using Library.Date;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;

namespace TestWinform
{
    public partial class DateForm : Form
    {
        public DateForm()
        {
            InitializeComponent();
            List<DictionaryEntry> list = new List<DictionaryEntry>();
            list.Add(new DictionaryEntry("------農      曆------", ""));
            list.AddRange(CalendarInfo.LunarHolidays.Select(holiday => new DictionaryEntry(holiday.HolidayName, holiday)));
            list.Add(new DictionaryEntry("------星      期------", ""));
            list.AddRange(CalendarInfo.WeekHolidays.Select(holiday => new DictionaryEntry(holiday.HolidayName, holiday)));
            list.Add(new DictionaryEntry("------西      曆------", ""));
            list.AddRange(CalendarInfo.SolarHolidays.Select(holiday => new DictionaryEntry(holiday.HolidayName, holiday)));
            list.Add(new DictionaryEntry("------二十四節氣------", ""));
            list.AddRange(CalendarInfo.TheSolarTermsHolidays.Select(holiday => new DictionaryEntry(holiday.HolidayName, holiday)));
            listBox1.DataSource = list;
            listBox1.DisplayMember = "Key";
            listBox1.ValueMember = "Value";
            comboBox1.SelectedIndex = 0;
            listBox1.SelectedIndex = 1;
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
            label1.Text = selectedValue.ToString(comboBox1.SelectedItem.ToString(), checkBox1.Checked ? CultureInfo.GetCultureInfo("zh-CN") : CultureInfo.GetCultureInfo("en-US"));
        }
    }
}