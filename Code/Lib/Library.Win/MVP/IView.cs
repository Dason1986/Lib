namespace Library.Win.MVP
{
    /// <summary>
    ///
    /// </summary>
    public interface IView
    {
        /// <summary>
        /// 激活
        /// </summary>
        void Activate();

        /// <summary>
        /// 禁用
        /// </summary>
        void Deactivate();
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
        TModel Model { get; set; }
    }

    /// <summary>
    ///
    /// </summary>
    public interface ICompositeView : IView
    {
        /// <summary>
        /// Adds the specified view instance to the composite view collection.
        /// </summary>
        void Add(IView view);
    }
}