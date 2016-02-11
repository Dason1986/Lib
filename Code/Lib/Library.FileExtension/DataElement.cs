using Library.Draw;
using System;
using System.Collections;
using System.ComponentModel;

namespace Library.FileExtension
{
    public abstract class DataElement
    {
        /// <summary>
        ///
        /// </summary>
        public string Name { get; set; }

        public ElementPosition Position { get; set; }

        public RGBColor Color { get; set; }
    }

    public enum Alignment
    {
    }

    public struct ElementPosition
    {
        public ElementPosition(float x, float y)
            : this()
        {
            X = x;
            Y = y;
        }

        public float X { get; set; }
        public float Y { get; set; }
    }

    public struct ElementSize
    {
        public ElementSize(float width, float height)
            : this()
        {
            Width = width;
            Height = height;
        }

        public float Width { get; set; }
        public float Height { get; set; }
    }

    public interface IConvertTo
    {
        object Converter(object source, object arg, Type targetType);
    }

    public class TextElement : DataElement
    {
        /// <summary>
        ///
        /// </summary>
        public string FontName { get; set; }

        /// <summary>
        ///
        /// </summary>
        public double? FontSize { get; set; }

        /// <summary>
        ///
        /// </summary>
        public Margin? Margin { get; set; }

        /// <summary>
        ///
        /// </summary>
        public IConvertTo Convert { get; set; }
    }

    public class ImageElement : DataElement
    {
        public byte[] Image { get; set; }

        public ElementSize Size { get; set; }
    }

    public class TableElement : DataElement
    {
        public TableElement(ElementPosition position, object dt)
        {
            Position = position;
            DataSource = dt;
        }

        public static float DefalutWidth = 100;
        private object _dataSource;

        public object DataSource
        {
            get { return _dataSource; }
            set
            {
                if (value != null)
                {
                    if (value is IListSource == false && value is IList == false)
                    {
                        throw new Exception("datasoucre");
                    }
                }
                _dataSource = value;
            }
        }

        /// <summary>
        /// 不夠指定行數時填充空白行記錄
        /// </summary>
        public bool FillRows { get; set; }

        /// <summary>
        /// 數據表必須有多少行記錄
        /// </summary>
        public int FillRowCounts { get; set; }

        public TableHeadElement[] Heads { get; set; }
        public bool FillPage { get; set; }

        /// <summary>
        /// 下一頁時採用間距定位
        /// </summary>
        public bool NextPageMarginPosition { get; set; }
    }

    public class RectangleElement : DataElement
    {
        public RectangleElement(ElementPosition position, ElementSize size)
        {
            Position = position;
            Size = size;
        }

        public ElementSize Size { get; set; }
        public float Border { get; set; }
    }

    public class LineElement : DataElement
    {
        private readonly float _lineWidth = 1;

        public LineElement(ElementPosition position, ElementSize size)
        {
            Size = size;
            Position = position;
        }

        public LineElement(ElementPosition position, float width)
        {
            Size = new ElementSize(width, position.Y);
            Position = position;
        }

        public LineElement(ElementPosition position, int width, float lineWidth)
        {
            Size = new ElementSize(width, position.Y);
            Position = position;
            _lineWidth = lineWidth;
        }

        public ElementSize Size { get; set; }

        public float LineWidth
        {
            get { return _lineWidth; }
        }
    }

    public class TableHeadElement
    {
        private float _width = TableElement.DefalutWidth;
        public string DisplayName { get; set; }
        public string BindName { get; set; }

        public float Width
        {
            get { return _width; }
            set { _width = value; }
        }

        public string Font { get; set; }
        public int FontSize { get; set; }
        public int VerticalAlignment { get; set; }
        public int HorizontalAlignment { get; set; }
        public TableCellElement CellElement { get; set; }
    }

    public class TableCellElement
    {
    }
}