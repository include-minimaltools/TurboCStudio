using System;
using System.IO;
using Newtonsoft.Json;
using WindowsCAssistant.Dosbox;
using WindowsCAssistant.Model;

namespace WindowsCAssistant
{
    class Archive
    {
        public string Path { get; set; }
        public string Content { get; set; }

        public Archive(string path = null, string content = null)
        {
            Path = path;
            Content = content;
        }

        public bool Save()
        {
            try
            {
                File.WriteAllText(Path, Content);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool Exist()
        {
            try
            {
                if (File.Exists(Path))
                    return true;
                else
                    return false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static void SaveException(Exception ex)
        {
            try
            {
                string logbookPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\Minimal Tools\logbook.json";
                string exception = JsonConvert.SerializeObject(ex);

                if (!File.Exists(logbookPath))
                    File.WriteAllText(logbookPath, exception);
                else
                {
                    using (StreamWriter logbook = File.AppendText(logbookPath))
                    {
                        logbook.WriteLine(exception);
                        logbook.Close();
                    }
                }
            }
            catch { }
        }

        public static void SaveSettings(Settings settings)
        {
            try
            {
                Archive settingsFile = new Archive(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\Minimal Tools\settings.json", JsonConvert.SerializeObject(settings));
                settingsFile.Save();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static Settings GetSettings()
        {
            try
            {
                StreamReader settingsFile = new StreamReader(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\Minimal Tools\settings.json");
                return JsonConvert.DeserializeObject<Settings>(settingsFile.ReadLine());
            }
            catch (Exception ex)
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

                return newSettings;
            }
        }

        public static void SaveDosboxConfig(DosboxModel model)
        {
            string dbconfig = "# << Configuracion auto-generada por Turbo C Studio, Minimal Tools >>\n\n";

            foreach (string item in model.getModel())
                dbconfig += item + "\n";

            File.WriteAllText(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"\DOSBox\dosbox-0.74-3.conf", dbconfig);
        }
    }
}
