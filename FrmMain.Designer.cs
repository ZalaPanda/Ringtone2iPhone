namespace Ringtone2iPhone
{
    partial class FrmMain
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
            System.Windows.Forms.ListViewGroup listViewGroup1 = new System.Windows.Forms.ListViewGroup("Ringtone", System.Windows.Forms.HorizontalAlignment.Left);
            System.Windows.Forms.ListViewGroup listViewGroup2 = new System.Windows.Forms.ListViewGroup("Audio", System.Windows.Forms.HorizontalAlignment.Left);
            System.Windows.Forms.ListViewGroup listViewGroup3 = new System.Windows.Forms.ListViewGroup("Other", System.Windows.Forms.HorizontalAlignment.Left);
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmMain));
            System.Windows.Forms.ListViewGroup listViewGroup4 = new System.Windows.Forms.ListViewGroup("iPhone", System.Windows.Forms.HorizontalAlignment.Left);
            this.lstAudioLocal = new System.Windows.Forms.ListView();
            this.colAudioLocal = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.imgAudio = new System.Windows.Forms.ImageList(this.components);
            this.lstAudioRemote = new System.Windows.Forms.ListView();
            this.colAudioRemote = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.btnRefreshLocal = new System.Windows.Forms.Button();
            this.btnRefreshRemote = new System.Windows.Forms.Button();
            this.btnCut = new System.Windows.Forms.Button();
            this.cboDevice = new System.Windows.Forms.ComboBox();
            this.barPhone = new Ringtone2iPhone.Controls.StorageBar();
            this.plrAudioLocal = new Ringtone2iPhone.Controls.AudioPlayer();
            this.SuspendLayout();
            // 
            // lstAudioLocal
            // 
            this.lstAudioLocal.AllowDrop = true;
            this.lstAudioLocal.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lstAudioLocal.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colAudioLocal});
            this.lstAudioLocal.GridLines = true;
            listViewGroup1.Header = "Ringtone";
            listViewGroup1.Name = null;
            listViewGroup2.Header = "Audio";
            listViewGroup2.Name = null;
            listViewGroup3.Header = "Other";
            listViewGroup3.Name = null;
            this.lstAudioLocal.Groups.AddRange(new System.Windows.Forms.ListViewGroup[] {
            listViewGroup1,
            listViewGroup2,
            listViewGroup3});
            this.lstAudioLocal.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            this.lstAudioLocal.HideSelection = false;
            this.lstAudioLocal.LabelEdit = true;
            this.lstAudioLocal.Location = new System.Drawing.Point(12, 12);
            this.lstAudioLocal.Name = "lstAudioLocal";
            this.lstAudioLocal.ShowItemToolTips = true;
            this.lstAudioLocal.Size = new System.Drawing.Size(394, 367);
            this.lstAudioLocal.SmallImageList = this.imgAudio;
            this.lstAudioLocal.Sorting = System.Windows.Forms.SortOrder.Ascending;
            this.lstAudioLocal.TabIndex = 7;
            this.lstAudioLocal.UseCompatibleStateImageBehavior = false;
            this.lstAudioLocal.View = System.Windows.Forms.View.Details;
            this.lstAudioLocal.AfterLabelEdit += new System.Windows.Forms.LabelEditEventHandler(this.LstAudioLocal_AfterLabelEdit);
            this.lstAudioLocal.ItemDrag += new System.Windows.Forms.ItemDragEventHandler(this.LstAudioLocal_ItemDrag);
            this.lstAudioLocal.SelectedIndexChanged += new System.EventHandler(this.LstAudioLocal_SelectedIndexChanged);
            this.lstAudioLocal.DragDrop += new System.Windows.Forms.DragEventHandler(this.LstAudioLocal_DragDrop);
            this.lstAudioLocal.DragEnter += new System.Windows.Forms.DragEventHandler(this.LstAudioLocal_DragEnter);
            this.lstAudioLocal.DoubleClick += new System.EventHandler(this.LstAudioLocal_DoubleClick);
            this.lstAudioLocal.KeyDown += new System.Windows.Forms.KeyEventHandler(this.LstAudioLocal_KeyDown);
            // 
            // colAudioLocal
            // 
            this.colAudioLocal.Width = 370;
            // 
            // imgAudio
            // 
            this.imgAudio.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imgAudio.ImageStream")));
            this.imgAudio.TransparentColor = System.Drawing.Color.Transparent;
            this.imgAudio.Images.SetKeyName(0, "audio");
            this.imgAudio.Images.SetKeyName(1, "ringtone_local");
            this.imgAudio.Images.SetKeyName(2, "ringtone_remote");
            // 
            // lstAudioRemote
            // 
            this.lstAudioRemote.AllowDrop = true;
            this.lstAudioRemote.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lstAudioRemote.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colAudioRemote});
            this.lstAudioRemote.Enabled = false;
            this.lstAudioRemote.GridLines = true;
            listViewGroup4.Header = "iPhone";
            listViewGroup4.Name = null;
            this.lstAudioRemote.Groups.AddRange(new System.Windows.Forms.ListViewGroup[] {
            listViewGroup4});
            this.lstAudioRemote.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            this.lstAudioRemote.HideSelection = false;
            this.lstAudioRemote.LabelEdit = true;
            this.lstAudioRemote.Location = new System.Drawing.Point(412, 12);
            this.lstAudioRemote.Name = "lstAudioRemote";
            this.lstAudioRemote.ShowItemToolTips = true;
            this.lstAudioRemote.Size = new System.Drawing.Size(394, 367);
            this.lstAudioRemote.SmallImageList = this.imgAudio;
            this.lstAudioRemote.Sorting = System.Windows.Forms.SortOrder.Ascending;
            this.lstAudioRemote.TabIndex = 13;
            this.lstAudioRemote.UseCompatibleStateImageBehavior = false;
            this.lstAudioRemote.View = System.Windows.Forms.View.Details;
            this.lstAudioRemote.AfterLabelEdit += new System.Windows.Forms.LabelEditEventHandler(this.LstAudioRemote_AfterLabelEdit);
            this.lstAudioRemote.ItemDrag += new System.Windows.Forms.ItemDragEventHandler(this.LstAudioRemote_ItemDrag);
            this.lstAudioRemote.DragDrop += new System.Windows.Forms.DragEventHandler(this.LstAudioRemote_DragDrop);
            this.lstAudioRemote.DragEnter += new System.Windows.Forms.DragEventHandler(this.LstAudioRemote_DragEnter);
            this.lstAudioRemote.KeyDown += new System.Windows.Forms.KeyEventHandler(this.LstAudioRemote_KeyDown);
            // 
            // colAudioRemote
            // 
            this.colAudioRemote.Width = 370;
            // 
            // btnRefreshLocal
            // 
            this.btnRefreshLocal.BackColor = System.Drawing.SystemColors.Control;
            this.btnRefreshLocal.FlatAppearance.BorderSize = 0;
            this.btnRefreshLocal.Image = global::Ringtone2iPhone.Properties.Resources.refresh;
            this.btnRefreshLocal.Location = new System.Drawing.Point(348, 399);
            this.btnRefreshLocal.Name = "btnRefreshLocal";
            this.btnRefreshLocal.Size = new System.Drawing.Size(28, 28);
            this.btnRefreshLocal.TabIndex = 12;
            this.btnRefreshLocal.UseVisualStyleBackColor = false;
            this.btnRefreshLocal.Click += new System.EventHandler(this.BtnRefreshLocal_Click);
            // 
            // btnRefreshRemote
            // 
            this.btnRefreshRemote.BackColor = System.Drawing.SystemColors.Control;
            this.btnRefreshRemote.FlatAppearance.BorderSize = 0;
            this.btnRefreshRemote.Image = global::Ringtone2iPhone.Properties.Resources.refresh;
            this.btnRefreshRemote.Location = new System.Drawing.Point(778, 399);
            this.btnRefreshRemote.Name = "btnRefreshRemote";
            this.btnRefreshRemote.Size = new System.Drawing.Size(28, 28);
            this.btnRefreshRemote.TabIndex = 10;
            this.btnRefreshRemote.UseVisualStyleBackColor = false;
            this.btnRefreshRemote.Click += new System.EventHandler(this.BtnRefreshRemote_Click);
            // 
            // btnCut
            // 
            this.btnCut.BackColor = System.Drawing.SystemColors.Control;
            this.btnCut.Enabled = false;
            this.btnCut.FlatAppearance.BorderSize = 0;
            this.btnCut.Image = global::Ringtone2iPhone.Properties.Resources.cut;
            this.btnCut.Location = new System.Drawing.Point(378, 399);
            this.btnCut.Name = "btnCut";
            this.btnCut.Size = new System.Drawing.Size(28, 28);
            this.btnCut.TabIndex = 9;
            this.btnCut.UseVisualStyleBackColor = false;
            this.btnCut.Click += new System.EventHandler(this.BtnCut_Click);
            // 
            // cboDevice
            // 
            this.cboDevice.FormattingEnabled = true;
            this.cboDevice.Location = new System.Drawing.Point(412, 399);
            this.cboDevice.Name = "cboDevice";
            this.cboDevice.Size = new System.Drawing.Size(360, 29);
            this.cboDevice.TabIndex = 15;
            this.cboDevice.SelectedIndexChanged += new System.EventHandler(this.CboDevice_SelectedIndexChanged);
            // 
            // barPhone
            // 
            this.barPhone.FreeBytes = ((long)(0));
            this.barPhone.Location = new System.Drawing.Point(412, 386);
            this.barPhone.Name = "barPhone";
            this.barPhone.Size = new System.Drawing.Size(394, 12);
            this.barPhone.TabIndex = 14;
            this.barPhone.Text = "iPhone";
            this.barPhone.TotalBytes = ((long)(0));
            // 
            // plrAudioLocal
            // 
            this.plrAudioLocal.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.plrAudioLocal.Location = new System.Drawing.Point(12, 385);
            this.plrAudioLocal.Name = "plrAudioLocal";
            this.plrAudioLocal.Size = new System.Drawing.Size(393, 42);
            this.plrAudioLocal.TabIndex = 8;
            // 
            // FrmMain
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(818, 431);
            this.Controls.Add(this.cboDevice);
            this.Controls.Add(this.barPhone);
            this.Controls.Add(this.lstAudioRemote);
            this.Controls.Add(this.btnRefreshLocal);
            this.Controls.Add(this.btnRefreshRemote);
            this.Controls.Add(this.btnCut);
            this.Controls.Add(this.plrAudioLocal);
            this.Controls.Add(this.lstAudioLocal);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ForeColor = System.Drawing.Color.Black;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = global::Ringtone2iPhone.Properties.Resources.program;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmMain";
            this.Text = "Ringtone2iPhone";
            this.Deactivate += new System.EventHandler(this.FrmMain_Deactivate);
            this.Load += new System.EventHandler(this.FrmMain_Load);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.ListView lstAudioLocal;
        private System.Windows.Forms.ImageList imgAudio;
        private System.Windows.Forms.ColumnHeader colAudioLocal;
        private Controls.AudioPlayer plrAudioLocal;
        private System.Windows.Forms.Button btnCut;
        private System.Windows.Forms.Button btnRefreshRemote;
        private System.Windows.Forms.Button btnRefreshLocal;
        private System.Windows.Forms.ListView lstAudioRemote;
        private System.Windows.Forms.ColumnHeader colAudioRemote;
        private Controls.StorageBar barPhone;
        private System.Windows.Forms.ComboBox cboDevice;
    }
}