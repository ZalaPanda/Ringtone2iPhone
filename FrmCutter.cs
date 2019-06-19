using NAudio.Wave;
using NAudio.Wave.SampleProviders;
using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace Ringtone2iPhone
{
    public partial class FrmCutter : Form
    {
        const string OUTPUTFILENAME = "trimmed.m4a";
        const string RINGTONEEXTENSION = ".m4r";
        const int FADESECONDS = 3;
        const int STOPSECONDS = 6;
        const int LENGTHLIMIT = 40;
        const int BARWIDTH = 2;
        const int BARSPACE = 1;
        const string TIMEFORMAT = "0.0##";

        private readonly WaveOutEvent player = new WaveOutEvent();
        private AudioFileReader reader = null;

        public FrmCutter()
        {
            InitializeComponent();
            MinimumSize = Size;
            MaximumSize = new Size(int.MaxValue, Height);
            player.PlaybackStopped += Player_PlaybackStopped;
        }

        public void Init(string path)
        {
            Reset();
            reader = new AudioFileReader(path);
            Render();
            barEditor.TotalTime = reader.TotalTime;
            barEditor.CutStartTime = TimeSpan.FromSeconds(Math.Round(reader.TotalTime.TotalSeconds / 10));
            barEditor.CutStopTime = barEditor.CutStartTime + TimeSpan.FromSeconds(30);
            txtCutStartTime.Text = barEditor.CutStartTime.TotalSeconds.ToString(TIMEFORMAT);
            txtCutStopTime.Text = barEditor.CutStopTime.TotalSeconds.ToString(TIMEFORMAT);
            tmrRefresh.Start();
            btnVolumeBoost.Visible = reader.WaveFormat.Channels > 1;
            Play(barEditor.CutStartTime);
            Text = Path.GetFileNameWithoutExtension(path);
        }

        public void Reset()
        {
            if (reader == null) return;
            if (player.PlaybackState != PlaybackState.Stopped) player.Stop();
            reader.Close();
            reader = null;
            tmrRefresh.Stop();
        }

        private void RefreshPlayButton()
        {
            btnPlay.Image = player.PlaybackState == PlaybackState.Playing ? Properties.Resources.pause : Properties.Resources.play;
        }

        private void Play(TimeSpan time)
        {
            if (reader == null) return;
            if (player.PlaybackState != PlaybackState.Stopped) player.Stop();
            reader.CurrentTime = barEditor.CutStartTime;
            var duration = barEditor.CutStopTime - barEditor.CutStartTime;
            var fadeIn = rdbFadeIn.Checked ? TimeSpan.FromSeconds(FADESECONDS) : TimeSpan.Zero;
            var fadeOut = rdbFadeOut.Checked ? TimeSpan.FromSeconds(FADESECONDS) : TimeSpan.Zero;
            var skip = time - barEditor.CutStartTime;
            var volume = Convert.ToBoolean(btnVolumeBoost.Tag) ? 1F : 0.5F;
            player.Init(GetProvider(reader, duration, fadeIn, fadeOut, TimeSpan.Zero, volume).Skip(skip));
            player.Play();
            txtDuration.Text = duration.TotalSeconds.ToString(TIMEFORMAT);
            txtDuration.ForeColor = duration.TotalSeconds <= LENGTHLIMIT ? Color.Black : Color.Red;
            RefreshPlayButton();
        }

        private void Save()
        {
            if (reader == null) return;
            if (player.PlaybackState != PlaybackState.Stopped) player.Stop();
            reader.CurrentTime = TimeSpan.Zero;
            var duration = barEditor.CutStopTime - barEditor.CutStartTime;
            var fadeIn = rdbFadeIn.Checked ? TimeSpan.FromSeconds(FADESECONDS) : TimeSpan.Zero;
            var fadeOut = rdbFadeOut.Checked ? TimeSpan.FromSeconds(FADESECONDS) : TimeSpan.Zero;
            var skip = barEditor.CutStartTime;
            var volume = Convert.ToBoolean(btnVolumeBoost.Tag) ? 1F : 0.5F;
            var temp = Path.Combine(Path.GetTempPath(), OUTPUTFILENAME);
            if (File.Exists(temp)) File.Delete(temp);
            var resampler = new MediaFoundationResampler(GetProvider(reader, duration, fadeIn, fadeOut, skip, volume).ToWaveProvider(), 48000);
            MediaFoundationEncoder.EncodeToAac(resampler, temp, 0);
            reader.Close();
            reader.Dispose();
            var folder = Path.GetDirectoryName(reader.FileName);
            var name = Path.GetFileNameWithoutExtension(reader.FileName);
            if (File.Exists(name + RINGTONEEXTENSION))
            {
                var i = 1;
                while (File.Exists(name + i + RINGTONEEXTENSION)) i++;
                name += i;
            }
            var result = Path.Combine(folder, name + RINGTONEEXTENSION);
            File.Move(temp, result);
        }

        private static ISampleProvider GetProvider(ISampleProvider source, TimeSpan take, TimeSpan fadeIn, TimeSpan fadeOut, TimeSpan skip, float volume)
        {
            var offset = new OffsetSampleProvider(source) { SkipOver = skip, Take = take };
            var mono = offset.ToMono(volume, volume);
            if (fadeIn == TimeSpan.Zero && fadeOut == TimeSpan.Zero) return mono;
            var fade = new TimedFadeInOutSampleProvider(mono, fadeIn != TimeSpan.Zero);
            if (fadeIn != TimeSpan.Zero) fade.AddFadeIn(TimeSpan.Zero, fadeIn);
            if (fadeOut != TimeSpan.Zero) fade.AddFadeOut(take - fadeOut, fadeOut);
            return fade;
        }

        private void Render()
        {
            if (reader == null) return;
            if (player.PlaybackState != PlaybackState.Stopped) player.Stop();
            reader.CurrentTime = TimeSpan.Zero;
            var timer = Stopwatch.StartNew();
            var bitmap = new Bitmap(barEditor.Width, barEditor.Height);
            var graphics = Graphics.FromImage(bitmap);
            var barCount = bitmap.Width / (BARWIDTH + BARSPACE);
            var sampleCount = reader.Length / (reader.WaveFormat.BitsPerSample / 8);
            var samplesPerBar = (int)(sampleCount / barCount);
            var samplesStep = samplesPerBar / 20;
            var center = bitmap.Height / 2;
            var buffer = new float[samplesPerBar];
            for (var i = 0; i < barCount; i++)
            {
                var samplesRead = reader.Read(buffer, 0, buffer.Length);
                var sum = 0F;
                var num = 0;
                for (int x = 0; x < samplesRead; x += samplesStep)
                {
                    sum += Math.Abs(buffer[x]);
                    num++;
                }
                var height = bitmap.Height * Math.Min(sum / num, 1);
                graphics.FillRectangle(Brushes.LightGray, i * (BARWIDTH + BARSPACE), center - height, BARWIDTH, height * 2);
            }
            graphics.Dispose();
            if (barEditor.BackgroundImage != null) barEditor.BackgroundImage.Dispose();
            barEditor.BackgroundImage = bitmap;
            Trace.WriteLine(string.Format("Render time: {0:0.0}s", timer.Elapsed.TotalSeconds));
        }

        private static bool ValidTimeSpan(string text, TimeSpan start, TimeSpan stop, out double value)
        {
            var valid = double.TryParse(text, out value);
            return valid && value >= start.TotalSeconds && value <= stop.TotalSeconds;
        }

        private static bool ValidTimeSpan(string text, TimeSpan start, TimeSpan stop)
        {
            return ValidTimeSpan(text, start, stop, out _);
        }

        #region EventHandlers
        private void BtnPlay_Click(object sender, EventArgs e)
        {
            if (reader == null) return;
            switch (player.PlaybackState)
            {
                case PlaybackState.Stopped:
                    Play(barEditor.CutStartTime);
                    break;
                case PlaybackState.Playing:
                    player.Pause();
                    break;
                case PlaybackState.Paused:
                    player.Play();
                    break;
            }
            RefreshPlayButton();
        }

        private void BtnJumpStart_Click(object sender, EventArgs e)
        {
            Play(barEditor.CutStartTime);
        }

        private void BtnJumpStop_Click(object sender, EventArgs e)
        {
            var time = barEditor.CutStopTime - TimeSpan.FromSeconds(STOPSECONDS);
            if (time < barEditor.CutStartTime) time = barEditor.CutStartTime;
            Play(time);
        }

        private void BtnVolumeBoost_Click(object sender, EventArgs e)
        {
            var boost = !Convert.ToBoolean(btnVolumeBoost.Tag);
            btnVolumeBoost.Image = boost ? Properties.Resources.volumeboost : Properties.Resources.volumenormal;
            btnVolumeBoost.Tag = boost;
            Play(barEditor.CurrentTime);
        }

        private void TmrRefresh_Tick(object sender, EventArgs e)
        {
            barEditor.CurrentTime = reader.CurrentTime;
        }

        private void Player_PlaybackStopped(object sender, StoppedEventArgs e)
        {
            RefreshPlayButton();
        }

        private void BarEditor_CurrentTimeChanged(object sender, EventArgs e)
        {
            Play(barEditor.CurrentTime);
        }

        private void BarEditor_CutStartChanged(object sender, EventArgs e)
        {
            txtCutStartTime.Text = barEditor.CutStartTime.TotalSeconds.ToString(TIMEFORMAT);
            BtnJumpStart_Click(null, null);
        }

        private void BarEditor_CutStopChanged(object sender, EventArgs e)
        {
            txtCutStopTime.Text = barEditor.CutStopTime.TotalSeconds.ToString(TIMEFORMAT);
            BtnJumpStop_Click(null, null);
        }

        private void RdbFadeIn_Click(object sender, EventArgs e)
        {
            rdbFadeIn.Checked = !rdbFadeIn.Checked;
        }

        private void RdbFadeIn_CheckedChanged(object sender, EventArgs e)
        {
            BtnJumpStart_Click(null, null);
        }

        private void RdbFadeOut_Click(object sender, EventArgs e)
        {
            rdbFadeOut.Checked = !rdbFadeOut.Checked;
        }

        private void RdbFadeOut_CheckedChanged(object sender, EventArgs e)
        {
            BtnJumpStop_Click(null, null);
        }

        private void BtnComplete_Click(object sender, EventArgs e)
        {
            Save();
            DialogResult = DialogResult.OK;
            Close();
        }

        private void BtnDiscard_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void FrmCutter_FormClosing(object sender, FormClosingEventArgs e)
        {
            Reset();
        }

        private void TxtCutStartTime_TextChanged(object sender, EventArgs e)
        {
            txtCutStartTime.ForeColor = ValidTimeSpan(txtCutStartTime.Text, TimeSpan.Zero, barEditor.CutStopTime) ? Color.Black : Color.Red;
        }

        private void TxtCutStopTime_TextChanged(object sender, EventArgs e)
        {
            txtCutStopTime.ForeColor = ValidTimeSpan(txtCutStopTime.Text, barEditor.CutStartTime, barEditor.TotalTime) ? Color.Black : Color.Red;
        }

        private void TxtCutStartTime_Validated(object sender, EventArgs e)
        {
            if (!ValidTimeSpan(txtCutStartTime.Text, TimeSpan.Zero, barEditor.CutStopTime, out var value)) return;
            barEditor.CutStartTime = TimeSpan.FromSeconds(value);
            BtnJumpStop_Click(null, null);
        }

        private void TxtCutStopTime_Validated(object sender, EventArgs e)
        {
            if (!ValidTimeSpan(txtCutStartTime.Text, barEditor.CutStartTime, barEditor.TotalTime, out var value)) return;
            barEditor.CutStopTime = TimeSpan.FromSeconds(value);
            BtnJumpStop_Click(null, null);
        }

        private void FrmCutter_Deactivate(object sender, EventArgs e)
        {
            player.Stop();
        }

        //private void FrmCutter_Resize(object sender, EventArgs e)
        //{
        //    Render();
        //}
        #endregion
    }
}
