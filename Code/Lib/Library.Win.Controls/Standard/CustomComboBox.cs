using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace Library.Controls
{
    /// <summary>
    /// <c>CustomComboBox</c> is an extension of <c>ComboBox</c> which provides drop-down customization.
    /// </summary>
    [SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
    [Designer(typeof(CustomComboBoxDesigner))]
    public class CustomComboBox : ComboBox, IPopupControlHost
    {
        #region Construction and destruction

        public CustomComboBox()
          
        {
            m_sizeCombo = new Size(base.DropDownWidth, base.DropDownHeight);
            m_popupCtrl.Closing += new ToolStripDropDownClosingEventHandler(m_dropDown_Closing);
        }

        void m_dropDown_Closing(object sender, ToolStripDropDownClosingEventArgs e)
        {
            m_lastHideTime = DateTime.Now;
        }

        public CustomComboBox(Control dropControl)
            : this()
        {
            DropDownControl = dropControl;
        }

        #endregion

        #region ComboBox overrides

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (m_timerAutoFocus != null)
                {
                    m_timerAutoFocus.Dispose();
                    m_timerAutoFocus = null;
                }
            }
            base.Dispose(disposing);
        }

        #endregion

        #region Event handlers

        private void timerAutoFocus_Tick(object sender, EventArgs e)
        {
            if (m_popupCtrl.Visible && !DropDownControl.Focused)
            {
                DropDownControl.Focus();
                m_timerAutoFocus.Enabled = false;
            }

            if (base.DroppedDown)
                base.DroppedDown = false;
        }

        private void m_dropDown_LostFocus(object sender, EventArgs e)
        {
            m_lastHideTime = DateTime.Now;
        }

        #endregion

        #region Events

        public new event EventHandler DropDown;
        public new event EventHandler DropDownClosed;

        public new event OldNewEventHandler<object> SelectedValueChanged;

        public void RaiseDropDownEvent()
        {
            EventHandler eventHandler = this.DropDown;
            if (eventHandler != null)
                this.DropDown(this, EventArgs.Empty);
        }

        public void RaiseDropDownClosedEvent()
        {
            EventHandler eventHandler = this.DropDownClosed;
            if (eventHandler != null)
                this.DropDownClosed(this, EventArgs.Empty);
        }

        public void RaiseSelectedValueChangedEvent(object oldValue, object newValue)
        {
            OldNewEventHandler<object> eventHandler = this.SelectedValueChanged;
            if (eventHandler != null)
                this.SelectedValueChanged(this, new OldNewEventArgs<object>(oldValue, newValue));
        }

        #endregion

        #region IPopupControlHost Members

        /// <summary>
        /// Displays drop-down area of combo box, if not already shown.
        /// </summary>
        public virtual void ShowDropDown()
        {
            if (m_popupCtrl != null && !IsDroppedDown)
            {
                // Raise drop-down event.
                RaiseDropDownEvent();

                // Restore original control size.
                AutoSizeDropDown();

                Point location = PointToScreen(new Point(0, Height));

                // Actually show popup.
                PopupResizeMode resizeMode = (this.m_bIsResizable ? PopupResizeMode.BottomRight : PopupResizeMode.None);
                m_popupCtrl.Show(this.DropDownControl, location.X, location.Y, Width, Height, resizeMode);
                m_bDroppedDown = true;

                m_popupCtrl.PopupControlHost = this;

                // Initialize automatic focus timer?
                if (m_timerAutoFocus == null)
                {
                    m_timerAutoFocus = new Timer();
                    m_timerAutoFocus.Interval = 10;
                    m_timerAutoFocus.Tick += new EventHandler(timerAutoFocus_Tick);
                }
                // Enable the timer!
                m_timerAutoFocus.Enabled = true;
                m_sShowTime = DateTime.Now;
            }
        }

        /// <summary>
        /// Hides drop-down area of combo box, if shown.
        /// </summary>
        public virtual void HideDropDown()
        {
            if (m_popupCtrl != null && IsDroppedDown)
            {
                // Hide drop-down control.
                m_popupCtrl.Hide();
                m_bDroppedDown = false;

                // Disable automatic focus timer.
                if (m_timerAutoFocus != null && m_timerAutoFocus.Enabled)
                    m_timerAutoFocus.Enabled = false;

                // Raise drop-down closed event.
                RaiseDropDownClosedEvent();
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Automatically resize drop-down from properties.
        /// </summary>
        protected void AutoSizeDropDown()
        {
            if (DropDownControl != null)
            {
                switch (DropDownSizeMode)
                {
                    case SizeMode.UseComboSize:
                        DropDownControl.Size = new Size(Width, m_sizeCombo.Height);
                        break;

                    case SizeMode.UseControlSize:
                        DropDownControl.Size = new Size(m_sizeOriginal.Width, m_sizeOriginal.Height);
                        break;

                    case SizeMode.UseDropDownSize:
                        DropDownControl.Size = m_sizeCombo;
                        break;
                }
            }
        }

        /// <summary>
        /// Assigns control to custom drop-down area of combo box.
        /// </summary>
        /// <param name="control">Control to be used as drop-down. Please note that this control must not be contained elsewhere.</param>
        protected virtual void AssignControl(Control control)
        {
            // If specified control is different then...
            if (control != DropDownControl)
            {
                // Preserve original container size.
                m_sizeOriginal = control.Size;

                // Reference the user-specified drop down control.
                m_dropDownCtrl = control;
            }
        }

        #endregion

        #region Win32 message handlers

        public const uint WM_COMMAND = 0x0111;
        public const uint WM_USER = 0x0400;
        public const uint WM_REFLECT = WM_USER + 0x1C00;
        public const uint WM_LBUTTONDOWN = 0x0201;

        public const uint CBN_DROPDOWN = 7;
        public const uint CBN_CLOSEUP = 8;

        public static uint HIWORD(int n)
        {
            return (uint)(n >> 16) & 0xffff;
        }

        public override bool PreProcessMessage(ref Message m)
        {
            if (m.Msg == (WM_REFLECT + WM_COMMAND))
            {
                if (HIWORD((int)m.WParam) == CBN_DROPDOWN)
                    return false;
            }
            return base.PreProcessMessage(ref m);
        }

        private static DateTime m_sShowTime = DateTime.Now;

        private void AutoDropDown()
        {
            if (m_popupCtrl != null && m_popupCtrl.Visible)
                HideDropDown();
            else if ((DateTime.Now - m_lastHideTime).Milliseconds > 50)
                ShowDropDown();
        }

        protected override void WndProc(ref Message m)
        {
            if (m.Msg == WM_LBUTTONDOWN)
            {
                AutoDropDown();
                return;
            }

            if (m.Msg == (WM_REFLECT + WM_COMMAND))
            {
                switch (HIWORD((int)m.WParam))
                {
                    case CBN_DROPDOWN:
                        AutoDropDown();
                        return;

                    case CBN_CLOSEUP:
                        if ((DateTime.Now - m_sShowTime).Seconds > 1)
                            HideDropDown();
                        return;
                }
            }

            base.WndProc(ref m);
        }

        #endregion

        #region Enumerations

        public enum SizeMode
        {
            UseComboSize,
            UseControlSize,
            UseDropDownSize,
        }

        #endregion

        #region Properties

        /// <summary>
        /// Actual drop-down control itself.
        /// </summary>
        [Browsable(false)]
        public Control DropDownControl
        {
            get { return m_dropDownCtrl; }
            set { AssignControl(value); }
        }

        /// <summary>
        /// Indicates if drop-down is currently shown.
        /// </summary>
        [Browsable(false)]
        public bool IsDroppedDown
        {
            get { return this.m_bDroppedDown /*&& m_popupCtrl.Visible*/; }
        }

        /// <summary>
        /// Indicates if drop-down is resizable.
        /// </summary>
        [Category("Custom Drop-Down"), Description("Indicates if drop-down is resizable.")]
        public bool AllowResizeDropDown
        {
            get { return this.m_bIsResizable; }
            set { this.m_bIsResizable = value; }
        }

        /// <summary>
        /// Indicates current sizing mode.
        /// </summary>
        [Category("Custom Drop-Down"), Description("Indicates current sizing mode."), DefaultValue(SizeMode.UseComboSize)]
        public SizeMode DropDownSizeMode
        {
            get { return this.m_sizeMode; }
            set
            {
                if (value != this.m_sizeMode)
                {
                    this.m_sizeMode = value;
                    AutoSizeDropDown();
                }
            }
        }

        [Category("Custom Drop-Down")]
        public Size DropSize
        {
            get { return m_sizeCombo; }
            set
            {
                m_sizeCombo = value;
                if (DropDownSizeMode == SizeMode.UseDropDownSize)
                    AutoSizeDropDown();
            }
        }

        [Category("Custom Drop-Down"), Browsable(false)]
        public Size ControlSize
        {
            get { return m_sizeOriginal; }
            set
            {
                m_sizeOriginal = value;
                if (DropDownSizeMode == SizeMode.UseControlSize)
                    AutoSizeDropDown();
            }
        }

        #endregion

        #region Hide some unwanted properties

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Browsable(false), ReadOnly(true)]
        public new ComboBoxStyle DropDownStyle
        {
            get { return base.DropDownStyle; }
            set { }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Browsable(false), ReadOnly(true)]
        public new ObjectCollection Items
        {
            get { return base.Items; }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Browsable(false), ReadOnly(true)]
        public new int ItemHeight
        {
            get { return base.ItemHeight; }
            set { }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Browsable(false), ReadOnly(true)]
        public new int MaxDropDownItems
        {
            get { return base.MaxDropDownItems; }
            set { }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Browsable(false), ReadOnly(true)]
        public new string DisplayMember
        {
            get { return base.DisplayMember; }
            set { }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Browsable(false), ReadOnly(true)]
        public new string ValueMember
        {
            get { return base.ValueMember; }
            set { }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Browsable(false), ReadOnly(true)]
        public new int DropDownWidth
        {
            get { return base.DropDownWidth; }
            set { }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Browsable(false), ReadOnly(true)]
        public new int DropDownHeight
        {
            get { return base.DropDownHeight; }
            set { }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Browsable(false), ReadOnly(true)]
        public new bool IntegralHeight
        {
            get { return base.IntegralHeight; }
            set { }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Browsable(false), ReadOnly(true)]
        public new bool Sorted
        {
            get { return base.Sorted; }
            set { }
        }

        #endregion

        #region Attributes

        /// <summary>
        /// Popup control.
        /// </summary>
        private PopupControl m_popupCtrl = new PopupControl();

        /// <summary>
        /// Actual drop-down control itself.
        /// </summary>
        Control m_dropDownCtrl;
        /// <summary>
        /// Indicates if drop-down is currently shown.
        /// </summary>
        bool m_bDroppedDown = false;
        /// <summary>
        /// Indicates current sizing mode.
        /// </summary>
        SizeMode m_sizeMode = SizeMode.UseComboSize;
        /// <summary>
        /// Time drop-down was last hidden.
        /// </summary>
        DateTime m_lastHideTime = DateTime.Now;

        /// <summary>
        /// Automatic focus timer helps make sure drop-down control is focused for user
        /// input upon drop-down.
        /// </summary>
        Timer m_timerAutoFocus;
        /// <summary>
        /// Original size of control dimensions when first assigned.
        /// </summary>
        Size m_sizeOriginal = new Size(1, 1);
        /// <summary>
        /// Original size of combo box dropdown when first assigned.
        /// </summary>
        Size m_sizeCombo;
        /// <summary>
        /// Indicates if drop-down is resizable.
        /// </summary>
        bool m_bIsResizable = true;

        #endregion
    }

    internal class CustomComboBoxDesigner : ParentControlDesigner
    {
        #region ParentControlDesigner Overrides

        protected override void PreFilterProperties(IDictionary properties)
        {
            base.PreFilterProperties(properties);
            properties.Remove("DropDownStyle");
            properties.Remove("Items");
            properties.Remove("ItemHeight");
            properties.Remove("MaxDropDownItems");
            properties.Remove("DisplayMember");
            properties.Remove("ValueMember");
            properties.Remove("DropDownWidth");
            properties.Remove("DropDownHeight");
            properties.Remove("IntegralHeight");
            properties.Remove("Sorted");
        }

        #endregion
    }


    public enum PopupResizeMode
    {
        None = 0,

        // Individual styles.
        Left = 1,
        Top = 2,
        Right = 4,
        Bottom = 8,

        // Combined styles.
        All = (Top | Left | Bottom | Right),
        TopLeft = (Top | Left),
        TopRight = (Top | Right),
        BottomLeft = (Bottom | Left),
        BottomRight = (Bottom | Right),
    }

    public enum GripAlignMode
    {
        TopLeft,
        TopRight,
        BottomLeft,
        BottomRight,
    }

    public sealed class GripRenderer
    {
        #region Construction and destruction

        private GripRenderer()
        {
        }

        #endregion

        #region Methods

        private static void InitializeGripBitmap(Graphics g, Size size, bool forceRefresh)
        {
            if (m_sGripBitmap == null || forceRefresh || size != m_sGripBitmap.Size)
            {
                // Draw size grip into a bitmap image.
                m_sGripBitmap = new Bitmap(size.Width, size.Height, g);
                using (Graphics gripG = Graphics.FromImage(m_sGripBitmap))
                    ControlPaint.DrawSizeGrip(gripG, SystemColors.ButtonFace, 0, 0, size.Width, size.Height);
            }
        }

        public static void RefreshSystemColors(Graphics g, Size size)
        {
            InitializeGripBitmap(g, size, true);
        }

        public static void Render(Graphics g, Point location, Size size, GripAlignMode mode)
        {
            InitializeGripBitmap(g, size, false);

            // Calculate display size and position of grip.
            switch (mode)
            {
                case GripAlignMode.TopLeft:
                    size.Height = -size.Height;
                    size.Width = -size.Width;
                    break;

                case GripAlignMode.TopRight:
                    size.Height = -size.Height;
                    break;

                case GripAlignMode.BottomLeft:
                    size.Width = -size.Height;
                    break;
            }

            // Reverse size grip for left-aligned.
            if (size.Width < 0)
                location.X -= size.Width;
            if (size.Height < 0)
                location.Y -= size.Height;

            g.DrawImage(GripBitmap, location.X, location.Y, size.Width, size.Height);
        }

        public static void Render(Graphics g, Point location, GripAlignMode mode)
        {
            Render(g, location, new Size(16, 16), mode);
        }

        #endregion

        #region Properties

        private static Bitmap GripBitmap
        {
            get { return m_sGripBitmap; }
        }

        #endregion

        #region Attributes

        private static Bitmap m_sGripBitmap;

        #endregion
    }
    [ToolboxItem(false)]
    public class PopupDropDown : ToolStripDropDown
    {
        #region Construction and destruction

        public PopupDropDown(bool autoSize)
        {
            AutoSize = autoSize;
            Padding = Margin = Padding.Empty;
        }

        #endregion

        #region ToolStripDropDown overrides

        protected override void OnClosing(ToolStripDropDownClosingEventArgs e)
        {
            Control hostedControl = GetHostedControl();
            if (hostedControl != null)
                hostedControl.SizeChanged -= hostedControl_SizeChanged;
            base.OnClosing(e);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            GripBounds = Rectangle.Empty;

            if (CompareResizeMode(PopupResizeMode.BottomLeft))
            {
                // Draw grip area at bottom-left of popup.
                e.Graphics.FillRectangle(SystemBrushes.ButtonFace, 1, Height - 16, Width - 2, 14);
                GripBounds = new Rectangle(1, Height - 16, 16, 16);
                GripRenderer.Render(e.Graphics, GripBounds.Location, GripAlignMode.BottomLeft);
            }
            else if (CompareResizeMode(PopupResizeMode.BottomRight))
            {
                // Draw grip area at bottom-right of popup.
                e.Graphics.FillRectangle(SystemBrushes.ButtonFace, 1, Height - 16, Width - 2, 14);
                GripBounds = new Rectangle(Width - 17, Height - 16, 16, 16);
                GripRenderer.Render(e.Graphics, GripBounds.Location, GripAlignMode.BottomRight);
            }
            else if (CompareResizeMode(PopupResizeMode.TopLeft))
            {
                // Draw grip area at top-left of popup.
                e.Graphics.FillRectangle(SystemBrushes.ButtonFace, 1, 1, Width - 2, 14);
                GripBounds = new Rectangle(1, 0, 16, 16);
                GripRenderer.Render(e.Graphics, GripBounds.Location, GripAlignMode.TopLeft);
            }
            else if (CompareResizeMode(PopupResizeMode.TopRight))
            {
                // Draw grip area at top-right of popup.
                e.Graphics.FillRectangle(SystemBrushes.ButtonFace, 1, 1, Width - 2, 14);
                GripBounds = new Rectangle(Width - 17, 0, 16, 16);
                GripRenderer.Render(e.Graphics, GripBounds.Location, GripAlignMode.TopRight);
            }
        }

        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);

            // When drop-down window is being resized by the user (i.e. not locked),
            // update size of hosted control.
            if (!m_lockedThisSize)
                RecalculateHostedControlLayout();
        }

        protected void hostedControl_SizeChanged(object sender, EventArgs e)
        {
            // Only update size of this container when it is not locked.
            if (!m_lockedHostedControlSize)
                ResizeFromContent(-1);
        }

        #endregion

        #region Methods

        public new void Show(int x, int y)
        {
            Show(x, y, -1, -1);
        }

        public void Show(int x, int y, int width, int height)
        {
            // If no hosted control is associated, this procedure is pointless!
            Control hostedControl = GetHostedControl();
            if (hostedControl == null)
                return;

            // Initially hosted control should be displayed within a drop down of 1x1, however
            // its size should exceed the dimensions of the drop-down.
            {
                m_lockedHostedControlSize = true;
                m_lockedThisSize = true;

                // Display actual popup and occupy just 1x1 pixel to avoid automatic reposition.
                Size = new Size(1, 1);
                base.Show(x, y);

                m_lockedHostedControlSize = false;
                m_lockedThisSize = false;
            }

            // Resize drop-down to fit its contents.
            ResizeFromContent(width);

            // If client area was enlarged using the minimum width paramater, then the hosted
            // control must also be enlarged.
            if (m_refreshSize)
                RecalculateHostedControlLayout();

            // If popup is overlapping the initial position then move above!
            if (y > Top && y <= Bottom)
            {
                Top = y - Height - (height != -1 ? height : 0);

                PopupResizeMode previous = ResizeMode;
                if (ResizeMode == PopupResizeMode.BottomLeft)
                    ResizeMode = PopupResizeMode.TopLeft;
                else if (ResizeMode == PopupResizeMode.BottomRight)
                    ResizeMode = PopupResizeMode.TopRight;

                if (ResizeMode != previous)
                    RecalculateHostedControlLayout();
            }

            // Assign event handler to control.
            hostedControl.SizeChanged += hostedControl_SizeChanged;
        }

        protected void ResizeFromContent(int width)
        {
            if (m_lockedThisSize)
                return;

            // Prevent resizing hosted control to 1x1 pixel!
            m_lockedHostedControlSize = true;

            // Resize from content again because certain information was not available before.
            Rectangle bounds = Bounds;
            bounds.Size = SizeFromContent(width);

            if (!CompareResizeMode(PopupResizeMode.None))
            {
                if (width > 0 && bounds.Width - 2 > width)
                    if (!CompareResizeMode(PopupResizeMode.Right))
                        bounds.X -= bounds.Width - 2 - width;
            }

            Bounds = bounds;

            m_lockedHostedControlSize = false;
        }

        protected void RecalculateHostedControlLayout()
        {
            if (m_lockedHostedControlSize)
                return;

            m_lockedThisSize = true;

            // Update size of hosted control.
            Control hostedControl = GetHostedControl();
            if (hostedControl != null)
            {
                // Fetch control bounds and adjust as necessary.
                Rectangle bounds = hostedControl.Bounds;
                if (CompareResizeMode(PopupResizeMode.TopLeft) || CompareResizeMode(PopupResizeMode.TopRight))
                    bounds.Location = new Point(1, 16);
                else
                    bounds.Location = new Point(1, 1);

                bounds.Width = ClientRectangle.Width - 2;
                bounds.Height = ClientRectangle.Height - 2;
                if (IsGripShown)
                    bounds.Height -= 16;

                if (bounds.Size != hostedControl.Size)
                    hostedControl.Size = bounds.Size;
                if (bounds.Location != hostedControl.Location)
                    hostedControl.Location = bounds.Location;
            }

            m_lockedThisSize = false;
        }

        public Control GetHostedControl()
        {
            if (Items.Count > 0)
            {
                ToolStripControlHost host = Items[0] as ToolStripControlHost;
                if (host != null)
                    return host.Control;
            }
            return null;
        }

        public bool CompareResizeMode(PopupResizeMode resizeMode)
        {
            return (ResizeMode & resizeMode) == resizeMode;
        }

        protected Size SizeFromContent(int width)
        {
            Size contentSize = Size.Empty;

            m_refreshSize = false;

            // Fetch hosted control.
            Control hostedControl = GetHostedControl();
            if (hostedControl != null)
            {
                if (CompareResizeMode(PopupResizeMode.TopLeft) || CompareResizeMode(PopupResizeMode.TopRight))
                    hostedControl.Location = new Point(1, 16);
                else
                    hostedControl.Location = new Point(1, 1);
                contentSize = SizeFromClientSize(hostedControl.Size);

                // Use minimum width (if specified).
                if (width > 0 && contentSize.Width < width)
                {
                    contentSize.Width = width;
                    m_refreshSize = true;
                }
            }

            // If a grip box is shown then add it into the drop down height.
            if (IsGripShown)
                contentSize.Height += 16;

            // Add some additional space to allow for borders.
            contentSize.Width += 2;
            contentSize.Height += 2;

            return contentSize;
        }

        #endregion

        #region Win32 message processing

        #region Win32 stuff

        protected const int WM_GETMINMAXINFO = 0x0024;
        protected const int WM_NCHITTEST = 0x0084;

        protected const int HTTRANSPARENT = -1;
        protected const int HTLEFT = 10;
        protected const int HTRIGHT = 11;
        protected const int HTTOP = 12;
        protected const int HTTOPLEFT = 13;
        protected const int HTTOPRIGHT = 14;
        protected const int HTBOTTOM = 15;
        protected const int HTBOTTOMLEFT = 16;
        protected const int HTBOTTOMRIGHT = 17;

        [StructLayout(LayoutKind.Sequential)]
        internal struct MINMAXINFO
        {
            public Point reserved;
            public Size maxSize;
            public Point maxPosition;
            public Size minTrackSize;
            public Size maxTrackSize;
        }

        protected static int HIWORD(int n)
        {
            return (n >> 16) & 0xffff;
        }
        protected static int HIWORD(IntPtr n)
        {
            return HIWORD(unchecked((int)(long)n));
        }
        protected static int LOWORD(int n)
        {
            return n & 0xffff;
        }
        protected static int LOWORD(IntPtr n)
        {
            return LOWORD(unchecked((int)(long)n));
        }

        #endregion

        [SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
        protected override void WndProc(ref Message m)
        {
            if (!ProcessGrip(ref m, false))
                base.WndProc(ref m);
        }

        /// <summary>
        /// Processes the resizing messages.
        /// </summary>
        /// <param name="m">The message.</param>
        /// <returns>true, if the WndProc method from the base class shouldn't be invoked.</returns>
        [SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
        public bool ProcessGrip(ref Message m)
        {
            return ProcessGrip(ref m, true);
        }

        [SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
        private bool ProcessGrip(ref Message m, bool contentControl)
        {
            if (ResizeMode != PopupResizeMode.None)
            {
                switch (m.Msg)
                {
                    case WM_NCHITTEST:
                        return OnNcHitTest(ref m, contentControl);

                    case WM_GETMINMAXINFO:
                        return OnGetMinMaxInfo(ref m);
                }
            }
            return false;
        }

        [SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
        private bool OnGetMinMaxInfo(ref Message m)
        {
            Control hostedControl = GetHostedControl();
            if (hostedControl != null)
            {
                MINMAXINFO minmax = (MINMAXINFO)Marshal.PtrToStructure(m.LParam, typeof(MINMAXINFO));

                // Maximum size.
                if (hostedControl.MaximumSize.Width != 0)
                    minmax.maxTrackSize.Width = hostedControl.MaximumSize.Width;
                if (hostedControl.MaximumSize.Height != 0)
                    minmax.maxTrackSize.Height = hostedControl.MaximumSize.Height;

                // Minimum size.
                minmax.minTrackSize = new Size(32, 32);
                if (hostedControl.MinimumSize.Width > minmax.minTrackSize.Width)
                    minmax.minTrackSize.Width = hostedControl.MinimumSize.Width;
                if (hostedControl.MinimumSize.Height > minmax.minTrackSize.Height)
                    minmax.minTrackSize.Height = hostedControl.MinimumSize.Height;

                Marshal.StructureToPtr(minmax, m.LParam, false);
            }
            return true;
        }

        private bool OnNcHitTest(ref Message m, bool contentControl)
        {
            Point location = PointToClient(new Point(LOWORD(m.LParam), HIWORD(m.LParam)));
            IntPtr transparent = new IntPtr(HTTRANSPARENT);

            // Check for simple gripper dragging.
            if (GripBounds.Contains(location))
            {
                if (CompareResizeMode(PopupResizeMode.BottomLeft))
                {
                    m.Result = contentControl ? transparent : (IntPtr)HTBOTTOMLEFT;
                    return true;
                }
                else if (CompareResizeMode(PopupResizeMode.BottomRight))
                {
                    m.Result = contentControl ? transparent : (IntPtr)HTBOTTOMRIGHT;
                    return true;
                }
                else if (CompareResizeMode(PopupResizeMode.TopLeft))
                {
                    m.Result = contentControl ? transparent : (IntPtr)HTTOPLEFT;
                    return true;
                }
                else if (CompareResizeMode(PopupResizeMode.TopRight))
                {
                    m.Result = contentControl ? transparent : (IntPtr)HTTOPRIGHT;
                    return true;
                }
            }
            else   // Check for edge based dragging.
            {
                Rectangle rectClient = ClientRectangle;
                if (location.X > rectClient.Right - 3 && location.X <= rectClient.Right && CompareResizeMode(PopupResizeMode.Right))
                {
                    m.Result = contentControl ? transparent : (IntPtr)HTRIGHT;
                    return true;
                }
                else if (location.Y > rectClient.Bottom - 3 && location.Y <= rectClient.Bottom && CompareResizeMode(PopupResizeMode.Bottom))
                {
                    m.Result = contentControl ? transparent : (IntPtr)HTBOTTOM;
                    return true;
                }
                else if (location.X > -1 && location.X < 3 && CompareResizeMode(PopupResizeMode.Left))
                {
                    m.Result = contentControl ? transparent : (IntPtr)HTLEFT;
                    return true;
                }
                else if (location.Y > -1 && location.Y < 3 && CompareResizeMode(PopupResizeMode.Top))
                {
                    m.Result = contentControl ? transparent : (IntPtr)HTTOP;
                    return true;
                }
            }
            return false;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Type of resize mode, grips are automatically drawn at bottom-left and bottom-right corners.
        /// </summary>
        public PopupResizeMode ResizeMode
        {
            get { return m_resizeMode; }
            set
            {
                if (value != m_resizeMode)
                {
                    m_resizeMode = value;
                    Invalidate();
                }
            }
        }

        /// <summary>
        /// Bounds of active grip box position.
        /// </summary>
        protected Rectangle GripBounds
        {
            get { return this.m_gripBounds; }
            set { this.m_gripBounds = value; }
        }

        /// <summary>
        /// Indicates when a grip box is shown.
        /// </summary>
        protected bool IsGripShown
        {
            get
            {
                return (ResizeMode == PopupResizeMode.TopLeft || ResizeMode == PopupResizeMode.TopRight ||
                        ResizeMode == PopupResizeMode.BottomLeft || ResizeMode == PopupResizeMode.BottomRight);
            }
        }

        #endregion

        #region Attributes

        private PopupResizeMode m_resizeMode = PopupResizeMode.None;
        private Rectangle m_gripBounds = Rectangle.Empty;

        private bool m_lockedHostedControlSize = false;
        private bool m_lockedThisSize = false;
        private bool m_refreshSize = false;

        #endregion
    }

    public interface IPopupControlHost
    {
        #region Methods

        /// <summary>
        /// Displays drop-down area of combo box, if not already shown.
        /// </summary>
        void ShowDropDown();

        /// <summary>
        /// Hides drop-down area of combo box, if shown.
        /// </summary>
        void HideDropDown();

        #endregion
    }

    public class PopupControl
    {
        #region Construction and destruction

        public PopupControl()
        {
            InitializeDropDown();
        }

        #endregion

        #region Event handlers

        private void m_dropDown_Closed(object sender, ToolStripDropDownClosedEventArgs e)
        {
            if (AutoResetWhenClosed)
                DisposeHost();

            // Hide drop down within popup control.
            if (PopupControlHost != null)
                PopupControlHost.HideDropDown();
        }

        #endregion

        #region Events

        public event ToolStripDropDownClosingEventHandler Closing
        {
            add
            {
                m_dropDown.Closing += value;
            }
            remove
            {
                m_dropDown.Closing -= value;
            }
        }

        #endregion

        #region Methods

        public void Show(Control control, int x, int y)
        {
            Show(control, x, y, PopupResizeMode.None);
        }

        public void Show(Control control, int x, int y, PopupResizeMode resizeMode)
        {
            Show(control, x, y, -1, -1, resizeMode);
        }

        public void Show(Control control, int x, int y, int width, int height, PopupResizeMode resizeMode)
        {
            Size controlSize = control.Size;

            InitializeHost(control);

            m_dropDown.ResizeMode = resizeMode;
            m_dropDown.Show(x, y, width, height);

            control.Focus();
        }

        public void Hide()
        {
            if (m_dropDown != null && m_dropDown.Visible)
            {
                m_dropDown.Hide();
                DisposeHost();
            }
        }

        public void Reset()
        {
            DisposeHost();
        }

        #endregion

        #region Internal methods

        protected void DisposeHost()
        {
            if (m_host != null)
            {
                // Make sure host is removed from drop down.
                if (m_dropDown != null)
                    m_dropDown.Items.Clear();

                // Dispose of host.
                m_host = null;
            }

            PopupControlHost = null;
        }

        protected void InitializeHost(Control control)
        {
            InitializeDropDown();

            // If control is not yet being hosted then initialize host.
            if (control != Control)
                DisposeHost();

            // Create a new host?
            if (m_host == null)
            {
                m_host = new ToolStripControlHost(control);
                m_host.AutoSize = false;
                m_host.Padding = Padding;
                m_host.Margin = Margin;
            }

            // Add control to drop-down.
            m_dropDown.Items.Clear();
            m_dropDown.Padding = m_dropDown.Margin = Padding.Empty;
            m_dropDown.Items.Add(m_host);
        }

        protected void InitializeDropDown()
        {
            // Does a drop down exist?
            if (m_dropDown == null)
            {
                m_dropDown = new PopupDropDown(false);
                m_dropDown.Closed += new ToolStripDropDownClosedEventHandler(m_dropDown_Closed);
            }
        }

        #endregion

        #region Properties

        public bool Visible
        {
            get { return (this.m_dropDown != null && this.m_dropDown.Visible) ? true : false; }
        }

        public Control Control
        {
            get { return (this.m_host != null) ? this.m_host.Control : null; }
        }

        public Padding Padding
        {
            get { return this.m_padding; }
            set { this.m_padding = value; }
        }

        public Padding Margin
        {
            get { return this.m_margin; }
            set { this.m_margin = value; }
        }

        public bool AutoResetWhenClosed
        {
            get { return this.m_autoReset; }
            set { this.m_autoReset = value; }
        }

        /// <summary>
        /// Gets or sets the popup control host, this is used to hide/show popup.
        /// </summary>
        public IPopupControlHost PopupControlHost { get; set; }

        #endregion

        #region Attributes

        private ToolStripControlHost m_host;
        private PopupDropDown m_dropDown;

        private Padding m_padding = Padding.Empty;
        private Padding m_margin = new Padding(1, 1, 1, 1);

        private bool m_autoReset = false;

        #endregion
    }

    public class OldNewEventArgs<T> : EventArgs
    {
        public OldNewEventArgs(T oldValue, T newValue)
        {
            OldValue = oldValue;
            NewValue = newValue;
        }

        public T OldValue
        {
            get { return this.m_oldValue; }
            protected set { this.m_oldValue = value; }
        }
        public T NewValue
        {
            get { return this.m_newValue; }
            protected set { this.m_newValue = value; }
        }

        T m_oldValue = default(T);
        T m_newValue = default(T);
    }

    public delegate void OldNewEventHandler<T>(object sender, OldNewEventArgs<T> e);
}
