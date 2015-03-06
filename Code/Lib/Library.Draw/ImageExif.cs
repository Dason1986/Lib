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
        [LanguageCategory("FileInfo"), LanguageDisplayName(@"����")]
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
        //18246 �Ǽ�
        /// <summary>
        /// 
        /// </summary>
        [LanguageCategory("FileInfo"), LanguageDisplayName(@"�Ǽ�")]
        public int Rating { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// 36864
        [LanguageCategory("FileInfo"), LanguageDisplayName(@"Exif�汾")]
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

        #region ���C��Ϣ

        //271 ��ͷ��˾
        /// <summary>
        /// 
        /// </summary>
        [Category("���C��Ϣ"), DisplayName(@"���C�S��")]
        public string EquipmentMake { get; set; }

        //272 ��ͷ�ͺ�
        /// <summary>
        /// 
        /// </summary>
        [Category("���C��Ϣ"), DisplayName(@"���C�ͺ�")]
        public string EquipmentModel { get; set; }

        #endregion






        #region base
        /// <summary>
        /// 
        /// </summary>
        [Category("�D����Ϣ"), DisplayName(@"�ֱ��ʵ�Ԫ")]
        public int ResolutionUnit { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [Category("�D����Ϣ"), DisplayName(@"YCbCr��λ")]
        public int YCbCrPositioning { get; set; }
        //41994 ������(һ��\���\ǿ��)
        /// <summary>
        /// 
        /// </summary>
        [Category("�D����Ϣ"), DisplayName(@"������")]
        public int Sharpness { get; set; }
        //37384 ��Դ
        /// <summary>
        /// 
        /// </summary>
        [Category("�D����Ϣ"), DisplayName(@"��Դ")]
        public int LightSource { get; set; }
        //34850 �عⷽʽ(�o\�ք�\һ��\��Ȧ�țQ\���T�țQ\�����\���T����\ֱ��ģʽ\�M��ģʽ)
        /// <summary>
        /// 
        /// </summary>
        [Category("�D����Ϣ"), DisplayName(@"�عⷽʽ")]
        public int ExposureProgram { get; set; }
        //41989 35mm����
        /// <summary>
        /// 
        /// </summary>
        [Category("�D����Ϣ"), DisplayName(@"35mm����")]
        public int FocalLengthIn35mmFilm { get; set; }
        //20625
        /// <summary>
        /// 
        /// </summary>
        [Category("�D����Ϣ"), DisplayName(@"ɫ�ȱ�")]
        public int ChrominanceTable { get; set; }
        //20624
        /// <summary>
        /// 
        /// </summary>
        [Category("�D����Ϣ"), DisplayName(@"���ȱ�")]
        public int LuminanceTable { get; set; }
        //36867
        /// <summary>
        /// 
        /// </summary>
        [Category("�D����Ϣ"), DisplayName(@"�Ĕz�r�g")]
        public DateTime? DateTimeOriginal { get; set; }
        //36868
        /// <summary>
        /// 
        /// </summary>
        [Category("�D����Ϣ"), DisplayName(@"�����r�g")]
        public DateTime? DateTimeDigitized { get; set; }
        //34855 ��Ȧ
        /// <summary>
        /// 
        /// </summary>
        [Category("�D����Ϣ"), DisplayName(@"ISO��Ȧ")]
        public int ISOSpeedRatings { get; set; }
        //37383 ���ģʽ
        /// <summary>
        /// 
        /// </summary>
        [Category("�D����Ϣ"), DisplayName(@"���ģʽ")]
        public int MeteringMode { get; set; }

        //37385 �����ģʽ
        /// <summary>
        /// 
        /// </summary>
        [Category("�D����Ϣ"), DisplayName(@"�����ģʽ")]
        public int Flash { get; set; }
        //41987 ��ƽ��(�ֶ�\�Զ�)
        /// <summary>
        /// 
        /// </summary>
        [Category("�D����Ϣ"), DisplayName(@"��ƽ��")]
        public int WhiteBalance { get; set; }
        //41992 �ȶ�(��׼\���\ǿ��)
        /// <summary>
        /// 
        /// </summary>
        [Category("�D����Ϣ"), DisplayName(@"�ȶ�")]
        public int Contrast { get; set; }

        //41993 ���Ͷ�(��׼\�ͱ���\�߱���)
        /// <summary>
        /// 
        /// </summary>
        [Category("�D����Ϣ"), DisplayName(@"���Ͷ�")]
        public int Saturation { get; set; }

        /// <summary>
        /// 282,283
        /// </summary>
        [Category("�D����Ϣ"), DisplayName(@"������")]
        public Size? Resolution { get; set; }

        /// <summary>
        /// 37377
        /// </summary>
        [Category("�D����Ϣ"), DisplayName(@"�����ٶ�")]
        public double ShutterSpeed { get; set; }
        /// <summary>
        /// 33434
        /// </summary>
        [Category("�D����Ϣ"), DisplayName(@"�ع�ʱ��")]
        public double ExposureTime { get; set; }
        #endregion

        #region Thumbnail

        /// <summary>
        /// 20525,20526
        /// </summary>
        [Category("�s�ԈD��Ϣ"), DisplayName(@"������")]
        public Size? ThumbnailResolution { get; set; }

        /// <summary>
        /// 20528
        /// </summary>
        [Category("�s�ԈD��Ϣ"), DisplayName(@"�����ȵ�Ԫ")]
        public int ThumbnailResolutionUnit { get; set; }

        /// <summary>
        /// 20515
        /// </summary>
        [Category("�s�ԈD��Ϣ"), DisplayName(@"ѹ��")]
        public int ThumbnailCompression { get; set; }

        /// <summary>
        /// 20507
        /// </summary>
        [Category("�s�ԈD��Ϣ"), DisplayName(@"�s�ԈD")]
        public byte[] ThumbnailData { get; set; }

        #endregion

        #region GPS
        /// <summary>
        /// 
        /// </summary>
        [Category("������Ϣ"), DisplayName(@"GPS����")]
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
                case 0: return "�o";
                case 1: return "ƽ���y��";
                case 2: return "����ƫ��ƽ���y��";
                case 3: return "���";
                case 4: return "���y��";
                case 5: return "�օ^�y��";
                case 6: return "�ֲ��y��";
            }
            return string.Empty;
        }
        public static string GetLightSourceName(int LightSource)
        {
            switch (LightSource)
            {
                case 0: return "��";
                case 1: return "�չ�";
                case 2: return "Ξ���";
                case 3: return "�Z�z�";
                case 17: return "�˜��ն�A";
                case 18: return "�˜��ն�B";
                case 19: return "�˜��ն�C";
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
                case 0: return "�o�W���";
                case 1: return "�W���";
                case 5: return "�W���,�o�ؑ��W��";
                case 7: return "�W���,�ؑ��W��";
                case 9: return "�W���,ǿ��";
                case 13: return "�W���,ǿ��,�o�ؑ��W��";
                case 15: return "�W���,ǿ��,�ؑ��W��";
                case 16: return "�o���,ǿ��";
                case 24: return "�o���,�Զ�";
                case 25: return "�W���,�Զ�";
                case 29: return "�W���,�Զ�,�o�ؑ��W��";
                case 31: return "�W���,�Զ�,�ؑ��W��";
                case 32: return "�o�W�������";
                case 65: return "�W���,�t��";
                case 69: return "�W���,�t��,�o�ؑ��W��";
                case 71: return "�W���,�t��,�ؑ��W��";
                case 73: return "�W���,ǿ��,�t��";
                case 77: return "�W���,ǿ��,�t��,�o�ؑ��W��";
                case 79: return "�W���,ǿ��,�t��,�ؑ��W��";
                case 83: return "�W���,�Զ�,�t��";
                case 93: return "�W���,�Զ�,�t��,�o�ؑ��W��";
                case 95: return "�W���,�Զ�,�t��,�ؑ��W��";
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