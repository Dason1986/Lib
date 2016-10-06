using Library.Att;
using Library.HelperUtility;
using Library.Map;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;

namespace Library.Draw
{
    public enum Orientation:ushort
    {
        Horizontal = 1,
        MirrorHorizontal = 2,
        Rotate180 = 3,
        MirrorVertical = 4,
        MirrorHorizontalAndRotate270CW = 5,
        Rotate90CW = 6,
        MirrorHorizontalAndRotate90CW = 7,
        Rotate270CW = 8,
    }
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
        [LanguageCategory("FileInfo"), LanguageDisplayName(@"Pix", typeof(DrawResource))]
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
        [LanguageCategory("FileInfo"), LanguageDisplayName(@"Rating")]
        public int Rating { get; set; }

        /// <summary>
        ///
        /// </summary>
        /// 36864
        [LanguageCategory("FileInfo"), LanguageDisplayName(@"ExifVersion", typeof(DrawResource))]
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

        private static readonly Guid BMP = new Guid("B96B3CAB-0728-11D3-9D7B-0000F81EF32E");
        private static readonly Guid PNG = new Guid("B96B3CAF-0728-11D3-9D7B-0000F81EF32E");
        private static readonly Guid GIF = new Guid("B96B3CB0-0728-11D3-9D7B-0000F81EF32E");
        private static readonly Guid JPEG = new Guid("B96B3CAE-0728-11D3-9D7B-0000F81EF32E");
        private static readonly Guid TIFF = new Guid("B96B3CB1-0728-11D3-9D7B-0000F81EF32E");

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

        #endregion FileInfo

        #region 相機信息

        //271 镜头公司
        /// <summary>
        ///
        /// </summary>
        [LanguageCategory("EquipmentInfo", typeof(DrawResource)), LanguageDisplayName(@"EquipmentMake", typeof(DrawResource))]
        public string EquipmentMake { get; set; }

        //272 镜头型号
        /// <summary>
        ///
        /// </summary>
        [LanguageCategory("EquipmentInfo", typeof(DrawResource)), LanguageDisplayName(@"EquipmentModel", typeof(DrawResource))]
        public string EquipmentModel { get; set; }

        #endregion 相機信息

        #region base
        /// <summary>
        ///
        /// </summary>
        [LanguageCategory("ImageInfo", typeof(DrawResource)), LanguageDisplayName(@"Orientation", typeof(DrawResource))]
        public Orientation Orientation { get; set; }
        /// <summary>
        ///
        /// </summary>
        [LanguageCategory("ImageInfo", typeof(DrawResource)), LanguageDisplayName(@"ResolutionUnit", typeof(DrawResource))]
        public int ResolutionUnit { get; set; }

        /// <summary>
        ///
        /// </summary>
        [LanguageCategory("ImageInfo", typeof(DrawResource)), LanguageDisplayName(@"YCbCrPositioning", typeof(DrawResource))]
        public int YCbCrPositioning { get; set; }

        //41994 清晰度(一般\柔和\强烈)
        /// <summary>
        ///
        /// </summary>
        [LanguageCategory("ImageInfo", typeof(DrawResource)), LanguageDisplayName(@"Sharpness", typeof(DrawResource))]
        public int Sharpness { get; set; }

        //37384 光源
        /// <summary>
        ///
        /// </summary>
        [LanguageCategory("ImageInfo", typeof(DrawResource)), LanguageDisplayName(@"LightSource", typeof(DrawResource))]
        public int LightSource { get; set; }

        //34850 曝光方式(無\手動\一般\光圈先決\快門先決\景深優先\快門優先\直向模式\橫向模式)
        /// <summary>
        ///
        /// </summary>
        [LanguageCategory("ImageInfo", typeof(DrawResource)), LanguageDisplayName(@"ExposureProgram", typeof(DrawResource))]
        public int ExposureProgram { get; set; }

        //41989 35mm胶卷
        /// <summary>
        ///
        /// </summary>
        [LanguageCategory("ImageInfo", typeof(DrawResource)), LanguageDisplayName(@"FocalLengthIn35mmFilm", typeof(DrawResource))]
        public int FocalLengthIn35mmFilm { get; set; }

        //20625
        /// <summary>
        ///
        /// </summary>
        [LanguageCategory("ImageInfo", typeof(DrawResource)), LanguageDisplayName(@"ChrominanceTable", typeof(DrawResource))]
        public int ChrominanceTable { get; set; }

        //20624
        /// <summary>
        ///
        /// </summary>
        [LanguageCategory("ImageInfo", typeof(DrawResource)), LanguageDisplayName(@"LuminanceTable", typeof(DrawResource))]
        public int LuminanceTable { get; set; }

        //36867
        /// <summary>
        ///
        /// </summary>
        [LanguageCategory("ImageInfo", typeof(DrawResource)), LanguageDisplayName(@"DateTimeOriginal", typeof(DrawResource))]
        public DateTime? DateTimeOriginal { get; set; }

        //36868
        /// <summary>
        ///
        /// </summary>
        [LanguageCategory("ImageInfo", typeof(DrawResource)), LanguageDisplayName(@"DateTimeDigitized", typeof(DrawResource))]
        public DateTime? DateTimeDigitized { get; set; }

        //34855 光圈
        /// <summary>
        ///
        /// </summary>
        [LanguageCategory("ImageInfo", typeof(DrawResource)), LanguageDisplayName(@"ISOSpeedRatings", typeof(DrawResource))]
        public int ISOSpeedRatings { get; set; }

        //37383 测光模式
        /// <summary>
        ///
        /// </summary>
        [LanguageCategory("ImageInfo", typeof(DrawResource)), LanguageDisplayName(@"MeteringMode", typeof(DrawResource))]
        public int MeteringMode { get; set; }

        //37385 闪光灯模式
        /// <summary>
        ///
        /// </summary>
        [LanguageCategory("ImageInfo", typeof(DrawResource)), LanguageDisplayName(@"Flash", typeof(DrawResource))]
        public int Flash { get; set; }

