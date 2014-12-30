using System.Drawing;

namespace Library.Draw.Print
{
    /// <summary>
    /// 
    /// </summary>
    public interface IPrintBuilder : System.Collections.IEnumerator
    {
        /// <summary>
        /// 数据
        /// </summary>
        object Model { get; set; }

        /// <summary>
        /// 是否多页
        /// </summary>
        bool HasMorePages { get; }

        /// <summary>
        /// 当前页索引
        /// </summary>
        int CurrentPageIndex { get; }

        /// <summary>
        /// 能否下一页
        /// </summary>
        bool CanNextPange();
        /// <summary>
        /// 总页数
        /// </summary>
        uint TotalPages { get; }

        /// <summary>
        /// 页面区域
        /// </summary>
        Rectangle PageRectangle { get; set; }
        /// <summary>
        /// 预览模式下的底图
        /// </summary>
        Image PreviewBackgroundImage { get; }
        /// <summary>
        /// 超始页
        /// </summary>
        uint FromPage { get; }

        /// <summary>
        /// 结束页
        /// </summary>
        uint ToPage { get; }

        /// <summary>
        /// 生成当前打印内容
        /// </summary>
        /// <returns></returns>
        Image CreateCurrentBitmap();

        /// <summary>
        /// 生成下一页打印内容
        /// </summary>
        /// <returns></returns>
        Image CreateNextBitmap();
        /// <summary>
        /// 验证数据
        /// </summary>
        void Validate();

        /// <summary>
        /// 重置打印索引页
        /// </summary>
        void ResetIndex();

        /// <summary>
        /// 设置打印区间页
        /// </summary>
        /// <param name="formPage">超始页</param>
        /// <param name="toPage">结束页</param>
        void SetPageRange(int formPage, int toPage);
    }
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IPrintBuilder<T> : IPrintBuilder
    {
        /// <summary>
        /// 
        /// </summary>
        new T Model { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public abstract class PrintBuilder : IPrintBuilder
    {
        /// <summary>
        /// 
        /// </summary>
        protected PrintBuilder()
        {
            CurrentPageIndex = -1;
            FromPage = 1;
            ToPage = 1;
            TotalPages = 1;
        }
        /// <summary>
        /// 页面区域
        /// </summary>
        public Rectangle PageRectangle { get; set; }
        /// <summary>
        /// 预览模式下的底图
        /// </summary>
        public Image PreviewBackgroundImage{get;protected set;}
        /// <summary>
        /// 验证数据
        /// </summary>
        public abstract void Validate();
        /// <summary>
        /// 能否下一页
        /// </summary>
        public bool CanNextPange()
        {
            var tmpend = (ToPage == TotalPages) ? TotalPages : ToPage;
            return CurrentPageIndex + 1 < tmpend;
        }
        /// <summary>
        /// 是否多页
        /// </summary>
        public bool HasMorePages { get { return TotalPages > 1; } }

         
        /// <summary>
        /// 当前页索引
        /// </summary>
        public int CurrentPageIndex { get; protected set; }
        /// <summary>
        /// 总页数
        /// </summary>
        public uint TotalPages { get; protected set; }

        public virtual object Model { get; set; }
        /// <summary>
        /// 生成当前打印内容
        /// </summary>
        /// <returns></returns>
        public abstract Image CreateCurrentBitmap();

        /// <summary>
        /// 生成下一页打印内容
        /// </summary>
        /// <returns></returns>
        public virtual Image CreateNextBitmap()
        {
            if (!CanNextPange()) throw new PrintException("没有下一页");
            CurrentPageIndex++;
            return CreateCurrentBitmap();
        }
        /// <summary>
        /// 超始页
        /// </summary>
        public uint FromPage { get; private set; }
        /// <summary>
        /// 结束页
        /// </summary>
        public uint ToPage { get; private set; }
        /// <summary>
        /// 重置打印索引页
        /// </summary>
        public void ResetIndex()
        {

            CurrentPageIndex = -1;
            FromPage = 1;
            ToPage = TotalPages;
        }

        /// <summary>
        /// 设置打印区间页
        /// </summary>
        /// <param name="formPage">超始页</param>
        /// <param name="toPage">结束页</param>
        public void SetPageRange(int formPage, int toPage)
        {
            if (formPage < 1 || toPage < formPage || formPage > TotalPages) throw new PrintException("起始页不能大于结束页，或大于总页数");
            if (toPage < formPage || toPage> TotalPages) throw new PrintException("结束页不能小于起始页，或大于总页数");
            CurrentPageIndex = (formPage - 2);
            FromPage = (uint)formPage;
            ToPage = (uint)toPage;
        }



        private object _current;
        object System.Collections.IEnumerator.Current
        {
            get { return _current; }
        }

        bool System.Collections.IEnumerator.MoveNext()
        {
            var flag = CanNextPange();
            if (!flag) return false;
            _current = CreateNextBitmap();
            return true;
        }

        void System.Collections.IEnumerator.Reset()
        {
            ResetIndex();
        }
    }
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class PrintBuilder<T> : PrintBuilder, IPrintBuilder<T>
    {


        public new T Model
        {
            get
            {
                if (base.Model is T) return (T)base.Model;
                return default(T);
            }
            set
            {
                if (Equals(value, base.Model)) return;
                base.Model = value;
            }
        }
    }
}