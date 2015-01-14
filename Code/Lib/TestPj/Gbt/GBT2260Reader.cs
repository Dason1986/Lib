using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace TestPj.Gbt
{
    /// <summary>
    /// 
    /// </summary>
    public class GBT2260Reader
    {
        public void BuilderCode(string path)
        {
            var reader = System.IO.File.ReadLines(path, Encoding.UTF8);
            var str = new StringWriter();
            XmlTextWriter xtw = new XmlTextWriter(str);
            xtw.WriteStartDocument();
            xtw.WriteStartElement("Code");
            string currnetProvince = null;
            string currnetCity = null;
            int prcount = 0, citycount = 0;
            foreach (string cont in reader)
            {
                if (string.IsNullOrEmpty(cont)) continue;

                string provinceCode = cont.Substring(0, 2);
                if (currnetProvince != provinceCode)
                {
                    if (prcount > 0)
                    {
                        for (int i = 0; i < prcount + citycount; i++)
                        {
                            xtw.WriteEndElement();
                        }
                        citycount = 0;
                        prcount = 0;
                    }
                    currnetProvince = provinceCode;
                    xtw.WriteStartElement("Province");
                    prcount++;
                    xtw.WriteAttributeString("ID", provinceCode);
                    xtw.WriteAttributeString("Full", cont.Substring(0, 6));
                    xtw.WriteAttributeString("Name", cont.Substring(6).Trim());
                    continue;
                    // xtw.WriteEndAttribute();
                }

                string cityCode = cont.Substring(2, 2);
                if (currnetCity != cityCode)
                {
                    if (citycount > 0)
                    {
                        for (int i = 0; i < citycount; i++)
                        {
                            xtw.WriteEndElement();
                        }

                        citycount = 0;
                    }
                    currnetCity = cityCode;
                    xtw.WriteStartElement("City");
                    citycount++;
                    xtw.WriteAttributeString("ID", cityCode);
                    xtw.WriteAttributeString("Full", cont.Substring(0, 6));
                    xtw.WriteAttributeString("Name", cont.Substring(6).Trim());
                    continue;
                }

                string countyCode = cont.Substring(4, 2);
                xtw.WriteStartElement("County");
                xtw.WriteAttributeString("ID", countyCode);
                xtw.WriteAttributeString("Full", cont.Substring(0, 6));
                xtw.WriteAttributeString("Name", cont.Substring(6).Trim());
                xtw.WriteEndElement();
            }
            xtw.WriteEndElement();
            xtw.WriteEndDocument();
            var context = str.ToString();
            Console.WriteLine(context);
        }
    }
}