        //41987 白平衡(手动\自动)
        /// <summary>
        ///
        /// </summary>
        [LanguageCategory("ImageInfo", typeof(DrawResource)), LanguageDisplayName(@"WhiteBalance", typeof(DrawResource))]
        public int WhiteBalance { get; set; }

        //41992 比对(标准\柔和\强烈)
        /// <summary>
        ///
        /// </summary>
        [LanguageCategory("ImageInfo", typeof(DrawResource)), LanguageDisplayName(@"Contrast", typeof(DrawResource))]
        public int Contrast { get; set; }

        //41993 饱和度(标准\低饱和\高饱和)
        /// <summary>
        ///
        /// </summary>
        [LanguageCategory("ImageInfo", typeof(DrawResource)), LanguageDisplayName(@"Saturation", typeof(DrawResource))]
        public int Saturation { get; set; }

        /// <summary>
        /// 282,283
        /// </summary>
        [LanguageCategory("ImageInfo", typeof(DrawResource)), LanguageDisplayName(@"Resolution", typeof(DrawResource))]
        public Size? Resolution { get; set; }

        /// <summary>
        /// 37377
        /// </summary>
        [LanguageCategory("ImageInfo", typeof(DrawResource)), LanguageDisplayName(@"ShutterSpeed", typeof(DrawResource))]
        public double ShutterSpeed { get; set; }

        /// <summary>
        /// 33434
        /// </summary>
        [LanguageCategory("ImageInfo", typeof(DrawResource)), LanguageDisplayName(@"ExposureTime", typeof(DrawResource))]
        public double ExposureTime { get; set; }

        #endregion base

        #region Thumbnail

        /// <summary>
        /// 20525,20526
        /// </summary>
        [LanguageCategory("Thumbnail", typeof(DrawResource)), LanguageDisplayName(@"Resolution", typeof(DrawResource))]
        public Size? ThumbnailResolution { get; set; }

        /// <summary>
        /// 20528
        /// </summary>
        [LanguageCategory("Thumbnail", typeof(DrawResource)), LanguageDisplayName(@"ResolutionUnit", typeof(DrawResource))]
        public int ThumbnailResolutionUnit { get; set; }

        /// <summary>
        /// 20515
        /// </summary>
        [LanguageCategory("Thumbnail", typeof(DrawResource)), LanguageDisplayName(@"Compression")]
        public int ThumbnailCompression { get; set; }

        /// <summary>
        /// 20507
        /// </summary>
        [LanguageCategory("Thumbnail", typeof(DrawResource)), LanguageDisplayName(@"Thumbnail", typeof(DrawResource))]
        public byte[] ThumbnailData { get; set; }

        #endregion Thumbnail

        #region GPS

        /// <summary>
        ///
        /// </summary>
        [LanguageCategory("GeoInfo", typeof(DrawResource)), LanguageDisplayName(@"GPS", typeof(DrawResource))]
        public GPSGeo GPS { get; set; }

        #endregion GPS

        /// <summary>
        ///
        /// </summary>
        [LanguageCategory("ImageInfo", typeof(DrawResource)), LanguageDisplayName(@"AllExifs", typeof(DrawResource))]
        public ExifPropertyCollection Properties { get; protected set; }



        /// <summary>
        ///
        /// </summary>
        public class ExifPropertyCollection : ICollection
        {
            private readonly ICollection list;

            /// <summary>
            ///
            /// </summary>
            /// <param name="enumerable"></param>
            public ExifPropertyCollection(IEnumerable<ExifProperty> enumerable)
            {
                list = new List<ExifProperty>(enumerable);
            }

            /// <summary>
            ///
            /// </summary>
            /// <returns></returns>
            public IEnumerator GetEnumerator()
            {
                return list.GetEnumerator();
            }

            /// <summary>
            ///
            /// </summary>
            /// <param name="array"></param>
            /// <param name="index"></param>
            public void CopyTo(Array array, int index)
            {
                list.CopyTo(array, index);
            }

            /// <summary>
            ///
            /// </summary>
            public int Count
            {
                get { return list.Count; }
            }

            /// <summary>
            ///
            /// </summary>
            public object SyncRoot
            {
                get { return list.SyncRoot; }
            }

            /// <summary>
            ///
            /// </summary>
            public bool IsSynchronized
            {
                get { return list.IsSynchronized; }
            }
        }

        /// <summary>
        ///
        /// </summary>
        public sealed class ExifProperty
        {
            private byte[] _value;
            private int _hex;

            /// <summary>
            ///
            /// </summary>
            /// <param name="property"></param>
            public ExifProperty(PropertyItem property)
            {
                Hex = property.Id;
                Type = (PropertyTagType)property.Type;
                Value = property.Value;
                //  Len = property.Len;
            }

            /// <summary>
            ///
            /// </summary>
            public PropertyTagId TagId { get; internal set; }

            /// <summary>
            ///
            /// </summary>
            /// <param name="hex"></param>
            /// <param name="type"></param>
            /// <param name="value"></param>
            public ExifProperty(int hex, PropertyTagType type, byte[] value)
            {
                this.Hex = hex;
                this.Type = type;
                this.Value = value;
            }

            /// <summary>
            ///
            /// </summary>
            public int Len { get; private set; }

            /// <summary>
            ///
            /// </summary>
            public byte[] Value
            {
                get { return _value; }
                set
                {
                    _value = value;
                    Len = _value != null ? _value.Length : 0;
                }
            }

            /// <summary>
            ///
            /// </summary>
            public PropertyTagType Type { get; private set; }

            private object _displayValue;

