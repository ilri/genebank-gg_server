using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace GrinGlobal.UAC {
    public class Program {
        public static void Main(string[] args) {
            if (args == null || args.Length == 0) {
                showUsage();
                return;
            }

            bool waitForExit = false;
            bool hideWindow = false;
            string program = null;
            List<string> otherArgs = new List<string>();

            for (int i = 0; i < args.Length; i++) {
                string a = args[i];
                switch (a.ToLower()) {
                    case "/w":
                    case "-w":
                    case "/wait":
                    case "-wait":
                    case "--wait":
                        waitForExit = true;
                        break;
                    case "/hide":
                    case "-hide":
                    case "--hide":
                        hideWindow = true;
                        break;
                    case "/showusage":
                    case "-showusage":
                    case "--showusage":
                    case "/?":
                    case "-?":
                    case "--?":
                    case "/help":
                    case "-help":
                    case "--help":
                        showUsage();
                        return;
                    default:
                        if (program == null) {

                            if (a.StartsWith("/") || a.StartsWith("-")) {
                                showUsage();
                                return;
                            }

                            if (a.Contains(" ")) {
                                program = @"""" + a + @"""";
                            } else {
                                program = a;
                            }
                        } else {
                            if (a.Contains(" ")) {
                                otherArgs.Add(@"""" + a + @"""");
                            } else {
                                otherArgs.Add(a);
                            }
                        }
                        break;
                }
            }


            if (String.IsNullOrEmpty(program)) {
                throw new ArgumentException("Name of program to run under UAC elevated privileges must be given.");
            }

            ProcessStartInfo psi = new ProcessStartInfo(program, String.Join(" ", otherArgs.ToArray()));

            if (hideWindow) {
                psi.CreateNoWindow = true;
                psi.WindowStyle = ProcessWindowStyle.Hidden;
            } else {
                psi.CreateNoWindow = false;
                psi.WindowStyle = ProcessWindowStyle.Normal;
            }

            try {
                if (program.StartsWith(@"""")) {
                    program = program.Substring(1);
                }
                if (program.EndsWith(@"""")) {
                    program = program.Substring(0, program.Length - 1);
                }

                psi.UseShellExecute = true;

                if (IsVistaOrBetter) {
                    psi.Verb = "runas";
                }
            } catch (Exception ex) {
                Console.WriteLine("Error: " + ex.Message + " -- Exe=" + psi.FileName + ", Args=" + psi.Arguments);
            }

            try {

                //Console.WriteLine("Hit enter to execute: " + psi.FileName + " " + psi.Arguments + " in directory " + psi.WorkingDirectory + " ... ");
                //Console.ReadLine();

                Process p = Process.Start(psi);
                if (waitForExit) {

                    p.WaitForExit();

                }
            } catch (Exception ex2) {
                Console.WriteLine("Error: " + ex2.Message + " -- Exe=" + psi.FileName + ", Args=" + psi.Arguments + ", WorkingDirectory=" + psi.WorkingDirectory);
            }

            return;

        }

        private static void p_ErrorDataReceived(object sender, DataReceivedEventArgs e) {
            Console.WriteLine(e.Data);
        }

        private static void p_OutputDataReceived(object sender, DataReceivedEventArgs e) {
            Console.WriteLine(e.Data);
        }


        private static void showUsage() {
            Console.WriteLine(@"
Usage:  gguac.exe [/wait] [/hide] program [args]
      Prompts for elevation via UAC then runs program with elevated privileges.
      /wait            wait for program to finish before exiting. 
      /hide            suppresses the command box window from displaying

      Note: uac command uses Shell Execute to prompt for UAC.
        This means output cannot be redirected nor can the working directory
        be set.  Relative paths may not work properly.
     
");
            //     /workingdir      sets working directory for program

        }

        private static bool IsVistaOrBetter {
            get {
                return Environment.OSVersion.Version.Major >= 6
                    && Environment.OSVersion.Version.Minor >= 0;
            }
        }

    }
}
