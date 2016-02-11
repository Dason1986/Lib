using Library.HelperUtility;
using System;
using System.ComponentModel;
using System.Globalization;
using System.Runtime.InteropServices;

namespace Library.IDCrad
{
    /// <summary>
    /// 中華人民共和國臺灣居民身份證
    /// </summary>
    [Guid("385F62C2-2255-4886-A81E-01A5C4355DAB")]
    public class TaiwanIDCard : IIDCard
    {
        /*
目前的中华民国身分证字号一共有十码，包括起首一个大写的英文字母与接续的九个阿拉伯数字。

中华民国身分证字号中的英文字母是以初次登记的户籍地来区分编号的，而首位数字则是拿来区分性别，男性为1、女性为2，与ISO 5218以及类似的《中华民国国家标准》CNS 8381《资讯交换-人类性别表示法》的用法相同。

中华民国身分证字号英文字首的编号规则数字如下：

使用中的字母：	已停发的字母：
字母	转换字符	县市
A	10	台北市
B	11	台中市
C	12	基隆市
D	13	台南市
E	14	高雄市
F	15	新北市
G	16	宜兰县
H	17	桃园市
I	34	嘉义市
J	18	新竹县
K	19	苗栗县
字母	转换字符	县市
M	21	南投县
N	22	彰化县
O	35	新竹市
P	23	云林县
Q	24	嘉义县
T	27	屏东县
U	28	花莲县
V	29	台东县
W	32	金门县
X	30	澎湖县
Z	33	连江县
字母	转换字符	原行政区	停发日期	现行行政区
L	20	台中县	2010年12月25日	台中市
R	25	台南县	2010年12月25日	台南市
S	26	高雄县	2010年12月25日	高雄市
Y	31	阳明山管理局	1975年	台北市
原英文字首的编号并无“I”和“O”，因容易和数字“1”和“0”混淆。1982年新竹市及嘉义市升格为省辖市后，才开始使用该两个英文字首编号。
1967年，台北市升格直辖市，阳明山管理局划归台北市管辖后，Y字头的身分证字号使用至1974年阳明山管理局虚位化为止。1975年以后于台北市士林区、北投区初次登记户籍者，改发与台北市相同A字头的身分证字号。
2010年12月25日，部分县市改制直辖市。台中县、台南县、高雄县于县市合并之后裁撤，其代码L、R、S停发。改制后登记户籍者改发台中市（B）、台南市（D）、高雄市（E）之代码。
验证规则[编辑]
中华民国身分证的号码可不是随意就可产生，是经由一串公式所产生出来的[19]
例如：
B120863158[20]，B的转换字符是11，所以是

1	1	1	2	0	8	6	3	1	5	8
n1	n2	n3	n4	n5	n6	n7	n8	n9	n10	n11
然后再把每一个数字，依序乘上 1 9 8 7 6 5 4 3 2 1 1 ，再相加
n1\times 1+n2\times 9+n3\times 8 +n4\times 7+n5\times 6 +n6\times 5+n7\times 4 +n8\times 3+n9\times 2 +n10\times 1+n11\times 1
套入公式，结果为
1 \times 1+1 \times 9+1 \times 8 +2 \times 7+0 \times 6 +8 \times 5+6 \times 4 +3 \times 3+1 \times 2 +5 \times 1+8 \times 1
=1+9+8+14+40+24+9+2+5+8
=120
然后再除以10
\frac{120}{10}
如果整除，该组号码有效
120 \equiv 0 \pmod{10}

         */

        private static readonly string[,] Citycodes =
        {
            {"A","10","台北市"},{"B","11","台中市"},{"C","12","基隆市"},{"D","13","台南市"},{"E","14","高雄市"},
            {"F","15","新北市"},{"G","16","宜兰县"},{"H","17","桃园市"},{"I","34","嘉义市"},{"J","18","新竹县"},
            {"K","19","苗栗县"},{"M","21","南投县"},{"N","22","彰化县"},{"O","35","新竹市"},{"P","23","云林县"},
            {"Q","24","嘉义县"},{"T","27","屏东县"},{"U","28","花莲县"},{"V","29","台东县"},{"W","32","金门县"},
            {"X","30","澎湖县"},{"Z","33","连江县"},{"L","20","台中县"},{"R","25","台南县"},{"S","26","高雄县"},{"Y","31","阳明山管理局"}
        };

        private static readonly Guid Cardtype = Guid.Parse("385F62C2-2255-4886-A81E-01A5C4355DAB");
        private const string Cardname = "中華人民共和國臺灣居民身份證";

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
        public int Version { get { return 6; } }

        /// <summary>
        ///
        /// </summary>
        [Category("證件信息"), DisplayName("證件號碼")]
        public string IDNumber { get; private set; }

        /// <summary>
        ///
        /// </summary>
        /// <param name="idnumber"></param>
        public TaiwanIDCard(string idnumber)
        {
            IDNumber = idnumber;
            Validate();
        }

        private readonly int[] _coefficientCodes = { 0x9, 0x8, 0x7, 0x6, 0x5, 0x4, 0x3, 0x2, 0x1, 0x1 };

        /// <summary>
        ///
        /// </summary>
        /// <exception cref="IDCardException"></exception>
        public void Validate()
        {
            if (IDNumber == null || IDNumber.Length != 15) throw new IDCardException("證件號碼格式不符合");
            CityCode = IDNumber[0].ToString(CultureInfo.InvariantCulture).ToUpper();
            var number = 0;
            ChecksumDigitCode = IDNumber.Substring(12, 2);
            for (int i = 0; i < Citycodes.Length; i++)
            {
                if (Citycodes[i, 0] != CityCode) continue;
                CityName = Citycodes[0, 2];
                number = StringUtility.TryCast<int>(Citycodes[0, 1]);
                break;
            }

            for (int i = 1; i < _coefficientCodes.Length; i++)
            {
                number += number + (Convert.ToInt32(IDNumber[i].ToString(CultureInfo.InvariantCulture)) * _coefficientCodes[i]);
            }
            if (number % 10 != 0) throw new IDCardException("證件號碼驗證不通過");
            if (number.ToString(CultureInfo.InvariantCulture).Substring(1, 2) != ChecksumDigitCode) throw new IDCardException("證件號碼检验码不通過");
        }

        /// <summary>
        /// 检验码
        /// </summary>
        [Category("證件信息"), DisplayName("检验码")]
        public string ChecksumDigitCode { get; private set; }

        /// <summary>
        /// 城市名称
        /// </summary>
        [Category("證件信息"), DisplayName("城市名称")]
        public string CityName { get; private set; }

        /// <summary>
        /// 城市代码
        /// </summary>
        [Category("證件信息"), DisplayName("城市代码")]
        public string CityCode { get; private set; }
    }
}