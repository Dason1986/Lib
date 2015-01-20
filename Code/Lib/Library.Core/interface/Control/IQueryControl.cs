using Library.Data;

namespace Library.Controls
{
    /// <summary>
    /// 
    /// </summary>
    public interface IQueryControl
    {
        /// <summary>
        /// 
        /// </summary>
        string QueryDataID { get; set; }
        /// <summary>
        /// 
        /// </summary>
        FieldCollection Fields { get; }
        /// <summary>
        /// 
        /// </summary>
        object DataSource { get; set; }
        /// <summary>
        /// 
        /// </summary>
        FilterCollection Filters { get; }
        /// <summary>
        /// 
        /// </summary>
        OrderCollection Orders { get; }
        /// <summary>
        /// 
        /// </summary>
        string ValueMember { get; set; }
        /// <summary>
        /// 
        /// </summary>
        string DisplayMember { get; set; }

        /// <summary>
        /// ßx“ñÖµ
        /// </summary>
        object SelectedValue { get; set; }
    }
}