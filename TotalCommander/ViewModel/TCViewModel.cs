using System.Collections.ObjectModel;
using System.IO;
using System.Windows.Controls;
using System.Windows.Input;
using TotalCommander.Model;

namespace TotalCommander.ViewModel
{
    public class TCViewModel
    {
        ObservableCollection<DirectoryEntryData> entries = new ObservableCollection<DirectoryEntryData>();
        ObservableCollection<DirectoryEntryData> subEntries = new ObservableCollection<DirectoryEntryData>();

        public void Loaded(ListView listView)
        {
            foreach (string s in Directory.GetLogicalDrives())
            {
                DirectoryEntryData d = new DirectoryEntryData(s, s, "<Driver>", "<DIR>", Directory.GetLastWriteTime(s), "Images/dir.gif", EntryType.Dir);
                entries.Add(d);
            }
            listView.DataContext = entries;
        }

        public void LookItems(ListView sender, MouseButtonEventArgs e)
        {
            ListViewItem item = e.Source as ListViewItem;

            DirectoryEntryData entry = item.DataContext as DirectoryEntryData;

            if (entry.Type == EntryType.Dir)
            {
                subEntries.Clear();

                foreach (string s in Directory.GetDirectories(entry.Fullpath))
                {
                    DirectoryInfo dir = new DirectoryInfo(s);
                    DirectoryEntryData d = new DirectoryEntryData(
                        dir.Name, dir.FullName, "<Folder>", "<DIR>",
                        Directory.GetLastWriteTime(s),
                        "Images/folder.png", EntryType.Dir);
                    subEntries.Add(d);
                }
                foreach (string f in Directory.GetFiles(entry.Fullpath))
                {
                    FileInfo file = new FileInfo(f);
                    DirectoryEntryData d = new DirectoryEntryData(
                        file.Name, file.FullName, file.Extension, file.Length.ToString(),
                        file.LastWriteTime,
                        "Images/file.png", EntryType.File);
                    subEntries.Add(d);
                }

                sender.DataContext = subEntries;
            }
        }
    }
}
