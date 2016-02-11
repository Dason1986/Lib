using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Design;
using System.Drawing.Drawing2D;
using System.Runtime.CompilerServices;
using System.Windows.Forms;

namespace Library.Controls
{
    /// <summary>
    /// Note.  This control has two challanges to overcome
    /// 1	allow the client to bind to the drop down click events
    /// 2	have design time support.  This design time support will
    ///	be covered in a subsequent article
    /// </summary>
    public partial class SplitButton : Button
    {
        #region Fields

        private bool _calculateSplitRect = true;
        private bool _fillSplitHeight = true;
        private int _splitHeight;
        private int _splitWidth;
        private bool _persistDropDownName;

        /// <summary>
        /// Store the 4 possible image names (5 image states).  _HoverImage and
        /// _FocusedImage share the same image state.
        /// </summary>
        private string _normalImage;

        private string _hoverImage;
        private string _clickedImage;
        private string _disabledImage;
        private string _focusedImage;

        /// <summary>
        ///
        /// </summary>
        public bool PersistDropDownName
        {
            get
            {
                return _persistDropDownName;
            }
            set
            {
                _persistDropDownName = value;
            }
        }

        /// <summary>
        /// Images are housed here
        /// </summary>
        private ImageList _defaultSplitImages;

        /// <summary>
        /// A dictionary allowing the events to be tied to the drop down list.
        ///
        /// The first generic type, string, is the identifier of the event the key
        /// and we will make it the text display of the drop down item.  The second
        /// generic type, EventHandler is the EventHandler for the event.
        ///
        /// This is the mechanism through which we keep the control's interface to
        /// its client without exposing the ContextMenuStrip itself.
        /// </summary>
        private Dictionary<string, EventHandler> _dropDownsEventHandlers = new Dictionary<string, EventHandler>();

        /// <summary>
        /// I am using the timers to keep track of the open/close state of the
        /// drop-down menu.
        /// </summary>
        static private readonly DateTime ZeroTime = new DateTime(0);

        private DateTime _closedTime = ZeroTime;

        #endregion Fields

        #region Events

        /// <summary>
        ///
        /// </summary>
        [Browsable(true)]
        [Category("Action")]
        [Description("Occurs when the button part of the SplitButton is clicked.")]
        public event EventHandler ButtonClick;

        #endregion Events

        #region Construction

        /// <summary>
        ///
        /// </summary>
        public SplitButton()
        {
            InitializeComponent();
        }

        #endregion Construction

        #region Helper Methods

        private void InitDefaultSplitImages()
        {
            _normalImage = "Normal";
            _hoverImage = "Hover";
            _clickedImage = "Clicked";
            _disabledImage = "Disabled";
            _focusedImage = "Hover";
            ImageKey = "Normal";
            InitDefaultSplitImages(false);
        }

