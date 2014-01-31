using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using love_widget.Properties;

namespace love_widget
{
    public partial class MainForm : Form
    {
        private Point _MouseOffset;
        private bool _IsMouseDown = false;

        public MainForm()
        {
            InitializeComponent();
        }

        private GraphicsPath GetWindowRegion(Bitmap bitmap)
        {
            Color TempColor;
            GraphicsPath gp = new GraphicsPath();
            if (bitmap == null) return null;

            for (int nX = 0; nX < bitmap.Width; nX++)
            {
                for (int nY = 0; nY < bitmap.Height; nY++)
                {
                    TempColor = bitmap.GetPixel(nX, nY);
                    if (TempColor.A == 255)
                    {
                        gp.AddRectangle(new Rectangle(nX, nY, 1, 1));
                    }
                }
            }
            return gp;
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            Bitmap bitmap = new Bitmap(this.BackgroundImage);
            this.Region = new Region(GetWindowRegion(bitmap));
            this.ShowInTaskbar = false;
            this.notifyIcon.Visible = true;
            this.notifyIcon.ShowBalloonTip(1000,
                this.notifyIcon.BalloonTipTitle,
                this.notifyIcon.BalloonTipText,
                ToolTipIcon.Info);
        }   

        private void MainForm_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            int xOffset;
            int yOffset;
            if (e.Button == MouseButtons.Left)
            {
                xOffset = -e.X;
                yOffset = -e.Y;
                this._MouseOffset = new Point(xOffset, yOffset);
                this._IsMouseDown = true;
            }
        }

        private void MainForm_MouseMove(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (this._IsMouseDown)
            {
                Point mousePos = Control.MousePosition;
                mousePos.Offset(this._MouseOffset.X, this._MouseOffset.Y);
                this.Location = mousePos;
            }
        }

        private void MainForm_MouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                this._IsMouseDown = false;
            }
        }

        private void MainForm_MouseDoubleClick(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            this.mainTooltip.Hide(this);
            this.mainTooltip.UseFading = true;
            this.mainTooltip.IsBalloon = true;
            this.mainTooltip.Show("请输入内容再登陆", this, 25, -30, 5000);
            Net net = new Net(this);
            net.UploadData("hehe");
        }

        private void MainForm_FormClosed(object sender, System.Windows.Forms.FormClosedEventArgs e)
        {
            this.notifyIcon.Visible = false;
            Settings.Default.Save();
        }

        private void 退出ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