            /// <summary>
            ///
            /// </summary>
            public object DisplayValue
            {
                get
                {
                    if (_displayValue != null) return _displayValue;
                    switch (Type)
                    {
                        case PropertyTagType.ASCII: _displayValue = Encoding.ASCII.GetString(Value); break;
                        case PropertyTagType.Short: _displayValue = BitConverter.ToUInt16(Value, 0); break;
                        case PropertyTagType.Long:
                        case PropertyTagType.Rational:
                            {
                                switch (Len)
                                {
                                    case 2:
                                        _displayValue = BitConverter.ToInt16(Value, 0); break;
                                    case 4:
                                        _displayValue = BitConverter.ToInt32(Value, 0); break;
                                    case 24:
                                        {
                                            float degrees = BitConverter.ToInt32(Value, 0) / (float)BitConverter.ToInt32(Value, 4);

                                            float minutes = BitConverter.ToInt32(Value, 8) / (float)BitConverter.ToInt32(Value, 12);

                                            float seconds = BitConverter.ToInt32(Value, 16) / (float)BitConverter.ToInt32(Value, 20);
                                            float coorditate = degrees + (minutes / 60f) + (seconds / 3600f);
                                            _displayValue = coorditate;
                                            break;
                                        }
                                    case 8:
                                        {
                                            _displayValue = BitConverter.ToInt32(Value, 0) / (float)BitConverter.ToInt32(Value, 4);
                                            break;
                                        }
                                    case 16:
                                        {
                                            float degrees = BitConverter.ToInt32(Value, 0) / (float)BitConverter.ToInt32(Value, 4);
                                            float minutes = BitConverter.ToInt32(Value, 8) / (float)BitConverter.ToInt32(Value, 12);
                                            float coorditate = degrees + (minutes / 60f);
                                            _displayValue = coorditate;
                                            break;
                                        }
                                    case 48:
                                        {
                                            // red[x], red[y], green[x], green[y], blue[x], and blue[y]
                                            float redx = BitConverter.ToInt32(Value, 0) / (float)BitConverter.ToInt32(Value, 4);
                                            float redy = BitConverter.ToInt32(Value, 8) / (float)BitConverter.ToInt32(Value, 12);
                                            float greenx = BitConverter.ToInt32(Value, 16) / (float)BitConverter.ToInt32(Value, 20);
                                            float greeny = BitConverter.ToInt32(Value, 24) / (float)BitConverter.ToInt32(Value, 28);
                                            float bluex = BitConverter.ToInt32(Value, 32) / (float)BitConverter.ToInt32(Value, 36);
                                            float bluey = BitConverter.ToInt32(Value, 40) / (float)BitConverter.ToInt32(Value, 44);
                                            _displayValue = string.Format("red[x]{0} ,red[y]{1} ,green[x]{2}, green[y]{3},blue[x]{4},blue[y]{5}", redx, redy, greenx, greeny, bluex, bluey);
                                            break;
                                        }
                                }
                                break;
                            }
                        case PropertyTagType.SLONG:
                        case PropertyTagType.SRational:
                            {
                                switch (Len)
                                {
                                    case 2:
                                        _displayValue = BitConverter.ToInt16(Value, 0); break;
                                    case 4:
                                        _displayValue = BitConverter.ToUInt32(Value, 0); break;
                                    case 24:
                                        {
                                            float degrees = BitConverter.ToUInt32(Value, 0) / (float)BitConverter.ToUInt32(Value, 4);

                                            float minutes = BitConverter.ToUInt32(Value, 8) / (float)BitConverter.ToUInt32(Value, 12);

                                            float seconds = BitConverter.ToUInt32(Value, 16) / (float)BitConverter.ToUInt32(Value, 20);
                                            float coorditate = degrees + (minutes / 60f) + (seconds / 3600f);
                                            _displayValue = coorditate;
                                            break;
                                        }
                                    case 8:
                                        {
                                            _displayValue = BitConverter.ToUInt32(Value, 0) / (float)BitConverter.ToUInt32(Value, 4);
                                            break;
                                        }
                                    case 16:
                                        {
                                            float degrees = BitConverter.ToUInt32(Value, 0) / (float)BitConverter.ToUInt32(Value, 4);
                                            float minutes = BitConverter.ToUInt32(Value, 8) / (float)BitConverter.ToUInt32(Value, 12);
                                            float coorditate = degrees + (minutes / 60f);
                                            _displayValue = coorditate;
                                            break;
                                        }
                                    case 48:
                                        {
                                            // red[x], red[y], green[x], green[y], blue[x], and blue[y]
                                            float redx = BitConverter.ToUInt32(Value, 0) / (float)BitConverter.ToUInt32(Value, 4);
                                            float redy = BitConverter.ToUInt32(Value, 8) / (float)BitConverter.ToUInt32(Value, 12);
                                            float greenx = BitConverter.ToUInt32(Value, 16) / (float)BitConverter.ToUInt32(Value, 20);
                                            float greeny = BitConverter.ToUInt32(Value, 24) / (float)BitConverter.ToUInt32(Value, 28);
                                            float bluex = BitConverter.ToUInt32(Value, 32) / (float)BitConverter.ToUInt32(Value, 36);
                                            float bluey = BitConverter.ToUInt32(Value, 40) / (float)BitConverter.ToUInt32(Value, 44);
                                            _displayValue = string.Format("red[x]{0} ,red[y]{1} ,green[x]{2}, green[y]{3},blue[x]{4},blue[y]{5}", redx, redy, greenx, greeny, bluex, bluey);
                                            break;
                                        }
                                }
                                break;
                            }
                    }
                    return _displayValue;
                }
            }

            /// <summary>
            ///
            /// </summary>
            public int Hex
            {
                private set
                {
                    _hex = value;
                    TagId = (PropertyTagId)value;
                }
                get { return _hex; }
            }

            /// <summary>
            ///
            /// </summary>
            /// <returns></returns>
            public override string ToString()
            {
                return string.Format("{0}[{1}]", TagId, Type);
            }
        }

        /// <summary>
        ///
        /// </summary>
        public enum PropertyTagType : short
        {
            /// <summary>
            ///
            /// </summary>
            PixelFormat4bppIndexed = 0,

            /// <summary>
            ///
            /// </summary>
            Byte = 1,

            /// <summary>
            ///
            /// </summary>
            ASCII = 2,

            /// <summary>
            ///
            /// </summary>
            Short = 3,

            /// <summary>
            ///
            /// </summary>
            Long = 4,

