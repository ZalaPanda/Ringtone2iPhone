using Claunia.PropertyList;
using iMobileDevice;
using iMobileDevice.Afc;
using iMobileDevice.iDevice;
using iMobileDevice.Lockdown;
using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace Ringtone2iPhone
{
    public partial class FrmMain : Form
    {
        public class DeviceItem
        {
            public string Text;
            public string Udid;
            public override string ToString()
            {
                return Text;
            }
        }

        const string PROGRAMNAME = "Ringtone2iPhone";
        const string AUDIOFOLDER = "Audio";
        const string RINGTONEEXTENSION = ".m4r";
        const string RINGTONESPATH = "/iTunes_Control/Ringtones";
        const string RINGTONESPLIST = "/iTunes_Control/iTunes/Ringtones.plist";

        public iDeviceHandle CurrentDevice
        {
            get
            {
                var item = cboDevice.SelectedItem as DeviceItem;
                if (item == null) return null;
                LibiMobileDevice.Instance.iDevice.idevice_new(out iDeviceHandle device, item.Udid).ThrowOnError();
                return device;
            }
        }

        public FrmMain()
        {
            InitializeComponent();

            if (!Directory.Exists(AUDIOFOLDER)) Directory.CreateDirectory(AUDIOFOLDER);
            NativeLibraries.Load();
        }

        #region GUI
        private void FrmMain_Load(object sender, EventArgs e)
        {
            RefreshAudios();
            RefreshDevices();
        }

        private void RefreshAudios()
        {
            try
            {
                var folder = new DirectoryInfo(AUDIOFOLDER);
                lstAudioLocal.BeginUpdate();
                lstAudioLocal.Items.Clear();
                foreach (var info in folder.EnumerateFiles())
                {
                    var item = new ListViewItem { Name = info.FullName, Text = info.Name, Group = lstAudioLocal.Groups[2] };
                    try
                    {
                        var reader = new AudioFileReader(info.FullName);
                        if (info.Extension.Equals(RINGTONEEXTENSION))
                        {
                            item.ImageKey = "ringtone_local";
                            item.Group = lstAudioLocal.Groups[0];
                        }
                        else
                        {
                            item.ImageKey = "audio";
                            item.Group = lstAudioLocal.Groups[1];
                        }
                        reader.Close();
                    }
                    catch { }
                    lstAudioLocal.Items.Add(item);
                }
                lstAudioLocal.EndUpdate();
            }
            catch (Exception ex) { HandleException(ex); }
        }

        private void RefreshDevices()
        {
            try
            {
                var count = 0;
                // get device list
                LibiMobileDevice.Instance.iDevice.idevice_get_device_list(out ReadOnlyCollection<string> rudids, ref count);
                cboDevice.BeginUpdate();
                var udids = new List<string>(rudids);
                var remove = new List<DeviceItem>();
                foreach (DeviceItem item in cboDevice.Items)
                {
                    if (udids.Contains(item.Udid)) udids.Remove(item.Udid);
                    else remove.Add(item);
                }
                foreach (DeviceItem item in remove)
                {
                    if (cboDevice.SelectedItem == item) cboDevice.SelectedIndex = -1;
                    cboDevice.Items.Remove(item);
                }
                foreach (var udid in udids)
                {
                    var item = new DeviceItem { Udid = udid };
                    try
                    {
                        LibiMobileDevice.Instance.iDevice.idevice_new(out iDeviceHandle device, udid).ThrowOnError();
                        LibiMobileDevice.Instance.Lockdown.lockdownd_client_new(device, out LockdownClientHandle client, PROGRAMNAME).ThrowOnError();
                        LibiMobileDevice.Instance.Lockdown.lockdownd_get_device_name(client, out var deviceName);
                        item.Text = deviceName;
                        client.Close();
                        client.Dispose();
                        device.Close();
                        device.Dispose();
                    }
                    catch (Exception ex)
                    {
                        item.Text = ex.Message;
                    }
                    cboDevice.Items.Add(item);
                }
                if (cboDevice.Items.Count > 0 && cboDevice.SelectedItem == null) cboDevice.SelectedIndex = 0;
                cboDevice.EndUpdate();
            }
            catch (Exception ex)
            {
                HandleException(ex);
                tmrRemoteRefresh.Enabled = false;
            }
        }

        private void RefreshDevice()
        {
            try
            {
                // connect
                var device = CurrentDevice;
                lstAudioRemote.Enabled = device != null;
                if (device == null)
                {
                    barPhone.FreeBytes = 0;
                    barPhone.TotalBytes = 0;
                    return;
                }
                // start service
                LibiMobileDevice.Instance.Afc.afc_client_start_service(device, out AfcClientHandle client, PROGRAMNAME).ThrowOnError();
                LibiMobileDevice.Instance.Afc.afc_get_device_info(client, out ReadOnlyCollection<string> deviceInformation);
                // refresh bar
                barPhone.FreeBytes = GetFreeBytes(deviceInformation);
                barPhone.TotalBytes = GetTotalBytes(deviceInformation);
                // download Ringtones.plist
                var data = DownloadFile(client, RINGTONESPLIST);
                // disconnect
                client.Close();
                client.Dispose();
                device.Close();
                device.Dispose();

                var plist = PropertyListParser.Parse(data);
                lstAudioRemote.BeginUpdate();
                lstAudioRemote.Items.Clear();
                foreach (var entry in (NSDictionary)((NSDictionary)plist)["Ringtones"])
                {
                    var values = (NSDictionary)entry.Value;
                    var item = new ListViewItem { Name = entry.Key, Text = values["Name"].ToString(), ImageKey = "ringtone_remote", Group = lstAudioRemote.Groups[0] };
                    lstAudioRemote.Items.Add(item);
                }
                lstAudioRemote.EndUpdate();
            }
            catch (Exception ex) { HandleException(ex); }
        }
        #endregion

        #region iPhone
        static void UploadFile(AfcClientHandle client, string filename, byte[] data)
        {
            var handle = 0UL;
            LibiMobileDevice.Instance.Afc.afc_file_open(client, filename, AfcFileMode.FopenWronly, ref handle).ThrowOnError();
            LibiMobileDevice.Instance.Afc.afc_file_truncate(client, handle, 0UL).ThrowOnError();
            var filesize = (uint)data.Length;
            var complete = 0U;
            LibiMobileDevice.Instance.Afc.afc_file_write(client, handle, data, filesize, ref complete).ThrowOnError();
            LibiMobileDevice.Instance.Afc.afc_file_close(client, handle).ThrowOnError();
        }

        static byte[] DownloadFile(AfcClientHandle client, string filename, uint filesize)
        {
            var data = new byte[filesize];
            var handle = 0UL;
            LibiMobileDevice.Instance.Afc.afc_file_open(client, filename, AfcFileMode.FopenRdonly, ref handle).ThrowOnError();
            var complete = 0U;
            LibiMobileDevice.Instance.Afc.afc_file_read(client, handle, data, filesize, ref complete).ThrowOnError();
            LibiMobileDevice.Instance.Afc.afc_file_close(client, handle).ThrowOnError();
            return data;
        }

        static byte[] DownloadFile(AfcClientHandle client, string filename)
        {
            var info = FileInfo(client, filename);
            var size = GetFileSize(info);
            return DownloadFile(client, filename, size);
        }

        static void DeleteFile(AfcClientHandle client, string path)
        {
            LibiMobileDevice.Instance.Afc.afc_remove_path(client, path);
        }

        static ReadOnlyCollection<string> FileInfo(AfcClientHandle client, string filename)
        {
            LibiMobileDevice.Instance.Afc.afc_get_file_info(client, filename, out ReadOnlyCollection<string> fileInformation).ThrowOnError();
            return fileInformation;
        }

        static uint GetFileSize(ReadOnlyCollection<string> fileInformation)
        {
            return Convert.ToUInt32(fileInformation[fileInformation.IndexOf("st_size") + 1]);
        }

        static long GetFreeBytes(ReadOnlyCollection<string> deviceInformation)
        {
            return Convert.ToInt64(deviceInformation[deviceInformation.IndexOf("FSFreeBytes") + 1]);
        }

        static long GetTotalBytes(ReadOnlyCollection<string> deviceInformation)
        {
            return Convert.ToInt64(deviceInformation[deviceInformation.IndexOf("FSTotalBytes") + 1]);
        }

        static string RandomString(string characters, int length)
        {
            var random = new Random();
            var builder = new StringBuilder();
            for (var i = 0; i < length; i++)
                builder.Append(characters[random.Next(characters.Length)]);
            return builder.ToString();
        }

        static string Combine(params string[] paths)
        {
            var segments = new List<string>();
            foreach (var path in paths) segments.AddRange(path.Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries));
            for (var i = 0; i < segments.Count; i++)
            {
                if (segments[i] == "..") segments.RemoveRange(--i, 2);
                if (segments[i] == ".") segments.RemoveAt(i--);
            }
            return string.Format("/{0}", string.Join("/", segments));
        }
        #endregion

        public static void HandleException(Exception ex)
        {
            MessageBox.Show(ex.ToString(), ex.Source, MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        /// <summary>Add files to audio folder via file drop.</summary>
        /// <param name="files">Local file paths.</param>
        /// <returns>True if refresh is needed.</returns>
        private bool AddLocalFiles(IEnumerable<string> files)
        {
            try
            {
                var folder = new DirectoryInfo(AUDIOFOLDER);
                var refresh = false;
                foreach (var file in files)
                {
                    if (!File.Exists(file)) continue; // throw exception?
                    var name = Path.GetFileName(file);
                    var target = Path.Combine(folder.FullName, name);
                    if (file == target) continue;
                    if (File.Exists(target))
                    {
                        var result = MessageBox.Show(name, "Overwrite file?", MessageBoxButtons.YesNoCancel);
                        if (result == DialogResult.Cancel) break;
                        if (result != DialogResult.Yes) continue;
                    }
                    File.Copy(file, target, true);
                    refresh = true;
                }
                return refresh;
            }
            catch (Exception ex) { HandleException(ex); return false; }
        }

        /// <summary>Add files to audio folder from the phone.</summary>
        /// <param name="items">Remote listview items.</param>
        /// <returns>True if refresh is needed.</returns>
        private bool AddLocalFiles(ListView.SelectedListViewItemCollection items)
        {
            try
            {
                var folder = new DirectoryInfo(AUDIOFOLDER);
                var refresh = false;
                // connect
                var device = CurrentDevice;
                if (device == null) return false;
                // start service
                LibiMobileDevice.Instance.Afc.afc_client_start_service(device, out AfcClientHandle client, PROGRAMNAME).ThrowOnError();
                foreach (ListViewItem item in items)
                {
                    var name = item.Text + Path.GetExtension(item.Name); // RINGTONEEXTENSION
                    var target = Path.Combine(folder.FullName, name);
                    if (File.Exists(target))
                    {
                        var result = MessageBox.Show(name, "Overwrite file?", MessageBoxButtons.YesNoCancel);
                        if (result == DialogResult.Cancel) break;
                        if (result != DialogResult.Yes) continue;
                    }
                    // download XXXX.m4r
                    var data = DownloadFile(client, Combine(RINGTONESPATH, item.Name));
                    // store
                    File.WriteAllBytes(target, data);
                    refresh = true;
                }
                // disconnect
                client.Close();
                client.Dispose();
                device.Close();
                device.Dispose();
                return refresh;
            }
            catch (Exception ex) { HandleException(ex); return false; }
        }

        /// <summary>Add ringtones to phone via file drop.</summary>
        /// <param name="files">Local file paths.</param>
        /// <returns>True if refresh is needed.</returns>
        private bool AddRemoteFiles(IEnumerable<string> files)
        {
            try
            {
                var refresh = false;
                // connect
                var device = CurrentDevice;
                if (device == null) return false;
                // start service
                LibiMobileDevice.Instance.Afc.afc_client_start_service(device, out AfcClientHandle client, PROGRAMNAME).ThrowOnError();
                // download Ringtones.plist
                var data = DownloadFile(client, RINGTONESPLIST);
                var plist = PropertyListParser.Parse(data);
                var ringtones = new Dictionary<string, string>();
                var guids = new Dictionary<string, string>();
                foreach (var entry in (NSDictionary)((NSDictionary)plist)["Ringtones"])
                {
                    var values = (NSDictionary)entry.Value;
                    ringtones[values["Name"].ToString()] = entry.Key; // "Song name" -> "XXXX.m4r"
                    guids[entry.Key] = values["GUID"].ToString(); // "XXXX.m4r" -> "2C3003DD9E10A27B"
                }
                foreach (var file in files)
                {
                    if (!File.Exists(file)) continue; // throw exception?
                    if (!Path.GetExtension(file).Equals(RINGTONEEXTENSION)) continue;
                    var name = Path.GetFileNameWithoutExtension(file);
                    // get length of ringtone
                    var reader = new AudioFileReader(file);
                    var totaltime = reader.TotalTime.TotalMilliseconds;
                    reader.Close();
                    if (ringtones.ContainsValue(name))
                    {
                        var result = MessageBox.Show(name, "Overwrite ringtone?", MessageBoxButtons.YesNoCancel);
                        if (result == DialogResult.Cancel) break;
                        if (result != DialogResult.Yes) continue;

                        var key = ringtones[name];
                        // modify ringtone entry
                        var value = (NSDictionary)((NSDictionary)((NSDictionary)plist)["Ringtones"])[key];
                        value["Total Time"] = new NSNumber(totaltime);
                    }
                    else
                    {
                        // generate unique filename
                        var key = RandomString("ABCDEFGHIJKLMNOPQRSTUVWXYZ", 4) + RINGTONEEXTENSION;
                        while (guids.ContainsKey(key)) key = RandomString("ABCDEFGHIJKLMNOPQRSTUVWXYZ", 4) + RINGTONEEXTENSION;
                        ringtones[name] = key;
                        // generate unique GUID
                        var guid = RandomString("0123456789ABCDEF", 16);
                        while (guids.ContainsValue(guid)) guid = RandomString("0123456789ABCDEF", 16);
                        guids[key] = guid;
                        // new ringtone entry
                        var value = new NSDictionary();
                        value["Name"] = new NSString(name);
                        value["GUID"] = new NSString(guid);
                        value["Total Time"] = new NSNumber(totaltime);
                        ((NSDictionary)((NSDictionary)plist)["Ringtones"])[key] = value;
                    }
                    // upload ringtone
                    UploadFile(client, Combine(RINGTONESPATH, ringtones[name]), File.ReadAllBytes(file));
                    refresh = true;
                }
                if (refresh)
                {
                    // upload new Ringtones.plist
                    var stream = new MemoryStream();
                    PropertyListParser.SaveAsBinary(plist, stream);
                    UploadFile(client, RINGTONESPLIST, stream.ToArray());
                }
                // disconnect
                client.Close();
                client.Dispose();
                device.Close();
                device.Dispose();
                return refresh;
            }
            catch (Exception ex) { HandleException(ex); return false; }
        }

        private bool RenameLocalFile(ListViewItem node, string label)
        {
            try
            {
                var file = node.Name;
                var extension = Path.GetExtension(file);
                if (!label.EndsWith(extension)) label += extension;
                var target = Path.Combine(Path.GetDirectoryName(file), label);
                File.Move(file, target);
                node.Name = target;
                return true;
            }
            catch (Exception ex) { HandleException(ex); return false; }
        }

        private bool RenameRemoteFile(ListViewItem node, string label)
        {
            try
            {
                var found = false;
                // connect
                var device = CurrentDevice;
                if (device == null) return false;
                // start service
                LibiMobileDevice.Instance.Afc.afc_client_start_service(device, out AfcClientHandle client, PROGRAMNAME).ThrowOnError();
                var data = DownloadFile(client, RINGTONESPLIST);
                // download Ringtones.plist
                var plist = PropertyListParser.Parse(data);
                foreach (var entry in (NSDictionary)((NSDictionary)plist)["Ringtones"])
                {
                    if (entry.Key != node.Name) continue;
                    // modify Ringtones.plist
                    var values = (NSDictionary)entry.Value;
                    values["Name"] = new NSString(label);
                    found = true;
                    break;
                }
                if (found)
                {
                    // upload new Ringtones.plist
                    var stream = new MemoryStream();
                    PropertyListParser.SaveAsBinary(plist, stream);
                    UploadFile(client, RINGTONESPLIST, stream.ToArray());
                }
                // disconnect
                client.Close();
                client.Dispose();
                device.Close();
                device.Dispose();
                return found;
            }
            catch (Exception ex) { HandleException(ex); return false; }
        }

        public bool RemoveLocalFiles(ListView.SelectedListViewItemCollection items)
        {
            try
            {
                var refresh = false;
                foreach (ListViewItem item in items)
                {
                    var file = item.Name;
                    if (!File.Exists(file)) continue; // throw exception?
                    File.Delete(file);
                    refresh = true;
                }
                return refresh;
            }
            catch (Exception ex) { HandleException(ex); return false; }
        }

        private bool RemoveRemoteFiles(ListView.SelectedListViewItemCollection items)
        {
            try
            {
                var refresh = false;
                // connect
                var device = CurrentDevice;
                if (device == null) return false;
                // start service
                LibiMobileDevice.Instance.Afc.afc_client_start_service(device, out AfcClientHandle client, PROGRAMNAME).ThrowOnError();
                var data = DownloadFile(client, RINGTONESPLIST);
                // download Ringtones.plist
                var plist = PropertyListParser.Parse(data);
                foreach (ListViewItem item in items)
                {
                    // delete file
                    DeleteFile(client, Combine(RINGTONESPATH, item.Name));
                    // remove plist entry
                    ((NSDictionary)((NSDictionary)plist)["Ringtones"]).Remove(item.Name);
                    refresh = true;
                }
                if (refresh)
                {
                    // upload new Ringtones.plist
                    var stream = new MemoryStream();
                    PropertyListParser.SaveAsBinary(plist, stream);
                    UploadFile(client, RINGTONESPLIST, stream.ToArray());
                }
                return refresh;
            }
            catch (Exception ex) { HandleException(ex); return false; }
        }

        private void EditLocalFile(string path)
        {
            using (var cutter = new FrmCutter())
            {
                cutter.Init(path);
                var result = cutter.ShowDialog(this);
                if (result == DialogResult.OK) RefreshAudios();
            }
        }

        #region AudioLocalEvents
        private void LstAudioLocal_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                var item = lstAudioLocal.FocusedItem;
                btnCut.Enabled = item != null;
                if (item == null) plrAudioLocal.Reset();
                else plrAudioLocal.Init(item.Name);
            }
            catch (Exception ex) { HandleException(ex); }
        }

        private void LstAudioLocal_ItemDrag(object sender, ItemDragEventArgs e)
        {
            if (lstAudioLocal.SelectedItems.Count == 0) return;
            var files = new System.Collections.Specialized.StringCollection();
            foreach (ListViewItem item in lstAudioLocal.SelectedItems) files.Add(item.Name);
            var data = new DataObject();
            data.SetData(DataFormats.Text, "AudioLocal"); // prevent self drop
            data.SetFileDropList(files);
            lstAudioLocal.DoDragDrop(data, DragDropEffects.Copy);
        }

        private void LstAudioLocal_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.Text) && e.Data.GetData(DataFormats.Text).ToString().Equals("AudioLocal")) return;
            if (e.Data.GetDataPresent(DataFormats.FileDrop) || e.Data.GetDataPresent(DataFormats.Serializable)) e.Effect = DragDropEffects.Copy;
        }

        private void LstAudioLocal_DragDrop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                if (!(e.Data.GetData(DataFormats.FileDrop) is string[] files)) return;
                if (AddLocalFiles(files)) RefreshAudios();
            }
            if (e.Data.GetDataPresent(DataFormats.Serializable))
            {
                if (!(e.Data.GetData(DataFormats.Serializable) is ListView.SelectedListViewItemCollection items)) return;
                if (AddLocalFiles(items)) RefreshAudios();
            }
        }

        private void LstAudioLocal_AfterLabelEdit(object sender, LabelEditEventArgs e)
        {
            var item = lstAudioLocal.Items[e.Item];
            plrAudioLocal.Reset();
            e.CancelEdit = string.IsNullOrEmpty(e.Label) || !RenameLocalFile(item, e.Label);
            plrAudioLocal.Init(item.Name);
        }

        private void LstAudioLocal_KeyDown(object sender, KeyEventArgs e)
        {
            if (lstAudioLocal.SelectedItems.Count == 0) return;
            if (e.KeyCode == Keys.Delete)
            {
                if (lstAudioLocal.SelectedItems.Count == 1)
                {
                    var result = MessageBox.Show(lstAudioLocal.SelectedItems[0].Text, "Delete file?", MessageBoxButtons.YesNo);
                    if (result != DialogResult.Yes) return;
                }
                else
                {
                    var result = MessageBox.Show(string.Format("Number of files: {0}", lstAudioLocal.SelectedItems.Count), "Delete files?", MessageBoxButtons.YesNo);
                    if (result != DialogResult.Yes) return;
                }
                plrAudioLocal.Reset();
                if (RemoveLocalFiles(lstAudioLocal.SelectedItems)) RefreshAudios();
            }
        }

        private void LstAudioLocal_DoubleClick(object sender, EventArgs e)
        {
            BtnCut_Click(null, null);
        }
        #endregion

        #region AudioRemoteEvents
        private void LstAudioRemote_ItemDrag(object sender, ItemDragEventArgs e)
        {
            if (lstAudioRemote.SelectedItems.Count == 0) return;
            var data = new DataObject();
            data.SetData(DataFormats.Text, "AudioRemote"); // prevent self drop
            data.SetData(DataFormats.Serializable, lstAudioRemote.SelectedItems);
            lstAudioLocal.DoDragDrop(data, DragDropEffects.Copy);
        }

        private void LstAudioRemote_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.Text) && e.Data.GetData(DataFormats.Text).ToString().Equals("AudioRemote")) return;
            if (e.Data.GetDataPresent(DataFormats.FileDrop)) e.Effect = DragDropEffects.Copy;
        }

        private void LstAudioRemote_DragDrop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                if (!(e.Data.GetData(DataFormats.FileDrop) is string[] files)) return;
                if (AddRemoteFiles(files)) RefreshDevice();
            }
        }

        private void LstAudioRemote_AfterLabelEdit(object sender, LabelEditEventArgs e)
        {
            var item = lstAudioLocal.Items[e.Item];
            e.CancelEdit = string.IsNullOrEmpty(e.Label) || !RenameRemoteFile(item, e.Label);
        }

        private void LstAudioRemote_KeyDown(object sender, KeyEventArgs e)
        {
            if (lstAudioRemote.SelectedItems.Count == 0) return;
            if (e.KeyCode == Keys.Delete)
            {
                if (lstAudioRemote.SelectedItems.Count == 1)
                {
                    var result = MessageBox.Show(lstAudioRemote.SelectedItems[0].Text, "Delete file?", MessageBoxButtons.YesNo);
                    if (result != DialogResult.Yes) return;
                }
                else
                {
                    var result = MessageBox.Show(string.Format("Number of files: {0}", lstAudioRemote.SelectedItems.Count), "Delete files?", MessageBoxButtons.YesNo);
                    if (result != DialogResult.Yes) return;
                }
                if (RemoveRemoteFiles(lstAudioRemote.SelectedItems)) RefreshDevice();
            }
        }
        #endregion

        private void CboDevice_SelectedIndexChanged(object sender, EventArgs e)
        {
            RefreshDevice();
        }

        private void BtnRefreshRemote_Click(object sender, EventArgs e)
        {
            RefreshDevice();
        }

        private void TmrRemoteRefresh_Tick(object sender, EventArgs e)
        {
            RefreshDevices();
        }

        private void BtnRefreshLocal_Click(object sender, EventArgs e)
        {
            RefreshAudios();
        }

        private void BtnCut_Click(object sender, EventArgs e)
        {
            var item = lstAudioLocal.FocusedItem;
            if (item == null) return;
            EditLocalFile(item.Name);
        }

        private void FrmMain_Deactivate(object sender, EventArgs e)
        {
            lstAudioLocal.SelectedItems.Clear();
        }
    }
}
