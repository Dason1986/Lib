using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Windows.Forms;

namespace Library.Controls
{
    /// <summary>
    ///
    /// </summary>
    public class DataGridViewColumnSelector
    {
        // the DataGridView to which the DataGridViewColumnSelector is attached
        private DataGridView mDataGridView = null;

        // a CheckedListBox containing the column header text and checkboxes
        ///       private CheckedListBox mCheckedListBox;
        // a ToolStripDropDown object used to show the popup
        private ToolStripDropDown mPopup;

        /// <summary>
        /// The max height of the popup
        /// </summary>
        public int MaxHeight = 300;

        /// <summary>
        /// The width of the popup
        /// </summary>
        public int Width = 200;

        /// <summary>
        /// Gets or sets the DataGridView to which the DataGridViewColumnSelector is attached
        /// </summary>
        public DataGridView DataGridView
        {
            get { return mDataGridView; }
            set
            {
                // If any, remove handler from current DataGridView
                if (mDataGridView != null) mDataGridView.CellMouseClick -= new DataGridViewCellMouseEventHandler(mDataGridView_CellMouseClick);
                // Set the new DataGridView
                mDataGridView = value;
                // Attach CellMouseClick handler to DataGridView
                if (mDataGridView != null) mDataGridView.CellMouseClick += new DataGridViewCellMouseEventHandler(mDataGridView_CellMouseClick);
            }
        }

        /// <summary>
        ///
        /// </summary>
        public bool IsLiftButton { get; set; }

        // When user right-clicks the cell origin, it clears and fill the CheckedListBox with
        // columns header text. Then it shows the popup.
        // In this way the CheckedListBox items are always refreshed to reflect changes occurred in
        // DataGridView columns (column additions or name changes and so on).
        private void mDataGridView_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if ((IsLiftButton || e.Button != MouseButtons.Right) && (!IsLiftButton || e.Button != MouseButtons.Left))
                return;
            if (e.RowIndex != -1 || e.ColumnIndex != -1) return;
            pUserControl1.Initialize(mDataGridView);
            mPopup.Show(mDataGridView.PointToScreen(new Point(e.X, e.Y)));
        }

        private DataGridViewMenu pUserControl1 = new DataGridViewMenu();

        // The constructor creates an instance of CheckedListBox and ToolStripDropDown.
        // the CheckedListBox is hosted by ToolStripControlHost, which in turn is
        // added to ToolStripDropDown.
        /// <summary>
        /// /
        /// </summary>
        public DataGridViewColumnSelector()
        {
            //mCheckedListBox = new CheckedListBox();
            //mCheckedListBox.CheckOnClick = true;
            //mCheckedListBox.ItemCheck += new ItemCheckEventHandler(mCheckedListBox_ItemCheck);

            //ToolStripControlHost mControlHost = new ToolStripControlHost(mCheckedListBox);
            pUserControl1.DoneEvent += new EventHandler(OnDone);
            pUserControl1.CheckedChangedEnent += new DataGridViewMenu.CheckedChanged(CheckedChangedEnent);
            ToolStripControlHost mControlHost = new ToolStripControlHost(pUserControl1);
            mControlHost.Padding = Padding.Empty;
            mControlHost.Margin = Padding.Empty;
            mControlHost.AutoSize = false;

            mPopup = new ToolStripDropDown();
            mPopup.Padding = Padding.Empty;
            mPopup.AutoClose = true;
            mPopup.Items.Add(mControlHost);
        }

        private void CheckedChangedEnent(int iIndex, bool bChecked)
        {
            mDataGridView.Columns[iIndex].Visible = bChecked;
        }