            /// <summary>
            ///
            /// </summary>
            Rational = 5,

            /// <summary>
            ///
            /// </summary>
            Undefined = 7,

            /// <summary>
            ///
            /// </summary>
            SLONG = 9,

            /// <summary>
            ///
            /// </summary>
            SRational = 10,
            /// <summary>
            /// 
            /// </summary>
            SingleFloat = 11,
            /// <summary>
            /// 
            /// </summary>
            DoubleFloat = 12
        }

        /// <summary>
        ///
        /// </summary>
        public enum PropertyTagId
        {
            /// <summary>
            ///
            /// </summary>
            GpsVer = 0x0000,

            /// <summary>
            ///
            /// </summary>
            GpsLatitudeRef = 0x0001,

            /// <summary>
            ///
            /// </summary>
            GpsLatitude = 0x0002,

            /// <summary>
            ///
            /// </summary>
            GpsLongitudeRef = 0x0003,

            /// <summary>
            ///
            /// </summary>
            GpsLongitude = 0x0004,

            /// <summary>
            ///
            /// </summary>
            GpsAltitudeRef = 0x0005,

            /// <summary>
            ///
            /// </summary>
            GpsAltitude = 0x0006,

            /// <summary>
            ///
            /// </summary>
            GpsGpsTime = 0x0007,

            /// <summary>
            ///
            /// </summary>
            GpsGpsSatellites = 0x0008,

            /// <summary>
            ///
            /// </summary>
            GpsGpsStatus = 0x0009,

            /// <summary>
            ///
            /// </summary>
            GpsGpsMeasureMode = 0x000A,

            /// <summary>
            ///
            /// </summary>
            GpsGpsDop = 0x000B,

            /// <summary>
            ///
            /// </summary>
            GpsSpeedRef = 0x000C,

            /// <summary>
            ///
            /// </summary>
            GpsSpeed = 0x000D,

            /// <summary>
            ///
            /// </summary>
            GpsTrackRef = 0x000E,

            /// <summary>
            ///
            /// </summary>
            GpsTrack = 0x000F,

            /// <summary>
            ///
            /// </summary>
            GpsImgDirRef = 0x0010,

            /// <summary>
            ///
            /// </summary>
            GpsImgDir = 0x0011,

            /// <summary>
            ///
            /// </summary>
            GpsMapDatum = 0x0012,

            /// <summary>
            ///
            /// </summary>
            GpsDestLatRef = 0x0013,

            /// <summary>
            ///
            /// </summary>
            GpsDestLat = 0x0014,

            /// <summary>
            ///
            /// </summary>
            GpsDestLongRef = 0x0015,

            /// <summary>
            ///
            /// </summary>
            GpsDestLong = 0x0016,

            /// <summary>
            ///
            /// </summary>
            GpsDestBearRef = 0x0017,

            /// <summary>
            ///
            /// </summary>
            GpsDestBear = 0x0018,

            /// <summary>
            ///
            /// </summary>
            GpsDestDistRef = 0x0019,

            /// <summary>
            ///
            /// </summary>
            GpsDestDist = 0x001A,

            /// <summary>
            ///
            /// </summary>
            NewSubfileType = 0x00FE,

            /// <summary>
            ///
            /// </summary>
            SubfileType = 0x00FF,

            /// <summary>
            ///
            /// </summary>
            ImageWidth = 0x0100,

            /// <summary>
            ///
            /// </summary>
            ImageHeight = 0x0101,

            /// <summary>
            ///
            /// </summary>
            BitsPerSample = 0x0102,

            /// <summary>
            ///
            /// </summary>
            Compression = 0x0103,

            /// <summary>
            ///
            /// </summary>
            PhotometricInterp = 0x0106,

            /// <summary>
            ///
            /// </summary>
            ThreshHolding = 0x0107,

            /// <summary>
            ///
            /// </summary>
            CellWidth = 0x0108,

            /// <summary>
            ///
            /// </summary>
            CellHeight = 0x0109,

            /// <summary>
            ///
            /// </summary>
            FillOrder = 0x010A,

            /// <summary>
            ///
            /// </summary>
            DocumentName = 0x010D,

            /// <summary>
            ///
            /// </summary>
            ImageDescription = 0x010E,

            /// <summary>
            ///
            /// </summary>
            EquipMake = 0x010F,

            /// <summary>
            ///
            /// </summary>
            EquipModel = 0x0110,

            /// <summary>
            ///
            /// </summary>
            StripOffsets = 0x0111,

            /// <summary>
            ///
            /// </summary>
            Orientation = 0x0112,

            /// <summary>
            ///
            /// </summary>
            SamplesPerPixel = 0x0115,

            /// <summary>
            ///
            /// </summary>
            RowsPerStrip = 0x0116,

            /// <summary>
            ///
            /// </summary>
            StripBytesCount = 0x0117,

            /// <summary>
            ///
            /// </summary>
            MinSampleValue = 0x0118,

            /// <summary>
            ///
            /// </summary>
            MaxSampleValue = 0x0119,

            /// <summary>
            ///
            /// </summary>
            XResolution = 0x011A,

            /// <summary>
            ///
            /// </summary>
            YResolution = 0x011B,

            /// <summary>
            ///
            /// </summary>
            PlanarConfig = 0x011C,

            /// <summary>
            ///
            /// </summary>
            PageName = 0x011D,

            /// <summary>
            ///
            /// </summary>
            XPosition = 0x011E,

            /// <summary>
            ///
            /// </summary>
            YPosition = 0x011F,

            /// <summary>
            ///
            /// </summary>
            FreeOffset = 0x0120,

            /// <summary>
            ///
            /// </summary>
            FreeByteCounts = 0x0121,

            /// <summary>
            ///
            /// </summary>
            GrayResponseUnit = 0x0122,

            /// <summary>
            ///
            /// </summary>
            GrayResponseCurve = 0x0123,

            /// <summary>
            ///
            /// </summary>
            T4Option = 0x0124,

