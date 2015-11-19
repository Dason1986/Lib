using System.Windows.Forms;

namespace Library.Win.MVP
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class Bootstrapper<T> where T : Form, new()
    {
        /// <summary>
        /// 
        /// </summary>
        public  ApplicationContext Context { get; protected set; }
        /// <summary>
        /// 
        /// </summary>
        public virtual void Run()
        {
            Context = new ApplicationContext {MainForm = new T()};



            Application.Run(Context);
        }

        /// <summary>
        /// 
        /// </summary>
        protected abstract void OnConfig();
    }
}