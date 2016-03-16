using System;
using System.Windows.Forms;

namespace Library.Win.MVP
{
    /// <summary>
    ///
    /// </summary>
    public abstract class Bootstrapper<T> where T : IPresenter, new()
    {
       
        /// <summary>
        ///
        /// </summary>
        public ApplicationContext Context { get; protected set; }

        /// <summary>
        ///
        /// </summary>
        public ApplicationFacade Facade { get; protected set; }
        /// <summary>
        /// 
        /// </summary>
        public DateTime StartedTime { get; protected set; }
        /// <summary>
        /// 已經運行時間
        /// </summary>
        public TimeSpan RunTime { get { return DateTime.Now - StartedTime; } }
        /// <summary>
        ///
        /// </summary>
        public void Run()
        {
            OnInitiation();
            OnConfig();
            IPresenter pre = new T();
            Context = new ApplicationContext { MainForm = pre.GetView() as Form };
            Application.Run(Context);



        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void OnInitiation()
        {
            Facade = ApplicationFacade.Instance;
            System.Diagnostics.Process current = System.Diagnostics.Process.GetCurrentProcess();
            StartedTime = current.StartTime;
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void OnConfig()
        {
        }
    }
}