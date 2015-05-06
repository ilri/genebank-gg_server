using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GrinGlobal.Web {
    public class UploadedFileInfo {
        public string DisplayName { get; set; }
        public string FileName { get; set; }
        public DateTime CreationTime { get; set; }
        public bool IsValid { get; set; }
        public DateTime LastWriteTime { get; set; }
        public string Size { get; set; }
    }

}
