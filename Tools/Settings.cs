using System;
using WindowsCAssistant.Dosbox;

namespace WindowsCAssistant.Model
{
    class Settings
    {
        public string DOSBoxPath { get; set; }
        public string VSCodePath { get; set; }
        public string DirectoryPath { get; set; }
        public char DefaultAction { get; set; }
        public bool CreateNewFile { get; set; }
        public DosboxModel DosboxConfig { get; set; }

        public Settings(string DOSBox = null, string VSCode = null, string Dir = null, char defAction = '\0', bool newFile = false, DosboxModel dbconfig = null)
        {
            DOSBoxPath = DOSBox == null ? $@"{Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86)}\DOSBox-0.74-3\DOSBox.exe" : DOSBox;
            VSCodePath = VSCode == null ? string.Empty : VSCode;
            DirectoryPath = Dir == null ? string.Empty : Dir;
            DefaultAction = defAction;
            CreateNewFile = newFile;
            DosboxConfig = dbconfig == null ? new DosboxModel() : dbconfig;
        }
    }
}