        private void InitDefaultSplitImages(bool refresh)
        {
            if (string.IsNullOrEmpty(_normalImage)) _normalImage = "Normal";
            if (string.IsNullOrEmpty(_hoverImage)) _hoverImage = "Hover";
            if (string.IsNullOrEmpty(_clickedImage)) _clickedImage = "Clicked";
            if (string.IsNullOrEmpty(_disabledImage)) _disabledImage = "Disabled";
            if (string.IsNullOrEmpty(_focusedImage)) _focusedImage = "Hover";

            if (_defaultSplitImages == null)
                _defaultSplitImages = new ImageList();

            if (_defaultSplitImages.Images.Count == 0 || refresh)
            {
                if (_defaultSplitImages.Images.Count > 0)
                    _defaultSplitImages.Images.Clear();

                try
                {
                    int w;		// Width
                    int h;		// Height

                    if (!_calculateSplitRect && _splitWidth > 0)
                        w = _splitWidth;
                    else
                        w = 18;

                    if (!CalculateSplitRect && SplitHeight > 0)
                        h = SplitHeight;
                    else
                        h = Height;
                    h -= 8;

                    _defaultSplitImages.ImageSize = new Size(w, h);

                    //
                    // Middles
                    //
                    int mw = w / 2 + (w % 2);
                    int mh = h / 2;

                    //
                    // Draw images and place them in the _DefaultSplitImages
                    // class.
                    //
                    using (SolidBrush fBrush = new SolidBrush(ForeColor))
                    {
                        //
                        // Normal image
                        //
                        Bitmap imgN = new Bitmap(w, h);
                        using (Graphics g = Graphics.FromImage(imgN))
                        {
                            g.CompositingQuality = CompositingQuality.HighQuality;
                            g.DrawLine(SystemPens.ButtonShadow, new Point(1, 1), new Point(1, h - 2));
                            g.DrawLine(SystemPens.ButtonFace, new Point(2, 1), new Point(2, h));
                            g.FillPolygon(fBrush, new[] { new Point(mw - 2, mh - 1), new Point(mw + 3, mh - 1), new Point(mw, mh + 2) });
                        }
                        _defaultSplitImages.Images.Add(_normalImage, imgN);

                        //
                        // Hover image
                        //
                        Bitmap imgH = new Bitmap(w, h);
                        using (Graphics g = Graphics.FromImage(imgH))
                        {
                            g.CompositingQuality = CompositingQuality.HighQuality;
                            g.DrawLine(SystemPens.ButtonShadow, new Point(1, 1), new Point(1, h - 2));
                            g.DrawLine(SystemPens.ButtonFace, new Point(2, 1), new Point(2, h));
                            g.FillPolygon(fBrush, new[] { new Point(mw - 3, mh - 2), new Point(mw + 4, mh - 2), new Point(mw, mh + 2) });
                        }
                        _defaultSplitImages.Images.Add(_hoverImage, imgH);

                        //
                        // Clicked image
                        //
                        Bitmap imgC = new Bitmap(w, h);
                        using (Graphics g = Graphics.FromImage(imgC))
                        {
                            g.CompositingQuality = CompositingQuality.HighQuality;
                            g.DrawLine(SystemPens.ButtonShadow, new Point(1, 1), new Point(1, h - 2));
                            g.DrawLine(SystemPens.ButtonFace, new Point(2, 1), new Point(2, h));
                            g.FillPolygon(fBrush, new[] { new Point(mw - 2, mh - 1), new Point(mw + 3, mh - 1), new Point(mw, mh + 2) });
                        }
                        _defaultSplitImages.Images.Add(_clickedImage, imgC);

                        //
                        // Focused image
                        //
                        Bitmap imgF = new Bitmap(w, h);
                        using (Graphics g = Graphics.FromImage(imgF))
                        {
                            g.CompositingQuality = CompositingQuality.HighQuality;
                            g.DrawLine(SystemPens.ButtonShadow, new Point(1, 1), new Point(1, h - 2));
                            g.DrawLine(SystemPens.ButtonFace, new Point(2, 1), new Point(2, h));
                            g.FillPolygon(fBrush, new[] { new Point(mw - 2, mh - 1), new Point(mw + 3, mh - 1), new Point(mw, mh + 2) });
                        }
                        _defaultSplitImages.Images.Add(_focusedImage, imgF);
                    }

                    //
                    // Disabled image.  Gets a different brush
                    //
                    using (SolidBrush sBrush = new SolidBrush(SystemColors.GrayText))
                    {
                        Bitmap imgD = new Bitmap(w, h);
                        using (Graphics g = Graphics.FromImage(imgD))
                        {
                            g.CompositingQuality = CompositingQuality.HighQuality;
                            g.DrawLine(SystemPens.GrayText, new Point(1, 1), new Point(1, h - 2));
                            g.FillPolygon(sBrush, new[] { new Point(mw - 2, mh - 1), new Point(mw + 3, mh - 1), new Point(mw, mh + 2) });
                        }
                        _defaultSplitImages.Images.Add(_disabledImage, imgD);
                    }
                }
                catch (Exception ex)
                {
                    // eat up the exception
                    Trace.WriteLine(string.Format("Exception in InitDefaultSplitImages(refresh:={0}).  Exception = {1}", refresh, ex.ToString()));
                }
            }
        }

        private void SetSplitImage(string imageName)
        {
            if (imageName != null && ImageList != null && ImageList.Images.ContainsKey(imageName))
            {
                ImageKey = imageName;
            }
        }

        private bool IsMouseInSplit()
        {
            Rectangle splitRect = GetImageRect(_normalImage);

            if (!_calculateSplitRect)
            {
                splitRect.Width = _splitWidth;
                splitRect.Height = _splitHeight;
            }

            return splitRect.Contains(PointToClient(MousePosition));
        }

