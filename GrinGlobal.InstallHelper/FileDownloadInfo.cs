using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GrinGlobal.InstallHelper {
    public class FileDownloadInfo {

        public FileDownloadInfo() {
            ProductGuid = Guid.Empty;
        }

        public string DisplayName { get; set; }
        public string InstalledVersion { get; set; }
        public string LatestVersion { get; set; }
        public long SizeInBytes { get; set; }
        public string AppRelativeUrl { get; set; }
        public string AbsoluteUrl { get; set; }
        public string FileName { get; set; }
        public string FullFilePath { get; set; }
        public bool NeedToDownload { get; set; }
        public bool NeedToInstall { get; set; }
        public bool TriedToInstall { get; set; }
        public string UninstallString { get; set; }
        public Guid ProductGuid { get; set; }
        public FileDownloadInfo Parent { get; set; }
        public FileDownloadInfo Child { get; set; }
        public string Status { get; set; }

        public bool IsInstalledOlderThanLatest() {
            return Utility.IsVersionAGreater(LatestVersion, InstalledVersion);
            //if (String.IsNullOrEmpty(LatestVersion)) {
            //    // no 'latest version' specified, assume installed is current
            //    return false;
            //} else if (String.IsNullOrEmpty(InstalledVersion)) {
            //    // no 'installed version' exists, assume it's out of date if a 'latest version' exist
            //    return !String.IsNullOrEmpty(LatestVersion);
            //} else {
            //    // both versions exist, compare each portion of version numbers (i.e. X.Y.Z)
            //    string[] installed = InstalledVersion.Split(new char[] { '.' });
            //    string[] latest = LatestVersion.Split(new char[] { '.' });

            //    int i = 0;
            //    while (i < installed.Length && i < latest.Length) {
            //        if (Utility.ToInt32(installed[i], 0) < Utility.ToInt32(latest[i], 0)) {
            //            return true;
            //        }
            //        i++;
            //        if (i >= installed.Length && i < latest.Length) {
            //            return true;
            //        }
            //    }

            //    return false;
            //}
        }

        public override string ToString() {
            return DisplayName + ", FileName=" + FileName + ", InstalledVersion=" + InstalledVersion + ", LatestVersion=" + LatestVersion + ", SizeInMB=" + SizeInMB + ", NeedsInstalled=" + NeedToInstall + ", NeedsDownloaded=" + NeedToDownload + ", AppRelativeUrl=" + AppRelativeUrl;
        }

        public decimal SizeInKB {
            get {
                return ((decimal)SizeInBytes) / 1024.0M;
            }
        }

        public decimal SizeInMB {
            get {
                return ((decimal)SizeInBytes) / 1024.0M / 1024.0M;
            }
        }

        public static string DownloadedCacheFolder(string componentType) {
            return Utility.ResolveDirectoryPath("*Application Data*/GRIN-Global/Updater/downloaded/" + componentType, true);
        }

        public static string InstalledCacheFolder(string componentType) {
            return Utility.ResolveDirectoryPath("*Application Data*/GRIN-Global/Updater/installed/" + componentType, true);
        }

    }
}
