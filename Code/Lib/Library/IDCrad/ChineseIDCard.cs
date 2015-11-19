using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Xml;
using Library.Annotations;
using Library.ComponentModel;
using Library.HelperUtility;

namespace Library.IDCrad
{
    /// <summary>
    /// 中華人民共和國大陆居民身份證
    /// </summary>
    [Guid("98FBE5BE-1030-4AB6-A9D1-F0D3E7CF3865")]
    public class ChineseIDCard : IIDCard
    {

        /*
1．号码的结构 
　　公民身份号码是特征组合码，由十七位数字本体码和一位校验码组成。排列顺序从左至右依次为：六位数字地址码，八位数字出生日期码，三位数字顺序码和一位数字校验码。 
　　2．地址码 
　　表示编码对象常住户口所在县（市、旗、区）的行政区划代码，按GB/T2260的规定执行。 
　　3．出生日期码 
　　表示编码对象出生的年、月、日，按GB/T7408的规定执行，年、月、日代码之间不用分隔符。 
　　4．顺序码 
　　表示在同一地址码所标识的区域范围内，对同年、同月、同日出生的人编定的顺序号，顺序码的奇数分配给男性，偶数分配给女性。 
　　5．校验码
　　根据前面十七位数字码，按照ISO 7064:1983.MOD 11-2校验码计算出来的检验码。
地址码

（身份证号码前六位）表示编码对象常住户口所在县（市、镇、区）的行政区划代码。
北京市|110000，天津市|120000，河北省|130000，山西省|140000，内蒙古自治区|150000，辽宁省|210000，吉林省|220000，黑龙江省|230000，上海市|310000，江苏省|320000，浙江省|330000，安徽省|340000，福建省|350000，江西省|360000，山东省|370000，河南省|410000，湖北省|420000，湖南省|430000，广东省|440000，广西壮族自治区|450000，海南省|460000，重庆市|500000，四川省|510000，贵州省|520000，云南省|530000，西藏自治区|540000，陕西省|610000，甘肃省|620000，青海省|630000，宁夏回族自治区|640000，新疆维吾尔自治区|650000，台湾省（886)|710000,香港特别行政区（852)|810000，澳门特别行政区（853)|820000
大陆居民身份证号码中的地址码的数字编码规则为：
第一、二位表示省（自治区、直辖市、特别行政区）。
第三、四位表示市（地区、自治州、盟及国家直辖市所属市辖区和县的汇总码）。其中，01-20，51-70表示省直辖市；21-50表示地区（自治州、盟）。
第五、六位表示县（市辖区、县级市、旗）。01-18表示市辖区或地区（自治州、盟）辖县级市；21-80表示县（旗）；81-99表示省直辖县级市。
生日期码

（身份证号码第七位到第十四位）表示编码对象出生的年、月、日，其中年份用四位数字表示，年、月、日之间不用分隔符。例如：1981年05月11日就用19810511表示。
顺序码

（身份证号码第十五位到十七位）地址码所标识的区域范围内，对同年、月、日出生的人员编定的顺序号。其中第十七位奇数分给男性，偶数分给女性。
校验码

作为尾号的校验码，是由号码编制单位按统一的公式计算出来的，如果某人的尾号是0-9，都不会出现X，但如果尾号是10，那么就得用X来代替，因为如果用10做尾号，那么此人的身份证就变成了19位，而19位的号码违反了国家标准，并且中国的计算机应用系统也不承认19位的身份证号码。Ⅹ是罗马数字的10，用X来代替10，可以保证公民的身份证符合国家标准。         

计算方法

1、将前面的身份证号码17位数分别乘以不同的系数。从第一位到第十七位的系数分别为：7－9－10－5－8－4－2－1－6－3－7－9－10－5－8－4－2。
2、将这17位数字和系数相乘的结果相加。
3、用加出来和除以11，看余数是多少？
4、余数只可能有0－1－2－3－4－5－6－7－8－9－10这11个数字。其分别对应的最后一位身份证的号码为1－0－X －9－8－7－6－5－4－3－2。
5、通过上面得知如果余数是3，就会在身份证的第18位数字上出现的是9。如果对应的数字是2，身份证的最后一位号码就是罗马数字x。
例如：某男性的身份证号码为【53010219200508011x】， 我们看看这个身份证是不是合法的身份证。
首先我们得出前17位的乘积和【(5*7)+(3*9)+(0*10)+(1*5)+(0*8)+(2*4)+(1*2)+(9*1)+(2*6)+(0*3)+(0*7)+(5*9)+(0*10)+(8*5)+(0*8)+(1*4)+(1*2)】是189，然后用189除以11得出的结果是189/11=17----2，也就是说其余数是2。最后通过对应规则就可以知道余数2对应的检验码是X。所以，可以判定这是一个正确的身份证号码。         
      
         * 
     地址码     http://www.stats.gov.cn/tjsj/tjbz/xzqhdm/ 
         */

        internal static readonly int[] CoefficientCodes = { 7, 9, 10, 5, 8, 4, 2, 1, 6, 3, 7, 9, 10, 5, 8, 4, 2 };

