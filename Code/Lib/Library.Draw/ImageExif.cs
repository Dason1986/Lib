using System;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using Library.Att;
using Library.Map;

namespace Library.Draw
{
    /// <summary>
    /// 
    /// </summary>
    public class ImageExif
    {
        #region FileInfo

        //40092
        /// <summary>
        /// 
        /// </summary>
        [LanguageCategory("FileInfo"), LanguageDisplayName(@"Comment")]
        public string Comment { get; set; }

        //40093
        /// <summary>
        /// 
        /// </summary>
        [LanguageCategory("FileInfo"), LanguageDisplayName(@"Author")]
        public string Author { get; set; }

        //40094
        /// <summary>
        /// 
        /// </summary>
        [LanguageCategory("FileInfo"), LanguageDisplayName(@"Keyword")]
        public string Keyword { get; set; }

        //40095
        /// <summary>
        /// 
        /// </summary>
        [LanguageCategory("FileInfo"), LanguageDisplayName(@"Subject")]
        public string Subject { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [LanguageCategory("FileInfo"), LanguageDisplayName(@"像素")]
        public Size? Pix { get; set; }
        //40091
        /// <summary>
        /// 
        /// </summary>
        [LanguageCategory("FileInfo"), LanguageDisplayName(@"Title")]
        public string Title { get; set; }

        //33432 
        /// <summary>
        /// 
        /// </summary>
        [LanguageCategory("FileInfo"), LanguageDisplayName(@"Copyright")]
        public string Copyright { get; set; }
        //315
        //     public string Artist { get; set; }
        //18246 星级
        /// <summary>
        /// 
        /// </summary>
        [LanguageCategory("FileInfo"), LanguageDisplayName(@"星级")]
        public int Rating { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// 36864
        [LanguageCategory("FileInfo"), LanguageDisplayName(@"Exif版本")]
        public string ExifVersion { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// 270 
        [LanguageCategory("FileInfo"), LanguageDisplayName(@"Description")]
        public string Description { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [LanguageCategory("FileInfo"), LanguageDisplayName(@"RawFormatID")]
        public Guid RawFormatID { get; set; }
        readonly static Guid BMP = new Guid("B96B3CAB-0728-11D3-9D7B-0000F81EF32E");
        readonly static Guid PNG = new Guid("B96B3CAF-0728-11D3-9D7B-0000F81EF32E");
        readonly static Guid GIF = new Guid("B96B3CB0-0728-11D3-9D7B-0000F81EF32E");
        readonly static Guid JPEG = new Guid("B96B3CAE-0728-11D3-9D7B-0000F81EF32E");
        readonly static Guid TIFF = new Guid("B96B3CB1-0728-11D3-9D7B-0000F81EF32E");
        /// <summary>
        /// 
        /// </summary>
        [LanguageCategory("FileInfo"), LanguageDisplayName(@"RawFormat")]
        public string RawFormat
        {
            get
            {
                if (RawFormatID == BMP) return "BMP";
                if (RawFormatID == PNG) return "PNG";
                if (RawFormatID == GIF) return "GIF";
                if (RawFormatID == JPEG) return "JPEG";
                if (RawFormatID == TIFF) return "TIFF";
                return "unkwon";
            }
        }
        #endregion

        #region 相機信息

        //271 镜头公司
        /// <summary>
        /// 
        /// </summary>
        [Category("相機信息"), DisplayName(@"相機廠商")]
        public string EquipmentMake { get; set; }

        //272 镜头型号
        /// <summary>
        /// 
        /// </summary>
        [Category("相機信息"), DisplayName(@"相機型号")]
        public string EquipmentModel { get; set; }

        #endregion






        #region base
        /// <summary>
        /// 
        /// </summary>
        [Category("圖像信息"), DisplayName(@"分辨率单元")]
        public int ResolutionUnit { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [Category("圖像信息"), DisplayName(@"YCbCr定位")]
        public int YCbCrPositioning { get; set; }
        //41994 清晰度(一般\柔和\强烈)
        /// <summary>
        /// 
        /// </summary>
        [Category("圖像信息"), DisplayName(@"清晰度")]
        public int Sharpness { get; set; }
        //37384 光源
        /// <summary>
        /// 
        /// </summary>
        [Category("圖像信息"), DisplayName(@"光源")]
        public int LightSource { get; set; }
        //34850 曝光方式(無\手動\一般\光圈先決\快門先決\景深優先\快門優先\直向模式\橫向模式)
        /// <summary>
        /// 
        /// </summary>
        [Category("圖像信息"), DisplayName(@"曝光方式")]
        public int ExposureProgram { get; set; }
        //41989 35mm胶卷
        /// <summary>
        /// 
        /// </summary>
        [Category("圖像信息"), DisplayName(@"35mm胶卷")]
        public int FocalLengthIn35mmFilm { get; set; }
        //20625
        /// <summary>
        /// 
        /// </summary>
        [Category("圖像信息"), DisplayName(@"色度表")]
        public int ChrominanceTable { get; set; }
        //20624
        /// <summary>
        /// 
        /// </summary>
        [Category("圖像信息"), DisplayName(@"亮度表")]
        public int LuminanceTable { get; set; }
        //36867
        /// <summary>
        /// 
        /// </summary>
        [Category("圖像信息"), DisplayName(@"拍攝時間")]
        public DateTime? DateTimeOriginal { get; set; }
        //36868
        /// <summary>
        /// 
        /// </summary>
        [Category("圖像信息"), DisplayName(@"建立時間")]
        public DateTime? DateTimeDigitized { get; set; }
        //34855 光圈
        /// <summary>
        /// 
        /// </summary>
        [Category("圖像信息"), DisplayName(@"ISO光圈")]
        public int ISOSpeedRatings { get; set; }
        //37383 测光模式
        /// <summary>
        /// 
        /// </summary>
        [Category("圖像信息"), DisplayName(@"测光模式")]
        public int MeteringMode { get; set; }

        //37385 闪光灯模式
        /// <summary>
        /// 
        /// </summary>
        [Category("圖像信息"), DisplayName(@"闪光灯模式")]
        public int Flash { get; set; }
        //41987 白平衡(手动\自动)
        /// <summary>
        /// 
        /// </summary>
        [Category("圖像信息"), DisplayName(@"白平衡")]
        public int WhiteBalance { get; set; }
        //41992 比对(标准\柔和\强烈)
        /// <summary>
        /// 
        /// </summary>
        [Category("圖像信息"), DisplayName(@"比对")]
        public int Contrast { get; set; }

        //41993 饱和度(标准\低饱和\高饱和)
        /// <summary>
        /// 
        /// </summary>
        [Category("圖像信息"), DisplayName(@"饱和度")]
        public int Saturation { get; set; }

        /// <summary>
        /// 282,283
        /// </summary>
        [Category("圖像信息"), DisplayName(@"解析度")]
        public Size? Resolution { get; set; }

        /// <summary>
        /// 37377
        /// </summary>
        [Category("圖像信息"), DisplayName(@"快门速度")]
        public double ShutterSpeed { get; set; }
        /// <summary>
        /// 33434
        /// </summary>
        [Category("圖像信息"), DisplayName(@"曝光时间")]
        public double ExposureTime { get; set; }
        #endregion

        #region Thumbnail

        /// <summary>
        /// 20525,20526
        /// </summary>
        [Category("縮略圖信息"), DisplayName(@"解析度")]
        public Size? ThumbnailResolution { get; set; }

        /// <summary>
        /// 20528
        /// </summary>
        [Category("縮略圖信息"), DisplayName(@"解析度单元")]
        public int ThumbnailResolutionUnit { get; set; }

        /// <summary>
        /// 20515
        /// </summary>
        [Category("縮略圖信息"), DisplayName(@"压缩")]
        public int ThumbnailCompression { get; set; }

        /// <summary>
        /// 20507
        /// </summary>
        [Category("縮略圖信息"), DisplayName(@"縮略圖")]
        public byte[] ThumbnailData { get; set; }

        #endregion

        #region GPS
        /// <summary>
        /// 
        /// </summary>
        [Category("地理信息"), DisplayName(@"GPS坐標")]
        public GPSGeo GPS { get; set; }
        #endregion
        /// <summary>
        /// 
        /// </summary>
        /// <param name="image"></param>
        /// <returns></returns>
        public static ImageExif GetInfo(Image image)
        {
            //http://www.exiv2.org/tags.html
            ImageExif exif = new ImageExif();

            exif.RawFormatID = image.RawFormat.Guid;
            foreach (int hex in image.PropertyIdList)
            {
                switch (hex)
                {
                    case 40091: exif.Title = GetStringAsc(image, hex); break;
                    case 40092: exif.Comment = GetStringUnicode(image, hex); break;
                    case 40093: exif.Author = GetStringAsc(image, hex); break;
                    case 40094: exif.Keyword = GetStringUnicode(image, hex); break;
                    case 40095: exif.Subject = GetStringUnicode(image, hex); break;
                    case 33432: exif.Copyright = GetStringAsc(image, hex); break;
                    case 270: exif.Description = GetStringAsc(image, hex); break;
                    case 271: exif.EquipmentMake = GetStringAsc(image, hex); break;
                    case 272: exif.EquipmentModel = GetStringAsc(image, hex); break;
                    case 34850: exif.ExposureProgram = GetShort(image, hex); break;
                    case 34855: exif.ISOSpeedRatings = GetShort(image, hex); break;
                    case 37384: exif.Flash = GetShort(image, hex); break;
                    case 37385: exif.LightSource = GetShort(image, hex); break;
                    case 37383: exif.MeteringMode = GetShort(image, hex); break;
                    case 18246: exif.Rating = GetShort(image, hex); break;
                    case 41987: exif.WhiteBalance = GetShort(image, hex); break;
                    case 41992: exif.Contrast = GetShort(image, hex); break;
                    case 41993: exif.Saturation = GetShort(image, hex); break;
                    case 41994: exif.Sharpness = GetShort(image, hex); break;
                    case 33434: exif.ExposureTime = GetDouble(image, hex); break;
                    case 41989: exif.FocalLengthIn35mmFilm = GetShort(image, hex); break;
                    case 36867: exif.DateTimeOriginal = GetDateTime(image, hex); break;
                    case 37377: exif.ShutterSpeed = GetDouble(image, hex); break;
                    case 36868: exif.DateTimeDigitized = GetDateTime(image, hex); break;
                    case 36864: exif.ExifVersion = GetStringAsc(image, hex); break;
                    case 531: exif.YCbCrPositioning = GetShort(image, hex); break;
                    case 20625: exif.ChrominanceTable = GetShort(image, hex); break;
                    case 20624: exif.LuminanceTable = GetShort(image, hex); break;
                    case 20507: exif.ThumbnailData = image.GetPropertyItem(hex).Value; break;
                    case 20528: exif.ThumbnailResolutionUnit = GetShort(image, hex); break;
                    case 20515: exif.ThumbnailCompression = GetShort(image, hex); break;
                    case 296: exif.ResolutionUnit = GetShort(image, hex); break;
                    case 282:
                    case 283:
                        {
                            if (exif.Resolution == null)
                                exif.Resolution = new Size(GetInt(image, 282), GetInt(image, 283)); break;
                        }
                    case 20525:
                    case 20526:
                        {
                            if (exif.Pix == null)
                                exif.ThumbnailResolution = new Size(GetInt(image, 20525), GetInt(image, 20526)); break;
                        }
                    case 40962:
                    case 40963:
                        {
                            if (exif.Pix == null)
                                exif.Pix = new Size(GetInt(image, 40962), GetInt(image, 40963)); break;
                        }

                    case 1:
                    case 2:
                    case 3:
                    case 4:
                    case 5:
                    case 6:
                    case 29:
                        {
                            if (exif.GPS == null && image.PropertyIdList.Contains(1) && image.PropertyIdList.Contains(2) && image.PropertyIdList.Contains(3) && image.PropertyIdList.Contains(4) && image.PropertyIdList.Contains(5) && image.PropertyIdList.Contains(6))
                            {

                                var Gps = new GPSGeo()
                                {

                                    LatitudeRef = GetStringAsc(image, 1),
                                    LongitudeRef = GetStringAsc(image, 3),
                                    AltitudeRef = GetStringAsc(image, 5),
                                    Latitude = GetDouble(image, 2),
                                    Longitude = GetDouble(image, 4),
                                    Altitude = GetDouble(image, 6),
                                };
                                if (image.PropertyIdList.Contains(29)) Gps.DateStamp = DateTime.ParseExact(GetStringAsc(image, 29), "yyyy:MM:dd", null);
                                exif.GPS = Gps;
                            }

                            break;
                        }
                    //default:
                    //    {
                    //        var property = image.GetPropertyItem(hex);
                    //        switch (property.Type)
                    //        {
                    //            case 3: Console.WriteLine("short {0}:{1}", hex, GetShort(image, hex));
                    //                break;
                    //            case 2: Console.WriteLine("string_asc {0}:{1}", hex, GetStringAsc(image, hex));
                    //                break;
                    //            case 5:
                    //                Console.WriteLine("deouble {0}:{1}", hex, GetDouble(image, hex));
                    //                break;
                    //            default:
                    //                Console.Write("type:{0} hex:{1} len:{2}", property.Type, hex, property.Len);
                    //                switch (property.Len)
                    //                {
                    //                    default:
                    //                        Console.WriteLine("[{0}]", GetStringUnicode(image, hex));
                    //                        break;
                    //                    case 4:

                    //                    case 8:
                    //                        Console.WriteLine("[{0:f8}]", GetDouble(image, hex));
                    //                        break;
                    //                }


                    //                break;
                    //        }
                    //        break;
                    //    }

                }
            }
            return exif;
        }



        public static string GetMeteringModeName(int meteringMode)
        {
            switch (meteringMode)
            {
                case 0: return "無";
                case 1: return "平均測光";
                case 2: return "中央偏重平均測光";
                case 3: return "测光";
                case 4: return "多点測光";
                case 5: return "分區測光";
                case 6: return "局部測光";
            }
            return string.Empty;
        }
        public static string GetLightSourceName(int LightSource)
        {
            switch (LightSource)
            {
                case 0: return "无";
                case 1: return "日光";
                case 2: return "螢光燈";
                case 3: return "鵝絲類";
                case 17: return "標準照度A";
                case 18: return "標準照度B";
                case 19: return "標準照度C";
                case 20: return "D55";
                case 21: return "D65";
                case 22: return "D75";
            }
            return string.Empty;
        }
        public static string GetFlashName(int flash)
        {
            switch (flash)
            {
                case 0: return "無閃光燈";
                case 1: return "閃光燈";
                case 5: return "閃光燈,無回應閃光";
                case 7: return "閃光燈,回應閃光";
                case 9: return "閃光燈,强制";
                case 13: return "閃光燈,强制,無回應閃光";
                case 15: return "閃光燈,强制,回應閃光";
                case 16: return "無光灯,强制";
                case 24: return "無光灯,自动";
                case 25: return "閃光燈,自动";
                case 29: return "閃光燈,自动,無回應閃光";
                case 31: return "閃光燈,自动,回應閃光";
                case 32: return "無閃光燈功能";
                case 65: return "閃光燈,紅眼";
                case 69: return "閃光燈,紅眼,無回應閃光";
                case 71: return "閃光燈,紅眼,回應閃光";
                case 73: return "閃光燈,强制,紅眼";
                case 77: return "閃光燈,强制,紅眼,無回應閃光";
                case 79: return "閃光燈,强制,紅眼,回應閃光";
                case 83: return "閃光燈,自动,紅眼";
                case 93: return "閃光燈,自动,紅眼,無回應閃光";
                case 95: return "閃光燈,自动,紅眼,回應閃光";
            }
            return string.Empty;
        }
        static DateTime? GetDateTime(Image getImage, int hex)
        {




            string dateTakenTag = Encoding.ASCII.GetString(getImage.GetPropertyItem(hex).Value);
            string[] parts = dateTakenTag.Split(':', ' ');
            int year = int.Parse(parts[0]);
            int month = int.Parse(parts[1]);
            int day = int.Parse(parts[2]);
            int hour = int.Parse(parts[3]);
            int minute = int.Parse(parts[4]);
            int second = int.Parse(parts[5]);
            return new DateTime(year, month, day, hour, minute, second);
        }
        static short GetShort(Image getImage, int hex)
        {

            return BitConverter.ToInt16(getImage.GetPropertyItem(hex).Value, 0);

        }
        static int GetInt(Image getImage, int hex)
        {

            return BitConverter.ToInt32(getImage.GetPropertyItem(hex).Value, 0);

        }
        static double GetDouble(Image getImage, int hex)
        {
            var propety = getImage.GetPropertyItem(hex);
            //     if (propety.Type != 5) return 0;
            var value = propety.Value;
            switch (propety.Value.Length)
            {
                case 4:
                    return BitConverter.ToUInt32(value, 0);
                case 24:
                    {
                        float degrees = BitConverter.ToUInt32(value, 0) / (float)BitConverter.ToUInt32(value, 4);

                        float minutes = BitConverter.ToUInt32(value, 8) / (float)BitConverter.ToUInt32(value, 12);

                        float seconds = BitConverter.ToUInt32(value, 16) / (float)BitConverter.ToUInt32(value, 20);
                        float coorditate = degrees + (minutes / 60f) + (seconds / 3600f);
                        return coorditate;
                    }
                case 8:
                    {
                        return BitConverter.ToUInt32(value, 0) / (float)BitConverter.ToUInt32(value, 4);
                    }
            }
            return 0;
        }
        static string GetStringUnicode(Image getImage, int hex)
        {

            string dateTakenTag = Encoding.Unicode.GetString(getImage.GetPropertyItem(hex).Value);
            return dateTakenTag.Replace("\0", string.Empty);
        }
        static string GetStringAsc(Image getImage, int hex)
        {

            string dateTakenTag = Encoding.ASCII.GetString(getImage.GetPropertyItem(hex).Value);
            return dateTakenTag.Replace("\0", string.Empty);
        }
    }
}