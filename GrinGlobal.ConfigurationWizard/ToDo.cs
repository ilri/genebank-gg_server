using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GrinGlobal.ConfigurationWizard {
    public class ActionDetail {

        public string SpecialCommand;
        public string FriendlyName;

        public string PrimaryExe;
        public string PrimaryArgs;
        public string PrimaryWorkingDirectory;

        public string TempExe;
        public string TempArgs;
        public string TempWorkingDirectory;

        public Dictionary<string, string> ConfigurationDictionary;

    }
}
