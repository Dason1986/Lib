using System.Drawing;
using System.Text;

namespace Library.Win.MVC
{
    using System;
    using System.Linq;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq.Expressions;
    using System.Windows.Forms;
    using System.Reflection;
    using System.ComponentModel;
    using System.Runtime.CompilerServices;
    using System.Dynamic;
    using System.Collections;
    using System.Security.Principal;
    using System.ComponentModel.DataAnnotations;

    #region Filter

    /// <summary>
    ///
    /// </summary>
    public static class GlobalFilters
    {
        static GlobalFilters()
        {
            Filters = new GlobalFilterCollection();
        }

        /// <summary>
        ///
        /// </summary>
        public static GlobalFilterCollection Filters { get; private set; }
    }

    /// <summary>
    ///
    /// </summary>
    public interface IActionFilter
    {
        /// <summary>
        /// 在执行操作方法后由 ASP.NET MVC 框架调用。
        /// </summary>
        /// <param name="filterContext"> 筛选器上下文。</param>
        void OnActionExecuted(ActionExecutedContext filterContext);

        /// <summary>
        /// 在执行操作方法之前由 ASP.NET MVC 框架调用。
        /// </summary>
        /// <param name="filterContext">筛选器上下文。</param>
        void OnActionExecuting(ActionExecutingContext filterContext);
    }

    /// <summary>
    ///
    /// </summary>
    public class ActionExecutedContext : ControllerContext
    {
        private ControllerContext controllerContext;

        /// <summary>
        ///
        /// </summary>
        /// <param name="controllerContext"></param>
        public ActionExecutedContext(ControllerContext controllerContext)
        {
            this.controllerContext = controllerContext;
        }

        /// <summary>
        /// 获取或设置操作描述符。
        /// </summary>
        public virtual ActionDescriptor ActionDescriptor { get; set; }

        /// <summary>
        /// 获取或设置一个值，该值指示此 System.Web.Mvc.ActionExecutedContext 对象已被取消。
        /// </summary>
        public virtual bool Canceled { get; set; }

        /// <summary>
        ///  获取或设置在操作方法的执行过程中发生的异常（如果有）。
        /// </summary>
        public virtual Exception Exception { get; set; }

        /// <summary>
        ///  获取或设置一个值，该值指示是否处理异常。
        /// </summary>
        public bool ExceptionHandled { get; set; }

        /// <summary>
        /// 获取或设置由操作方法返回的结果。
        /// </summary>
        public ActionResult Result { get; set; }
    }

    /// <summary>
    ///
    /// </summary>
    public class ResultExecutingContext : ControllerContext
    {
        private ControllerContext controllerContext;

        /// <summary>
        ///
        /// </summary>
        /// <param name="controllerContext"></param>
        public ResultExecutingContext(ControllerContext controllerContext)
        {
            this.controllerContext = controllerContext;
        }

        /// <summary>
        /// 获取或设置一个值，该值指示此 System.Web.Mvc.ResultExecutingContext 值是否为“cancel”。
        /// </summary>
        public bool Cancel { get; set; }

        /// <summary>
        /// 获取或设置操作结果。
        /// </summary>
        public virtual ActionResult Result { get; set; }
    }

    /// <summary>
    ///
    /// </summary>
    public class ActionExecutingContext : ControllerContext
    {
        private ControllerContext controllerContext;

        /// <summary>
        ///
        /// </summary>
        /// <param name="controllerContext"></param>
        public ActionExecutingContext(ControllerContext controllerContext)
        {
            this.controllerContext = controllerContext;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="controllerContext"></param>
        /// <param name="paramter"></param>
        public ActionExecutingContext(ControllerContext controllerContext, IDictionary<string, object> paramter) : this(controllerContext)
        {
            ActionParameters = paramter;
        }

        /// <summary>
        /// 获取或设置操作描述符。
        /// </summary>
        public virtual ActionDescriptor ActionDescriptor { get; set; }

        /// <summary>
        ///  获取或设置操作-方法参数。
        /// </summary>
        public virtual IDictionary<string, object> ActionParameters { get; protected set; }

        /// <summary>
        /// 获取或设置由操作方法返回的结果。
        /// </summary>
        public ActionResult Result { get; set; }
    }

    /// <summary>
    ///
    /// </summary>
    public class ResultExecutedContext : ControllerContext
    {
        private ControllerContext controllerContext;

        /// <summary>
        ///
        /// </summary>
        /// <param name="controllerContext"></param>
        public ResultExecutedContext(ControllerContext controllerContext)
        {
            this.controllerContext = controllerContext;
        }

        /// <summary>
        /// 获取或设置一个值，该值指示是否取消此 System.Web.Mvc.ResultExecutedContext 实例。
        /// </summary>
        public virtual bool Canceled { get; set; }

        /// <summary>
        ///  获取或设置异常对象。
        /// </summary>
        public virtual Exception Exception { get; set; }

        /// <summary>
        /// 获取或设置一个值，该值指示是否已处理异常。
        /// </summary>
        public bool ExceptionHandled { get; set; }

        /// <summary>
        /// 获取或设置操作结果。
        /// </summary>
        public virtual ActionResult Result { get; set; }
    }

    /// <summary>
    ///
    /// </summary>
    public interface IResultFilter
    {
        /// <summary>
        /// 在执行操作结果后由 ASP.NET MVC 框架调用。
        /// </summary>
        /// <param name="filterContext"></param>
        void OnResultExecuted(ResultExecutedContext filterContext);

        /// <summary>
        ///  在执行操作结果之前由 ASP.NET MVC 框架调用。
        /// </summary>
        /// <param name="filterContext"></param>
        void OnResultExecuting(ResultExecutingContext filterContext);
    }

    /// <summary>
    ///
    /// </summary>
    public abstract class Filter
    {
        /// <summary>
        ///  获取此类的实例。
        /// </summary>
        public object Instance { get; protected set; }

        /// <summary>
        ///  获取筛选器的应用顺序。
        /// </summary>
        public int Order { get; protected set; }
    }

    /// <summary>
    ///
    /// </summary>
    public sealed class GlobalFilterCollection : IEnumerable<Filter>
    {
        /// <summary>
        ///
        /// </summary>
        public GlobalFilterCollection()
        {
            filterlist = new List<Filter>();
        }

        private readonly List<Filter> filterlist;

        /// <summary>
        ///
        /// </summary>
        public int Count { get { return filterlist.Count; } }

