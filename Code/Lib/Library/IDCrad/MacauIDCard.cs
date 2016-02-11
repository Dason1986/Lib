using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;

namespace Library.IDCrad
{
    /// <summary>
    /// 中華人民共和國澳門特別行政區居民身份證
    /// </summary>
    [Guid("4B9AF4D5-C837-4087-B031-4BCB9B94A3F2")]
    public class MacauIDCard : IIDCard
    {
        /*
         身份证上会印有葡萄牙文字母（例如ASM），其确实代表的意思如下：
符号注解备注
A持证人于澳门出生
B持证人于香港出生
C持证人于中国大陆、台湾出生
D持证人于其他国家及地区出生
N持证人出生地不明指持证人不知道自己在何处出生
S持证人有出生证明文件如无出生证明文件则会漏空 智能身份证时新增
M 持证人为男性（Masculino）智能身份证时新增
F持证人为女性（Feminino）智能身份证时新增 [4]

1992年起发出的居民身份证均印有一个身份证号码，身份证号码由8个拉丁数字组成（格式为“X/NNNNNN/Y”）；澳葡政府把当时的葡萄牙国民身份证号码的6个拉丁数字作为澳门身份证的主要号码；在“/”符号前加上一个拉丁数字1、5或7以代表其取证时代，而在“/”符号后加上的拉丁数字则为查核用数码，是为方便电脑处理资料及检查号码输入的正确性而设。智能身份证将原有格式（X/NNNNNN/Y）改为XNNNNNN(Y)。
1字开首的身份证号码代表是因龙的行动或1992年后领取身份证之人士，现在新发出的身份证亦以1字开首；5字开首的身份证号码代表持有或曾经持有葡萄牙国民身份证或葡萄牙给外国人身份证之人士；7字开首代表曾经取得蓝卡之人士，大多都是在1970年代至1980年代期间从中国大陆持合法证件到澳门的人士。
         */
        private static readonly Guid Cardtype = Guid.Parse("4B9AF4D5-C837-4087-B031-4BCB9B94A3F2");
        private const string Cardname = "中華人民共和國澳門特別行政區居民身份證";

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
        public MacauIDCard(string idnumber)
        {
            IDNumber = idnumber;
            Validate();
        }

        /// <summary>
        ///
        /// </summary>
        public void Validate()
        {
            if ((!Regex.IsMatch(IDNumber, @"\d{7}(d{1})", RegexOptions.IgnoreCase))) throw new IDCardException("證件號碼格式不符合");
            var code = IDNumber[0];
            switch (code)
            {
                case '1': break;
                case '5': break;
                case '7': break;
                default:
                    throw new IDCardException("證件號碼格式不符合");
            }
        }
    }
}