        internal static readonly IDictionary<int, string> DicCoefficientCodes = new Dictionary<int, string>
        {
            {0,"1"},{1,"0"},{2,"x"},{3,"9"},{4,"8"},{5,"7"},
            {6,"6"},{7,"5"},{8,"4"},{9,"3"},{10,"2"},
        };

        internal static readonly IDictionary<int, string> DicProvinceCodes = new Dictionary<int, string>
        {
            {11,"北京市"},{12,"天津市"},{13,"河北省"},{14,"山西省"},{15,"内蒙古自治区"},
            {21,"辽宁省"},{22,"吉林省"},{23,"黑龙江省"},    
            {31,"上海市"},{32,"江苏省"},{33,"浙江省"},{34,"安徽省"},{35,"福建省"},{36,"江西省"},{37,"山东省"},
            {41,"河南省"},{42,"湖北省"},{43,"湖南省"},{44,"广东省"},{45,"广西壮族自治区"},{46,"海南省"},    
            {50,"重庆市"},{51,"四川省"},{52,"贵州省"},{53,"云南省"},{54,"西藏自治区"},
            {61,"陕西省"},{62,"甘肃省"},{63,"青海省"},{64,"宁夏回族自治区"},{65,"新疆维吾尔自治区"},
            {7121,"台湾省"},{81,"香港特别行政区"},{82,"澳门特别行政区"},  
        };
        private static readonly Guid Cardtype = Guid.Parse("98FBE5BE-1030-4AB6-A9D1-F0D3E7CF3865");
        private const string Cardname = "中華人民共和國大陆居民身份證";
        /// <summary>
        /// 
        /// </summary>
        [Category("證件"), DisplayName("證件類型Guid")]
        public Guid CardTypeID { get { return Cardtype; } }
        /// <summary>
        /// 
        /// </summary>
        [Category("證件"), DisplayName("證件類型名稱")]
        public string CardTypeName { get { return Cardname; } }

        /// <summary>
        /// 
        /// </summary>
        [Category("證件"), DisplayName("證件版本")]
        public int Version { get { return 2; } }
        /// <summary>
        /// 
        /// </summary>
        [Category("證件信息"), DisplayName("證件號碼")]
        public string IDNumber { get; private set; }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="idnumber"></param>
        public ChineseIDCard(string idnumber)
        {
            IDNumber = idnumber;
            Validate();

        }

        /// <summary>
        /// 
        /// </summary>
        /// <exception cref="NotImplementedException"></exception>
        public void Validate()
        {

            if ((!Regex.IsMatch(IDNumber, @"^\d{18}$|^\d{17}(\d|X|x)$", RegexOptions.IgnoreCase))) throw new IDCardException("證件號碼格式不符合", 11001.1);

            ValidateProvince();
            ValidateCity();
            ValidateCounty();
            ValidateBirthday();
            ValidateSix();
            ValidateByChecksumDigit();
        }
        /// <summary>
        /// 
        /// </summary>
        [Category("證件信息"), DisplayName("省份值")]
        public int ProvinceCode { get; private set; }
        /// <summary>
        /// 
        /// </summary>
        [Category("證件信息"), DisplayName("省名稱")]
        public string ProvinceName { get; private set; }
        /// <summary>
        /// 
        /// </summary>
        [Category("證件信息"), DisplayName("城市值")]
        public int CityCode { get; private set; }
        /// <summary>
        /// 
        /// </summary>
        [Category("證件信息"), DisplayName("縣區值")]
        public int CountyCode { get; private set; }
        /// <summary>
        /// 
        /// </summary>
        [Category("證件信息"), DisplayName("校驗碼")]
        public string ChecksumDigitCode { get; private set; }
        /// <summary>
        /// 
        /// </summary>
        [Category("證件信息"), DisplayName("出生日期值")]
        public string BirthdayCode { get; private set; }
        /// <summary>
        /// 
        /// </summary>
        [Category("證件信息"), DisplayName("出生日期")]
        public DateTime Birthday { get; private set; }
        /// <summary>
        /// 
        /// </summary>
        [Category("證件信息"), DisplayName("性別值")]
        public int SixCode { get; private set; }
        /// <summary>
        /// 
        /// </summary>
        [Category("證件信息"), DisplayName("性")]
        public SexEnum Sex { get; private set; }
        private void ValidateProvince()
        {
            var code = StringUtility.TryCast<int>(IDNumber.Substring(0, 2));
            if (code.HasError) throw new IDCardException("轉換值出錯,不為數字", 11001.105, code.Error);
            if (!DicProvinceCodes.ContainsKey(code.Value)) throw new IDCardException("不存在對應省份", 11001.106);
            ProvinceName = DicProvinceCodes[code.Value];
            ProvinceCode = code.Value;
        }


        private void ValidateCity()
        {
            var code = StringUtility.TryCast<int>(IDNumber.Substring(2, 2));
            if (code.HasError) throw new IDCardException("轉換值出錯,不為數字", 11001.105, code.Error);
            CityCode = code;

        }