            /// <summary>
            ///
            /// </summary>
            T6Option = 0x0125,

            /// <summary>
            ///
            /// </summary>
            ResolutionUnit = 0x0128,

            /// <summary>
            ///
            /// </summary>
            PageNumber = 0x0129,

            /// <summary>
            ///
            /// </summary>
            TransferFunction = 0x012D,

            /// <summary>
            ///
            /// </summary>
            SoftwareUsed = 0x0131,

            /// <summary>
            ///
            /// </summary>
            DateTime = 0x0132,

            /// <summary>
            ///
            /// </summary>
            Artist = 0x013B,

            /// <summary>
            ///
            /// </summary>
            HostComputer = 0x013C,

            /// <summary>
            ///
            /// </summary>
            Predictor = 0x013D,

            /// <summary>
            ///
            /// </summary>
            WhitePoint = 0x013E,

            /// <summary>
            ///
            /// </summary>
            PrimaryChromaticities = 0x013F,

            /// <summary>
            ///
            /// </summary>
            ColorMap = 0x0140,

            /// <summary>
            ///
            /// </summary>
            HalftoneHints = 0x0141,

            /// <summary>
            ///
            /// </summary>
            TileWidth = 0x0142,

            /// <summary>
            ///
            /// </summary>
            TileLength = 0x0143,

            /// <summary>
            ///
            /// </summary>
            TileOffset = 0x0144,

            /// <summary>
            ///
            /// </summary>
            TileByteCounts = 0x0145,

            /// <summary>
            ///
            /// </summary>
            InkSet = 0x014C,

            /// <summary>
            ///
            /// </summary>
            InkNames = 0x014D,

            /// <summary>
            ///
            /// </summary>
            NumberOfInks = 0x014E,

            /// <summary>
            ///
            /// </summary>
            DotRange = 0x0150,

            /// <summary>
            ///
            /// </summary>
            TargetPrinter = 0x0151,

            /// <summary>
            ///
            /// </summary>
            ExtraSamples = 0x0152,

            /// <summary>
            ///
            /// </summary>
            SampleFormat = 0x0153,

            /// <summary>
            ///
            /// </summary>
            SMinSampleValue = 0x0154,

            /// <summary>
            ///
            /// </summary>
            SMaxSampleValue = 0x0155,

            /// <summary>
            ///
            /// </summary>
            TransferRange = 0x0156,

            /// <summary>
            ///
            /// </summary>
            JPEGProc = 0x0200,

            /// <summary>
            ///
            /// </summary>
            JPEGInterFormat = 0x0201,

            /// <summary>
            ///
            /// </summary>
            JPEGInterLength = 0x0202,

            /// <summary>
            ///
            /// </summary>
            JPEGRestartInterval = 0x0203,

            /// <summary>
            ///
            /// </summary>
            JPEGLosslessPredictors = 0x0205,

            /// <summary>
            ///
            /// </summary>
            JPEGPointTransforms = 0x0206,

            /// <summary>
            ///
            /// </summary>
            JPEGQTables = 0x0207,

            /// <summary>
            ///
            /// </summary>
            JPEGDCTables = 0x0208,

            /// <summary>
            ///
            /// </summary>
            JPEGACTables = 0x0209,

            /// <summary>
            ///
            /// </summary>
            YCbCrCoefficients = 0x0211,

            /// <summary>
            ///
            /// </summary>
            YCbCrSubsampling = 0x0212,

            /// <summary>
            ///
            /// </summary>
            YCbCrPositioning = 0x0213,

            /// <summary>
            ///
            /// </summary>
            REFBlackWhite = 0x0214,

            /// <summary>
            ///
            /// </summary>
            Gamma = 0x0301,

            /// <summary>
            ///
            /// </summary>
            ICCProfileDescriptor = 0x0302,

            /// <summary>
            ///
            /// </summary>
            SRGBRenderingIntent = 0x0303,

            /// <summary>
            ///
            /// </summary>
            ImageTitle = 0x0320,

            /// <summary>
            ///
            /// </summary>
            ResolutionXUnit = 0x5001,

            /// <summary>
            ///
            /// </summary>
            ResolutionYUnit = 0x5002,

            /// <summary>
            ///
            /// </summary>
            ResolutionXLengthUnit = 0x5003,

            /// <summary>
            ///
            /// </summary>
            ResolutionYLengthUnit = 0x5004,

            /// <summary>
            ///
            /// </summary>
            PrintFlags = 0x5005,

            /// <summary>
            ///
            /// </summary>
            PrintFlagsVersion = 0x5006,

            /// <summary>
            ///
            /// </summary>
            PrintFlagsCrop = 0x5007,

            /// <summary>
            ///
            /// </summary>
            PrintFlagsBleedWidth = 0x5008,

            /// <summary>
            ///
            /// </summary>
            PrintFlagsBleedWidthScale = 0x5009,

            /// <summary>
            ///
            /// </summary>
            HalftoneLPI = 0x500A,

            /// <summary>
            ///
            /// </summary>
            HalftoneLPIUnit = 0x500B,

            /// <summary>
            ///
            /// </summary>
            HalftoneDegree = 0x500C,

            /// <summary>
            ///
            /// </summary>
            HalftoneShape = 0x500D,

            /// <summary>
            ///
            /// </summary>
            HalftoneMisc = 0x500E,

            /// <summary>
            ///
            /// </summary>
            HalftoneScreen = 0x500F,

            /// <summary>
            ///
            /// </summary>
            JPEGQuality = 0x5010,

            /// <summary>
            ///
            /// </summary>
            GridSize = 0x5011,

            /// <summary>
            ///
            /// </summary>
            ThumbnailFormat = 0x5012,

            /// <summary>
            ///
            /// </summary>
            ThumbnailWidth = 0x5013,

            /// <summary>
            ///
            /// </summary>
            ThumbnailHeight = 0x5014,

            /// <summary>
            ///
            /// </summary>
            ThumbnailColorDepth = 0x5015,

            /// <summary>
            ///
            /// </summary>
            ThumbnailPlanes = 0x5016,

