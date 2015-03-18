using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using Library.Att;
using Library.HelperUtility;
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
        [LanguageCategory("FileInfo"), LanguageDisplayName(@"Pix", "Draw")]
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
        [LanguageCategory("FileInfo"), LanguageDisplayName(@"ExifVersion", "Draw")]
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
        [LanguageCategory("EquipmentInfo", "Draw"), LanguageDisplayName(@"EquipmentMake", "Draw")]
        public string EquipmentMake { get; set; }

        //272 镜头型号
        /// <summary>
        /// 
        /// </summary>
        [LanguageCategory("EquipmentInfo", "Draw"), LanguageDisplayName(@"EquipmentModel", "Draw")]
        public string EquipmentModel { get; set; }

        #endregion






        #region base
        /// <summary>
        /// 
        /// </summary>
        [LanguageCategory("ImageInfo", "Draw"), LanguageDisplayName(@"ResolutionUnit", "Draw")]
        public int ResolutionUnit { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [LanguageCategory("ImageInfo", "Draw"), LanguageDisplayName(@"YCbCrPositioning", "Draw")]
        public int YCbCrPositioning { get; set; }
        //41994 清晰度(一般\柔和\强烈)
        /// <summary>
        /// 
        /// </summary>
        [LanguageCategory("ImageInfo", "Draw"), LanguageDisplayName(@"Sharpness", "Draw")]
        public int Sharpness { get; set; }
        //37384 光源
        /// <summary>
        /// 
        /// </summary>
        [LanguageCategory("ImageInfo", "Draw"), LanguageDisplayName(@"LightSource", "Draw")]
        public int LightSource { get; set; }
        //34850 曝光方式(無\手動\一般\光圈先決\快門先決\景深優先\快門優先\直向模式\橫向模式)
        /// <summary>
        /// 
        /// </summary>
        [LanguageCategory("ImageInfo", "Draw"), LanguageDisplayName(@"ExposureProgram", "Draw")]
        public int ExposureProgram { get; set; }
        //41989 35mm胶卷
        /// <summary>
        /// 
        /// </summary>
        [LanguageCategory("ImageInfo", "Draw"), LanguageDisplayName(@"FocalLengthIn35mmFilm", "Draw")]
        public int FocalLengthIn35mmFilm { get; set; }
        //20625
        /// <summary>
        /// 
        /// </summary>
        [LanguageCategory("ImageInfo", "Draw"), LanguageDisplayName(@"ChrominanceTable", "Draw")]
        public int ChrominanceTable { get; set; }
        //20624
        /// <summary>
        /// 
        /// </summary>
        [LanguageCategory("ImageInfo", "Draw"), LanguageDisplayName(@"LuminanceTable", "Draw")]
        public int LuminanceTable { get; set; }
        //36867
        /// <summary>
        /// 
        /// </summary>
        [LanguageCategory("ImageInfo", "Draw"), LanguageDisplayName(@"DateTimeOriginal", "Draw")]
        public DateTime? DateTimeOriginal { get; set; }
        //36868
        /// <summary>
        /// 
        /// </summary>
        [LanguageCategory("ImageInfo", "Draw"), LanguageDisplayName(@"DateTimeDigitized", "Draw")]
        public DateTime? DateTimeDigitized { get; set; }
        //34855 光圈
        /// <summary>
        /// 
        /// </summary>
        [LanguageCategory("ImageInfo", "Draw"), LanguageDisplayName(@"ISOSpeedRatings", "Draw")]
        public int ISOSpeedRatings { get; set; }
        //37383 测光模式
        /// <summary>
        /// 
        /// </summary>
        [LanguageCategory("ImageInfo", "Draw"), LanguageDisplayName(@"MeteringMode", "Draw")]
        public int MeteringMode { get; set; }

        //37385 闪光灯模式
        /// <summary>
        /// 
        /// </summary>
        [LanguageCategory("ImageInfo", "Draw"), LanguageDisplayName(@"Flash", "Draw")]
        public int Flash { get; set; }
        //41987 白平衡(手动\自动)
        /// <summary>
        /// 
        /// </summary>
        [LanguageCategory("ImageInfo", "Draw"), LanguageDisplayName(@"WhiteBalance", "Draw")]
        public int WhiteBalance { get; set; }
        //41992 比对(标准\柔和\强烈)
        /// <summary>
        /// 
        /// </summary>
        [LanguageCategory("ImageInfo", "Draw"), LanguageDisplayName(@"Contrast", "Draw")]
        public int Contrast { get; set; }

        //41993 饱和度(标准\低饱和\高饱和)
        /// <summary>
        /// 
        /// </summary>
        [LanguageCategory("ImageInfo", "Draw"), LanguageDisplayName(@"Saturation", "Draw")]
        public int Saturation { get; set; }

        /// <summary>
        /// 282,283
        /// </summary>
        [LanguageCategory("ImageInfo", "Draw"), LanguageDisplayName(@"Resolution", "Draw")]
        public Size? Resolution { get; set; }

        /// <summary>
        /// 37377
        /// </summary>
        [LanguageCategory("ImageInfo", "Draw"), LanguageDisplayName(@"ShutterSpeed", "Draw")]
        public double ShutterSpeed { get; set; }
        /// <summary>
        /// 33434
        /// </summary>
        [LanguageCategory("ImageInfo", "Draw"), LanguageDisplayName(@"ExposureTime", "Draw")]
        public double ExposureTime { get; set; }
        #endregion

        #region Thumbnail

        /// <summary>
        /// 20525,20526
        /// </summary>
        [LanguageCategory("Thumbnail", "Draw"), LanguageDisplayName(@"Resolution", "Draw")]
        public Size? ThumbnailResolution { get; set; }

        /// <summary>
        /// 20528
        /// </summary>
        [LanguageCategory("Thumbnail", "Draw"), LanguageDisplayName(@"ResolutionUnit", "Draw")]
        public int ThumbnailResolutionUnit { get; set; }

        /// <summary>
        /// 20515
        /// </summary>
        [LanguageCategory("Thumbnail", "Draw"), LanguageDisplayName(@"Compression")]
        public int ThumbnailCompression { get; set; }

        /// <summary>
        /// 20507
        /// </summary>
        [LanguageCategory("Thumbnail", "Draw"), LanguageDisplayName(@"Thumbnail", "Draw")]
        public byte[] ThumbnailData { get; set; }

        #endregion

        #region GPS
        /// <summary>
        /// 
        /// </summary>
        [LanguageCategory("GeoInfo", "Draw"), LanguageDisplayName(@"GPS", "Draw")]
        public GPSGeo GPS { get; set; }
        #endregion

        /// <summary>
        /// 
        /// </summary>
        [LanguageCategory("ImageInfo", "Draw"), LanguageDisplayName(@"AllExifs", "Draw")]
        public ExifPropertyCollection Properties { get; protected set; }

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


            public IEnumerator GetEnumerator()
            {
                return list.GetEnumerator();
            }

            public void CopyTo(Array array, int index)
            {
                list.CopyTo(array, index);
            }

            public int Count
            {
                get { return list.Count; }
            }

            public object SyncRoot
            {
                get { return list.SyncRoot; }
            }

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

            public PropertyTagId TagId { get; protected set; }
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
                                }
                                break;
                            }
                        case PropertyTagType.SLONG:
                        case PropertyTagType.SRational:
                            {
                                switch (Len)
                                {
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

            public override string ToString()
            {
                return string.Format("{0}[{1}]", TagId, Type);
            }
        }
        public enum PropertyTagType
        {
            PixelFormat4bppIndexed = 0,
            Byte = 1,
            ASCII = 2,
            Short = 3,
            Long = 4,
            Rational = 5,
            Undefined = 7,
            SLONG = 9,
            SRational = 10
        }
        public enum PropertyTagId
        {
            GpsVer = 0x0000,
            GpsLatitudeRef = 0x0001,
            GpsLatitude = 0x0002,
            GpsLongitudeRef = 0x0003,
            GpsLongitude = 0x0004,
            GpsAltitudeRef = 0x0005,
            GpsAltitude = 0x0006,
            GpsGpsTime = 0x0007,
            GpsGpsSatellites = 0x0008,
            GpsGpsStatus = 0x0009,
            GpsGpsMeasureMode = 0x000A,
            GpsGpsDop = 0x000B,
            GpsSpeedRef = 0x000C,
            GpsSpeed = 0x000D,
            GpsTrackRef = 0x000E,
            GpsTrack = 0x000F,
            GpsImgDirRef = 0x0010,
            GpsImgDir = 0x0011,
            GpsMapDatum = 0x0012,
            GpsDestLatRef = 0x0013,
            GpsDestLat = 0x0014,
            GpsDestLongRef = 0x0015,
            GpsDestLong = 0x0016,
            GpsDestBearRef = 0x0017,
            GpsDestBear = 0x0018,
            GpsDestDistRef = 0x0019,
            GpsDestDist = 0x001A,
            NewSubfileType = 0x00FE,
            SubfileType = 0x00FF,
            ImageWidth = 0x0100,
            ImageHeight = 0x0101,
            BitsPerSample = 0x0102,
            Compression = 0x0103,
            PhotometricInterp = 0x0106,
            ThreshHolding = 0x0107,
            CellWidth = 0x0108,
            CellHeight = 0x0109,
            FillOrder = 0x010A,
            DocumentName = 0x010D,
            ImageDescription = 0x010E,
            EquipMake = 0x010F,
            EquipModel = 0x0110,
            StripOffsets = 0x0111,
            Orientation = 0x0112,
            SamplesPerPixel = 0x0115,
            RowsPerStrip = 0x0116,
            StripBytesCount = 0x0117,
            MinSampleValue = 0x0118,
            MaxSampleValue = 0x0119,
            XResolution = 0x011A,
            YResolution = 0x011B,
            PlanarConfig = 0x011C,
            PageName = 0x011D,
            XPosition = 0x011E,
            YPosition = 0x011F,
            FreeOffset = 0x0120,
            FreeByteCounts = 0x0121,
            GrayResponseUnit = 0x0122,
            GrayResponseCurve = 0x0123,
            T4Option = 0x0124,
            T6Option = 0x0125,
            ResolutionUnit = 0x0128,
            PageNumber = 0x0129,
            TransferFunction = 0x012D,
            SoftwareUsed = 0x0131,
            DateTime = 0x0132,
            Artist = 0x013B,
            HostComputer = 0x013C,
            Predictor = 0x013D,
            WhitePoint = 0x013E,
            PrimaryChromaticities = 0x013F,
            ColorMap = 0x0140,
            HalftoneHints = 0x0141,
            TileWidth = 0x0142,
            TileLength = 0x0143,
            TileOffset = 0x0144,
            TileByteCounts = 0x0145,
            InkSet = 0x014C,
            InkNames = 0x014D,
            NumberOfInks = 0x014E,
            DotRange = 0x0150,
            TargetPrinter = 0x0151,
            ExtraSamples = 0x0152,
            SampleFormat = 0x0153,
            SMinSampleValue = 0x0154,
            SMaxSampleValue = 0x0155,
            TransferRange = 0x0156,
            JPEGProc = 0x0200,
            JPEGInterFormat = 0x0201,
            JPEGInterLength = 0x0202,
            JPEGRestartInterval = 0x0203,
            JPEGLosslessPredictors = 0x0205,
            JPEGPointTransforms = 0x0206,
            JPEGQTables = 0x0207,
            JPEGDCTables = 0x0208,
            JPEGACTables = 0x0209,
            YCbCrCoefficients = 0x0211,
            YCbCrSubsampling = 0x0212,
            YCbCrPositioning = 0x0213,
            REFBlackWhite = 0x0214,
            Gamma = 0x0301,
            ICCProfileDescriptor = 0x0302,
            SRGBRenderingIntent = 0x0303,
            ImageTitle = 0x0320,
            ResolutionXUnit = 0x5001,
            ResolutionYUnit = 0x5002,
            ResolutionXLengthUnit = 0x5003,
            ResolutionYLengthUnit = 0x5004,
            PrintFlags = 0x5005,
            PrintFlagsVersion = 0x5006,
            PrintFlagsCrop = 0x5007,
            PrintFlagsBleedWidth = 0x5008,
            PrintFlagsBleedWidthScale = 0x5009,
            HalftoneLPI = 0x500A,
            HalftoneLPIUnit = 0x500B,
            HalftoneDegree = 0x500C,
            HalftoneShape = 0x500D,
            HalftoneMisc = 0x500E,
            HalftoneScreen = 0x500F,
            JPEGQuality = 0x5010,
            GridSize = 0x5011,
            ThumbnailFormat = 0x5012,
            ThumbnailWidth = 0x5013,
            ThumbnailHeight = 0x5014,
            ThumbnailColorDepth = 0x5015,
            ThumbnailPlanes = 0x5016,
            ThumbnailRawBytes = 0x5017,
            ThumbnailSize = 0x5018,
            ThumbnailCompressedSize = 0x5019,
            ColorTransferFunction = 0x501A,
            ThumbnailData = 0x501B,
            ThumbnailImageWidth = 0x5020,
            ThumbnailImageHeight = 0x5021,
            ThumbnailBitsPerSample = 0x5022,
            ThumbnailCompression = 0x5023,
            ThumbnailPhotometricInterp = 0x5024,
            ThumbnailImageDescription = 0x5025,
            ThumbnailEquipMake = 0x5026,
            ThumbnailEquipModel = 0x5027,
            ThumbnailStripOffsets = 0x5028,
            ThumbnailOrientation = 0x5029,
            ThumbnailSamplesPerPixel = 0x502A,
            ThumbnailRowsPerStrip = 0x502B,
            ThumbnailStripBytesCount = 0x502C,
            ThumbnailResolutionX = 0x502D,
            ThumbnailResolutionY = 0x502E,
            ThumbnailPlanarConfig = 0x502F,
            ThumbnailResolutionUnit = 0x5030,
            ThumbnailTransferFunction = 0x5031,
            ThumbnailSoftwareUsed = 0x5032,
            ThumbnailDateTime = 0x5033,
            ThumbnailArtist = 0x5034,
            ThumbnailWhitePoint = 0x5035,
            ThumbnailPrimaryChromaticities = 0x5036,
            ThumbnailYCbCrCoefficients = 0x5037,
            ThumbnailYCbCrSubsampling = 0x5038,
            ThumbnailYCbCrPositioning = 0x5039,
            ThumbnailRefBlackWhite = 0x503A,
            ThumbnailCopyRight = 0x503B,
            LuminanceTable = 0x5090,
            ChrominanceTable = 0x5091,
            FrameDelay = 0x5100,
            LoopCount = 0x5101,
            GlobalPalette = 0x5102,
            IndexBackground = 0x5103,
            IndexTransparent = 0x5104,
            PixelUnit = 0x5110,
            PixelPerUnitX = 0x5111,
            PixelPerUnitY = 0x5112,
            PaletteHistogram = 0x5113,
            Copyright = 0x8298,
            ExifExposureTime = 0x829A,
            ExifFNumber = 0x829D,
            ExifIFD = 0x8769,
            ICCProfile = 0x8773,
            ExifExposureProg = 0x8822,
            ExifSpectralSense = 0x8824,
            GpsIFD = 0x8825,
            ExifISOSpeed = 0x8827,
            ExifOECF = 0x8828,
            ExifVer = 0x9000,
            ExifDTOrig = 0x9003,
            ExifDTDigitized = 0x9004,
            ExifCompConfig = 0x9101,
            ExifCompBPP = 0x9102,
            ExifShutterSpeed = 0x9201,
            ExifAperture = 0x9202,
            ExifBrightness = 0x9203,
            ExifExposureBias = 0x9204,
            ExifMaxAperture = 0x9205,
            ExifSubjectDist = 0x9206,
            ExifMeteringMode = 0x9207,
            ExifLightSource = 0x9208,
            ExifFlash = 0x9209,
            ExifFocalLength = 0x920A,
            ExifMakerNote = 0x927C,
            ExifUserComment = 0x9286,
            ExifDTSubsec = 0x9290,
            ExifDTOrigSS = 0x9291,
            ExifDTDigSS = 0x9292,
            ExifFPXVer = 0xA000,
            ExifColorSpace = 0xA001,
            ExifPixXDim = 0xA002,
            ExifPixYDim = 0xA003,
            ExifRelatedWav = 0xA004,
            ExifInterop = 0xA005,
            ExifFlashEnergy = 0xA20B,
            ExifSpatialFR = 0xA20C,
            ExifFocalXRes = 0xA20E,
            ExifFocalYRes = 0xA20F,
            ExifFocalResUnit = 0xA210,
            ExifSubjectLoc = 0xA214,
            ExifExposureIndex = 0xA215,
            ExifSensingMethod = 0xA217,
            ExifFileSource = 0xA300,
            ExifSceneType = 0xA301,
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
                switch (hex)
                {
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
        /// <summary>
        /// 
        /// </summary>
        /// <param name="LightSource"></param>
        /// <returns></returns>
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
        /// <summary>
        /// 
        /// </summary>
        /// <param name="flash"></param>
        /// <returns></returns>
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