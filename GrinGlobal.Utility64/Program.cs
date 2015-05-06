using System;
using Microsoft.Win32;
using System.Diagnostics;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace Circaware.AppEngine.Util64 {
    class Program {

        static void Main(string[] args) {

            try {
                string parentKeyName = null;
                string regPath = null;
                string regType = null;
                string workingDir = null;
                string launchApp = null;
                string launchArgs = null;
                string regValue = null; 
                bool writingRegKey = false;
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
                            case "/writeregkey":
                            case "-writeregkey":
                            case "--writeregkey":
                                writingRegKey = true;
                                if (i < args.Length - 1) {
                                    regPath = args[++i];
                                }
                                break;
                            case "/readregsubkeys":
                            case "-readregsubkeys":
                            case "--readregsubkeys":
                                if (i < args.Length - 1) {
                                    parentKeyName = args[++i];
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
                            case "/value":
                            case "-value":
                            case "--value":
                                if (i < args.Length - 1) {
                                    regValue = args[++i];
                                }
                                break;
                            case "/workingdir":
                            case "-workingdir":
                            case "--workingdir":
                                if (i < args.Length - 1){
                                    workingDir = args[++i];
                                }
                                break;
                            case "/launch":
                            case "--launch":
                            case "-launch":
                                if (i < args.Length - 1) {
                                    launchApp = args[++i];
                                }
                                while (i < args.Length - 1) {
                                    i++;
                                    if (args[i].Contains(" ") && args[i][0] != '"') {
                                        // doesn't have quotes, but needs quotes...
                                        launchArgs += @" """ + args[i] + @"""";
                                    } else {
                                        launchArgs += " " + args[i];
                                    }
                                }
                                break;
                        }
                    }
                }

                //            Console.WriteLine(Toolkit.Is64BitOperatingSystem().ToString());

                if (!String.IsNullOrEmpty(regPath)) {

                    // To debug this portion, add to Command Line in Debug property for the project:
                    // /readregkey "HKEY_LOCAL_MACHINE\SOFTWARE\Circaware\App Engine\AutoFormatURL" /type int


                    var pos = regPath.LastIndexOf('\\');
                    if (pos < 0 || regPath.Length <= pos) {
                        Console.WriteLine("Invalid registry path.  Does not contain '\\'.");
                        return;
                    }
                    var regValueName = regPath.Substring(pos + 1);
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
                            if (writingRegKey) {
                                Registry.SetValue(regPath, regValueName, regValue, RegistryValueKind.DWord);
                                Console.Write("Success");
                            } else {
                                int defaultValue = 0;
                                int.TryParse(regValue, out defaultValue);
                                Console.Write(getRegValue(regPath, regValueName, defaultValue));
                            }
                            break;
                        case "string":
                        default:
                            if (writingRegKey) {
                                Registry.SetValue(regPath, regValueName, regValue, RegistryValueKind.String);
                                Console.Write("Success");
                            } else {
                                Console.Write(getRegValue(regPath, regValueName, regValue + ""));
                            }
                            break;
                        case "string[]":
                            if (writingRegKey) {
                                var values = regValue.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
                                Registry.SetValue(regPath, regValueName, values, RegistryValueKind.MultiString);
                                Console.Write("Success");
                            } else {
                                var values = getRegValue(regPath, regValueName, (string[])null);
                                Console.Write(String.Join(", ", values));
                            }
                            break;
                    }

                    //var input = Console.ReadLine();
                    //if (!String.IsNullOrEmpty(input)) {
                    //    Console.WriteLine("is64 bit process=" + (IntPtr.Size == 8).ToString());
                    //    Console.ReadLine();
                    //}

                    return;
                } else if (!String.IsNullOrEmpty(parentKeyName)) {
                    using (var rk = Registry.LocalMachine.OpenSubKey(parentKeyName)) {
                        if (rk == null) {
                            // key did not exist
                            Console.Write("");
                        } else {
                            // key exists, spit out keynames
                            Console.Write(String.Join(", ", rk.GetSubKeyNames()));
                        }
                    }
                    return;
                }

                if (showInfo) {
                    if (IntPtr.Size == 4) {
                        Console.WriteLine("apputil64.exe is running as 32-bit process");
                    } else {
                        Console.WriteLine("apputil64.exe is running as 64-bit process");
                    }
                }

                if (!String.IsNullOrEmpty(launchApp)) {

                    // To debug this portion, add following as Command Line to Debug in project properties:
                    // /launch notepad.exe "C:\hi.txt"


                    var psi = new ProcessStartInfo(launchApp, launchArgs);
                    psi.CreateNoWindow = true;
                    psi.WindowStyle = ProcessWindowStyle.Hidden;
                    psi.UseShellExecute = false;
                    psi.RedirectStandardOutput = true;
                    psi.RedirectStandardError = true;

                    if (!String.IsNullOrEmpty(workingDir)) {
                        psi.WorkingDirectory = workingDir;
                    }

                    //if (Debugger.IsAttached) {
                    //    if (DialogResult.Cancel == MessageBox.Show("Going to run:\nWorkingDir=" + psi.WorkingDirectory + "\nExe=" + psi.FileName + "\nArgs=" + psi.Arguments, "Is 64bit=" + (IntPtr.Size == 8), MessageBoxButtons.OKCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2)) {
                    //        return;
                    //    }
                    //}

                    __processError = new StringBuilder();
                    __processOutput = new StringBuilder();
                    var p = new Process();
                    p.StartInfo = psi;
                    p.ErrorDataReceived += new DataReceivedEventHandler(p_ErrorDataReceived);
                    p.OutputDataReceived += new DataReceivedEventHandler(p_OutputDataReceived);

                    p.Start();

                    p.BeginErrorReadLine();
                    p.BeginOutputReadLine();

                    while (!p.HasExited) {
                        Thread.Sleep(100);
                        Application.DoEvents();
                    }

                    string output = __processOutput.ToString() + " " + __processError.ToString();
                    Console.WriteLine(output);
                    return;
                }

            } catch (Exception ex) {
                var msg = "Error: " + ex.Message + " CommandLine: " + String.Join(", ", args);
                Console.WriteLine(msg);
                EventLog.WriteEntry("apputil64", msg, EventLogEntryType.Error);
            }
        }

        private static StringBuilder __processError;
        private static StringBuilder __processOutput;

        static void p_ErrorDataReceived(object sender, DataReceivedEventArgs e) {
            if (__processError != null) {
                __processError.AppendLine(e.Data);
            }
        }

        static void p_OutputDataReceived(object sender, DataReceivedEventArgs e) {
            if (__processOutput != null) {
                __processOutput.AppendLine(e.Data);
            }
        }

        static string getRegValue(string regPath, string regKey, string defaultValue) {
            var output = Registry.GetValue(regPath, regKey, defaultValue) as string;
            if (output == null) {
                output = defaultValue;
            }
            return output;
        }

        static int getRegValue(string regPath, string regKey, int defaultValue) {
            var output = Registry.GetValue(regPath, regKey, null);
            if (output == null) {
                return defaultValue;
            } else {
                var ival = 0;
                if (int.TryParse(output.ToString(), out ival)){
                    return ival;
                } else {
                    return defaultValue;
                }
            }
        }

        static string[] getRegValue(string regPath, string regKey, string[] defaultValue) {
            var output = Registry.GetValue(regPath, regKey, defaultValue) as string[];
            if (output == null) {
                output = defaultValue;
            }
            return output;
        }
    }
}
