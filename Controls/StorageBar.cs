using System;
using System.Drawing;
using System.Windows.Forms;

namespace Ringtone2iPhone.Controls
{
    class StorageBar : Control
    {
        private static readonly Pen BarPen = Pens.Gray;
        private static readonly Brush BarBrush = Brushes.Green;
        //private static readonly StringFormat StringFormat = new StringFormat { LineAlignment = StringAlignment.Center };

        public long FreeBytes { get; set; }
        public long TotalBytes { get; set; }
        private Rectangle rectangle;

        protected override void OnResize(EventArgs e)
        {
            rectangle.X = Padding.Left;
            rectangle.Y = Padding.Top;
            rectangle.Width = Width - Padding.Horizontal - 1;
            rectangle.Height = Height - Padding.Vertical - 1;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            e.Graphics.Clear(BackColor);
            if (TotalBytes > 0)
            {
                var width = Convert.ToInt32(Width * (TotalBytes - FreeBytes) / TotalBytes);
                e.Graphics.FillRectangle(BarBrush, rectangle.X, rectangle.Y, width, rectangle.Height);
            }
            e.Graphics.DrawRectangle(BarPen, rectangle);
        }
    }
}
