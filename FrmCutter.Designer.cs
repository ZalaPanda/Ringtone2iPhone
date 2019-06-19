using Ringtone2iPhone.Controls;

namespace Ringtone2iPhone
{
    partial class FrmCutter
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.btnPlay = new System.Windows.Forms.Button();
            this.tmrRefresh = new System.Windows.Forms.Timer(this.components);
            this.rdbFadeIn = new System.Windows.Forms.RadioButton();
            this.rdbFadeOut = new System.Windows.Forms.RadioButton();
            this.btnComplete = new System.Windows.Forms.Button();
            this.btnDiscard = new System.Windows.Forms.Button();
            this.txtCutStartTime = new System.Windows.Forms.TextBox();
            this.txtCutStopTime = new System.Windows.Forms.TextBox();
            this.btnJumpStart = new System.Windows.Forms.Button();
            this.btnJumpStop = new System.Windows.Forms.Button();
            this.txtDuration = new System.Windows.Forms.TextBox();
            this.btnVolumeBoost = new System.Windows.Forms.Button();
            this.barEditor = new Ringtone2iPhone.Controls.AudioBar();
            this.SuspendLayout();
            // 
            // btnPlay
            // 
            this.btnPlay.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnPlay.BackColor = System.Drawing.SystemColors.Control;
            this.btnPlay.FlatAppearance.BorderSize = 0;
            this.btnPlay.Image = global::Ringtone2iPhone.Properties.Resources.play;
            this.btnPlay.Location = new System.Drawing.Point(11, 119);
            this.btnPlay.Name = "btnPlay";
            this.btnPlay.Size = new System.Drawing.Size(30, 30);
            this.btnPlay.TabIndex = 1;
            this.btnPlay.UseVisualStyleBackColor = false;
            this.btnPlay.Click += new System.EventHandler(this.BtnPlay_Click);
            // 
            // tmrRefresh
            // 
            this.tmrRefresh.Tick += new System.EventHandler(this.TmrRefresh_Tick);
            // 
            // rdbFadeIn
            // 
            this.rdbFadeIn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.rdbFadeIn.AutoCheck = false;
            this.rdbFadeIn.AutoSize = true;
            this.rdbFadeIn.Location = new System.Drawing.Point(254, 120);
            this.rdbFadeIn.Name = "rdbFadeIn";
            this.rdbFadeIn.Size = new System.Drawing.Size(77, 25);
            this.rdbFadeIn.TabIndex = 5;
            this.rdbFadeIn.TabStop = true;
            this.rdbFadeIn.Text = "Fade in";
            this.rdbFadeIn.UseVisualStyleBackColor = true;
            this.rdbFadeIn.CheckedChanged += new System.EventHandler(this.RdbFadeIn_CheckedChanged);
            this.rdbFadeIn.Click += new System.EventHandler(this.RdbFadeIn_Click);
            // 
            // rdbFadeOut
            // 
            this.rdbFadeOut.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.rdbFadeOut.AutoCheck = false;
            this.rdbFadeOut.Location = new System.Drawing.Point(458, 120);
            this.rdbFadeOut.Name = "rdbFadeOut";
            this.rdbFadeOut.Size = new System.Drawing.Size(87, 25);
            this.rdbFadeOut.TabIndex = 8;
            this.rdbFadeOut.TabStop = true;
            this.rdbFadeOut.Text = "Fade out";
            this.rdbFadeOut.UseVisualStyleBackColor = true;
            this.rdbFadeOut.CheckedChanged += new System.EventHandler(this.RdbFadeOut_CheckedChanged);
            this.rdbFadeOut.Click += new System.EventHandler(this.RdbFadeOut_Click);
            // 
            // btnComplete
            // 
            this.btnComplete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnComplete.BackColor = System.Drawing.SystemColors.Control;
            this.btnComplete.FlatAppearance.BorderSize = 0;
            this.btnComplete.Image = global::Ringtone2iPhone.Properties.Resources.complete;
            this.btnComplete.Location = new System.Drawing.Point(610, 118);
            this.btnComplete.Name = "btnComplete";
            this.btnComplete.Size = new System.Drawing.Size(30, 30);
            this.btnComplete.TabIndex = 9;
            this.btnComplete.UseVisualStyleBackColor = false;
            this.btnComplete.Click += new System.EventHandler(this.BtnComplete_Click);
            // 
            // btnDiscard
            // 
            this.btnDiscard.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnDiscard.BackColor = System.Drawing.SystemColors.Control;
            this.btnDiscard.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnDiscard.FlatAppearance.BorderSize = 0;
            this.btnDiscard.Image = global::Ringtone2iPhone.Properties.Resources.discard;
            this.btnDiscard.Location = new System.Drawing.Point(644, 118);
            this.btnDiscard.Name = "btnDiscard";
            this.btnDiscard.Size = new System.Drawing.Size(30, 30);
            this.btnDiscard.TabIndex = 10;
            this.btnDiscard.UseVisualStyleBackColor = false;
            this.btnDiscard.Click += new System.EventHandler(this.BtnDiscard_Click);
            // 
            // txtCutStartTime
            // 
            this.txtCutStartTime.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.txtCutStartTime.Location = new System.Drawing.Point(184, 119);
            this.txtCutStartTime.Name = "txtCutStartTime";
            this.txtCutStartTime.Size = new System.Drawing.Size(64, 29);
            this.txtCutStartTime.TabIndex = 4;
            this.txtCutStartTime.Text = "0.0";
            this.txtCutStartTime.TextChanged += new System.EventHandler(this.TxtCutStartTime_TextChanged);
            this.txtCutStartTime.Validated += new System.EventHandler(this.TxtCutStartTime_Validated);
            // 
            // txtCutStopTime
            // 
            this.txtCutStopTime.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.txtCutStopTime.Location = new System.Drawing.Point(388, 119);
            this.txtCutStopTime.Name = "txtCutStopTime";
            this.txtCutStopTime.Size = new System.Drawing.Size(64, 29);
            this.txtCutStopTime.TabIndex = 7;
            this.txtCutStopTime.Text = "0.0";
            this.txtCutStopTime.TextChanged += new System.EventHandler(this.TxtCutStopTime_TextChanged);
            this.txtCutStopTime.Validated += new System.EventHandler(this.TxtCutStopTime_Validated);
            // 
            // btnJumpStart
            // 
            this.btnJumpStart.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnJumpStart.BackColor = System.Drawing.SystemColors.Control;
            this.btnJumpStart.FlatAppearance.BorderSize = 0;
            this.btnJumpStart.Image = global::Ringtone2iPhone.Properties.Resources.prev;
            this.btnJumpStart.Location = new System.Drawing.Point(152, 118);
            this.btnJumpStart.Name = "btnJumpStart";
            this.btnJumpStart.Size = new System.Drawing.Size(30, 30);
            this.btnJumpStart.TabIndex = 3;
            this.btnJumpStart.UseVisualStyleBackColor = false;
            this.btnJumpStart.Click += new System.EventHandler(this.BtnJumpStart_Click);
            // 
            // btnJumpStop
            // 
            this.btnJumpStop.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnJumpStop.BackColor = System.Drawing.SystemColors.Control;
            this.btnJumpStop.FlatAppearance.BorderSize = 0;
            this.btnJumpStop.Image = global::Ringtone2iPhone.Properties.Resources.next;
            this.btnJumpStop.Location = new System.Drawing.Point(356, 118);
            this.btnJumpStop.Name = "btnJumpStop";
            this.btnJumpStop.Size = new System.Drawing.Size(30, 30);
            this.btnJumpStop.TabIndex = 6;
            this.btnJumpStop.UseVisualStyleBackColor = false;
            this.btnJumpStop.Click += new System.EventHandler(this.BtnJumpStop_Click);
            // 
            // txtDuration
            // 
            this.txtDuration.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.txtDuration.BackColor = System.Drawing.Color.White;
            this.txtDuration.Location = new System.Drawing.Point(43, 119);
            this.txtDuration.Name = "txtDuration";
            this.txtDuration.ReadOnly = true;
            this.txtDuration.Size = new System.Drawing.Size(64, 29);
            this.txtDuration.TabIndex = 2;
            this.txtDuration.Text = "0.0";
            // 
            // btnVolumeBoost
            // 
            this.btnVolumeBoost.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnVolumeBoost.BackColor = System.Drawing.SystemColors.Control;
            this.btnVolumeBoost.FlatAppearance.BorderSize = 0;
            this.btnVolumeBoost.Image = global::Ringtone2iPhone.Properties.Resources.volumenormal;
            this.btnVolumeBoost.Location = new System.Drawing.Point(551, 118);
            this.btnVolumeBoost.Name = "btnVolumeBoost";
            this.btnVolumeBoost.Size = new System.Drawing.Size(30, 30);
            this.btnVolumeBoost.TabIndex = 11;
            this.btnVolumeBoost.UseVisualStyleBackColor = false;
            this.btnVolumeBoost.Click += new System.EventHandler(this.BtnVolumeBoost_Click);
            // 
            // barEditor
            // 
            this.barEditor.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.barEditor.BarHeight = 10;
            this.barEditor.CurrentTime = System.TimeSpan.Parse("00:01:30");
            this.barEditor.CutStartTime = System.TimeSpan.Parse("00:00:00");
            this.barEditor.CutStopTime = System.TimeSpan.Parse("00:00:00");
            this.barEditor.LineHeight = 16;
            this.barEditor.Location = new System.Drawing.Point(12, 6);
            this.barEditor.Name = "barEditor";
            this.barEditor.SeekPosition = 0;
            this.barEditor.Size = new System.Drawing.Size(658, 103);
            this.barEditor.TabIndex = 0;
            this.barEditor.TotalTime = System.TimeSpan.Parse("00:03:30");
            this.barEditor.CurrentTimeChanged += new System.EventHandler(this.BarEditor_CurrentTimeChanged);
            this.barEditor.CutStartChanged += new System.EventHandler(this.BarEditor_CutStartChanged);
            this.barEditor.CutStopChanged += new System.EventHandler(this.BarEditor_CutStopChanged);
            // 
            // FrmCutter
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(684, 161);
            this.Controls.Add(this.btnVolumeBoost);
            this.Controls.Add(this.txtDuration);
            this.Controls.Add(this.btnJumpStop);
            this.Controls.Add(this.btnJumpStart);
            this.Controls.Add(this.txtCutStopTime);
            this.Controls.Add(this.txtCutStartTime);
            this.Controls.Add(this.btnDiscard);
            this.Controls.Add(this.btnComplete);
            this.Controls.Add(this.rdbFadeOut);
            this.Controls.Add(this.rdbFadeIn);
            this.Controls.Add(this.btnPlay);
            this.Controls.Add(this.barEditor);
            this.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ForeColor = System.Drawing.Color.Black;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = global::Ringtone2iPhone.Properties.Resources.program;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmCutter";
            this.Text = "Cutter";
            this.TopMost = true;
            this.Deactivate += new System.EventHandler(this.FrmCutter_Deactivate);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FrmCutter_FormClosing);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private AudioBar barEditor;
        private System.Windows.Forms.Button btnPlay;
        private System.Windows.Forms.Timer tmrRefresh;
        private System.Windows.Forms.RadioButton rdbFadeIn;
        private System.Windows.Forms.RadioButton rdbFadeOut;
        private System.Windows.Forms.Button btnComplete;
        private System.Windows.Forms.Button btnDiscard;
        private System.Windows.Forms.TextBox txtCutStartTime;
        private System.Windows.Forms.TextBox txtCutStopTime;
        private System.Windows.Forms.Button btnJumpStart;
        private System.Windows.Forms.Button btnJumpStop;
        private System.Windows.Forms.TextBox txtDuration;
        private System.Windows.Forms.Button btnVolumeBoost;
    }
}