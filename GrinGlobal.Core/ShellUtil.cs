using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace GrinGlobal.Core {
    public class ShellUtil {

        private static byte[] __defaultZipFile = new byte[]{80,75,5,6,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0};

        private static void initZipFile(string fileName) {
            //Create an empty zip file
            using (FileStream fs = File.Create(fileName)) {
                fs.Write(__defaultZipFile, 0, __defaultZipFile.Length);
            }
        }

        public static void Zip(string inputFolderName, string outputZipFileName) {

            if (!Directory.Exists(inputFolderName)) {
                throw new InvalidOperationException("Source Folder '" + inputFolderName + "' does not exist.");
            }
            if (File.Exists(outputZipFileName)) {
                File.Delete(outputZipFileName);
            }

            initZipFile(outputZipFileName);


            //Copy a folder and its contents into the newly created zip file
            IShellDispatch sc = createShellClass();
            Folder input = sc.NameSpace(inputFolderName);
            Folder output = sc.NameSpace(outputZipFileName);
            FolderItems items = input.Items();
            output.CopyHere(items, 20);
        }

        public static void Unzip(string inputZipFilename, string outputFolderName) {

            if (!File.Exists(inputZipFilename)) {
                throw new InvalidOperationException("Source Zip File '" + inputZipFilename + "' does not exist.");
            }

            if (Directory.Exists(outputFolderName)) {
                Directory.Delete(outputFolderName, true);
            }
            Directory.CreateDirectory(outputFolderName);


            //ShellClass sc = new ShellClass();
            IShellDispatch sc = createShellClass();
            Folder input = sc.NameSpace(inputZipFilename);
            Folder output = sc.NameSpace(outputFolderName);
            FolderItems items = input.Items();
            output.CopyHere(items, 20);

        }

        private static IShellDispatch createShellClass() {
            Type shellAppType = Type.GetTypeFromProgID("Shell.Application");
            object ret = Activator.CreateInstance(shellAppType);
            return (IShellDispatch)ret;
        }

    }
}
