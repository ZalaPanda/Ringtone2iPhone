using System;
using System.Windows.Forms;
using NAudio.Wave;

namespace Ringtone2iPhone.Controls
{
    public partial class AudioPlayer : UserControl
    {
        private readonly WaveOutEvent player = new WaveOutEvent();
        private AudioFileReader reader = null;

        public AudioPlayer()
        {
            InitializeComponent();
            player.PlaybackStopped += Player_PlaybackStopped;
        }

        public void Init(string path)
        {
            Reset();
            reader = new AudioFileReader(path);
            barPlayer.TotalTime = reader.TotalTime;
            player.Init(reader);
            tmrRefresh.Start();
            RefreshPlayButton();
        }

        public void Reset()
        {
            if (player.PlaybackState != PlaybackState.Stopped) player.Stop();
            if (reader != null) reader.Close();
            reader = null;
            lblTime.Text = string.Empty;
            tmrRefresh.Stop();
        }

        private void RefreshPlayButton()
        {
            btnPlay.Image = player.PlaybackState == PlaybackState.Playing ? Properties.Resources.pause : Properties.Resources.play;
        }

        private void TmrRefresh_Tick(object sender, EventArgs e)
        {
            barPlayer.CurrentTime = reader.CurrentTime;
            lblTime.Text = string.Format("{0} / {1}", reader.CurrentTime.ToString(@"mm\:ss"), reader.TotalTime.ToString(@"mm\:ss"));
        }

        private void Player_PlaybackStopped(object sender, StoppedEventArgs e)
        {
            RefreshPlayButton();
        }

        private void BtnPlay_Click(object sender, EventArgs e)
        {
            if (reader == null) return;
            if (player.PlaybackState == PlaybackState.Playing)
            {
                player.Pause();
                RefreshPlayButton();
            }
            else
            {
                if (reader.CurrentTime >= reader.TotalTime) reader.CurrentTime = TimeSpan.Zero; // rewind
                player.Play();
                RefreshPlayButton();
            }
        }

        private void BarPlayer_CurrentTimeChanged(object sender, EventArgs e)
        {
            if (reader == null) return;
            reader.CurrentTime = barPlayer.CurrentTime;
        }
    }
}
