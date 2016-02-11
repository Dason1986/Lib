using System.Drawing;

namespace Library.Draw.Print
{
    /// <summary>
    ///
    /// </summary>
    public class PrintOption
    {
        private Point _movePoint;

        /// <summary>
        ///
        /// </summary>
        public bool IsPreview { get; set; }

        /// <summary>
        ///
        /// </summary>
        public Point MovePoint
        {
            get { return _movePoint; }
            set
            {
                if (_movePoint == value) return;
                _movePoint = value;
                RebuildImage = true;
            }
        }

        internal bool RebuildImage = true;

        /// <summary>
        ///
        /// </summary>
        /// <param name="isPreview"></param>
        public void SetIsPreview(bool isPreview)
        {
            IsPreview = isPreview;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="movePoint"></param>
        public void SetMovePoint(Point movePoint)
        {
            MovePoint = movePoint;
        }
    }
}