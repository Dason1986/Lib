using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Library
{
    /// <summary>
    /// 
    /// </summary>
    public   class ByteSize
    {
        /// <summary>
        /// 
        /// </summary>
        public long Size { get; protected set; }
        /// <summary>
        /// 
        /// </summary>
        public virtual decimal Unit { get { return Size; } }
        /// <summary>
        /// 
        /// </summary>
        public virtual string UnitFullName { get { return "Byte"; } }
        /// <summary>
        /// 
        /// </summary>
        public virtual string UnitName { get { return "b"; } }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="size"></param>
        public ByteSize(long size)
        {
            Size = size;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return ToString().GetHashCode();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            if(UnitName=="b")return string.Format("{0}{1}", Unit, UnitName);
            return string.Format("{0:f2}{1}", Unit, UnitName);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class KBSize : ByteSize
    {


        /// <summary>
        /// 
        /// </summary>
        public decimal KBs { get; protected set; }

        /// <summary>
        /// 
        /// </summary>
        public override string UnitFullName { get { return "Kilobyte"; } }
        /// <summary>
        /// 
        /// </summary>
        public override string UnitName { get { return "KB"; } }

        /// <summary>
        /// 
        /// </summary>
        public override decimal Unit { get { return KBs; } }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="bytesdize"></param>
        public KBSize(long bytesdize) : base(bytesdize)
        {
            KBs = (decimal)bytesdize / 1024;
        }

    }
    /// <summary>
    /// 
    /// </summary>
    public class MBSize : KBSize
    {
        /// <summary>
        /// 
        /// </summary>
        public decimal MBs { get; protected set; }

        /// <summary>
        /// 
        /// </summary>
        public override decimal Unit { get { return MBs; } }
        /// <summary>
        /// 
        /// </summary>
        public override string UnitFullName { get { return "Megabyte"; } }
        /// <summary>
        /// 
        /// </summary>
        public override string UnitName { get { return "MB"; } }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="bytesdize"></param>
        public MBSize(long bytesdize) : base(bytesdize)
        {
            MBs = KBs / 1024;
        }

    }
    /// <summary>
    /// 
    /// </summary>
    public class GBSize : MBSize
    {
        /// <summary>
        /// 
        /// </summary>
        public decimal GBs { get; protected set; }

        /// <summary>
        /// 
        /// </summary>
        public override decimal Unit { get { return GBs; } }

        /// <summary>
        /// 
        /// </summary>
        public override string UnitFullName { get { return "Gigabyte"; } }
        /// <summary>
        /// 
        /// </summary>
        public override string UnitName { get { return "GB"; } }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="bytesdize"></param>
        public GBSize(long bytesdize) : base(bytesdize)
        {
            GBs = MBs / 1024;
        }

    }
    /// <summary>
    /// 
    /// </summary>
    public class TBSize : GBSize
    {
        /// <summary>
        /// 
        /// </summary>
        public decimal TBs { get; protected set; }

        /// <summary>
        /// 
        /// </summary>
        public override decimal Unit { get { return TBs; } }
        /// <summary>
        /// 
        /// </summary>
        public override string UnitFullName { get { return "Terabyte"; } }
        /// <summary>
        /// 
        /// </summary>
        public override string UnitName { get { return "TB"; } }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="bytesdize"></param>
        public TBSize(long bytesdize) : base(bytesdize)
        {
            TBs = GBs / 1024;
        }

    }

    /// <summary>
    /// 
    /// </summary>
    public class PBSize : TBSize
    {
        /// <summary>
        /// 
        /// </summary>
        public decimal PBs { get; protected set; }

        /// <summary>
        /// 
        /// </summary>
        public override decimal Unit { get { return PBs; } }
        /// <summary>
        /// 
        /// </summary>
        public override string UnitFullName { get { return "Petabyte"; } }
        /// <summary>
        /// 
        /// </summary>
        public override string UnitName { get { return "PB"; } }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="bytesdize"></param>
        public PBSize(long bytesdize) : base(bytesdize)
        {
            PBs = TBs / 1024;
        }

    }
}