            /// <summary>
            ///
            /// </summary>
            ThumbnailRawBytes = 0x5017,

            /// <summary>
            ///
            /// </summary>
            ThumbnailSize = 0x5018,

            /// <summary>
            ///
            /// </summary>
            ThumbnailCompressedSize = 0x5019,

            /// <summary>
            ///
            /// </summary>
            ColorTransferFunction = 0x501A,

            /// <summary>
            ///
            /// </summary>
            ThumbnailData = 0x501B,

            /// <summary>
            ///
            /// </summary>
            ThumbnailImageWidth = 0x5020,

            /// <summary>
            ///
            /// </summary>
            ThumbnailImageHeight = 0x5021,

            /// <summary>
            ///
            /// </summary>
            ThumbnailBitsPerSample = 0x5022,

            /// <summary>
            ///
            /// </summary>
            ThumbnailCompression = 0x5023,

            /// <summary>
            ///
            /// </summary>
            ThumbnailPhotometricInterp = 0x5024,

            /// <summary>
            ///
            /// </summary>
            ThumbnailImageDescription = 0x5025,

            /// <summary>
            ///
            /// </summary>
            ThumbnailEquipMake = 0x5026,

            /// <summary>
            ///
            /// </summary>
            ThumbnailEquipModel = 0x5027,

            /// <summary>
            ///
            /// </summary>
            ThumbnailStripOffsets = 0x5028,

            /// <summary>
            ///
            /// </summary>
            ThumbnailOrientation = 0x5029,

            /// <summary>
            ///
            /// </summary>
            ThumbnailSamplesPerPixel = 0x502A,

            /// <summary>
            ///
            /// </summary>
            ThumbnailRowsPerStrip = 0x502B,

            /// <summary>
            ///
            /// </summary>
            ThumbnailStripBytesCount = 0x502C,

            /// <summary>
            ///
            /// </summary>
            ThumbnailResolutionX = 0x502D,

            /// <summary>
            ///
            /// </summary>
            ThumbnailResolutionY = 0x502E,

            /// <summary>
            ///
            /// </summary>
            ThumbnailPlanarConfig = 0x502F,

            /// <summary>
            ///
            /// </summary>
            ThumbnailResolutionUnit = 0x5030,

            /// <summary>
            ///
            /// </summary>
            ThumbnailTransferFunction = 0x5031,

            /// <summary>
            ///
            /// </summary>
            ThumbnailSoftwareUsed = 0x5032,

            /// <summary>
            ///
            /// </summary>
            ThumbnailDateTime = 0x5033,

            /// <summary>
            ///
            /// </summary>
            ThumbnailArtist = 0x5034,

            /// <summary>
            ///
            /// </summary>
            ThumbnailWhitePoint = 0x5035,

            /// <summary>
            ///
            /// </summary>
            ThumbnailPrimaryChromaticities = 0x5036,

            /// <summary>
            ///
            /// </summary>
            ThumbnailYCbCrCoefficients = 0x5037,

            /// <summary>
            ///
            /// </summary>
            ThumbnailYCbCrSubsampling = 0x5038,

            /// <summary>
            ///
            /// </summary>
            ThumbnailYCbCrPositioning = 0x5039,

            /// <summary>
            ///
            /// </summary>
            ThumbnailRefBlackWhite = 0x503A,

            /// <summary>
            ///
            /// </summary>
            ThumbnailCopyRight = 0x503B,

            /// <summary>
            ///
            /// </summary>
            LuminanceTable = 0x5090,

            /// <summary>
            ///
            /// </summary>
            ChrominanceTable = 0x5091,

            /// <summary>
            ///
            /// </summary>
            FrameDelay = 0x5100,

            /// <summary>
            ///
            /// </summary>
            LoopCount = 0x5101,

            /// <summary>
            ///
            /// </summary>
            GlobalPalette = 0x5102,

            /// <summary>
            ///
            /// </summary>
            IndexBackground = 0x5103,

            /// <summary>
            ///
            /// </summary>
            IndexTransparent = 0x5104,

            /// <summary>
            ///
            /// </summary>
            PixelUnit = 0x5110,

            /// <summary>
            ///
            /// </summary>
            PixelPerUnitX = 0x5111,

            /// <summary>
            ///
            /// </summary>
            PixelPerUnitY = 0x5112,

            /// <summary>
            ///
            /// </summary>
            PaletteHistogram = 0x5113,

            /// <summary>
            ///
            /// </summary>
            Copyright = 0x8298,

            /// <summary>
            ///
            /// </summary>
            ExifExposureTime = 0x829A,

            /// <summary>
            ///
            /// </summary>
            ExifFNumber = 0x829D,

            /// <summary>
            ///
            /// </summary>
            ExifIFD = 0x8769,

            /// <summary>
            ///
            /// </summary>
            ICCProfile = 0x8773,

            /// <summary>
            ///
            /// </summary>
            ExifExposureProg = 0x8822,

            /// <summary>
            ///
            /// </summary>
            ExifSpectralSense = 0x8824,

            /// <summary>
            ///
            /// </summary>
            GpsIFD = 0x8825,

            /// <summary>
            ///
            /// </summary>
            ExifISOSpeed = 0x8827,

            /// <summary>
            ///
            /// </summary>
            ExifOECF = 0x8828,

            /// <summary>
            ///
            /// </summary>
            ExifVer = 0x9000,

            /// <summary>
            ///
            /// </summary>
            ExifDTOrig = 0x9003,

            /// <summary>
            ///
            /// </summary>
            ExifDTDigitized = 0x9004,

            /// <summary>
            ///
            /// </summary>
            ExifCompConfig = 0x9101,

            /// <summary>
            ///
            /// </summary>
            ExifCompBPP = 0x9102,

            /// <summary>
            ///
            /// </summary>
            ExifShutterSpeed = 0x9201,

            /// <summary>
            ///
            /// </summary>
            ExifAperture = 0x9202,

            /// <summary>
            ///
            /// </summary>
            ExifBrightness = 0x9203,

