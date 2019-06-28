using System;
using System.Drawing;
using System.Windows.Forms;

namespace Ringtone2iPhone.Controls
{
    public class AudioBar : Control
    {
        private static readonly Pen BarPen = Pens.Gray;
        private static readonly Brush BarBrush = Brushes.Red;
        private static readonly Pen SeekPen = Pens.DarkGray;
        private static readonly Brush TextBrush = Brushes.Black;
        private static readonly Brush SeekBrush = new SolidBrush(Color.FromArgb(60, Color.Black));
        private static readonly Pen CutNormalPen = new Pen(Color.DimGray, 1);
        private static readonly Pen CutHoverPen = new Pen(Color.DimGray, 5);
        private static readonly Brush CutBrush = new SolidBrush(Color.FromArgb(30, Color.Black));
        private static readonly StringFormat SeekFormat = new StringFormat { LineAlignment = StringAlignment.Far };
        public event EventHandler CurrentTimeChanged;
        public event EventHandler CutStartChanged;
        public event EventHandler CutStopChanged;
        public int BarHeight { get; set; } = 10;
        public int LineHeight { get; set; } = 16;
        public bool ShowSeekTime { get; set; } = false;
        public TimeSpan TotalTime
        {
            get => TimeSpan.FromTicks(totalTicks);
            set
            {
                totalTicks = value.Ticks;
                Invalidate();
                Update();
            }
        }

        public TimeSpan CurrentTime
        {
            get => TimeSpan.FromTicks(currentTicks);
            set
            {
                currentTicks = value.Ticks;
                if (totalTicks == 0) return;
                CurrentPosition = Convert.ToInt32(barRectangle.Width * currentTicks / totalTicks);
            }
        }
        private int CurrentPosition
        {
            get => currentPosition;
            set
            {
                if (currentPosition == value) return;
                currentPosition = value;
                Invalidate();
                Update();
            }
        }

        public TimeSpan CutStartTime
        {
            get => TimeSpan.FromTicks(cutStartTicks);
            set
            {
                cutStartTicks = value.Ticks;
                if (totalTicks == 0) return;
                CutStartPosition = Convert.ToInt32(barRectangle.Width * cutStartTicks / totalTicks);
            }
        }
        private int CutStartPosition
        {
            get => cutStartPosition;
            set
            {
                if (cutStartPosition == value) return;
                cutStartPosition = value;
                Invalidate();
                Update();
            }
        }

        public TimeSpan CutStopTime
        {
            get => TimeSpan.FromTicks(cutStopTicks);
            set
            {
                cutStopTicks = value.Ticks;
                if (totalTicks == 0) return;
                CutStopPosition = Convert.ToInt32(barRectangle.Width * cutStopTicks / totalTicks);
            }
        }
        private int CutStopPosition
        {
            get => cutStopPosition;
            set
            {
                if (cutStopPosition == value) return;
                cutStopPosition = value;
                Invalidate();
                Update();
            }
        }

        private Target HoverTarget
        {
            get => hoverTarget;
            set
            {
                if (hoverTarget == value) return;
                hoverTarget = value;
                Invalidate();
                Update();
            }
        }
        private Target DragTarget
        {
            get => dragTarget;
            set
            {
                if (dragTarget == value) return;
                dragTarget = value;
                Invalidate();
                Update();
            }
        }

        public int SeekPosition
        {
            get => seekPosition;
            set
            {
                if (seekPosition == value) return;
                seekPosition = value;
                Invalidate();
                Update();
            }
        }

        public Rectangle barRectangle;
        private long currentTicks = 0L;
        private long totalTicks = 0L;
        private long cutStartTicks = 0L;
        private long cutStopTicks = 0L;
        private int currentPosition;
        private int seekPosition;
        private int cutStartPosition;
        private int cutStopPosition;
        private int lineTop;
        private Target hoverTarget;
        private Target dragTarget;

        enum Target
        {
            None,
            Bar,
            CutStart,
            CutStop
        }

        public AudioBar()
        {
            DoubleBuffered = true;
        }

