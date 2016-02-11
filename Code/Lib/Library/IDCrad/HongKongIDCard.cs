using System;
using System.ComponentModel;
using System.Runtime.InteropServices;

namespace Library.IDCrad
{
    /// <summary>
    /// 中華人民共和國香港特別行政區居民身份證
    /// </summary>
    [Guid("BE8FEC7D-7DE9-46F1-A1FF-A2B257CAEDBC")]
    public class HongKongIDCard : IIDCard
    {
        /*
         在香港身份证上，出生日期项目之下，会印有一串符号及英文字母及数字（例如***AZ）

第一及第二代电脑身份证

符号	注解
***	持证人年龄为18岁或以上及有资格申领（香港特别行政区）回港证
*	持证人年龄为11岁至17岁及有资格申领（香港特别行政区）回港证
A	持证人拥有香港居留权
C	持证人登记领证时在香港的居留受到（人民）入境事务处处长的限制
F	持证人的性别是女性
H1	身份证发出的办事处代号
L	持证人曾遗失身份证，L1表示遗失一次，L2表示遗失二次，如此类推
M	持证人的性别是男性
N	持证人所报的姓名自首次登记以后，曾作出更改
O	持证人报称在香港、澳门及中国以外其他地区或国家出生（包括台湾和英国）
K1, K2	身份证发出的办事处代号
P1	身份证发出的办事处代号
R	持证人拥有香港入境权
S1, S2	身份证发出的办事处代号
U	持证人登记领证时在香港的居留不受（人民）入境事务处处长的限制
V1	身份证发出的办事处代号
W	持证人报称在澳门地区出生
X	持证人报称在中国出生
Y	持证人所报的出生日期已由（人民）入境事务处与其出生证明书或护照核对
Z	持证人报称在香港出生
智能身份证[9]

符号	注解
***	持证人年龄为18岁或以上及有资格申领香港特别行政区回港证
*	持证人年龄为11岁至17岁及有资格申领香港特别行政区回港证
A	持证人拥有香港居留权
B	持证人所报称的出生日期或地点自首次登记以后，曾作出更改
C	持证人登记领证时在香港的居留受到入境事务处处长的限制
N	持证人所报的姓名自首次登记以后，曾作出更改
O	持证人报称在香港、澳门及中国以外其他地区或国家出生
R	持证人拥有香港入境权
U	持证人登记领证时在香港的居留不受入境事务处处长的限制
W	持证人报称在澳门地区出生
X	持证人报称在中国出生
Z	持证人报称在香港出生

         */

        private static readonly Guid Cardtype = Guid.Parse("BE8FEC7D-7DE9-46F1-A1FF-A2B257CAEDBC");
        private const string Cardname = "中華人民共和國香港特別行政區居民身份證";

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
        public HongKongIDCard(string idnumber)
        {
            IDNumber = idnumber;
            Validate();
        }

        /// <summary>
        ///
        /// </summary>
        public void Validate()
        {
            throw new NotImplementedException();
        }
    }
}