            /// <summary>
            ///
            /// </summary>
            ExifExposureBias = 0x9204,

            /// <summary>
            ///
            /// </summary>
            ExifMaxAperture = 0x9205,

            /// <summary>
            ///
            /// </summary>
            ExifSubjectDist = 0x9206,

            /// <summary>
            ///
            /// </summary>
            ExifMeteringMode = 0x9207,

            /// <summary>
            ///
            /// </summary>
            ExifLightSource = 0x9208,

            /// <summary>
            ///
            /// </summary>
            ExifFlash = 0x9209,

            /// <summary>
            ///
            /// </summary>
            ExifFocalLength = 0x920A,

            /// <summary>
            ///
            /// </summary>
            ExifMakerNote = 0x927C,

            /// <summary>
            ///
            /// </summary>
            ExifUserComment = 0x9286,

            /// <summary>
            ///
            /// </summary>
            ExifDTSubsec = 0x9290,

            /// <summary>
            ///
            /// </summary>
            ExifDTOrigSS = 0x9291,

            /// <summary>
            ///
            /// </summary>
            ExifDTDigSS = 0x9292,

            /// <summary>
            ///
            /// </summary>
            ExifFPXVer = 0xA000,

            /// <summary>
            ///
            /// </summary>
            ExifColorSpace = 0xA001,

            /// <summary>
            ///
            /// </summary>
            ExifPixXDim = 0xA002,

            /// <summary>
            ///
            /// </summary>
            ExifPixYDim = 0xA003,

            /// <summary>
            ///
            /// </summary>
            ExifRelatedWav = 0xA004,

            /// <summary>
            ///
            /// </summary>
            ExifInterop = 0xA005,

            /// <summary>
            ///
            /// </summary>
            ExifFlashEnergy = 0xA20B,

            /// <summary>
            ///
            /// </summary>
            ExifSpatialFR = 0xA20C,

            /// <summary>
            ///
            /// </summary>
            ExifFocalXRes = 0xA20E,

            /// <summary>
            ///
            /// </summary>
            ExifFocalYRes = 0xA20F,

            /// <summary>
            ///
            /// </summary>
            ExifFocalResUnit = 0xA210,

            /// <summary>
            ///
            /// </summary>
            ExifSubjectLoc = 0xA214,

            /// <summary>
            ///
            /// </summary>
            ExifExposureIndex = 0xA215,

            /// <summary>
            ///
            /// </summary>
            ExifSensingMethod = 0xA217,

            /// <summary>
            ///
            /// </summary>
            ExifFileSource = 0xA300,

            /// <summary>
            ///
            /// </summary>
            ExifSceneType = 0xA301,

