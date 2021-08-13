using DevExpress.XtraEditors;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WindowsCAssistant.Model;

namespace WindowsCAssistant
{
    public partial class frmSettings : XtraForm
    {
        public frmSettings()
        {
            InitializeComponent();
        }

        #region Events

        private void frmSettings_Load(object sender, EventArgs e)
        {
            try
            {
                btnVSCodePath.Text = Configuration.Default.VisualStudioCodePath;
                btnDosBoxPath.Text = Configuration.Default.DosboxPath;
                btnDir.Text = Configuration.Default.Directory;
                icbeDefaultAction.EditValue = Configuration.Default.DefaultAction;
                ceNewCFile.Checked = Configuration.Default.CreateNewFile;
            }
            catch
            {
                XtraMessageBox.Show(Tools.Message.Error, Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        
        private void btnVSCodePath_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            try
            {
                string newPath = getPath("Code");
                if (newPath != string.Empty)
                    btnVSCodePath.Text = newPath;
            }
            catch
            {
                XtraMessageBox.Show(Tools.Message.Error, Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnDosBoxPath_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            try
            {
                string newPath = getPath("DOSBox");
                if(newPath != string.Empty)
                    btnDosBoxPath.Text = newPath;
            }
            catch
            {
                XtraMessageBox.Show(Tools.Message.Error, Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnDiscard_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                savePath();
            }
            catch
            {
                XtraMessageBox.Show(Tools.Message.Error, Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }    
        }

        private void btnDir_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            try
            {
                getFolderPath();
            }
            catch
            {
                XtraMessageBox.Show(Tools.Message.Error, Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        #endregion

        #region Methods

        private void savePath()
        {
            if (!verifyPath())
                return;


            char action = (char) icbeDefaultAction.EditValue;


            Configuration.Default.VisualStudioCodePath = btnVSCodePath.Text;
            Configuration.Default.DosboxPath = btnDosBoxPath.Text;
            Configuration.Default.Directory = btnDir.Text;
            Configuration.Default.DefaultAction = action;
            Configuration.Default.CreateNewFile = ceNewCFile.Checked;

            Archive.SaveSettings(new Settings(btnDosBoxPath.Text, btnVSCodePath.Text, btnDir.Text,action,ceNewCFile.Checked));

            XtraMessageBox.Show("Se han guardado los cambios correctamente", Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
            Close();
        }

        private void getFolderPath()
        {
            using(OpenFileDialog Directory = new OpenFileDialog())
            {
                Directory.Title = "Elegir directorio";
                Directory.InitialDirectory = Configuration.Default.Directory;
                Directory.Filter = "Carpeta |*.folder";

                Directory.CheckFileExists = Directory.ValidateNames = false;
                Directory.CheckPathExists = true;
                
                Directory.FileName = "Seleccionar esta carpeta";

                if (Directory.ShowDialog() == DialogResult.OK)
                {
                    string directory = string.Empty;
                    foreach (string folder in Directory.FileName.Split('\\'))
                    {
                        if (!folder.Contains(".folder"))
                            directory += folder + '\\';
                    }
                    btnDir.Text = directory;
                }
            }
        }

        private bool verifyPath()
        {
            if (btnDosBoxPath.Text == string.Empty || !btnDosBoxPath.Text.Contains("DOSBox.exe"))
                XtraMessageBox.Show("La dirección de DosBox es incorrecta", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);

            else if (btnVSCodePath.Text == string.Empty || !btnVSCodePath.Text.Contains("Code.exe"))
                XtraMessageBox.Show("La dirección de VS Code es incorrecta", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);

            else if (btnDir.Text == string.Empty || btnDir.Text.Contains("Program Files") || btnDir.Text.Contains("Archivos de Programas"))
                XtraMessageBox.Show("La dirección del directorio es inválida", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);

            else if (icbeDefaultAction == null)
                XtraMessageBox.Show("Debe escoger una acción por defecto", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);

            else if (XtraMessageBox.Show("¿Desea guardar los cambios?", Text, MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.Yes)
                return true;

            return false;
        }

        private string getPath(string name)
        {
            using (OpenFileDialog Program = new OpenFileDialog())
            {
                Program.Title = "Nueva dirección";
                Program.InitialDirectory = "C:\\";
                Program.FileName = "Code";
                Program.Filter = "Application | *.exe";
                if(Program.ShowDialog() == DialogResult.OK)
                    return Program.FileName;
            }
            return string.Empty;
        }

        #endregion

        
    }
}