        private Rectangle GetImageRect(string imageKey)
        {
            Image currImg = GetImage(imageKey);
            if (currImg == null)
                return Rectangle.Empty;

            int x = 0;						// Composing the return rectangle
            int y = 0;						// Composing the return rectagle
            int w = currImg.Width + 1;
            int h = currImg.Height + 1;

            if (w > Width)
                w = Width;

            if (h > Width)
                h = Width;

            switch (ImageAlign)
            {
                //
                //	Top alignment
                //
                case ContentAlignment.TopLeft:
                    x = 0;
                    y = 0;
                    break;

                case ContentAlignment.TopCenter:
                    x = (Width - w) / 2;
                    y = 0;
                    x += (Width - w) % 2;
                    break;

                case ContentAlignment.TopRight:
                    x = Width - w;
                    y = 0;
                    break;

                //
                // Middle alignment
                //
                case ContentAlignment.MiddleLeft:
                    x = 0;
                    y = (Height - h) / 2;
                    y += (Height - h) % 2;
                    break;

                case ContentAlignment.MiddleCenter:
                    x = (Width - w) / 2;
                    x += (Width - w) % 2;
                    y = (Height - h) / 2;
                    y += (Height - h) % 2;
                    break;

                case ContentAlignment.MiddleRight:
                    x = Width - w;
                    y = (Height - h) / 2;
                    y += (Height - h) % 2;
                    break;

                //
                // Bottom
                //
                case ContentAlignment.BottomLeft:
                    x = 0;
                    y = Height - h;
                    y += (Height - h) % 2;
                    break;

                case ContentAlignment.BottomCenter:
                    x = (Width - w) / 2;
                    x += (Width - w) % 2;
                    y = Height - h;
                    break;

                case ContentAlignment.BottomRight:
                    x = Width - w;
                    y = Height - h;
                    break;
            }

            if (_fillSplitHeight && h < Height)
                h = Height;

            if (x > 0)
                x -= 1;

            if (y > 0)
                y -= 1;

            return new Rectangle(x, y, w, h);
        }

        private Image GetImage(string imageName)
        {
            if (ImageList != null && ImageList.Images.ContainsKey(imageName))
            {
                return ImageList.Images[imageName];
            }
            return null;
        }

        /// <summary>
        /// Notice the NoInlining decoration of the method as a mechanism for
        /// preventing the optimized compiler from optimizing this call away.
        /// See article for further discussion.
        /// </summary>
        /// <param name="evntHndlr"></param>
        /// <param name="ea"></param>
        [MethodImpl(MethodImplOptions.NoInlining)]
        private void EventFire(EventHandler evntHndlr, EventArgs ea)
        {
            if (evntHndlr == null)
                return;

            int i = 0;
            foreach (Delegate del in evntHndlr.GetInvocationList())
            {
                try
                {
                    ISynchronizeInvoke syncr = del.Target as ISynchronizeInvoke;
                    if (syncr == null)
                    {
                        evntHndlr.DynamicInvoke(new object[] { this, ea });
                    }
                    else if (syncr.InvokeRequired)
                    {
                        syncr.Invoke(evntHndlr, new object[] { this, ea });
                    }
                    else
                    {
                        evntHndlr.DynamicInvoke(new object[] { this, ea });
                    }
                }
                catch (Exception ex)
                {
                    //
                    // Eat the exception
                    //
                    Trace.WriteLine(string.Format("SplitButton failed delegate call {0}.  Exception {1}", i, ex.ToString()));
                }

                ++i;
            }
        }

        private bool IsTooSoonAfterCloseMenuDropDown()
        {
            const int tooCloseInMilliseconds = 300;

            //
            //	The drop-down is open or no timer started therefore
            // we are not too close to the Close of the drop-down
            // menu.
            //
            if (_closedTime == ZeroTime)
                return false;

            if (DateTime.Now.Subtract(_closedTime).Milliseconds < tooCloseInMilliseconds)
            {
                return true;
            }

            return false;
        }

        #endregion Helper Methods

        #region Properties Exposing States