            /// <summary>
            ///
            /// </summary>
            ExifCfaPattern = 0xA302
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="image"></param>
        /// <returns></returns>
        public static ImageExif GetExifInfo(Image image)
        {
            //http://www.exiv2.org/tags.html
            ImageExif exif = new ImageExif();
            List<ExifProperty> properties = new List<ExifProperty>();
            exif.RawFormatID = image.RawFormat.Guid;

            foreach (int hex in image.PropertyIdList)
            {
                var exit = new ExifProperty(image.GetPropertyItem(hex));
                properties.Add(exit);
                switch ((int)hex)
                {

                    case 274:
                        {
                            var value = Convert.ToUInt16(exit.DisplayValue);
                            exif.Orientation = ObjectUtility.Cast<Orientation>(value);
                            break;
                        }
                    case 40091: exif.Title = ObjectUtility.Cast<string>(exit.DisplayValue); break;
                    case 40092: exif.Comment = GetStringUnicode(image, hex); break;
                    case 40093: exif.Author = ObjectUtility.Cast<string>(exit.DisplayValue); break;
                    case 40094: exif.Keyword = GetStringUnicode(image, hex); break;
                    case 40095: exif.Subject = GetStringUnicode(image, hex); break;
                    case 33432: exif.Copyright = ObjectUtility.Cast<string>(exit.DisplayValue); break;
                    case 270: exif.Description = ObjectUtility.Cast<string>(exit.DisplayValue); break;
                    case 271: exif.EquipmentMake = ObjectUtility.Cast<string>(exit.DisplayValue); break;
                    case 272: exif.EquipmentModel = ObjectUtility.Cast<string>(exit.DisplayValue); break;
                    case 34850: exif.ExposureProgram = ObjectUtility.Cast<short>(exit.DisplayValue); break;
                    case 34855: exif.ISOSpeedRatings = ObjectUtility.Cast<short>(exit.DisplayValue); break;
                    case 37384: exif.Flash = ObjectUtility.Cast<short>(exit.DisplayValue); break;
                    case 37385: exif.LightSource = ObjectUtility.Cast<short>(exit.DisplayValue); break;
                    case 37383: exif.MeteringMode = ObjectUtility.Cast<short>(exit.DisplayValue); break;
                    case 18246: exif.Rating = ObjectUtility.Cast<short>(exit.DisplayValue); break;
                    case 41987: exif.WhiteBalance = ObjectUtility.Cast<short>(exit.DisplayValue); break;
                    case 41992: exif.Contrast = ObjectUtility.Cast<short>(exit.DisplayValue); break;
                    case 41993: exif.Saturation = ObjectUtility.Cast<short>(exit.DisplayValue); break;
                    case 41994: exif.Sharpness = ObjectUtility.Cast<short>(exit.DisplayValue); break;
                    case 33434: exif.ExposureTime = ObjectUtility.Cast<double>(exit.DisplayValue); break;
                    case 41989: exif.FocalLengthIn35mmFilm = ObjectUtility.Cast<short>(exit.DisplayValue); break;
                    case 36867: exif.DateTimeOriginal = GetDateTime(image, hex); break;
                    case 37377: exif.ShutterSpeed = GetDouble(image, hex); break;
                    case 36868: exif.DateTimeDigitized = GetDateTime(image, hex); break;
                    case 36864: exif.ExifVersion = ObjectUtility.Cast<string>(exit.DisplayValue); break;
                    case 531: exif.YCbCrPositioning = ObjectUtility.Cast<short>(exit.DisplayValue); break;
                    case 20625: exif.ChrominanceTable = ObjectUtility.Cast<short>(exit.DisplayValue); break;
                    case 20624: exif.LuminanceTable = ObjectUtility.Cast<short>(exit.DisplayValue); break;
                    case 20507: exif.ThumbnailData = image.GetPropertyItem(hex).Value; break;
                    case 20528: exif.ThumbnailResolutionUnit = ObjectUtility.Cast<short>(exit.DisplayValue); break;
                    case 20515: exif.ThumbnailCompression = ObjectUtility.Cast<short>(exit.DisplayValue); break;
                    case 296: exif.ResolutionUnit = ObjectUtility.Cast<short>(exit.DisplayValue); break;
                    case 282:
                    case 283:
                        {
                            if (exif.Resolution == null && image.PropertyIdList.Contains(282) && image.PropertyIdList.Contains(283))
                                exif.Resolution = new Size(GetInt(image, 282), GetInt(image, 283)); break;
                        }
                    case 20525:
                    case 20526:
                        {
                            if (exif.Pix == null && image.PropertyIdList.Contains(20525) && image.PropertyIdList.Contains(20526))
                                exif.ThumbnailResolution = new Size(GetInt(image, 20525), GetInt(image, 20526)); break;
                        }
                    case 40962:
                    case 40963:
                        {
                            if (exif.Pix == null && image.PropertyIdList.Contains(40962) && image.PropertyIdList.Contains(40963))
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
                }
            }
            exif.Properties = new ExifPropertyCollection(properties);
            return exif;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="meteringMode"></param>
        /// <returns></returns>
        public static string GetMeteringModeName(int meteringMode)
        {
            switch (meteringMode)
            {
                case 0: return DrawResource.Unknown;
                case 1: return DrawResource.Metering_ModeAverage;
                case 2: return DrawResource.Metering_CenterWeightedAverage;
                case 3: return DrawResource.Metering_Spot;
                case 4: return DrawResource.Metering_MultiSpot;
                case 5: return DrawResource.Metering_MultiSegment;
                case 6: return DrawResource.Metering_Partial;
                case 255: return DrawResource.Other;
            }

            return string.Empty;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="lightSource"></param>
        /// <returns></returns>
        public static string GetLightSourceName(int lightSource)
        {
            switch (lightSource)
            {
                case 0: return DrawResource.Unknown;
                case 1: return DrawResource.LightSource_Daylight;
                case 2: return DrawResource.LightSource_Fluorescent;
                case 3: return DrawResource.LightSource_Tungsten;
                case 10: return DrawResource.LightSource_Flash;
                case 17: return DrawResource.LightSource_StandardLightA;
                case 18: return DrawResource.LightSource_StandardLightB;
                case 19: return DrawResource.LightSource_StandardLightC;
                case 20: return DrawResource.LightSource_D55;
                case 21: return DrawResource.LightSource_D65;
                case 22: return DrawResource.LightSource_D75;
                case 255: return DrawResource.Other;
            }
            return string.Empty;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="flash"></param>
        /// <returns></returns>
        public static string GetFlashName(int flash)
        {
            switch (flash)
            {
                case 0: return DrawResource.Flash_Name0;
                case 1: return DrawResource.Flash_Name1;
                case 5: return DrawResource.Flash_Name5;
                case 7: return DrawResource.Flash_Name7;
                case 9: return DrawResource.Flash_Name9;
                case 13: return DrawResource.Flash_Name13;
                case 15: return DrawResource.Flash_Name15;
                case 16: return DrawResource.Flash_Name16;
                case 24: return DrawResource.Flash_Name24;
                case 25: return DrawResource.Flash_Name25;
                case 29: return DrawResource.Flash_Name29;
                case 31: return DrawResource.Flash_Name31;
                case 32: return DrawResource.Flash_Name32;
                case 65: return DrawResource.Flash_Name65;
                case 69: return DrawResource.Flash_Name69;
                case 71: return DrawResource.Flash_Name71;
                case 73: return DrawResource.Flash_Name73;
                case 77: return DrawResource.Flash_Name77;
                case 79: return DrawResource.Flash_Name79;
                case 83: return DrawResource.Flash_Name83;
                case 93: return DrawResource.Flash_Name93;
                case 95: return DrawResource.Flash_Name95;
            }
            return string.Empty;
        }

        private static DateTime? GetDateTime(Image getImage, int hex)
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

        private static short GetShort(Image getImage, int hex)
        {
            var propety = getImage.GetPropertyItem(hex);
            //     if (propety.Type != 5) return 0;
            var value = propety.Value;
            if (propety.Value.Length == 2)
                return BitConverter.ToInt16(value, 0);
            return 0;
        }

        private static int GetInt(Image getImage, int hex)
        {
            var propety = getImage.GetPropertyItem(hex);
            //     if (propety.Type != 5) return 0;
            var value = propety.Value;
            if (propety.Value.Length == 2)
                return BitConverter.ToInt16(value, 0);

            if (propety.Value.Length == 4)
                return BitConverter.ToInt32(value, 0);
            return 0;
        }

        private static double GetDouble(Image getImage, int hex)
        {
            var propety = getImage.GetPropertyItem(hex);
            //     if (propety.Type != 5) return 0;
            var value = propety.Value;
            switch (propety.Value.Length)
            {
                case 2:
                    return BitConverter.ToInt16(value, 0);
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

        private static string GetStringUnicode(Image getImage, int hex)
        {
            string dateTakenTag = Encoding.Unicode.GetString(getImage.GetPropertyItem(hex).Value);
            return dateTakenTag.Replace("\0", string.Empty);
        }

        private static string GetStringAsc(Image getImage, int hex)
        {
            string dateTakenTag = Encoding.ASCII.GetString(getImage.GetPropertyItem(hex).Value);
            return dateTakenTag.Replace("\0", string.Empty);
        }
    }
}