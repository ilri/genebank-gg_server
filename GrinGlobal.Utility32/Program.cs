using System;
using Microsoft.Win32;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Linq;
using System.Collections.Generic;

namespace GrinGlobal.Utility32 {
    class Program {
        static void Main(string[] args) {
            string regPath = null;
            string regType = null;
            string launchApp = null;
            string launchArgs = null;
            var launchArgList = new List<string>();
            bool showInfo = false;
            if (args != null && args.Length > 0) {
                for (int i = 0; i < args.Length; i++) {
                    switch (args[i].ToLower()) {
                        case "/readregkey":
                        case "-readregkey":
                        case "--readregkey":
                            if (i < args.Length - 1) {
                                regPath = args[++i];
                            }
                            break;
                        case "/type":
                        case "-type":
                        case "--type":
                            if (i < args.Length - 1) {
                                regType = args[++i];
                            }
                            break;
                        case "/info":
                        case "-info":
                        case "--info":
                            showInfo = true;
                            break;
                        case "/launch":
                        case "--launch":
                        case "-launch":
                            if (i < args.Length - 1) {
                                launchApp = args[++i];
                            }
                            while (i < args.Length - 1) {
                                launchArgs += " " + args[++i];
                                launchArgList.Add(args[i]);
                            }
                            break;
                    }
                }
            }

            //            Console.WriteLine(Toolkit.Is64BitOperatingSystem().ToString());

            if (!String.IsNullOrEmpty(regPath)) {

                var pos = regPath.LastIndexOf('\\');
                if (pos < 0 || regPath.Length <= pos) {
                    Console.WriteLine("Invalid registry path.  Does not contain '\\'.");
                    return;
                }
                var regValue = regPath.Substring(pos + 1);
                regPath = regPath.Substring(0, pos);

                switch (("" + regType).ToLower()) {
                    case "int16":
                    case "int32":
                    case "int64":
                    case "uint16":
                    case "uint32":
                    case "uint64":
                    case "short":
                    case "ushort":
                    case "long":
                    case "ulong":
                    case "int":
                    case "integer":
                    case "number":
                    case "dword":
                        Console.Write(Registry.GetValue(regPath, regValue, 0).ToString());
                        break;
                    case "string":
                    default:
                        Console.Write(Registry.GetValue(regPath, regValue, ""));
                        break;
                    case "string[]":
                        var values = Registry.GetValue(regPath, regValue, (string[])null) as string[];
                        Console.Write(String.Join(", ", values));
                        break;
                }

                //var input = Console.ReadLine();
                //if (!String.IsNullOrEmpty(input)) {
                //    Console.WriteLine("is64 bit process=" + (IntPtr.Size == 8).ToString());
                //    Console.ReadLine();
                //}

                return;
            }

            if (showInfo) {
                if (IntPtr.Size == 4) {
                    Console.WriteLine("ggutil32.exe is running as 32-bit process");
                } else {
                    Console.WriteLine("ggutil32.exe is running as 64-bit process");
                }
            }


            if (!String.IsNullOrEmpty(launchApp)) {

//                var directory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
//                var assemblyName = Path.Combine(directory, launchApp);
//                var assembly = Assembly.LoadFile(launchApp);

//                // find the entry point of the assembly
////                var type = assembly.GetTypes().FirstOrDefault(t => t.GetMethod("Main") != null);
////                var mi = type.GetMethod("Main");

//                // call the entry point of the wrapped assembly and forward the command line parameters
//                assembly.EntryPoint.Invoke(assembly.EntryPoint.GetType(), new object[] { launchArgList.ToArray() });

                Process.Start(launchApp, launchArgs);
                return;
            }

        }
    }
}
