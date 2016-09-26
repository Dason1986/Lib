using System.IO;

namespace Library.Win.MVP
{
    /// <summary>
    /// 
    /// </summary>
    public interface IApplicationFacade
    {
        /// <summary>
        /// 
        /// </summary>
        IApplicationMessage Message { get; }
        /// <summary>
        /// 
        /// </summary>
        TextWriter Writer { get; set; }
    }
}