        protected override void OnResize(EventArgs e)
        {
            barRectangle.X = 0;
            barRectangle.Y = Height / 2 - BarHeight / 2;
            barRectangle.Height = BarHeight;
            barRectangle.Width = Width - 1;
            lineTop = Height / 2 - LineHeight / 2;
            CurrentPosition = 0;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            // e.Graphics.Clear(BackColor);
            // if (BackgroundImage != null) e.Graphics.DrawImage(BackgroundImage, Point.Empty);
            e.Graphics.FillRectangle(BarBrush, barRectangle.X, barRectangle.Y, CurrentPosition, barRectangle.Height);
            if (HoverTarget == Target.Bar)
            {
                e.Graphics.FillRectangle(SeekBrush, barRectangle.X, barRectangle.Y, SeekPosition, barRectangle.Height);
                e.Graphics.DrawLine(SeekPen, barRectangle.X + SeekPosition, lineTop, barRectangle.X + SeekPosition, lineTop + LineHeight);
                if (ShowSeekTime)
                {
                    var time = TimeSpan.FromTicks(totalTicks * SeekPosition / barRectangle.Width).TotalSeconds.ToString("0.0");
                    e.Graphics.DrawString(time, Font, TextBrush, barRectangle.X + SeekPosition, barRectangle.Y, SeekFormat);
                }
            }
            else
            {
                e.Graphics.DrawLine(SeekPen, barRectangle.X + CurrentPosition, lineTop, barRectangle.X + CurrentPosition, lineTop + LineHeight);
            }
            e.Graphics.DrawRectangle(BarPen, barRectangle);
            if (CutStartPosition + CutStopPosition > 0)
            {
                e.Graphics.FillRectangle(CutBrush, barRectangle.X, 0, barRectangle.X + CutStartPosition, Height);
                e.Graphics.FillRectangle(CutBrush, barRectangle.X + CutStopPosition, 0, barRectangle.X + barRectangle.Width, Height);
                e.Graphics.DrawLine(HoverTarget == Target.CutStart ? CutHoverPen : CutNormalPen, barRectangle.X + CutStartPosition, 0, barRectangle.X + CutStartPosition, Height);
                e.Graphics.DrawLine(HoverTarget == Target.CutStop ? CutHoverPen : CutNormalPen, barRectangle.X + CutStopPosition, 0, barRectangle.X + CutStopPosition, Height);
            }
        }

        private Target GetTarget(Point point)
        {
            if (Math.Abs(point.X - barRectangle.X - CutStartPosition) < 3) return Target.CutStart;
            if (Math.Abs(point.X - barRectangle.X - CutStopPosition) < 3) return Target.CutStop;
            if (barRectangle.Contains(point)) return Target.Bar;
            return Target.None;
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            DragTarget = GetTarget(e.Location);
            if (DragTarget != Target.None) OnMouseMove(e);
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            if (DragTarget == Target.CutStart) CutStartPosition = Math.Max(Math.Min(e.X - barRectangle.X, CutStopPosition), 0);
            if (DragTarget == Target.CutStop) CutStopPosition = Math.Min(Math.Max(e.X - barRectangle.X, CutStartPosition), barRectangle.Width);
            if (DragTarget == Target.Bar) SeekPosition = e.X - barRectangle.X;
            if (DragTarget == Target.None)
            {
                HoverTarget = GetTarget(e.Location);
                if (HoverTarget == Target.Bar) SeekPosition = e.X - barRectangle.X;
            }
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            if (DragTarget == Target.CutStart)
            {
                CutStartTime = TimeSpan.FromTicks(totalTicks * CutStartPosition / barRectangle.Width);
                CutStartChanged?.Invoke(this, e);
            }
            if (DragTarget == Target.CutStop)
            {
                CutStopTime = TimeSpan.FromTicks(totalTicks * CutStopPosition / barRectangle.Width);
                CutStopChanged?.Invoke(this, e);
            }
            if (DragTarget == Target.Bar || DragTarget == Target.None && HoverTarget == Target.Bar)
            {
                CurrentTime = TimeSpan.FromTicks(totalTicks * SeekPosition / barRectangle.Width);
                CurrentTimeChanged?.Invoke(this, e);
            }
            HoverTarget = Target.None;
            DragTarget = Target.None;
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            HoverTarget = Target.None;
            DragTarget = Target.None;
        }
    }
}
