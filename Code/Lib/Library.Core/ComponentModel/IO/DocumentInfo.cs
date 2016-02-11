namespace Library.IO
{
    /// <summary>
    ///
    /// </summary>
    public interface IDocumentInfo
    {
        /// <summary>
        ///
        /// </summary>
        string Author { get; }

        /// <summary>
        ///
        /// </summary>
        string Keywords { get; }

        /// <summary>
        ///
        /// </summary>
        string Subject { get; }

        /// <summary>
        ///
        /// </summary>
        string Title { get; }
    }

    /// <summary>
    ///
    /// </summary>
    public class DocumentInfo : IDocumentInfo
    {
        /// <summary>
        ///
        /// </summary>
        public string Author { get; set; }

        /// <summary>
        ///
        /// </summary>
        public string Keywords { get; set; }

        /// <summary>
        ///
        /// </summary>
        public string Subject { get; set; }

        /// <summary>
        ///
        /// </summary>
        public string Title { get; set; }
    }
}