        /// <summary>
        ///
        /// </summary>
        /// <param name="filter"></param>
        public void Add(Filter filter)
        {
            filterlist.Add(filter);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="order"></param>
        public void Add(Filter filter, int order)
        {
            filterlist.Insert(order, filter);
        }

        /// <summary>
        ///
        /// </summary>
        public void Clear() { filterlist.Clear(); }

        /// <summary>
        ///
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        public bool Contains(Filter filter)
        {
            return filterlist.Contains(filter);
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public IEnumerator<Filter> GetEnumerator()
        {
            return this.filterlist.GetEnumerator();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="filter"></param>
        public void Remove(Filter filter)
        {
            filterlist.Remove(filter);
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.filterlist.GetEnumerator();
        }
    }

    #endregion Filter

    #region Bootstrapper

    /// <summary>
    ///
    /// </summary>
    public abstract class Bootstrapper
    {
        /// <summary>
        ///
        /// </summary>
        protected Bootstrapper()
        {
            if (Currnet != null)
            {
            }
            Currnet = this;
            OnInitiation();
            OnConfig();
        }

        /// <summary>
        ///
        /// </summary>
        public static Bootstrapper Currnet { get; set; }

        /// <summary>
        ///
        /// </summary>
        public ApplicationContext Context { get; protected set; }

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
        protected void OnInitiation()
        {
            System.Diagnostics.Process current = System.Diagnostics.Process.GetCurrentProcess();
            StartedTime = current.StartTime;
            Application.ThreadException += OnThreadException;
            AppDomain.CurrentDomain.UnhandledException += OnUnhandledException;
            AppDomain.CurrentDomain.FirstChanceException += OnFirstChanceException;
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void OnConfig()
        {
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected virtual void OnThreadException(object sender, System.Threading.ThreadExceptionEventArgs e)
        {
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected virtual void OnFirstChanceException(object sender, System.Runtime.ExceptionServices.FirstChanceExceptionEventArgs e)
        {
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected virtual void OnUnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="viewResult"></param>
        protected virtual void OnRun(ViewResult viewResult)
        {
            IView view = viewResult.View;
            var form = view as Form;
            form.StartPosition = FormStartPosition.CenterScreen;
            Context = new ApplicationContext { MainForm = form };
            Application.Run(Context);
        }
    }

    /// <summary>
    ///
    /// </summary>
    /// <typeparam name="TController"></typeparam>
    public abstract class Bootstrapper<TController> : Bootstrapper where TController : Controller, new()
    {
        /// <summary>
        ///
        /// </summary>
        /// <typeparam name="TView"></typeparam>
        /// <param name="view"></param>
        public void Run<TView>(Expression<Func<TController, TView>> view) where TView : ActionResult
        {
            var result = MVCUtility<TController>.FindViewResult(view);

            OnRun(result);
        }

        /// <summary>
        ///
        /// </summary>
        public void Run(string viewName)
        {
            var result = MVCUtility<TController>.FindViewResult(viewName);
            OnRun(result);
        }
    }

    #endregion Bootstrapper

    #region ActionResult

    /// <summary>
    ///
    /// </summary>
    public class ActionDescriptor
    {
    }

    /// <summary>
    ///
    /// </summary>
    public abstract class ActionResult
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="context"></param>
        public abstract void ExecuteResult(ControllerContext context);
    }

    /// <summary>
    ///
    /// </summary>
    public class EmptyResult : ActionResult
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="context"></param>
        public override void ExecuteResult(ControllerContext context)
        {
        }
    }

    /// <summary>
    ///
    /// </summary>
    public class ViewResult : ActionResult
    {
        //  public ViewEngineCollection ViewEngineCollection { get; set; }
        /// <summary>
        ///
        /// </summary>
        public object Model { get; set; }

        /// <summary>
        ///
        /// </summary>
        public IView View { get; set; }

        /// <summary>
        ///
        /// </summary>
        public string ViewName { get; set; }

        /// <summary>
        ///
        /// </summary>
        public dynamic ViewBag { get; private set; }

        /// <summary>
        ///
        /// </summary>
        public string Title { get; set; }

        // internal IBehavior[] Behaviors;

        /// <summary>
        ///
        /// </summary>
        public ViewResult()
        {
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="context"></param>
        public override void ExecuteResult(ControllerContext context)
        {
            ViewBag = context.Controller.ViewBag;
            if (View == null)
            {
                var rest = FindViewEngine(context);
                View = rest.View;
            }
            // if(ViewEngineCollection==null) ViewEngineCollection=
            //  ViewEngine = FindViewEngine(context);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>

        protected internal virtual ViewEngineResult FindViewEngine(ControllerContext context)
        {
            var type = context.Controller.GetType();
            var currnetnamespace = type.Assembly.GetName().Name;
            var persname = type.Name.Replace("Controller", "");
            var viewname = ViewName ?? context.ViewName;
            var classnames = ViewEngineResult.Default.SearchedLocations.Select(n => string.Format("{0}.{1},{0}", currnetnamespace, n.Replace("{controller}", persname).Replace("{viewname}", viewname)));
            var classtype = classnames.Select(n => Type.GetType(n)).FirstOrDefault(n => n != null);
            if (classtype != null)
                View = Activator.CreateInstance(classtype) as IView;

            return new ViewEngineResult(View, null);
        }
    }

    /// <summary>
    ///
    /// </summary>
    public class ViewEngineResult
    {
        static ViewEngineResult()
        {
            Default = new ViewEngineResult(new[] { "Views.{controller}.{viewname}View", "Views.Shared.{viewname}View" });
        }

        /// <summary>
        ///
        /// </summary>
        public static ViewEngineResult Default { get; private set; }

        /// <summary>
        ///
        /// </summary>
        /// <param name="searchedLocations"></param>
        public ViewEngineResult(IEnumerable<string> searchedLocations)
        {
            SearchedLocations = searchedLocations;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="view"></param>
        /// <param name="viewEngine"></param>
        public ViewEngineResult(IView view, IViewEngine viewEngine)
        {
            View = view;
            ViewEngine = viewEngine;
        }

        /// <summary>
        ///
        /// </summary>
        public IEnumerable<string> SearchedLocations { get; private set; }

        /// <summary>
        ///
        /// </summary>
        public IView View { get; private set; }

        /// <summary>
        ///
        /// </summary>
        public IViewEngine ViewEngine { get; private set; }
    }

    /// <summary>
    ///
    /// </summary>
    public class ContentResult : ActionResult
    {
        /// <summary>
        ///
        /// </summary>
        public object Content { get; set; }

        /// <summary>
        ///
        /// </summary>
        /// <param name="context"></param>
        public override void ExecuteResult(ControllerContext context)
        {
        }
    }

    /// <summary>
    ///
    /// </summary>
    public class AlertResult : ActionResult
    {
        /// <summary>
        ///
        /// </summary>
        public string Message { get; protected set; }

        /// <summary>
        ///
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        ///
        /// </summary>
        /// <param name="message"></param>
        public AlertResult(string message)
        {
            Message = message;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="context"></param>
        public override void ExecuteResult(ControllerContext context)
        {
            if (string.IsNullOrEmpty(Title))
            {
                MessageBox.Show(Message);
            }
            else
            {
                MessageBox.Show(Message, Title);
            }
        }
    }

    #endregion ActionResult

    #region ViewEngine

    /// <summary>
    ///
    /// </summary>
    public class ViewEngineCollection : Collection<IViewEngine>
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="context"></param>
        /// <param name="partialViewName"></param>
        /// <returns></returns>
        public virtual ViewEngineResult FindPartialView(ControllerContext context, string partialViewName)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="context"></param>
        /// <param name="viewName"></param>
        /// <param name="masterName"></param>
        /// <returns></returns>
        public virtual ViewEngineResult FindView(ControllerContext context, string viewName, string masterName)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    ///
    /// </summary>
    public interface IViewEngine
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="controllerContext"></param>
        /// <param name="partialViewName"></param>
        /// <returns></returns>
        ViewEngineResult FindPartialView(ControllerContext controllerContext, string partialViewName);

        /// <summary>
        ///
        /// </summary>
        /// <param name="controllerContext"></param>
        /// <param name="viewName"></param>
        /// <param name="masterName"></param>
        /// <returns></returns>
        ViewEngineResult FindView(ControllerContext controllerContext, string viewName, string masterName);

        /// <summary>
        ///
        /// </summary>
        /// <param name="controllerContext"></param>
        /// <param name="view"></param>
        void ReleaseView(ControllerContext controllerContext, IView view);
    }

    /// <summary>
    ///
    /// </summary>
    public class ViewEngine : IViewEngine
    {
        private ControllerContext context;

        /// <summary>
        ///
        /// </summary>
        /// <param name="context"></param>
        public ViewEngine(ControllerContext context)
        {
            this.context = context;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="controllerContext"></param>
        /// <param name="partialViewName"></param>
        /// <returns></returns>
        public ViewEngineResult FindPartialView(ControllerContext controllerContext, string partialViewName)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="controllerContext"></param>
        /// <param name="viewName"></param>
        /// <param name="masterName"></param>
        /// <returns></returns>
        public ViewEngineResult FindView(ControllerContext controllerContext, string viewName, string masterName)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="controllerContext"></param>
        /// <param name="view"></param>
        public void ReleaseView(ControllerContext controllerContext, IView view)
        {
            throw new NotImplementedException();
        }

        private IView GetView()
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    ///
    /// </summary>
    public interface IView
    {
        /// <summary>
        ///
        /// </summary>
        dynamic ViewBag { get; }
    }

    /// <summary>
    ///
    /// </summary>
    /// <typeparam name="TModel"></typeparam>
    public class ViewBase<TModel> : ViewBase, IView<TModel>
    {
        /// <summary>
        ///
        /// </summary>
        public virtual TModel Model { get; protected internal set; }
    }

    /// <summary>
    ///
    /// </summary>
    /// <typeparam name="TModel"></typeparam>
    public interface IView<TModel> : IView
    {
        /// <summary>
        ///
        /// </summary>
        TModel Model { get; }
    }

    /// <summary>
    ///
    /// </summary>
    public class ViewBase : Form, IView
    {
        /// <summary>
        ///
        /// </summary>
        protected internal ControllerContext ControllerContext { get; internal set; }

        /// <summary>
        ///
        /// </summary>
        public dynamic ViewBag { get { return ControllerContext.Controller.ViewBag; } }
    }

    #endregion ViewEngine

    #region Behavior

    /// <summary>
    ///
    /// </summary>
    /// <typeparam name="TView"></typeparam>
    public abstract class Behavior<TView> : Behavior, IBehavior where TView : IView
    {
        IView IBehavior.View
        {
            get { return this.View; }
            set { this.View = (TView)value; }
        }

        /// <summary>
        ///
        /// </summary>
        public new TView View { get { return (TView)base.View; } set { base.View = value; } }
    }

    /// <summary>
    ///
    /// </summary>
    /// <typeparam name="TView"></typeparam>
    /// <typeparam name="TModel"></typeparam>
    public abstract class BehaviorByModel<TView, TModel> : Behavior, IBehavior where TView : IView<TModel>
    {
        /// <summary>
        ///
        /// </summary>
        public new TView View { get { return (TView)base.View; } set { base.View = value; } }

        /// <summary>
        ///
        /// </summary>
        public new TModel Model { get { return (TModel)base.Model; } set { base.Model = value; } }

        object IBehavior.Model
        {
            get { return this.Model; }
            set { this.Model = (TModel)value; }
        }

        IView IBehavior.View
        {
            get { return this.View; }
            set { this.View = (TView)value; }
        }
    }

    /// <summary>
    ///
    /// </summary>
    public abstract class Behavior : System.ComponentModel.Component, IBehavior
    {
        /// <summary>
        ///
        /// </summary>
        protected internal ControllerContext PresenterContext { get; internal set; }

        /// <summary>
        ///
        /// </summary>
        /// <param name="methodname"></param>
        /// <param name="postData"></param>
        protected virtual void ToAction(string methodname, object postData = null)
        {
            MVCUtility.ToAction(PresenterContext, methodname, postData);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="methodname"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        protected virtual object GetContent(string methodname, object model = null)
        {
            return MVCUtility.GetContent(PresenterContext, methodname, model);
        }

        /// <summary>
        ///
        /// </summary>
        public IView View { get; set; }

        /// <summary>
        ///
        /// </summary>
        [Browsable(false)]
        public dynamic ViewBag { get { return PresenterContext.Controller.ViewBag; } }

        /// <summary>
        ///
        /// </summary>
        [Browsable(false)]
        public object Model { get; set; }

        /// <summary>
        ///
        /// </summary>
        public void Attach()
        {
            OnAttach();
        }

        /// <summary>
        ///
        /// </summary>
        protected abstract void OnAttach();

        /// <summary>
        ///
        /// </summary>
        public void Detach()
        {
            OnDetaching();
        }

        /// <summary>
        ///
        /// </summary>
        protected abstract void OnDetaching();
    }

    /// <summary>
    ///
    /// </summary>
    public interface IBehavior
    {
        /// <summary>
        ///
        /// </summary>
        IView View { get; set; }

        /// <summary>
        ///
        /// </summary>
        object Model { get; set; }

        /// <summary>
        ///
        /// </summary>
        void Attach();

        /// <summary>
        ///
        /// </summary>
        void Detach();
    }

    #endregion Behavior

    #region Controller

    /// <summary>
    ///
    /// </summary>
    public class ControllerDescriptor
    {
    }

    /// <summary>
    ///
    /// </summary>
    public class ControllerBuilder
    {
        /// <summary>
        ///
        /// </summary>
        public ControllerBuilder()
        {
            Controllers = new ControllerCollection();
        }

        /// <summary>
        ///
        /// </summary>
        public ControllerCollection Controllers { get; private set; }

        /// <summary>
        ///
        /// </summary>
        /// <typeparam name="TController"></typeparam>
        /// <returns></returns>
        public TController Get<TController>() where TController : Controller, new()
        {
            return new TController();
        }
    }

    /// <summary>
    ///
    /// </summary>
    public class ControllerCollection : Collection<ControllerDescriptor>
    {
    }

    /// <summary>
    ///
    /// </summary>
    public class ControllerContext
    {
        /// <summary>
        ///
        /// </summary>
        public ControllerContext()
        {
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="controller"></param>
        /// <param name="viewName"></param>
        public ControllerContext(ControllerBase controller, string viewName) : base()
        {
            this.Controller = controller;
            this.ViewName = viewName;
        }

        /// <summary>
        ///
        /// </summary>
        public IPrincipal User { get { return MVCUtility.User; } }

        /// <summary>
        ///
        /// </summary>
        public string ViewName { get; protected internal set; }

        /// <summary>
        ///
        /// </summary>
        public virtual ControllerBase Controller { get; protected set; }

        /// <summary>
        ///
        /// </summary>
        public object PostContent { get; internal set; }

        //public IDictionary<string, object> Paramter { get; protected set; }
    }

    /// <summary>
    ///
    /// </summary>
    public interface IController
    {
    }

    /// <summary>
    ///
    /// </summary>
    public class MVCDynamic : DynamicObject
    {
        private Dictionary<string, object> _properties = new Dictionary<string, object>();

        /// <summary>
        ///
        /// </summary>
        /// <param name="binder"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            return _properties.TryGetValue(binder.Name, out result);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="binder"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public override bool TrySetMember(SetMemberBinder binder, object value)
        {
            _properties[binder.Name] = value;
            return true;
        }
    }

    /// <summary>
    ///
    /// </summary>
    public class ControllerBase : IController
    {
        /// <summary>
        ///
        /// </summary>
        public ControllerBase()
        {
            ViewBag = new MVCDynamic();
        }

        /// <summary>
        ///
        /// </summary>
        protected internal ControllerContext ControllerContext { get; internal set; }

        /// <summary>
        ///
        /// </summary>
        public dynamic ViewBag { get; internal set; }
    }

    /// <summary>
    ///
    /// </summary>
    public class Controller : ControllerBase
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        protected ContentResult Content(object obj)
        {
            return new ContentResult() { Content = obj };
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        protected AlertResult Alert(string message)
        {
            return new AlertResult(message);
        }

        /// <summary>
        ///
        /// </summary>
        public IPrincipal User { get { return MVCUtility.User; } }

        /// <summary>
        ///
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        protected ViewResult View(object model = null)
        {
            return new ViewResult() { Model = model };
        }

        /// <summary>
        ///
        /// </summary>
        /// <typeparam name="TView"></typeparam>
        /// <param name="model"></param>
        /// <returns></returns>
        protected ViewResult View<TView>(object model = null) where TView : IView, new()
        {
            return new ViewResult() { Model = model, View = new TView(), ViewName = typeof(TView).Name.Replace("View", string.Empty) };
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="view"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        protected ViewResult View(IView view, object model = null)
        {
            return new ViewResult() { Model = model, View = view };
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="viewName"></param>
        /// <returns></returns>
        protected ViewResult View(string viewName)
        {
            return new ViewResult() { ViewName = viewName };
        }
    }

    #endregion Controller

    #region Utility

    /// <summary>
    ///
    /// </summary>
    public interface IShowViewResult
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="result"></param>
        void Show(ViewResult result);
    }

    /// <summary>
    ///
    /// </summary>
    public class ShowViewResult : IShowViewResult
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="result"></param>
        public void Show(ViewResult result)
        {
            Form form = null;
            if (result != null && result.View != null)
            {
                if (result.View is Form)
                {
                    form = result.View as Form;
                }
                else if (result.View is UserControl)
                {
                    form = new Form();
                    form.Controls.Add(result.View as UserControl);
                }
            }
            else
            {
                form = new Form();
                Label notfindView = new Label() { Text = "not find View" };
                form.Controls.Add(notfindView);
            }
            form.StartPosition = FormStartPosition.CenterParent;
            if (!string.IsNullOrEmpty(result.Title)) form.Text = result.Title;
            form.ShowDialog();
        }
    }

    /// <summary>
    ///
    /// </summary>
    public class MVCUtility
    {
        /// <summary>
        ///
        /// </summary>
        public static IPrincipal User { get; set; }

        //   public static PresenterCollection Presenters { get; internal set; }
        /// <summary>
        ///
        /// </summary>
        /// <param name="controller"></param>
        /// <param name="viewMethod"></param>
        /// <param name="postData"></param>
        public static void ToAction(string controller, string viewMethod, object postData = null)
        {
            var controllerType = Type.GetType(controller);
            if (controllerType == null) throw new Exception();
            if (controllerType.IsAbstract || controllerType.IsInterface) throw new Exception();
            var method = controllerType.GetMethod(viewMethod);
            if (method == null) throw new Exception();
            ControllerContext presenterContext = new ControllerContext(Activator.CreateInstance(controllerType) as ControllerBase, viewMethod);
            var result = OnRevolve(presenterContext, method, postData);
            OnExecuteResult(presenterContext, result);
        }

        internal static ActionResult OnRevolve(ControllerContext controllerContext, MethodInfo method, object postdata = null)
        {
            controllerContext.PostContent = postdata;

            controllerContext.Controller.ControllerContext = controllerContext;
            var actionfilter = GlobalFilters.Filters.OfType<IActionFilter>().ToArray();

            var Paramter = GetActionParamter(postdata, method.GetParameters());

            if (actionfilter != null && actionfilter.Length > 0)
            {
                var actionExecutingContext = new ActionExecutingContext(controllerContext, Paramter);

                foreach (var item in actionfilter)
                {
                    //Filters Action Before
                    item.OnActionExecuting(actionExecutingContext);
                }
            }

            var result = method.Invoke(controllerContext.Controller, Paramter.Values.ToArray()) as ActionResult;
            if (result == null)
            {
                result = new EmptyResult();
            }
            if (actionfilter != null && actionfilter.Length > 0)
            {
                var actionExecutedContext = new ActionExecutedContext(controllerContext);
                foreach (var item in actionfilter)
                {
                    item.OnActionExecuted(actionExecutedContext);
                    //Filters Action After
                }
            }

            var resultfilter = GlobalFilters.Filters.OfType<IResultFilter>().ToArray();
            if (resultfilter != null && resultfilter.Length > 0)
            {
                var resultExecutingContext = new ResultExecutingContext(controllerContext);
                foreach (var item in resultfilter)
                {
                    item.OnResultExecuting(resultExecutingContext);
                    //Filters Result Before
                }
            }
            var viewResult = result as ViewResult;

            if (viewResult != null)
            {
                if (viewResult.ViewName != null) controllerContext.ViewName = viewResult.ViewName;
                if (viewResult.View == null) viewResult.FindViewEngine(controllerContext);

                var behaviors = GetBehaviors(controllerContext);
                var view = viewResult.View as ViewBase;
                if (view != null)
                {
                    view.ControllerContext = controllerContext;
                    if (behaviors != null && behaviors.Length > 0)
                    {
                        EventHandler Load = null;
                        EventHandler Closed = null;
                        view.Load += Load = (x, y) =>
                        {
                            foreach (var item in behaviors)
                            {
                                item.PresenterContext = controllerContext;
                                item.View = view;
                                item.Model = postdata;
                                item.Attach();
                            }
                        };
                        view.Closed += Closed = (x, y) =>
                        {
                            view.Load -= Load;
                            view.Closed -= Closed;
                            foreach (var item in behaviors)
                            {
                                item.Detach();
                            }
                        };
                    }
                }
            }
            if (resultfilter != null && resultfilter.Length > 0)
            {
                var resultExecutedContext = new ResultExecutedContext(controllerContext);
                foreach (var item in resultfilter)
                {
                    item.OnResultExecuted(resultExecutedContext);
                    //Filters Result After
                }
            }
            return result;
        }

        private static Behavior[] GetBehaviors(ControllerContext controllerContext)
        {
            var presenterType = controllerContext.Controller.GetType();

            var currnetnamespace = presenterType.Assembly.GetName().Name;
            var persname = presenterType.Name.Replace("Controller", "");
            var viewname = controllerContext.ViewName;
            var path = new[] { "Behaviors.{controller}.{viewname}Behavior" };
            var classnames = path.Select(n => string.Format("{0}.{1},{0}", currnetnamespace, n.Replace("{controller}", persname).Replace("{viewname}", viewname)));
            var behaviors = classnames.Select(n =>
            {
                var typeBehavior = Type.GetType(n);
                if (typeBehavior != null)
                {
                    return Activator.CreateInstance(typeBehavior) as Behavior;
                }
                return null;
            }).Where(n => n != null).ToArray();

            return behaviors;
        }

        private static IDictionary<string, object> GetActionParamter(object postdata, ParameterInfo[] parameter)
        {
            var dictionary = new Dictionary<string, object>();
            if (parameter == null || parameter.Length == 0) return dictionary;
            if (postdata == null)
            {
                foreach (var item in parameter)
                {
                    object nullobj = null;
                    if (item.ParameterType.IsValueType) nullobj = Activator.CreateInstance(item.ParameterType);
                    dictionary.Add(item.Name, nullobj);
                }
            }
            if (CheckIfAnonymousType(postdata.GetType()))
            {
                foreach (var item in parameter)
                {
                    foreach (PropertyDescriptor propertyDescriptor in TypeDescriptor.GetProperties(postdata))
                    {
                        var name = propertyDescriptor.Name.Replace('_', '-');
                        if (string.Equals(item.Name, name, StringComparison.InvariantCultureIgnoreCase))
                        {
                            dictionary.Add(item.Name, propertyDescriptor.GetValue(postdata));
                            break;
                        }
                    }
                    if (!dictionary.ContainsKey(item.Name))
                    {
                        object nullobj = null;
                        if (item.ParameterType.IsValueType) nullobj = Activator.CreateInstance(item.ParameterType);
                        dictionary.Add(item.Name, nullobj);
                    }
                }
            }
            else
            {
                foreach (var item in parameter)
                {
                    if (item.ParameterType.IsInstanceOfType(postdata))
                    {
                        dictionary.Add(item.Name, postdata);
                    }
                    else
                    {
                        object nullobj = null;
                        if (item.ParameterType.IsValueType) nullobj = Activator.CreateInstance(item.ParameterType);
                        dictionary.Add(item.Name, nullobj);
                    }
                }
            }

            return dictionary;
        }

        private static bool CheckIfAnonymousType(Type type)
        {
            if (type == null)
                throw new ArgumentNullException("type");

            // HACK: The only way to detect anonymous types right now.
            return Attribute.IsDefined(type, typeof(CompilerGeneratedAttribute), false)
                && type.IsGenericType && type.Name.Contains("AnonymousType")
                && (type.Name.StartsWith("<>") || type.Name.StartsWith("VB$"))
                && (type.Attributes & TypeAttributes.NotPublic) == TypeAttributes.NotPublic;
        }

        internal static void ToAction(ControllerContext context, string viewMethod, object postdata = null)
        {
            var method = context.Controller.GetType().GetMethod(viewMethod);
            if (method == null) throw new Exception();

            var result = OnRevolve(context, method, postdata);
            OnExecuteResult(context, result);
        }

        private static IShowViewResult _ShowViewResult = new ShowViewResult();

        /// <summary>
        ///
        /// </summary>
        public static IShowViewResult ShowViewResult
        {
            get { return _ShowViewResult; }
            set
            {
                if (value == null) return;
                _ShowViewResult = value;
            }
        }

        internal static void OnExecuteResult(ControllerContext context, ActionResult result)
        {
            if (result is ViewResult)
                ShowViewResult.Show(result as ViewResult);
            if (result is AlertResult)
            {
                result.ExecuteResult(context);
                return;
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="context"></param>
        /// <param name="methodname"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public static object GetContent(ControllerContext context, string methodname, object model)
        {
            var method = context.Controller.GetType().GetMethod(methodname);
            if (method == null) throw new Exception();

            var result = OnRevolve(context, method, model) as ContentResult;

            if (result != null)
            {
                return result.Content;
            }
            return null;
        }
    }

    /// <summary>
    ///
    /// </summary>
    /// <typeparam name="TController"></typeparam>
    public class MVCUtility<TController> : MVCUtility where TController : Controller, new()
    {
        private static MethodInfo GetViewMethod<TMethod>(Expression<Func<TController, TMethod>> view)
        {
            if (view is LambdaExpression == false) return null;
            var lambda = (LambdaExpression)view;

            MethodCallExpression memberExpression;
            if (lambda.Body is MethodCallExpression)
            {
                memberExpression = (MethodCallExpression)lambda.Body;
            }
            else
            {
                throw new Exception();
            }
            return memberExpression.Method;
        }

        /// <summary>
        ///
        /// </summary>
        /// <typeparam name="TMethod"></typeparam>
        /// <param name="view"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public static ViewResult FindViewResult<TMethod>(Expression<Func<TController, TMethod>> view, object model = null)
        {
            var methodinfo = GetViewMethod(view);

            var presentercontext = BuildContext(methodinfo.Name);

            var res = OnRevolve(presentercontext, methodinfo, model);

            return res as ViewResult;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="view"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public static ViewResult FindViewResult(string view, object model = null)
        {
            var method = typeof(TController).GetMethod(view);
            if (method == null) throw new Exception();

            var presentercontext = BuildContext(method.Name);

            var res = OnRevolve(presentercontext, method, model);

            return res as ViewResult;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="viewMethod"></param>
        /// <returns></returns>
        public static ControllerContext BuildContext(string viewMethod)
        {
            return new ControllerContext(new TController(), viewMethod);
        }

        private static void OnRevolve(MethodInfo method, object model = null)
        {
            var presentercontext = BuildContext(method.Name);

            var res = OnRevolve(presentercontext, method, model);
            OnExecuteResult(presentercontext, res);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="viewMethod"></param>
        /// <param name="postData"></param>
        public static void ToAction(string viewMethod, object postData = null)
        {
            var method = typeof(TController).GetMethod(viewMethod);
            if (method == null) throw new Exception();
            OnRevolve(method, postData);
        }

        /// <summary>
        ///
        /// </summary>
        /// <typeparam name="TMethod"></typeparam>
        /// <param name="view"></param>
        /// <param name="postData"></param>
        public static void ToAction<TMethod>(Expression<Func<TController, TMethod>> view, object postData = null)
        {
            var methodinfo = GetViewMethod(view);
            OnRevolve(methodinfo, postData);
        }
    }

    #endregion Utility

    #region ICommand

    public interface IViewElement
    {
        /// <summary>
        /// Gets the Name of the element in the progressBar
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Gets or sets the Enabled status of the element
        /// </summary>
        bool Enabled { get; set; }

        /// <summary>
        /// Gets or sets the Visible status of the element
        /// </summary>
        bool Visible { get; set; }

        /// <summary>
        /// Gets or sets the Text of an element
        /// </summary>
        string Text { get; set; }

        /// <summary>
        /// Invoke a delegate if necessary, otherwise just call it
        /// </summary>
        void InvokeIfRequired(MethodInvoker _delegate);
    }

    /// <summary>
    /// CommandHandler is used to request an action
    /// </summary>
    public delegate void CommandHandler();

    /// <summary>
    /// CommandHandler<typeparamref name="T"/> is used to request an action
    /// taking a single argument/>
    /// </summary>
    public delegate void CommandHandler<T>(T arg);

    /// <summary>
    /// The ICommand interface represents a menu toolStripItem,
    /// which executes a command.
    /// </summary>
    public interface ICommand : IViewElement
    {
        /// <summary>
        /// Execute event is raised to signal the presenter
        /// to execute the command with which this menu
        /// toolStripItem is associated.
        /// </summary>
        event CommandHandler Execute;

        void CallClick();

        //  string ToolTipText { get; set; }
    }

    public interface ITextBoxElement : IViewElement
    {
        void SetAutoComplete(string[] source);
    }

    /// <summary>
    ///
    /// </summary>
    public interface ILinkLabelElement : IViewElement, ICommand
    {
    }

    public class TextBoxElement : ControlElement<TextBox>, ITextBoxElement
    {
        public TextBoxElement(TextBox control) : base(control)
        {
            textBox = control;
        }

        private TextBox textBox;

        public void SetAutoComplete(string[] source)
        {
            AutoCompleteStringCollection collection = new AutoCompleteStringCollection();
            collection.AddRange(source);
            textBox.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            textBox.AutoCompleteSource = AutoCompleteSource.CustomSource;
            textBox.AutoCompleteCustomSource = collection;
        }
    }

    /// <summary>
    /// IControlElement is implemented by elements that wrap controls.
    /// </summary>
    public interface IControlElement : IViewElement
    {
        Point Location { get; set; }
        Size Size { get; set; }
        Size ClientSize { get; set; }

        [Browsable(false)]
        Control Control { get; }

        bool Validation();

        void SetValidation(Func<bool> validFun, string validationMessage);
    }

    public interface IDateTimePickerElement : IControlElement<DateTimePicker>
    {
        DateTime? Value { get; set; }
    }

    public class DateTimePickerElement : ControlElement<DateTimePicker>, IDateTimePickerElement
    {
        public DateTimePickerElement(DateTimePicker control)
            : base(control)
        {
            //  System.Windows.Forms.Control.CheckForIllegalCrossThreadCalls = false;
        }

        public DateTime? Value
        {
            get
            {
                return InvokeIfRequired<DateTime?>(() =>
                {
                    if (Control.Checked) return Control.Value;
                    return null;
                });
            }
            set
            {
                InvokeIfRequired(() =>
                {
                    if (value == null)
                    {
                        Control.Checked = false;
                    }
                    else
                    {
                        Control.Checked = true;
                        Control.Value = value.Value;
                    }
                });
            }
        }
    }

    public interface INumericUpDownElement : IControlElement<NumericUpDown>
    {
        decimal Value { get; set; }
    }

    public class NumericUpDownElement : ControlElement<NumericUpDown>, INumericUpDownElement
    {
        public NumericUpDownElement(NumericUpDown control) : base(control)
        {
        }

        public decimal Value
        {
            get
            {
                return InvokeIfRequired<decimal>(() => Control.Value);
            }
            set
            {
                InvokeIfRequired(() =>
                {
                    Control.Value = value;
                });
            }
        }
    }

    public interface IControlElement<T> : IControlElement where T : Control
    {
        [Browsable(false)]
        new T Control { get; }
    }

    public class DataGridViewColumnElement : IDataGridViewColumnElement
    {
        public DataGridViewColumnElement(DataGridViewColumn column)
        {
            Column = column;
        }

        public DataGridViewColumn Column { get; protected set; }

        public int Width
        {
            get { return Column.Width; }
            set { Column.Width = value; }
        }

        public bool Visible
        {
            get { return Column.Visible; }
            set { Column.Visible = value; }
        }

        public bool Frozen
        {
            get { return Column.Frozen; }
            set { Column.Frozen = value; }
        }

        public string Name
        {
            get { return Column.Name; }
        }
    }

    public interface IDataGridViewColumnElement
    {
        DataGridViewColumn Column { get; }
        int Width { get; set; }
        bool Visible { get; set; }

        bool Frozen { get; set; }

        string Name { get; }
    }

    public interface IDataGridViewElement : IControlElement<DataGridView>
    {
        object DataSource { get; set; }
        DataGridViewSelectedRowCollection SelectedRows { get; }
    }

    /*
    public interface IDataGridViewNavigatorElement : IDataGridViewElement
    {
        DataGridViewNavigator Navigator { get; }

        PagingInfo GetPagingInfo();

        string[] SotrNames { get; set; }

        void SetPages(int pageIndex, int pageSize, int total);

        void ClearSortInfo();

        void SetOrderAscending(string v);
    }

    public class DataGridViewNavigatorElement : DataGridViewElement, IDataGridViewNavigatorElement
    {
        public DataGridViewNavigatorElement(DataGridView control, DataGridViewNavigator navigator)
            : base(control)
        {
            control.AutoGenerateColumns = false;
            Navigator = navigator;
            control.ColumnHeaderMouseClick += Control_ColumnHeaderMouseClick;
        }

        private string sortName;
        private DataGridViewColumn sortCell;

        private void Control_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (SotrNames == null) return;
            sortCell = Control.Columns[e.ColumnIndex];
            sortName = sortCell.DataPropertyName;
            if (SotrNames.Contains(sortName))
            {
                Navigator.PageIndex = 0;
                strSortOrder = getSortOrder();
                sortName = sortCell.DataPropertyName;
                Navigator.RefreshItem.CallClick();

                //   sortCell.HeaderCell.SortGlyphDirection = strSortOrder;
            }
            else
            {
                sortCell.HeaderCell.SortGlyphDirection = SortOrder.None;
            }
        }

        private SortOrder getSortOrder()
        {
            var cell = sortCell.HeaderCell;
            if (cell.SortGlyphDirection == SortOrder.None ||
                cell.SortGlyphDirection == SortOrder.Descending)
            {
                cell.SortGlyphDirection = SortOrder.Ascending;
                return SortOrder.Ascending;
            }
            else
            {
                cell.SortGlyphDirection = SortOrder.Descending;
                return SortOrder.Descending;
            }
        }

        private SortOrder strSortOrder;

        public DataGridViewNavigator Navigator { get; protected set; }
        public string[] SotrNames { get; set; }

        public override object DataSource
        {
            get
            {
                return this.Control.DataSource;
            }
            set
            {
                InvokeIfRequired(() =>
                {
                    Navigator.BindingSource = new BindingSource() { DataSource = value };
                    Control.DataSource = value;
                });
            }
        }

        public void SetPages(int pageIndex, int pageSize, int total)
        {
            var totalPage = (int)Math.Ceiling(total / (double)pageSize);
            InvokeIfRequired(() =>
            {
                //Navigator.PageIndex = pageIndex;
                //Navigator.PageSize = pageSize;
                //Navigator.TotalPage = totalPage;
                Navigator.SetPages(pageIndex, pageSize, totalPage, total);
                if (!string.IsNullOrEmpty(sortName) && sortCell != null)
                {
                    foreach (DataGridViewColumn item in Control.Columns)
                    {
                        item.HeaderCell.SortGlyphDirection = SortOrder.None;
                    }
                    sortName = sortCell.DataPropertyName;
                    sortCell.HeaderCell.SortGlyphDirection = strSortOrder;
                }
            });
        }

        public PagingInfo GetPagingInfo()
        {
            var tmpNo = Navigator.PageIndex;
            if (tmpNo != 0 && ((Navigator.TotalPage - 1) < tmpNo || Control.Rows.Count == 0))
            {
                tmpNo = 0;
            }
            return new PagingInfo() { Size = Navigator.PageSize, Index = tmpNo, SortName = sortName, IsDesc = strSortOrder == SortOrder.Descending };
        }

        public void ClearSortInfo()
        {
            sortName = null;
            if (Navigator != null)
                Navigator.PageIndex = 0;
            if (sortCell != null)
            {
                sortCell.HeaderCell.SortGlyphDirection = SortOrder.None;
                sortCell = null;
            }
        }

        public void SetOrderAscending(string columnName)
        {
            var column = Control.Columns.OfType<DataGridViewColumn>().FirstOrDefault(n => n.DataPropertyName == columnName);

            if (column == null) return;
            strSortOrder = SortOrder.Ascending;
            column.HeaderCell.SortGlyphDirection = strSortOrder;
            sortCell = column;
            sortName = columnName;
        }
    }
    */

    public class DataGridViewElement : ControlElement<DataGridView>, IDataGridViewElement
    {
        public DataGridViewElement(DataGridView control)
            : base(control)
        {
            control.AutoGenerateColumns = false;
        }

        public virtual object DataSource
        {
            get
            {
                return this.Control.DataSource;
            }
            set
            {
                InvokeIfRequired(() => { Control.DataSource = value; });
            }
        }

        public DataGridViewSelectedRowCollection SelectedRows
        {
            get
            {
                return this.Control.SelectedRows;
            }
        }
    }

    public interface IChecked
    {
        bool Checked { get; set; }

        event CommandHandler CheckedChanged;
    }

    public interface IToolStripElement : ICommand
    {
        string ToolTipText { get; set; }
    }

    public interface IToolStripButtonElement : IToolStripElement<ToolStripButton>
    {
    }

    public interface IToolStripElement<T> : IToolStripElement where T : ToolStripItem
    {
        [Browsable(false)]
        T ToolStripItem { get; }
    }

    public interface ISplitButtonElement : IToolStripElement<ToolStripButton>
    {
    }

    public interface IToolStripLabelElement : IToolStripElement<ToolStripLabel>
    {
    }

    public interface IContextMenuElement : IControlElement<ContextMenuStrip>
    {
        event CommandHandler Popup;

        ToolStripItemCollection Items { get; }

        void Add(MenuElement menuItem);

        void AddGroup(string groupName, MenuElement[] menus);

        void HideInvalidSeparator();
    }

    public delegate void TreeNodeActionHandler(TreeNode treeNode);

    public interface IPopup : IToolStripElement<ToolStripMenuItem>
    {
        /// <summary>
        /// Popup event is raised to signal the presenter
        /// that the dropdown associated with this menu toolStripItem
        /// is about to be displayed.
        /// </summary>
        event CommandHandler Popup;
    }

    public interface IPanelElement : IControlElement<Panel>
    {
    }

    public interface ITabControlElement : IControlElement<TabControl>, ISelection
    {
        bool Multiline { get; set; }
        IView Parent { get; }

        void SelectTab(string tabPageName);

        void SelectTab(int index);
    }

    public interface INavigation
    {
    }

    public class TabControlElement : ControlElement<TabControl>, ITabControlElement
    {
        public TabControlElement(TabControl control) : base(control)
        {
            Parent = Control.FindForm() as ViewBase;
        }

        public bool Multiline
        {
            get { return Control.Multiline; }
            set { InvokeIfRequired(() => { Control.Multiline = value; }); }
        }

        public IView Parent { get; protected set; }

        public int SelectedIndex
        {
            get { return Control.SelectedIndex; }
            set { InvokeIfRequired(() => { Control.SelectedIndex = value; }); }
        }

        public string SelectedItem
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        public event CommandHandler SelectionChanged;

        public void SelectTab(string tabPageName)
        {
            InvokeIfRequired(() =>
            {
                Control.SelectTab(tabPageName);
                OnSelectionChanged();
            });
        }

        public void SelectTab(int index)
        {
            InvokeIfRequired(() =>
            {
                Control.SelectTab(index);
                OnSelectionChanged();
            });
        }

        protected virtual void OnSelectionChanged()
        {
            if (SelectionChanged != null) SelectionChanged.Invoke();
        }
    }

    /// <summary>
    /// The ITreeViewElement interface provides additional methods
    /// used when wrapping a TreeView.
    /// </summary>
    public interface ITreeViewElement : IControlElement<TreeView>
    {
        event TreeNodeActionHandler SelectedNodeChanged;

        bool CheckBoxes { get; set; }
        int VisibleCount { get; }

        TreeNode SelectedNode { get; set; }
        IList<TreeNode> CheckedNodes { get; }

        IContextMenuElement ContextMenu { get; set; }

        void Clear();

        void ExpandAll();

        void CollapseAll();

        void Add(TreeNode treeNode);

        void Load(TreeNode treeNode);

        void SetImageIndex(TreeNode treeNode, int imageIndex);
    }

    /// <summary>
    /// The ISelection interface represents a single UI element
    /// or a group of elements that allow the user to select one
    /// of a set of items.
    /// </summary>
    public interface ISelection : IViewElement
    {
        /// <summary>
        /// Gets the index of the currently selected item
        /// </summary>
        int SelectedIndex { get; }

        /// <summary>
        /// Gets the string value of the currently selected item
        /// </summary>
        string SelectedItem { get; set; }

        /// <summary>
        /// Event raised when the selection is changed by the user
        /// </summary>
        event CommandHandler SelectionChanged;
    }

    public interface IElementCommandHandler
    {
        void BindHander<T>(T control) where T : Control;
    }

    public class DataGridViewSelectChangedRowCommandHandler : IElementCommandHandler
    {
        public CommandHandler<DataGridViewSelectedRowCollection> Execute;

        public void BindHander(DataGridView control)
        {
            if (Execute == null) return;
            control.CellClick += (xx, yy) =>
            {
                Execute(control.SelectedRows);
            };
        }

        void IElementCommandHandler.BindHander<T>(T control)
        {
            BindHander(control as DataGridView);
        }
    }

    public interface IListViewElement : IControlElement<ListView>
    {
        event EventHandler SelectedIndexChanged;
    }

    public class ListViewElement : ControlElement<ListView>, IListViewElement
    {
        public ListViewElement(ListView control)
            : base(control)
        {
        }

        public event EventHandler SelectedIndexChanged
        {
            add { this.Control.ItemActivate += value; }
            remove { this.Control.ItemActivate -= value; }
        }
    }

    public class DropFileCommandHandler : IElementCommandHandler
    {
        public CommandHandler<string[]> Execute;

        public void BindHander<T>(T control) where T : Control
        {
            if (Execute == null) return;
            control.AllowDrop = true;
            control.DragDrop += Control_DragDrop;
            control.DragEnter += Control_DragEnter;
            control.Disposed += Control_Disposed;
        }

        private void Control_Disposed(object sender, EventArgs e)
        {
            var control = sender as Control;
            if (control == null) return;
            control.Disposed -= Control_Disposed;
            control.DragDrop -= Control_DragDrop;
            control.DragEnter -= Control_DragEnter;
        }

        private void Control_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop)) e.Effect = DragDropEffects.Copy;
        }

        private void Control_DragDrop(object sender, DragEventArgs e)
        {
            CommandHandler<string[]> handel = Execute;
            if (handel == null) return;
            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
            handel.Invoke(files);
        }
    }

    public static class ControlElementHelper
    {
        public static void AddCommandHandler(this IControlElement controlElement, IElementCommandHandler hander)
        {
            hander.BindHander(controlElement.Control);
        }

        public static void LoadValidationConfig(this IControlElement controlElement)
        {
            LoadValidationConfig(controlElement, controlElement.Name);
        }

        public static void LoadValidationConfig(this IControlElement controlElement, string name)
        {
            //todo::
            string validationType = "";
            string message = "";
            switch (validationType)
            {
                case "Required": Required(controlElement, message); break;
                case "Range":
                    {
                        int min = 0, max = 5;
                        Range(controlElement, min, max, message); break;
                    }
                case "MinLength":
                    {
                        int min = 0;
                        MinLength(controlElement, min, message);
                        break;
                    }
                case "MaxLength":
                    {
                        int max = 5;
                        MaxLength(controlElement, max, message); break;
                    }
                case "Email":
                    {
                        Email(controlElement, message); break;
                    }
                default:
                    break;
            }
        }

        public static void Required(this IControlElement controlElement, string message = "不能爲空")
        {
            controlElement.SetValidation(() => !string.IsNullOrEmpty(controlElement.Text), message);
        }

        public static void Binding(this IControlElement controlElement, string property, object datasource, string member)
        {
            controlElement.Control.DataBindings.Add(new Binding(property, datasource, member, true));
        }

        public static void Binding(this IControlElement controlElement, object datasource, string member)
        {
            Binding(controlElement, "Text", datasource, member);
        }

        public static void Range(this IControlElement controlElement, int minlength, int maxlength, string message)
        {
            if (minlength > maxlength) throw new ValidationException("最小值不能大於最大值");
            controlElement.SetValidation(() => controlElement.Text.Length >= minlength && controlElement.Text.Length <= maxlength, message);
        }

        public static void MinLength(this IControlElement controlElement, int minlength, string message)
        {
            controlElement.SetValidation(() => controlElement.Text.Length >= minlength, message);
        }

        public static void MaxLength(this IControlElement controlElement, int maxlength, string message)
        {
            controlElement.SetValidation(() => controlElement.Text.Length <= maxlength, message);
        }

        private static System.Text.RegularExpressions.Regex email = new System.Text.RegularExpressions.Regex(@"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$");

        public static void Email(this IControlElement controlElement, string message = "不爲電子郵箱")
        {
            controlElement.SetValidation(() => email.IsMatch(controlElement.Text), message);
        }
    }

    public class PictureButtonElement : ControlElement<PictureBox>, ICommand
    {
        public PictureButtonElement(PictureBox control)
            : base(control)
        {
            control.Cursor = Cursors.Hand;

            control.Click += (xx, yy) => { if (Execute != null) Execute(); };
        }

        public event CommandHandler Execute;

        public void CallClick()
        {
            if (Execute != null) Execute();
        }
    }

    public class LinkLabelElement : ControlElement<LinkLabel>, ILinkLabelElement
    {
        public LinkLabelElement(LinkLabel linkLabel) : base(linkLabel)
        {
            linkLabel.Click += delegate { if (Execute != null) Execute(); };
        }

        public event CommandHandler Execute;

        public void CallClick()
        {
            if (Execute != null) Execute();
        }
    }

    public class ControlElement<T> : IControlElement<T> where T : Control
    {
        public ControlElement(T control)
        {
            this.Control = control;
            _errorProvider = new ErrorProvider();
            this.Control.Validating += Control_Validating;
            this.LoadValidationConfig();
        }

        private void Control_Validating(object sender, CancelEventArgs e)
        {
            bool flag = true;
            StringBuilder builder = new StringBuilder();
            foreach (var item in _valids)
            {
                if (!item.Item1())
                {
                    flag = false;
                    builder.AppendLine(item.Item2);
                }
            }
            if (flag)
                _errorProvider.Clear();
            else
            {
                _errorProvider.SetError(this.Control, builder.ToString());
            }
        }

        public bool Validation()
        {
            if (_valids.Count > 0)
            {
                Control_Validating(this, new CancelEventArgs());
            }
            //errorProvider.UpdateBinding();
            return string.IsNullOrEmpty(_errorProvider.GetError(this.Control));
        }

        private readonly List<System.Tuple<Func<bool>, string>> _valids = new List<Tuple<Func<bool>, string>>();

        public void SetValidation(Func<bool> validFun, string validationMessage)
        {
            _valids.Add(new Tuple<Func<bool>, string>(validFun, validationMessage));
        }

        Control IControlElement.Control { get { return Control; } }
        public T Control { get; private set; }
        private readonly ErrorProvider _errorProvider;

        public Point Location
        {
            get { return Control.Location; }
            set { InvokeIfRequired(() => { Control.Location = value; }); }
        }

        public Size Size
        {
            get { return Control.Size; }
            set { InvokeIfRequired(() => { Control.Size = value; }); }
        }

        public Size ClientSize
        {
            get { return Control.ClientSize; }
            set { InvokeIfRequired(() => { Control.ClientSize = value; }); }
        }

        public string Name
        {
            get { return Control.Name; }
        }

        public bool Enabled
        {
            get { return Control.Enabled; }
            set { InvokeIfRequired(() => { Control.Enabled = value; }); }
        }

        public bool Visible
        {
            get { return Control.Visible; }
            set { InvokeIfRequired(() => { Control.Visible = value; }); }
        }

        public string Text
        {
            get { return InvokeIfRequired<string>(() => Control.Text); }
            set { InvokeIfRequired(() => { Control.Text = value; }); }
        }

        public void InvokeIfRequired(MethodInvoker del)
        {
            if (Control.InvokeRequired)
                Control.BeginInvoke(del, new object[0]);
            else
                del();
        }

        public TValue InvokeIfRequired<TValue>(MethodInvoker<TValue> del)
        {
            if (Control.InvokeRequired)
            {
                IAsyncResult aResult = Control.BeginInvoke(del, new object[0]);
                aResult.AsyncWaitHandle.WaitOne(-1);
                var resultObj = Control.EndInvoke(aResult);
                if (resultObj is TValue)
                {
                    return (TValue)resultObj;
                }

                return default(TValue);
            }
            else
                return del();
        }

        public delegate TValue MethodInvoker<out TValue>();
    }

    public class RadioButtonElement : ControlElement<RadioButton>, IChecked
    {
        public RadioButtonElement(RadioButton button)
            : base(button)
        {
            _radioButton = button;
            button.CheckedChanged += delegate { if (CheckedChanged != null) CheckedChanged(); };
        }

        private readonly RadioButton _radioButton;

        public bool Checked
        {
            get
            {
                return _radioButton.Checked;
            }
            set
            {
                this.InvokeIfRequired(() =>
                {
                    _radioButton.Checked = value;
                });
            }
        }

        public event CommandHandler CheckedChanged;
    }

    public class ButtonElement : ControlElement<ButtonBase>, ICommand
    {
        public ButtonElement(ButtonBase button)
            : base(button)
        {
            button.Click += delegate { if (Execute != null) Execute(); };
        }

        public event CommandHandler Execute;

        public void CallClick()
        {
            if (Execute != null) Execute();
        }

        public static implicit operator ButtonElement(ButtonBase d)
        {
            return new ButtonElement(d);
        }
    }

    public interface IComboBoxElement : IControlElement<ComboBox>
    {
        object DataSource { get; set; }
        object SelectedItem { get; set; }
        object SelectedValue { get; set; }

        TValue GetSelectValue<TValue>();
    }

    public class ComboBoxElement : ControlElement<ComboBox>, IComboBoxElement
    {
        public event CommandHandler Changed;

        public ComboBoxElement(ComboBox comboBox)
            : base(comboBox)
        {
            comboBox.SelectedValueChanged += delegate { if (Changed != null) Changed(); };
        }

        public TValue GetSelectValue<TValue>()
        {
            var obj = SelectedValue;
            if (obj is TValue) return (TValue)obj;
            return default(TValue);
        }

        public object SelectedValue
        {
            get
            {
                return InvokeIfRequired<object>(() =>
                {
                    if (string.IsNullOrEmpty(Control.Text)) return null;
                    return Control.SelectedValue;
                });
            }
            set
            {
                if (Control.SelectedValue != value)
                {
                    InvokeIfRequired(() =>
                    {
                        Control.SelectedValue = value;
                    });
                }
            }
        }

        public object DataSource
        {
            get { return Control.DataSource; }
            set
            {
                if (Control.DataSource != value)
                {
                    InvokeIfRequired(() =>
                    {
                        Control.DataSource = value;
                    });
                }
            }
        }

        public object SelectedItem
        {
            get { return Control.SelectedItem; }
            set
            {
                if (Control.SelectedItem != value)
                {
                    InvokeIfRequired(() =>
                    {
                        Control.SelectedItem = value;
                    });
                }
            }
        }
    }

    public class CheckBoxElement : ControlElement<CheckBox>, IChecked
    {
        public event CommandHandler CheckedChanged;

        public CheckBoxElement(CheckBox checkBox)
            : base(checkBox)
        {
            checkBox.CheckedChanged += delegate { if (CheckedChanged != null) CheckedChanged(); };
        }

        public bool Checked
        {
            get { return Control.Checked; }
            set
            {
                if (Control.Checked != value)
                {
                    InvokeIfRequired(() =>
                    {
                        Control.Checked = value;
                    });
                }
            }
        }
    }

    public class CheckedMenuGroup : ISelection
    {
        private readonly ToolStripMenuItem[] _menuItems;

        public event CommandHandler SelectionChanged;

        public CheckedMenuGroup(string name, params ToolStripMenuItem[] menuItems)
        {
            this.Name = name;
            this._menuItems = menuItems;
            this.SelectedIndex = -1;

            for (int index = 0; index < menuItems.Length; index++)
            {
                ToolStripMenuItem menuItem = menuItems[index];

                // We need to handle this ourselves
                menuItem.CheckOnClick = false;

                if (menuItem.Checked)
                    // Select first menu item checked in designer
                    // and uncheck any others.
                    if (SelectedIndex == -1)
                        SelectedIndex = index;
                    else
                        menuItem.Checked = false;

                // Handle click by user
                menuItem.Click += menuItem_Click;
            }

            // If no items were checked, select first one
            if (SelectedIndex == -1 && menuItems.Length > 0)
            {
                menuItems[0].Checked = true;
                SelectedIndex = 0;
            }
        }

        // Note that all items must be on the same toolstrip
        private ToolStrip _toolStrip;

        public ToolStrip ToolStrip
        {
            get
            {
                if (_toolStrip == null && _menuItems.Length > 0)
                    _toolStrip = _menuItems[0].GetCurrentParent();

                return _toolStrip;
            }
        }

        public string Name { get; private set; }
        public string Text { get; set; }

        public int SelectedIndex
        {
            get
            {
                for (int i = 0; i < _menuItems.Length; i++)
                    if (_menuItems[i].Checked)
                        return i;

                return -1;
            }
            set
            {
                InvokeIfRequired(() =>
                {
                    for (int i = 0; i < _menuItems.Length; i++)
                        _menuItems[i].Checked = value == i;
                });
            }
        }

        public string SelectedItem
        {
            get { return (string)_menuItems[SelectedIndex].Tag; }
            set
            {
                for (int i = 0; i < _menuItems.Length; i++)
                    if ((string)_menuItems[i].Tag == value)
                    {
                        SelectedIndex = i;
                        break;
                    }
            }
        }

        private bool _enabled;

        public bool Enabled
        {
            get { return _enabled; }
            set
            {
                _enabled = value;

                foreach (ToolStripMenuItem menuItem in _menuItems)
                    menuItem.Enabled = value;
            }
        }

        private bool _visible;

        public bool Visible
        {
            get { return _visible; }
            set
            {
                _visible = value;

                foreach (ToolStripMenuItem menuItem in _menuItems)
                    menuItem.Visible = value;
            }
        }

        private void menuItem_Click(object sender, System.EventArgs e)
        {
            ToolStripMenuItem clicked = (ToolStripMenuItem)sender;

            // If user clicks selected item, ignore it
            if (clicked.Checked) return;
            for (int index = 0; index < _menuItems.Length; index++)
            {
                ToolStripMenuItem item = _menuItems[index];

                if (item == clicked)
                {
                    item.Checked = true;
                    SelectedIndex = index;
                }
                else
                {
                    item.Checked = false;
                }
            }

            if (SelectionChanged != null)
                SelectionChanged();
        }

        public void EnableItem(string tag, bool enabled)
        {
            foreach (ToolStripMenuItem item in _menuItems)
            {
                if ((string)item.Tag == tag)
                    item.Enabled = enabled;
            }
        }

        public void InvokeIfRequired(MethodInvoker _delegate)
        {
            if (ToolStrip.InvokeRequired)
                ToolStrip.BeginInvoke(_delegate);
            else
                _delegate();
        }
    }

    public class ContextMenuElement : ControlElement<ContextMenuStrip>, IContextMenuElement
    {
        public ContextMenuElement(ContextMenuStrip contextMenu)
            : base(contextMenu)
        {
            contextMenu.Opened += (s, cea) =>
            {
                if (Popup != null)
                    Popup();
            };
        }

        public event CommandHandler Popup;

        public ToolStripItemCollection Items
        {
            get { return Control.Items; }
        }

        public void AddSeparator()
        {
            Control.Items.Add(new ToolStripSeparator());
        }

        public void Add(MenuElement menuItem)
        {
            if (menuItem == null) throw new ArgumentNullException("menuItem");
            menuItem.ContextMenu = this;
            Control.Items.Add(menuItem.ToolStripItem);
        }

        public void AddGroup(string groupName, MenuElement[] menus)
        {
            ToolStripSeparator separator = null;
            if (Control.Items.Count != 0)
            {
                separator = new ToolStripSeparator() { Name = groupName };
                Control.Items.Add(separator);
            }
            foreach (var item in menus)
            {
                item.GroupName = groupName;
                Add(item);
            }
            if (_groupDic.ContainsKey(groupName))
            {
                _groupDic[groupName].Menus.AddRange(menus);
            }
            else
            {
                _groupDic.Add(groupName, new temp { Name = groupName, Menus = new List<MenuElement>(menus), Separator = separator });
            }
        }

        public void HideInvalidSeparator()
        {
            foreach (var item in _groupDic.Values)
            {
                if (item.Separator != null)
                {
                    var flag = item.Menus.Any(n => n.Visible);
                    item.Separator.Visible = flag;
                }
            }
        }

        private readonly IDictionary<string, temp> _groupDic = new Dictionary<string, temp>();

        private class temp
        {
            public string Name { get; set; }
            public List<MenuElement> Menus { get; set; }
            public ToolStripSeparator Separator { get; set; }
        }
    }

    public class ToolStripButtonElement : ToolStripElement<ToolStripButton>, IToolStripButtonElement
    {
        public ToolStripButtonElement(ToolStripButton toolStripItem) : base(toolStripItem)
        {
        }
    }

    public class ToolStripElement<T> : IToolStripElement<T> where T : ToolStripItem
    {
        public ToolStripElement(T toolStripItem)
        {
            id = Guid.NewGuid();
            this.ToolStripItem = toolStripItem;
            this.ToolStripItem.Click += delegate { if (execute != null) execute(); };
        }

        public Guid id { get; set; }
        public ContextMenuElement ContextMenu { get; protected internal set; }

        public virtual event CommandHandler Execute
        {
            add { execute += value; }
            remove { execute -= value; }
        }

        private CommandHandler execute;

        public string GroupName { get; set; }

        public virtual void CallClick()
        {
            if (execute != null) execute();
        }

        public T ToolStripItem { get; private set; }

        public string Name
        {
            get { return ToolStripItem.Name; }
        }

        public bool Enabled
        {
            get { return ToolStripItem.Enabled; }
            set { InvokeIfRequired(() => { ToolStripItem.Enabled = value; }); }
        }

        public bool Visible
        {
            get { return ToolStripItem.Visible; }
            set
            {
                InvokeIfRequired(() =>
                {
                    ToolStripItem.Visible = value;

                    if (ContextMenu != null && !string.IsNullOrWhiteSpace(GroupName))
                    {
                    }
                });
            }
        }

        public string Text
        {
            get { return ToolStripItem.Text; }
            set { InvokeIfRequired(() => { ToolStripItem.Text = value; }); }
        }

        public string ToolTipText
        {
            get { return ToolStripItem.ToolTipText; }
            set { InvokeIfRequired(() => { ToolStripItem.ToolTipText = value; }); }
        }

        public void InvokeIfRequired(MethodInvoker del)
        {
            var toolStrip = ToolStripItem.GetCurrentParent();

            if (toolStrip != null && toolStrip.InvokeRequired)
                toolStrip.BeginInvoke(del, new object[0]);
            else
                del();
        }
    }

    public class MenuElement : ToolStripElement<ToolStripMenuItem>, ICommand, IPopup, IChecked
    {
        public event CommandHandler Popup;

        public event CommandHandler CheckedChanged;

        public MenuElement(ToolStripMenuItem menuItem)
            : base(menuItem)
        {
            menuItem.Click += delegate { if (execute != null) execute(); };
            menuItem.DropDownOpening += delegate { if (Popup != null) Popup(); };
            menuItem.CheckedChanged += delegate { if (CheckedChanged != null) CheckedChanged(); };
        }

        public override event CommandHandler Execute
        {
            add { execute += value; }
            remove { execute -= value; }
        }

        private CommandHandler execute;

        public MenuElement(string text)
            : this(new ToolStripMenuItem(text) { Visible = true })
        {
        }

        public MenuElement(string text, Image ioc)
         : this(new ToolStripMenuItem(text, ioc) { Visible = true })
        {
        }

        public MenuElement(string text, CommandHandler execute)
            : this(text)
        {
            this.execute = execute;
        }

        public override void CallClick()
        {
            if (execute != null) execute();
        }

        public bool Checked
        {
            get { return ToolStripItem.Checked; }
            set
            {
                if (ToolStripItem.Checked != value)
                {
                    InvokeIfRequired(() =>
                    {
                        ToolStripItem.Checked = value;
                    });
                }
            }
        }
    }

    public class ToolStripLabelElement : ToolStripElement<ToolStripLabel>, IToolStripLabelElement
    {
        public ToolStripLabelElement(ToolStripLabel button)
            : base(button)
        {
        }
    }

    public class SplitButtonElement : ToolStripElement<ToolStripButton>, ISplitButtonElement
    {
        public SplitButtonElement(ToolStripButton button)
            : base(button)
        {
            button.Click += delegate { if (Execute != null) Execute(); };
        }

        public override event CommandHandler Execute;
    }

    public class StripSplitButtonElement : ToolStripElement<ToolStripSplitButton>, ICommand
    {
        public StripSplitButtonElement(ToolStripSplitButton button)
            : base(button)
        {
            button.ButtonClick += delegate { if (Execute != null) Execute(); };
        }

        public override event CommandHandler Execute;
    }

    public class TreeViewElement : ControlElement<TreeView>, ITreeViewElement
    {
        public event TreeNodeActionHandler SelectedNodeChanged;

        public TreeViewElement(TreeView treeView)
            : base(treeView)
        {
            _checkBoxes = treeView.CheckBoxes;

            treeView.AfterSelect += (s, e) =>
            {
                if (SelectedNodeChanged != null)
                    SelectedNodeChanged(e.Node);
            };

            treeView.MouseUp += (s, e) =>
            {
                if (e.Button == MouseButtons.Right)
                {
                    var treeNode = treeView.GetNodeAt(e.X, e.Y);
                    if (treeNode != null)
                        treeView.SelectedNode = treeNode;
                }
            };
        }

        private IContextMenuElement contextMenu;

        public IContextMenuElement ContextMenu
        {
            get
            {
                if (contextMenu == null && Control.ContextMenuStrip != null)
                    contextMenu = new ContextMenuElement(Control.ContextMenuStrip);

                return contextMenu;
            }
            set
            {
                InvokeIfRequired(() =>
                {
                    contextMenu = value;
                    Control.ContextMenuStrip = contextMenu.Control;
                });
            }
        }

        private bool _checkBoxes;

        public bool CheckBoxes
        {
            get { return _checkBoxes; }
            set
            {
                if (_checkBoxes != value)
                {
                    var expandedNodes = new List<TreeNode>();

                    // Turning off checkboxes collapses everything, so we
                    // have to save and restore the expanded nodes.
                    if (!value)
                        foreach (TreeNode node in Control.Nodes)
                            RecordExpandedNodes(expandedNodes, node);

                    InvokeIfRequired(() => { Control.CheckBoxes = _checkBoxes = value; });

                    if (!value)
                        foreach (var node in expandedNodes)
                            node.Expand();
                }
            }
        }

        public int VisibleCount
        {
            get { return Control.VisibleCount; }
        }

        public TreeNode SelectedNode
        {
            get { return Control.SelectedNode; }
            set { Control.SelectedNode = value; }
        }

        public IList<TreeNode> CheckedNodes
        {
            get { return GetCheckedNodes(); }
        }

        public void Clear()
        {
            InvokeIfRequired(() => { Control.Nodes.Clear(); });
        }

        public void ExpandAll()
        {
            InvokeIfRequired(() => { Control.ExpandAll(); });
        }

        public void CollapseAll()
        {
            InvokeIfRequired(() => { Control.CollapseAll(); });
        }

        public void Add(TreeNode treeNode)
        {
            Add(treeNode, false);
        }

        public void Insert(int index, TreeNode treeNode)
        {
            InvokeIfRequired(() =>
            {
                Control.Nodes.Insert(index, treeNode);
            });
        }

        public void Load(TreeNode treeNode)
        {
            Add(treeNode, true);
        }

        public void SetImageIndex(TreeNode treeNode, int imageIndex)
        {
            InvokeIfRequired(() =>
            {
                treeNode.ImageIndex = treeNode.SelectedImageIndex = imageIndex;
            });
        }

        #region Helper Methods

        private void Add(TreeNode treeNode, bool doClear)
        {
            InvokeIfRequired(() =>
            {
                if (doClear)
                    Control.Nodes.Clear();

                Control.Nodes.Add(treeNode);
                if (Control.SelectedNode == null)
                    Control.SelectedNode = treeNode;
            });
        }

        private void RecordExpandedNodes(List<TreeNode> expanded, TreeNode startNode)
        {
            if (startNode.IsExpanded)
                expanded.Add(startNode);

            foreach (TreeNode node in startNode.Nodes)
                RecordExpandedNodes(expanded, node);
        }

        private IList<TreeNode> GetCheckedNodes()
        {
            var checkedNodes = new List<TreeNode>();
            foreach (TreeNode node in Control.Nodes)
                CollectCheckedNodes(checkedNodes, node);
            return checkedNodes;
        }

        private void CollectCheckedNodes(List<TreeNode> checkedNodes, TreeNode node)
        {
            if (node.Checked)
                checkedNodes.Add(node);
            else
                foreach (TreeNode child in node.Nodes)
                    CollectCheckedNodes(checkedNodes, child);
        }

        #endregion Helper Methods
    }

    #endregion ICommand
}