        private void ValidateCounty()
        {
            var code = StringUtility.TryCast<int>(IDNumber.Substring(4, 2));
            if (code.HasError) throw new IDCardException("轉換值出錯,不為數字", 11001.105, code.Error);
            CountyCode = code;

        }


        private void ValidateBirthday()
        {
            var code = (IDNumber.Substring(6, 8));
            var yearcode = StringUtility.TryCast<int>(code.Substring(0, 4));
            var monthcode = StringUtility.TryCast<int>(code.Substring(4, 2));
            var daycode = StringUtility.TryCast<int>(code.Substring(6, 2));
            if (yearcode.HasError | monthcode.HasError | daycode.HasError) throw new IDCardException("轉換日期值出錯", 11001.107, yearcode.Error);
            try
            {
                Birthday = new DateTime(yearcode, monthcode, daycode);
            }
            catch (Exception ex)
            {

                throw new IDCardException("轉換出生日期值出錯", 11001.107, ex);
            }

            BirthdayCode = code;
        }




        private void ValidateSix()
        {
            var code = StringUtility.TryCast<int>(IDNumber.Substring(16, 1));
            if (code.HasError) throw new IDCardException("轉換值出錯,不為數字", 11001.105, code.Error);
            SixCode = code.Value;
            Sex = SixCode % 2 == 1 ? SexEnum.Man : SexEnum.Woman;
        }


        private void ValidateByChecksumDigit()
        {
            var digitCode = IDNumber.Substring(17, 1);
            int sum = 0;
            for (int i = 0; i < 17; i++)
            {
                var num = Convert.ToInt32(IDNumber[i].ToString());
                sum += CoefficientCodes[i] * num;
            }
            var remainder = sum % 11;
            if (!DicCoefficientCodes.ContainsKey(remainder)) throw new IDCardException("身份證校驗碼不正確", 11001.104);
            if (!string.Equals(digitCode, DicCoefficientCodes[remainder], StringComparison.OrdinalIgnoreCase)) throw new IDCardException("身份證校驗碼不正確", 11001.104);
            ChecksumDigitCode = digitCode;
        }


    }

    /// <summary>
    /// 
    /// </summary>
    public class ChineseIDCardProvider : IIDCardProvider
    {
        private int _codeRange = 100;
        private int _order = 1;

        /// <summary>
        /// 
        /// </summary>
        public string DistrictFullCode { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DateTime Birthday { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int CodeRange
        {
            get { return _codeRange; }
            set
            {

                _codeRange = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public int Order
        {
            get { return _order; }
            set
            {

                _order = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public SexEnum Sex { get; set; }

        private const string XmlPath = "Library.IDCrad.gbt2260.Db.xml";

        IIDCard IIDCardProvider.CreateNew()
        {
            return CreateNew();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ChineseIDCard CreateNew()
        {
            if (string.IsNullOrEmpty(DistrictFullCode) || DistrictFullCode.Length != 6) throw new IDCardException("省市全值代碼不符合", 11001.101);
            if (Order < 1) throw new IDCardException("順序號不能小於1", 11001.103);
            if (CodeRange < 100 || CodeRange > 990) throw new IDCardException("3位编码段有誤", 11001.102);
            using (Stream xmlStream = Assembly.GetExecutingAssembly().GetManifestResourceStream(XmlPath))
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(xmlStream);
                var node = doc.SelectSingleNode(string.Format("/Code/Province/City/County[@Full='{0}']", DistrictFullCode));
                if (node == null) throw new IDCardException("省市全值代碼不存在", 11001.112);
            }

            var idnumber = string.Format("{0}{1:yyyyMMdd}{2}", DistrictFullCode, Birthday, CodeRange + ((Order - 1) * 2 + (int)Sex));
            int sum = 0;
            for (int i = 0; i < idnumber.Length; i++)
            {
                var num = Convert.ToInt32(idnumber[i].ToString(CultureInfo.InvariantCulture));
                sum += ChineseIDCard.CoefficientCodes[i] * num;
            }
            var remainder = sum % 11;
            if (!ChineseIDCard.DicCoefficientCodes.ContainsKey(remainder)) throw new IDCardException("身份證校驗碼不正確", 11001.104);
            return new ChineseIDCard(string.Format("{0}{1}", idnumber, ChineseIDCard.DicCoefficientCodes[remainder]));

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="districtFullCode">省市全值代碼</param>
        /// <param name="birthday">出生日期</param>
        /// <param name="sex">性別</param>
        /// <param name="codeRange">3位编码段</param>
        /// <param name="order">領證順序</param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static ChineseIDCard CreateNew([NotNull] string districtFullCode, DateTime birthday, SexEnum sex, int codeRange, int order)
        {
            return new ChineseIDCardProvider
            {
                DistrictFullCode = districtFullCode,
                Birthday = birthday,
                Sex = sex,
                CodeRange = codeRange,
                Order = order
            }.CreateNew();
        }
    }
}