        /// <summary>
        ///
        /// </summary>
        [Category("Split Button")]
        [Description("Indicates whether the split rectangle must be calculated (basing on Split image size)")]
        [DefaultValue(true)]
        public bool CalculateSplitRect
        {
            get { return _calculateSplitRect; }
            set
            {
                bool flag1 = _calculateSplitRect;
                _calculateSplitRect = value;

                if (flag1 != _calculateSplitRect)
                {
                    if (_splitWidth > 0 && _splitHeight > 0)
                    {
                        InitDefaultSplitImages(true);
                    }
                }
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether [fill split height].
        /// </summary>
        /// <value><c>true</c> if [fill split height]; otherwise, <c>false</c>.</value>
        [Category("Split Button")]
        [Description("Indicates whether the split height must be filled to the button height even if the split image height is lower.")]
        [DefaultValue(true)]
        public bool FillSplitHeight
        {
            get { return _fillSplitHeight; }
            set { _fillSplitHeight = value; }
        }

        /// <summary>
        ///
        /// </summary>
        [Category("Split Button")]
        [Description("The split height (ignored if CalculateSplitRect is setted to true).")]
        [DefaultValue(0)]
        public int SplitHeight
        {
            get { return _splitHeight; }
            set
            {
                _splitHeight = value;

                if (!_calculateSplitRect)
                {
                    if (_splitWidth > 0 && _splitHeight > 0)
                    {
                        InitDefaultSplitImages(true);
                    }
                }
            }
        }

        /// <summary>
        ///
        /// </summary>
        [Category("Split Button")]
        [Description("The split width (ignored if CalculateSplitRect is setted to true).")]
        [DefaultValue(0)]
        public int SplitWidth
        {
            get { return _splitWidth; }
            set
            {
                _splitWidth = value;

                if (!_calculateSplitRect)
                {
                    if (_splitWidth > 0 && _splitHeight > 0)
                    {
                        InitDefaultSplitImages(true);
                    }
                }
            }
        }

        /// <summary>
        ///
        /// </summary>
        [Category("Split Button Images")]
        [Description("The Normal status image name in the ImageList, corresponding to the image name.")]
        [DefaultValue("Normal")]
        [Editor("System.Windows.Forms.Design.ImageIndexEditor, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor))]
        [Localizable(true)]
        [RefreshProperties(RefreshProperties.Repaint)]
        [TypeConverter(typeof(ImageKeyConverter))]
        public string NormalImage
        {
            get { return _normalImage; }
            set { _normalImage = value; }
        }

        /// <summary>
        ///
        /// </summary>
        [Category("Split Button Images")]
        [Description("The Hover status image name in the ImageList.")]
        [DefaultValue("Hover")]
        [Editor("System.Windows.Forms.Design.ImageIndexEditor, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor))]
        [Localizable(true)]
        [RefreshProperties(RefreshProperties.Repaint)]
        [TypeConverter(typeof(ImageKeyConverter))]
        public string HoverImage
        {
            get { return _hoverImage; }
            set { _hoverImage = value; }
        }

        /// <summary>
        ///
        /// </summary>
        [Category("Split Button Images")]
        [Description("The Clicked status image name in the ImageList.")]
        [DefaultValue("Clicked")]
        [Editor("System.Windows.Forms.Design.ImageIndexEditor, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor))]
        [Localizable(true)]
        [RefreshProperties(RefreshProperties.Repaint)]
        [TypeConverter(typeof(ImageKeyConverter))]
        public string ClickedImage
        {
            get { return _clickedImage; }
            set { _clickedImage = value; }
        }

        /// <summary>
        ///
        /// </summary>
        [Category("Split Button Images")]
        [Description("The Disabled status image name in the ImageList.")]
        [DefaultValue("Disabled")]
        [Editor("System.Windows.Forms.Design.ImageIndexEditor, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor))]
        [Localizable(true)]
        [RefreshProperties(RefreshProperties.Repaint)]
        [TypeConverter(typeof(ImageKeyConverter))]
        public string DisabledImage
        {
            get { return _disabledImage; }
            set { _disabledImage = value; }
        }

        /// <summary>
        ///
        /// </summary>
        [Category("Split Button Images")]
        [Description("The Focused status image name in the ImageList.")]
        [DefaultValue("Hover")]
        [Editor("System.Windows.Forms.Design.ImageIndexEditor, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor))]
        [Localizable(true)]
        [RefreshProperties(RefreshProperties.Repaint)]
        [TypeConverter(typeof(ImageKeyConverter))]
        public string FocusedImage
        {
            get { return _focusedImage; }
            set { _focusedImage = value; }
        }

        #endregion Properties Exposing States

        #region Overridable Methods

        /// <summary>
        ///
        /// </summary>
        protected override void OnCreateControl()
        {
            InitDefaultSplitImages();

            if (ImageList == null)
                ImageList = _defaultSplitImages;

            SetSplitImage(Enabled ? _normalImage : _disabledImage);

            base.OnCreateControl();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="mevent"></param>
        protected override void OnMouseMove(MouseEventArgs mevent)
        {
            if (Enabled)
            {
                SetSplitImage(IsMouseInSplit() ? _hoverImage : _normalImage);
            }

            base.OnMouseMove(mevent);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMouseLeave(EventArgs e)
        {
            if (Enabled)
                SetSplitImage(_normalImage);

            base.OnMouseLeave(e);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="mevent"></param>
        protected override void OnMouseDown(MouseEventArgs mevent)
        {
            if (Enabled)
            {
                SetSplitImage(IsMouseInSplit() ? _clickedImage : _normalImage);
            }

            base.OnMouseDown(mevent);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="mevent"></param>
        protected override void OnMouseUp(MouseEventArgs mevent)
        {
            if (Enabled)
            {
                if (IsMouseInSplit())
                {
                    SetSplitImage(_hoverImage);

                    if (ContextMenuStrip != null && ContextMenuStrip.Items.Count > 0)
                    {
                        if (!IsTooSoonAfterCloseMenuDropDown())
                            ContextMenuStrip.Show(this, new Point(0, Height));
                    }
                }
                else
                {
                    SetSplitImage(_normalImage);
                }
            }

            base.OnMouseUp(mevent);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="e"></param>
        protected override void OnEnabledChanged(EventArgs e)
        {
            if (Enabled)
            {
                SetSplitImage(IsMouseInSplit() ? _hoverImage : _normalImage);
            }
            else
            {
                SetSplitImage(_disabledImage);
            }

            base.OnEnabledChanged(e);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="e"></param>
        protected override void OnGotFocus(EventArgs e)
        {
            if (Enabled)
                SetSplitImage(_focusedImage);

            base.OnGotFocus(e);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="e"></param>
        protected override void OnLostFocus(EventArgs e)
        {
            if (Enabled)
                SetSplitImage(_normalImage);

            base.OnLostFocus(e);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="e"></param>
        protected override void OnClick(EventArgs e)
        {
            base.OnClick(e);

            if (!IsMouseInSplit())
            {
                EventFire(ButtonClick, e);
            }
        }

        #endregion Overridable Methods

        #region Additional Interface Methods

        /// <summary>
        ///
        /// </summary>
        public void ClearDropDownItems()
        {
            SplitButtonDropDown.Items.Clear();
            _dropDownsEventHandlers = new Dictionary<string, EventHandler>();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="text"></param>
        /// <param name="handler"></param>
        public void AddDropDownItemAndHandle(string text, EventHandler handler)
        {
            SplitButtonDropDown.Items.Add(text);

            if (!_dropDownsEventHandlers.ContainsKey(text))
                _dropDownsEventHandlers.Add(text, handler);
        }

        #endregion Additional Interface Methods

        #region Internal Events Handling

        /// <summary>
        ///
        /// </summary>
        /// <param name="index"></param>
        public void SetClickItem(int index)
        {
            if (!_persistDropDownName || SplitButtonDropDown == null) return;
            if (index <= -1 || index >= SplitButtonDropDown.Items.Count) return;
            var item = SplitButtonDropDown.Items[index];
            if (item == null) return;
            string textDisplay = item.Text;
            EventHandler adaptorEvent = _dropDownsEventHandlers[textDisplay];
            if (adaptorEvent == null) return;
            ButtonClick = adaptorEvent;
            Text = textDisplay;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="text"></param>
        public void SetClickItem(string text)
        {
            if (!_persistDropDownName || SplitButtonDropDown == null) return;
            if (string.IsNullOrEmpty(text)) return;
            var item = SplitButtonDropDown.Items[text];
            if (item == null) return;
            string textDisplay = item.Text;
            EventHandler adaptorEvent = _dropDownsEventHandlers[textDisplay];
            if (adaptorEvent == null) return;
            ButtonClick = adaptorEvent;
            Text = textDisplay;
        }

        private void SplitButtonDropDownItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            SplitButtonDropDown.Close();

            string textDisplay = e.ClickedItem.Text;
            EventHandler adaptorEvent = _dropDownsEventHandlers[textDisplay];

            EventFire(adaptorEvent, EventArgs.Empty);

            if (_persistDropDownName)
            {
                Text = textDisplay;
                ButtonClick = adaptorEvent;
            }
        }

        /// <summary>
        /// Set the time when the menu drop-down was closed.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SplitButtonDropDownClosed(object sender, ToolStripDropDownClosedEventArgs e)
        {
            // Start timer
            _closedTime = DateTime.Now;
        }

        /// <summary>
        /// Clear the time when the menu drop-down was closed.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SplitButtonDropDownOpening(object sender, CancelEventArgs e)
        {
            if (IsTooSoonAfterCloseMenuDropDown())
            {
            }

            _closedTime = ZeroTime;
        }

        #endregion Internal Events Handling
    }
}