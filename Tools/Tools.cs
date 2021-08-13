using DevExpress.XtraEditors;
using Microsoft.Win32;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Management;
using System.Windows.Forms;
using WindowsCAssistant.Dosbox;
using WindowsCAssistant.Model;

namespace WindowsCAssistant
{
    public static class Tools
    {
        #region Attributes
        public static string DosBoxStartUpPath { get; } = @"c:\PROGRA~2\MINIMA~1\TURBOC~1";
        public static string DosBoxConfigPath { get; } = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"\DOSBox\dosbox-0.74-3.conf";
        #endregion 

        public class Message
        {
            public static string Error = "Ha ocurrido un error en la ejecución del programa.";
            public static string GenerateSuccess = "Se ha generado el codigo correctamente.";
            public static string Overwrite = "Ya existe un archivo con ese nombre ¿Desea sobreescribirlo?";
            public static string InstalledSuccess = "Se ha instalado correctamente...";
        }

        #region Methods
        public static void openDOSBox()
        {
            Process Dosbox = new Process();
            Dosbox.StartInfo.FileName = Configuration.Default.DosboxPath;
            Dosbox.StartInfo.Arguments = " -noconsole -userconf";
            Dosbox.Start();
            Dosbox.WaitForExit();
        }


        public static void OpenCFile(string nameFile, string pathFile, bool Execute)
        {
            try
            {
                nameFile = nameFile.Replace(" ", string.Empty);

                if (nameFile.Length > 10)
                {
                    DirectoryInfo directory = new DirectoryInfo(pathFile);
                    var files = directory.GetFiles("*.c");

                    int i = 1;

                    foreach (var item in files)
                    {
                        string name = item.Name.Replace(" ", string.Empty);
                        if (name == nameFile)
                            break;
                        else if (name.Length > 10)
                            if (nameFile.Contains(name.Substring(0, 8)))
                                    i++;
                    }
                    nameFile = nameFile.Substring(0, 6) + "~" + i;
                }

                Settings config = Archive.GetSettings();

                config.DosboxConfig.AutoExecute = "mount c c:/ \nc:\ncd tc20\\bin";
                config.DosboxConfig.AutoExecute += Execute ?
                    $"\ntcc -Ic:\\TC20\\INCLUDE -Lc:\\TC20\\LIB -nc:\\TC20\\output {ConvertToMSDosPath(pathFile) + nameFile.ToUpper()} c:\\tc20\\lib\\graphics.lib" +
                    //"\ncls" +
                    $"\nc:\\TC20\\output\\{nameFile.Split('.')[0].ToUpper()}"
                    :
                    $"\ntc {ConvertToMSDosPath(pathFile) + nameFile.ToUpper()}" +
                    "\ncls";

                Archive.SaveDosboxConfig(config.DosboxConfig);
                Archive.SaveSettings(config);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static void ExecuteCommand(string Command)
        {
            try
            {
                Process cmd = new Process();
                cmd.StartInfo.FileName = "cmd.exe";
                cmd.StartInfo.RedirectStandardInput = true;
                cmd.StartInfo.RedirectStandardOutput = true;
                cmd.StartInfo.CreateNoWindow = true;
                cmd.StartInfo.UseShellExecute = false;
                cmd.Start();

                cmd.StandardInput.WriteLine("cd \\");
                cmd.StandardInput.WriteLine(Command);
                cmd.StandardInput.Flush();
                cmd.StandardInput.Close();
                cmd.WaitForExit();
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        public static void checkPrograms()
        {
            if (File.Exists(@"C:\TC20\BIN\TC.exe") && File.Exists(@"C:\TC20\BIN\TCC.exe") && File.Exists(@"C:\TC20\BIN\tlink.exe"))
                Configuration.Default.HasTurboC = true;

            if (File.Exists(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\Minimal Tools\settings.json"))
            {
                Settings oldsSettings = Archive.GetSettings();
                Configuration.Default.HasVisualStudioCode = Configuration.Default.HasDosbox = false;

                if (File.Exists(oldsSettings.VSCodePath))
                {
                    Configuration.Default.VisualStudioCodePath = oldsSettings.VSCodePath;
                    Configuration.Default.HasVisualStudioCode = true;
                }
                if (File.Exists(oldsSettings.DOSBoxPath))
                {
                    Configuration.Default.DosboxPath = oldsSettings.DOSBoxPath;
                    Configuration.Default.HasDosbox = true;
                }
                if (Directory.Exists(oldsSettings.DirectoryPath))
                    Configuration.Default.Directory = oldsSettings.DirectoryPath;

                Configuration.Default.DefaultAction = oldsSettings.DefaultAction;
                Configuration.Default.CreateNewFile = oldsSettings.CreateNewFile;

                Archive.SaveSettings(oldsSettings);
            }
            else
            {
                Settings newSettings = new Settings();

                if (File.Exists(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"\Programs\Microsoft VS Code\Code.exe"))
                    newSettings.VSCodePath = Configuration.Default.VisualStudioCodePath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"\Programs\Microsoft VS Code\Code.exe";

                else if (File.Exists(Environment.GetEnvironmentVariable("ProgramW6432") + @"\Microsoft VS Code\Code.exe"))
                    newSettings.VSCodePath = Configuration.Default.VisualStudioCodePath = Environment.GetEnvironmentVariable("ProgramW6432") + @"\Microsoft VS Code\Code.exe";

                else if (File.Exists(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86) + @"\Microsoft VS Code\Code.exe"))
                    newSettings.VSCodePath = Configuration.Default.VisualStudioCodePath = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86) + @"\Microsoft VS Code\Code.exe";

                if (File.Exists($@"{Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86)}\DOSBox-0.74-3\DOSBox.exe"))
                    newSettings.DOSBoxPath = Configuration.Default.DosboxPath = $@"{Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86)}\DOSBox-0.74-3\DOSBox.exe";

                if (Directory.Exists(@"c:\TC20\BIN"))
                    newSettings.DirectoryPath = Configuration.Default.Directory = @"c:\TC20\BIN";

                newSettings.DefaultAction = Configuration.Default.DefaultAction = 'C';
                newSettings.CreateNewFile = Configuration.Default.CreateNewFile = true;
                
                Archive.SaveSettings(newSettings);
            }
        }

        public static string ConvertToMSDosPath(string path)
        {
            
            string pathInAction = string.Empty;
            string result = string.Empty;

            var SubsPath = path.Split('\\');

            foreach (var folder in SubsPath)
            {
                if (folder.Length == 0)
                    continue;
                else if (folder.Length > 8)
                {
                    DirectoryInfo FolderNow;
                    FolderNow = new DirectoryInfo(pathInAction);

                    var searchResult = FolderNow.GetDirectories();
                    int i = 1;
                    foreach (var item in searchResult)
                    {
                        if (item.Name == folder)
                            break;
                        else if (item.Name.Contains(folder.Substring(0, 8)))
                            i++;
                    }
                    result += folder.Replace(" ", string.Empty).Substring(0, 6) + "~" + i;
                }
                else
                    result += folder;

                result += @"\";
                pathInAction += folder + @"\";
            }
            return result;
        }
        #endregion
    }
}
