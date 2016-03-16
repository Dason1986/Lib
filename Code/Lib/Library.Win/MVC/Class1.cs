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
        readonly List<Filter> filterlist;

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

    #endregion

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

    #endregion

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
    #endregion

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
    #endregion

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
    #endregion

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
    #endregion

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
                    else {
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
        static IShowViewResult _ShowViewResult = new ShowViewResult();
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
    #endregion
}