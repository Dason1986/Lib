using System;

namespace Library.Map
{
    /// <summary>
    /// 北斗卫星导航系统
    /// </summary>
    public class BDSGeo
    {

    }
    /// <summary>
    /// 伽利略定位系统
    /// </summary>
    public class GalileGeo
    {

    }

    /// <summary>
    /// 全球卫星定位系统
    /// </summary>
    public class GPSGeo
    {
        /// <summary>
        /// 
        /// </summary>
        public int VersionID { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string LatitudeRef { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public double Latitude { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string LongitudeRef { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string AltitudeRef { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public double Longitude { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public double Altitude { get; set; }

        public DateTime DateStamp { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="format"></param>
        /// <returns></returns>
        public string ToString(string format)
        {
            return string.Format(GPSFormat.FormatProvider, format, this);
        }

        /// <summary>
        /// 
        /// </summary> 
        /// <returns></returns>
        public override string ToString()
        {
            return GPSFormat.FormatProvider.Format("g", this, null);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="format"></param>
        /// <param name="formatProvider"></param>
        /// <returns></returns>
        public string ToString(string format, IFormatProvider formatProvider)
        {
            return GPSFormat.FormatProvider.Format(format, this, formatProvider);

        }

        /// <summary>
        /// 
        /// </summary>
        public class GPSFormat : ICustomFormatter, IFormatProvider
        {
            /*  
     
    
            Format              Description                             Real format                             Example
            =========           =================================       ======================                  =======================     
            "g"                 google map
            "f"
 49°30'00" S 12°30'00" E
        */


            /// <summary>
            /// 
            /// </summary>
            protected GPSFormat()
            {

            }
            /// <summary>
            /// 
            /// </summary>
            public static readonly GPSFormat FormatProvider = new GPSFormat();
            public object GetFormat(Type formatType)
            {
                return formatType == typeof(ICustomFormatter) ? this : null;
            }

            public string Format(string format, object arg, IFormatProvider formatProvider)
            {
                if (arg is GPSGeo == false) throw new CodeException(11002.106);
                GPSGeo gps = arg as GPSGeo;



                switch (format[0])
                {
                    case 'g': return string.Format("{0}{1:f6},{2}{3:f6}", gps.LatitudeRef == "N" ? "" : "-", gps.Latitude, gps.LongitudeRef == "E" ? "" : "-", gps.Longitude);
                    case 'f': return string.Format("{0}{1:f6},{2}{3:f6},{4}{5}", gps.LatitudeRef, gps.Latitude, gps.LongitudeRef, gps.Longitude, gps.AltitudeRef == "1" ? "-" : "", gps.Altitude);
                    //case 'G': return string.Format("{0}{1:f6},{2}{3:f6}", gps.LatitudeRef == "N" ? "" : "-", gps.Latitude, gps.LongitudeRef == "E" ? "" : "-", gps.Longitude);
                    //case 'F': return string.Format("{0}{1:f6},{2}{3:f6},{4}{5}", gps.LatitudeRef, gps.Latitude, gps.LongitudeRef, gps.Longitude, gps.AltitudeRef == "1" ? "-" : "", gps.Altitude);
                    default: return string.Format("{0}{1:f6},{2}{3:f6},{5}{4}", gps.LatitudeRef == "N" ? "" : "-", gps.Latitude, gps.LongitudeRef == "E" ? "" : "-", gps.Longitude, gps.AltitudeRef == "1" ? "-" : "", gps.Altitude);
                }


            }









        }
    }
}