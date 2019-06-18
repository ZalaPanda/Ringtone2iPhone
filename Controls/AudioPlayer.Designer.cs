namespace Ringtone2iPhone.Controls
{
    partial class AudioPlayer
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
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
            this.lblTime = new System.Windows.Forms.Label();
            this.btnPlay = new System.Windows.Forms.Button();
            this.tmrRefresh = new System.Windows.Forms.Timer(this.components);
            this.barPlayer = new Ringtone2iPhone.Controls.AudioBar();
            this.SuspendLayout();
            // 
            // lblTime
            // 
            this.lblTime.AutoSize = true;
            this.lblTime.Location = new System.Drawing.Point(34, 18);
            this.lblTime.Name = "lblTime";
            this.lblTime.Size = new System.Drawing.Size(71, 21);
            this.lblTime.TabIndex = 9;
            this.lblTime.Text = "No input";
            // 
            // btnPlay
            // 
            this.btnPlay.BackColor = System.Drawing.SystemColors.Control;
            this.btnPlay.FlatAppearance.BorderSize = 0;
            this.btnPlay.Image = global::Ringtone2iPhone.Properties.Resources.play;
            this.btnPlay.Location = new System.Drawing.Point(0, 14);
            this.btnPlay.Name = "btnPlay";
            this.btnPlay.Size = new System.Drawing.Size(28, 28);
            this.btnPlay.TabIndex = 7;
            this.btnPlay.UseVisualStyleBackColor = false;
            this.btnPlay.Click += new System.EventHandler(this.BtnPlay_Click);
            // 
            // tmrRefresh
            // 
            this.tmrRefresh.Tick += new System.EventHandler(this.TmrRefresh_Tick);
            // 
            // barPlayer
            // 
            this.barPlayer.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.barPlayer.BarHeight = 10;
            this.barPlayer.CurrentTime = System.TimeSpan.Parse("00:00:00");
            this.barPlayer.CutStartTime = System.TimeSpan.Parse("00:00:00");
            this.barPlayer.CutStopTime = System.TimeSpan.Parse("00:00:00");
            this.barPlayer.LineHeight = 16;
            this.barPlayer.Location = new System.Drawing.Point(0, 0);
            this.barPlayer.Name = "barPlayer";
            this.barPlayer.SeekPosition = 0;
            this.barPlayer.Size = new System.Drawing.Size(400, 14);
            this.barPlayer.TabIndex = 10;
            this.barPlayer.TotalTime = System.TimeSpan.Parse("00:00:00");
            this.barPlayer.CurrentTimeChanged += new System.EventHandler(this.BarPlayer_CurrentTimeChanged);
            // 
            // AudioPlayer
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.Controls.Add(this.barPlayer);
            this.Controls.Add(this.lblTime);
            this.Controls.Add(this.btnPlay);
            this.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "AudioPlayer";
            this.Size = new System.Drawing.Size(400, 42);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblTime;
        private System.Windows.Forms.Button btnPlay;
        private System.Windows.Forms.Timer tmrRefresh;
        private AudioBar barPlayer;
    }
}