        private void OnDone(object sender, EventArgs e)
        {
            mPopup.AutoClose = false;
            mPopup.Close();
            mPopup.AutoClose = true;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="dgv"></param>
        public DataGridViewColumnSelector(DataGridView dgv)
            : this()
        {
            this.DataGridView = dgv;
        }

        // When user checks / unchecks a checkbox, the related column visibility is
        // switched.
        private void mCheckedListBox_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            mDataGridView.Columns[e.Index].Visible = (e.NewValue == CheckState.Checked);
        }
    }

    internal class MenuControl
    {
        private static int m_iImageColumnWidth = 24;
        private static int m_iExtraWidth = 15;

        private class MenuCommand
        {
            public int Height { get { return Separator ? 5 : 21; } }
            public bool Separator { get { return m_csText == "-"; } }

            private string m_csText;

            //private int m_iIndex;
            private bool m_bChecked;

            private bool m_bDone;

            /// <summary>
            ///
            /// </summary>
            public string Text { get { return m_csText; } }

            //public int Index { get { return m_iIndex; } }
            /// <summary>
            ///
            /// </summary>
            public bool Done { get { return m_bDone; } }

            /// <summary>
            ///
            /// </summary>
            public bool Checked { get { return m_bChecked; } set { m_bChecked = value; } }

            //public MenuCommand(string csText, int iIndex, bool bChecked)
            /// <summary>
            ///
            /// </summary>
            /// <param name="csText"></param>
            /// <param name="bChecked"></param>
            public MenuCommand(string csText, bool bChecked)
                : this(csText, bChecked, false)
            {
            }

            /// <summary>
            ///
            /// </summary>
            /// <param name="csText"></param>
            public MenuCommand(string csText)
                : this(csText, false, false)
            {
            }

            /// <summary>
            ///
            /// </summary>
            /// <param name="csText"></param>
            /// <param name="bChecked"></param>
            /// <param name="bDone"></param>
            public MenuCommand(string csText, bool bChecked, bool bDone)
            {
                m_csText = csText;
                //m_iIndex = iIndex;
                m_bChecked = bChecked;
                m_bDone = bDone;
            }
        }

        private MenuCommand m_pTracMenuItem = null;
        private List<MenuCommand> m_pMenuCommands = new List<MenuCommand>();

        private Bitmap m_pMemBitmap;// = new Bitmap(panel1.Width, panel1.Height, PixelFormat.Format32bppArgb);
        private Graphics m_pMemGraphics;

        /// <summary>
        ///
        /// </summary>
        public int Width { get { return m_pMemBitmap.Width; } }

        /// <summary>
        ///
        /// </summary>
        public int Height { get { return m_pMemBitmap.Height; } }

        /// <summary>
        ///
        /// </summary>
        public bool Done
        {
            get
            {
                return m_pTracMenuItem != null && m_pTracMenuItem.Done;
            }
        }

        /// <summary>
        ///
        /// </summary>
        public int HitIndex
        {
            get
            {
                return m_pMenuCommands.IndexOf(m_pTracMenuItem);
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="iIndex"></param>
        /// <param name="g"></param>
        /// <returns></returns>
        public bool ChangeChecked(int iIndex, Graphics g)
        {
            m_pMenuCommands[iIndex].Checked = !m_pMenuCommands[iIndex].Checked;
            Draw(g);
            return m_pMenuCommands[iIndex].Checked;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="csText"></param>
        /// <param name="bChecked"></param>
        public void Add(string csText, bool bChecked)
        {
            m_pMenuCommands.Add(new MenuCommand(csText, bChecked));
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="g"></param>
        public void Prepare(Graphics g)
        {
            m_pMenuCommands.Add(new MenuCommand("-"));
            MenuCommand pDone = new MenuCommand("确定", false, true);
            m_pMenuCommands.Add(pDone);

            int iHeight = 4; //(2 + 2 top + bottom);
            float fWidth = 0;
            foreach (MenuCommand pMenuCommand in m_pMenuCommands)
            {
                iHeight += pMenuCommand.Height;
                SizeF pSizeF = g.MeasureString(pMenuCommand.Text, SystemInformation.MenuFont);
                fWidth = Math.Max(fWidth, pSizeF.Width);
            }
            int iWidth = (int)fWidth + m_iImageColumnWidth + m_iExtraWidth;

            m_pMemBitmap = new Bitmap(iWidth, iHeight);
            m_pMemGraphics = Graphics.FromImage(m_pMemBitmap);
        }

        private MenuCommand HitTest(int X, int Y)
        {
            if (X < 0 || X > Width || Y < 0 || Y > Height)
            {
                return null;
            }

            int iHeight = 2;
            foreach (MenuCommand pMenuCommand in m_pMenuCommands)
            {
                if (Y > iHeight && Y < iHeight + pMenuCommand.Height)
                {
                    return pMenuCommand.Separator ? null : pMenuCommand;
                }
                iHeight += pMenuCommand.Height;
            }
            return null;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="X"></param>
        /// <param name="Y"></param>
        /// <returns></returns>
        public bool HitTestMouseMove(int X, int Y)
        {
            MenuCommand pMenuCommand = HitTest(X, Y);
            if (pMenuCommand != m_pTracMenuItem)
            {
                m_pTracMenuItem = pMenuCommand;
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="X"></param>
        /// <param name="Y"></param>
        /// <returns></returns>
        public bool HitTestMouseDown(int X, int Y)
        {
            MenuCommand pMenuCommand = HitTest(X, Y);
            return pMenuCommand != null;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="g"></param>
        public void Draw(Graphics g)
        {
            Rectangle area = new Rectangle(0, 0, m_pMemBitmap.Width, m_pMemBitmap.Height);

            m_pMemGraphics.Clear(SystemColors.Control);

            // Draw the background area
            DrawBackground(m_pMemGraphics, area);

            // Draw the actual menu items
            DrawAllCommands(m_pMemGraphics);

            g.DrawImage(m_pMemBitmap, area, area, GraphicsUnit.Pixel);
        }

        private void DrawBackground(Graphics g, Rectangle rectWin)
        {
            Rectangle main = new Rectangle(0, 0, rectWin.Width, rectWin.Height);

            int xStart = 1;
            int yStart = 2;
            int yHeight = main.Height - yStart - 1;

            // Paint the main area background
            using (Brush backBrush = new SolidBrush(Color.FromArgb(249, 248, 247)))
                g.FillRectangle(backBrush, main);

            // Draw single line border around the main area
            using (Pen mainBorder = new Pen(Color.FromArgb(102, 102, 102)))
                g.DrawRectangle(mainBorder, main);

            Rectangle imageRect = new Rectangle(xStart, yStart, m_iImageColumnWidth, yHeight);

            // Draw the first image column
            using (Brush openBrush = new LinearGradientBrush(imageRect, Color.FromArgb(248, 247, 246), Color.FromArgb(215, 211, 204), 0f))
                g.FillRectangle(openBrush, imageRect);

            // Draw shadow around borders
            int rightLeft = main.Right + 1;
            int rightTop = main.Top + 4;
            int rightBottom = main.Bottom + 1;
            int leftLeft = main.Left + 4;
            int xExcludeStart = main.Left;
            int xExcludeEnd = main.Left;
        }

        private void DrawAllCommands(Graphics g)
        {
            int iTop = 2;
            foreach (MenuCommand pMenuCommand in m_pMenuCommands)
            {
                DrawSingleCommand(g, ref iTop, pMenuCommand, pMenuCommand == m_pTracMenuItem);
            }
        }

        private void DrawSingleCommand(Graphics g, ref int iTop, MenuCommand pMenuCommand, bool hotCommand)
        {
            int iHeight = pMenuCommand.Height;
            Rectangle drawRect = new Rectangle(1, iTop, Width, iHeight);
            iTop += iHeight;

            // Remember some often used values
            int textGapLeft = 4;
            int imageLeft = 4;

            // Calculate some common values
            int imageColWidth = 24;

            // Is this item a separator?
            if (pMenuCommand.Separator)
            {
                // Draw the image column background
                Rectangle imageCol = new Rectangle(drawRect.Left, drawRect.Top, imageColWidth, drawRect.Height);

                // Draw the image column
                using (Brush openBrush = new LinearGradientBrush(imageCol, Color.FromArgb(248, 247, 246), Color.FromArgb(215, 211, 204), 0f))
                    g.FillRectangle(openBrush, imageCol);

                // Draw a separator
                using (Pen separatorPen = new Pen(Color.FromArgb(166, 166, 166)))
                {
                    // Draw the separator as a single line
                    g.DrawLine(separatorPen,
                               drawRect.Left + imageColWidth + textGapLeft, drawRect.Top + 2,
                               drawRect.Right - 7,
                               drawRect.Top + 2);
                }
            }
            else
            {
                int leftPos = drawRect.Left + imageColWidth + textGapLeft;

                // Should the command be drawn selected?
                if (hotCommand)
                {
                    Rectangle selectArea = new Rectangle(drawRect.Left + 1, drawRect.Top, drawRect.Width - 9, drawRect.Height - 1);

                    using (SolidBrush selectBrush = new SolidBrush(ColorTable.QQHighLightColor))
                        g.FillRectangle(selectBrush, selectArea);

                    using (Pen selectPen = new Pen(ColorTable.QQBorderColor))
                        g.DrawRectangle(selectPen, selectArea);
                }
                else
                {
                    Rectangle imageCol = new Rectangle(drawRect.Left, drawRect.Top, imageColWidth, drawRect.Height);

                    // Paint the main background color
                    using (Brush backBrush = new SolidBrush(Color.FromArgb(249, 248, 247)))
                        g.FillRectangle(backBrush, new Rectangle(drawRect.Left + 1, drawRect.Top, drawRect.Width - 9, drawRect.Height));

                    using (Brush openBrush = new LinearGradientBrush(imageCol, Color.FromArgb(248, 247, 246), Color.FromArgb(215, 211, 204), 0f))
                        g.FillRectangle(openBrush, imageCol);
                }

                // Calculate text drawing rectangle
                Rectangle strRect = new Rectangle(
                    leftPos,
                    drawRect.Top,
                    Width - imageColWidth - textGapLeft - 5,
                    drawRect.Height);

                // Left align the text drawing on a single line centered vertically
                // and process the & character to be shown as an underscore on next character
                StringFormat format = new StringFormat();
                format.FormatFlags = StringFormatFlags.NoClip | StringFormatFlags.NoWrap;
                format.Alignment = StringAlignment.Near;
                format.LineAlignment = StringAlignment.Center;
                format.HotkeyPrefix = HotkeyPrefix.Show;

                SolidBrush textBrush = new SolidBrush(SystemColors.MenuText);
                g.DrawString(pMenuCommand.Text, SystemInformation.MenuFont, textBrush, strRect, format);

                // The image offset from top of cell is half the space left after
                // subtracting the height of the image from the cell height
                int imageTop = drawRect.Top + (drawRect.Height - 16) / 2;

                // Should a check mark be drawn?
                if (pMenuCommand.Checked)
                {
                    Pen boxPen = new Pen(Color.FromArgb(10, 36, 106));
                    Brush boxBrush;

                    if (hotCommand)
                        boxBrush = new SolidBrush(Color.FromArgb(133, 146, 181));
                    else
                        boxBrush = new SolidBrush(Color.FromArgb(212, 213, 216));

                    Rectangle boxRect = new Rectangle(imageLeft - 1, imageTop - 1, 16 + 2, 16 + 2);

                    // Fill the checkbox area very slightly
                    g.FillRectangle(boxBrush, boxRect);

                    // Draw the box around the checkmark area
                    g.DrawRectangle(boxPen, boxRect);

                    boxPen.Dispose();
                    boxBrush.Dispose();

                    g.DrawImage(_checkImg, boxRect, 0, 0, _checkImg.Width, _checkImg.Height, GraphicsUnit.Pixel);
                }
            }
        }

        private Image _checkImg = RenderHelper.GetImageFormResourceStream("Library.Win.Controls.Standard.Image.check.png");
    }

    internal class DataGridViewMenu : UserControl
    {
        public EventHandler DoneEvent;

        public delegate void CheckedChanged(int iIndex, bool bChecked);

        public event CheckedChanged CheckedChangedEnent;

        public virtual void OnCheckedChanged(int iIndex, bool bChecked)
        {
            if (CheckedChangedEnent != null)
                CheckedChangedEnent(iIndex, bChecked);
        }

        public virtual void OnDone()
        {
            if (DoneEvent != null)
                DoneEvent(this, EventArgs.Empty);
        }

        private MenuControl m_pMenuControl = new MenuControl();
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.SuspendLayout();
            //
            // timer1
            //
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            //
            // UserControlMenu
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Name = "UserControlMenu";
            this.Size = new System.Drawing.Size(150, 222);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.UserControlMenu_Paint);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.UserControlMenu_MouseMove);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.UserControlMenu_MouseDown);
            this.ResumeLayout(false);
        }

        #endregion Component Designer generated code

        private System.Windows.Forms.Timer timer1;

        /// <summary>
        ///
        /// </summary>
        public DataGridViewMenu()
        {
            InitializeComponent();
        }

        private void buttonDone_Click(object sender, EventArgs e)
        {
            Parent.Focus();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="pDataGridView"></param>
        public void Initialize(DataGridView pDataGridView)
        {
            m_pMenuControl = new MenuControl();

            foreach (DataGridViewColumn c in pDataGridView.Columns)
            {
                m_pMenuControl.Add(c.HeaderText, c.Visible);
            }

            m_pMenuControl.Prepare(CreateGraphics());

            Width = m_pMenuControl.Width;
            Height = m_pMenuControl.Height;

            timer1.Enabled = true;
        }

        private void UserControlMenu_Paint(object sender, PaintEventArgs e)
        {
            m_pMenuControl.Draw(e.Graphics);
        }

        private void UserControlMenu_MouseMove(object sender, MouseEventArgs e)
        {
            if (!m_pMenuControl.HitTestMouseMove(e.X, e.Y)) return;
            m_pMenuControl.Draw(CreateGraphics());
        }

        private void UserControlMenu_MouseDown(object sender, MouseEventArgs e)
        {
            if (!m_pMenuControl.HitTestMouseDown(e.X, e.Y)) return;
            if (m_pMenuControl.Done)
            {
                OnDone();
            }
            else
            {
                int iHitIndex = m_pMenuControl.HitIndex;
                if (iHitIndex == -1) return;
                bool bChecked = m_pMenuControl.ChangeChecked(iHitIndex, CreateGraphics());
                OnCheckedChanged(iHitIndex, bChecked);
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            Point pPoint = PointToClient(Cursor.Position);
            if (m_pMenuControl.HitTestMouseMove(pPoint.X, pPoint.Y))
            {
                m_pMenuControl.Draw(CreateGraphics());
            }
        }
    }
}