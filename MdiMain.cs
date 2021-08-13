using DevExpress.Utils;
using DevExpress.XtraEditors;
using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using WindowsCAssistant.Dosbox;
using WindowsCAssistant.Model;

namespace WindowsCAssistant
{
    public partial class MdiMain : XtraForm
    {
        public MdiMain()
        {
            InitializeComponent();
            
        }

        #region Events
        private void MdiMain_Load(object sender, EventArgs e)
        {
            WaitDialogForm WaitDialog = new WaitDialogForm("Cargando datos...", "Turbo C Studio");
            try
            {
                Tools.checkPrograms();
                if (Configuration.Default.HasTurboC)
                    uploadFiles();
                WaitDialog.Close();
            }
            catch (Exception ex)
            {
                Archive.SaveException(ex);
                if(WaitDialog.Enabled)
                    WaitDialog.Close();

                XtraMessageBox.Show(Tools.Message.Error + "\n" + ex.Message, Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void gvFiles_PopupMenuShowing(object sender, DevExpress.XtraGrid.Views.Grid.PopupMenuShowingEventArgs e)
        {
            pmFiles.ShowPopup(Cursor.Position);
        }

        #endregion

        #region Buttons

        #region Visual Studio Code
        private void btnOpenVSCode_Click(object sender, EventArgs e)
        {
            try
            {
                Tools.checkPrograms();
                if (!Configuration.Default.HasVisualStudioCode)
                {
                    XtraMessageBox.Show("Necesita tener instalado Visual Studio Code.", "Programa faltante", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }

                WaitDialogForm WaitForm = new WaitDialogForm("Se esta cargando Visual Studio Code...", "Visual Studio Code");
                Tools.ExecuteCommand($@"""{Configuration.Default.VisualStudioCodePath}""");
                WaitForm.Close();
            }
            catch (Exception ex)
            {
                Archive.SaveException(ex);
                XtraMessageBox.Show(Tools.Message.Error, Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void bbiOpenVSCode_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            WaitDialogForm WaitDialog = new WaitDialogForm("Ejecutando Visual Studio...", "Espere un momento");
            try
            {
                OpenInVSCode(WaitDialog);
            }
            catch(Exception ex)
            {
                Archive.SaveException(ex);
                if (WaitDialog.Enabled)
                    WaitDialog.Close();
                XtraMessageBox.Show(Tools.Message.Error, Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnInstallVSCode_Click(object sender, EventArgs e)
        {
            try
            {
                Process.Start("https://code.visualstudio.com/download");
                Tools.checkPrograms();
            }
            catch (Exception ex)
            {
                Archive.SaveException(ex);
                XtraMessageBox.Show(Tools.Message.Error, Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        #endregion

        #region DosBox
        private void InstallDOSBox_Click(object sender, EventArgs e)
        {
            WaitDialogForm WaitDialog = new WaitDialogForm("Abriendo instalador de DOSBox...", "Instalandor de Windows C Assistant");
            try
            {
                DOSBoxInstallerExecute(WaitDialog);
                WaitDialog.Close();
            }
            catch (Exception ex)
            {
                Archive.SaveException(ex);
                if (WaitDialog.Enabled)
                    WaitDialog.Close();
                XtraMessageBox.Show(Tools.Message.Error, Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnOpenDOSBox_Click(object sender, EventArgs e)
        {
            try
            {
                Tools.checkPrograms();
                if (!Configuration.Default.HasTurboC)
                {
                    XtraMessageBox.Show("Necesita tener instalado Turbo C 2.0.", "Programa faltante", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }
                if (!Configuration.Default.HasDosbox)
                {
                    XtraMessageBox.Show("Necesita tener instalado DOSBox-0.74-3.", "Programa faltante", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }

                using (OpenFileDialog Directory = new OpenFileDialog())
                {
                    Directory.Title = "Elegir programa";
                    Directory.InitialDirectory = Configuration.Default.Directory;

                    Directory.DefaultExt = "*.c";
                    Directory.Filter = "C | *.c";

                    Directory.CheckFileExists = Directory.ValidateNames = false;
                    Directory.CheckPathExists = true;

                    if (Directory.ShowDialog() == DialogResult.OK)
                    {
                        string currentDirectory = string.Empty;
                        foreach (var item in Directory.FileName.Split('\\'))
                            currentDirectory += item != Directory.SafeFileName ? item + "\\" : string.Empty;
                        Tools.OpenCFile(Directory.SafeFileName, currentDirectory, false);
                        Tools.openDOSBox();
                    }
                }
            }
            catch (Exception ex)
            {
                Archive.SaveException(ex);
                XtraMessageBox.Show(Tools.Message.Error, Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void bbiOpenDOSBox_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            WaitDialogForm WaitDialog = new WaitDialogForm("Ejecutando DOSBox...", "Espere un momento");
            try
            {
                OpenInTurboC(WaitDialog);
            }
            catch (Exception ex)
            {
                Archive.SaveException(ex);
                if (WaitDialog.Enabled)
                    WaitDialog.Close();
                XtraMessageBox.Show(Tools.Message.Error, Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            
        }

        private void bbiExecute_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            WaitDialogForm WaitDialog = new WaitDialogForm("Ejecutando DosBox...", "Espere un momento");
            try
            {
                Compile(WaitDialog);   
            }
            catch (Exception ex)
            {
                Archive.SaveException(ex);
                if (WaitDialog.Enabled)
                    WaitDialog.Close();

                XtraMessageBox.Show(Tools.Message.Error, Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        #endregion

        #region C programs
        private void btnNewForm_Click(object sender, EventArgs e)
        {
            try
            {
                Tools.checkPrograms();
                if (!Configuration.Default.HasTurboC)
                {
                    XtraMessageBox.Show("Necesita tener instalado Turbo C 2.0 para crear un nuevo formulario.", "Programa faltante", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }
                if (!Configuration.Default.HasDosbox)
                {
                    XtraMessageBox.Show("Necesita tener instalado DOSBox-0.74-3 para crear un nuevo formulario.", "Programa faltante", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }
                if (!Configuration.Default.HasVisualStudioCode)
                {
                    XtraMessageBox.Show("Necesita tener instalado Visual Studio Code para crear un nuevo formulario.", "Programa faltante", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }


                WaitDialogForm WaitForm = new WaitDialogForm("Se están cargando los archivos necesarios...", "Nuevo Formulario");
                frmNewForm frm = new frmNewForm();
                Visible = false;
                WaitForm.Close();
                frm.ShowDialog();
                Visible = true;
                uploadFiles();
            }
            catch (Exception ex)
            {
                Archive.SaveException(ex);
                XtraMessageBox.Show(Tools.Message.Error, Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnAddWindow_Click(object sender, EventArgs e)
        {
            WaitDialogForm WaitDialog = new WaitDialogForm("Cargando datos", "Instalando Windows C");
            try
            {
                Tools.checkPrograms();
                if (Configuration.Default.HasDosbox && Configuration.Default.HasTurboC)
                    AddWindow(WaitDialog);
                else
                {
                    WaitDialog.Caption = "Programas insuficientes";
                    Thread.Sleep(1000);
                    WaitDialog.Caption = "Cancelando...";
                    Thread.Sleep(1000);
                }
                WaitDialog.Close();
            }
            catch (Exception ex)
            {
                Archive.SaveException(ex);
                if (WaitDialog.Enabled)
                    WaitDialog.Close();
                XtraMessageBox.Show(Tools.Message.Error, Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnInstallTurboC_Click(object sender, EventArgs e)
        {
            try
            {
                Process TcInstaller = new Process();
                TcInstaller.StartInfo.FileName = $@"""{Application.StartupPath}\Resource\TC20.msi""";
                TcInstaller.Start();
                Enabled = false;
                TcInstaller.WaitForExit();
                Activate();
                Enabled = true;
                Tools.checkPrograms();
            }
            catch(Exception ex)
            {
                Archive.SaveException(ex);
                XtraMessageBox.Show(Tools.Message.Error, Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        #endregion

        #region Social Network
        private void btnGmail_Click(object sender, EventArgs e)
        {
            try
            {
                Clipboard.SetText("hsc.minimaltools@gmail.com");
                btnGmail.ToolTip = "Se ha copiado el correo electrónico al portapapel";
                Process.Start("https://www.google.com/intl/es/gmail/about/");
            }
            catch (Exception ex)
            {
                Archive.SaveException(ex);
                XtraMessageBox.Show(Tools.Message.Error, "Redes", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnGmail_MouseMove(object sender, MouseEventArgs e)
        {
            try
            {
                if (Clipboard.GetText() != "hsc.minimaltools@gmail.com")
                    btnGmail.ToolTip = "Copiar correo electrónico";
                else
                    btnGmail.ToolTip = "Se ha copiado el correo electrónico al portapapel";
            }
            catch (Exception ex)
            {
                Archive.SaveException(ex);
            }
        }

        private void btnDiscord_Click(object sender, EventArgs e)
        {
            try
            {
                Process.Start("https://discord.gg/CtxR7Q3HQJ");
            }
            catch (Exception ex)
            {
                Archive.SaveException(ex);
                XtraMessageBox.Show(Tools.Message.Error, "Redes", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        #endregion

        #region Others
        private void bbiRefresh_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                Tools.checkPrograms();
                uploadFiles();
            }
            catch (Exception ex)
            {
                Archive.SaveException(ex);
                XtraMessageBox.Show(Tools.Message.Error, Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnInfo_Click(object sender, EventArgs e)
        {
            try
            {
                frmInformation frm = new frmInformation();
                frm.ShowDialog();
            }
            catch (Exception ex)
            {
                Archive.SaveException(ex);
                XtraMessageBox.Show(Tools.Message.Error, "Redes", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnSettings_Click(object sender, EventArgs e)
        {
            try
            {
                frmSettings frm = new frmSettings();
                frm.ShowDialog();
                Tools.checkPrograms();
                uploadFiles();
            }
            catch (Exception ex)
            {
                Archive.SaveException(ex);
                XtraMessageBox.Show(Tools.Message.Error, "Redes", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void gvFiles_DoubleClick(object sender, EventArgs e)
        {
            WaitDialogForm WaitDialog = new WaitDialogForm("Ejecutando acción predeterminada...", "Espere un momento");
            switch (Configuration.Default.DefaultAction)
            {
                case 'A':
                    OpenInTurboC(WaitDialog);
                    break;
                case 'E':
                    OpenInVSCode(WaitDialog);
                    break;
                default:
                    Compile(WaitDialog);
                    break;
            }
        }
        #endregion

        #endregion


        #region Methods
        private void OpenInTurboC(WaitDialogForm WaitDialog)
        {
            Tools.checkPrograms();
            if (!Configuration.Default.HasTurboC)
            {
                WaitDialog.Close();
                XtraMessageBox.Show("Necesita tener instalado Turbo C 2.0 para abrir el archivo en DOSBox.", "Programa faltante", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            if (!Configuration.Default.HasDosbox)
            {
                WaitDialog.Close();
                XtraMessageBox.Show("Necesita tener instalado DOSBox-0.74-3 para poder abrir el archivo.", "Programa faltante", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            var file = gvFiles.GetRowCellValue(gvFiles.FocusedRowHandle, "Name").ToString();
            Tools.OpenCFile(file, Configuration.Default.Directory, false);
            Tools.ExecuteCommand($@"""{Configuration.Default.DosboxPath}"" -noconsole -userconf");
            if (Configuration.Default.CreateNewFile)
            {
                Thread.Sleep(1000);
                Tools.OpenCFile(string.Empty, Configuration.Default.Directory, false);
            }
            WaitDialog.Close();
        }

        private void OpenInVSCode(WaitDialogForm WaitDialog)
        {
            Tools.checkPrograms();
            if (!Configuration.Default.HasVisualStudioCode)
            {
                WaitDialog.Close();
                XtraMessageBox.Show("Necesita tener instalado Visual Studio Code para abrir el archivo en Visual Studio Code.", "Programa faltante", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            var file = gvFiles.GetRowCellValue(gvFiles.FocusedRowHandle, "Name").ToString();
            Tools.ExecuteCommand($@"""{Configuration.Default.VisualStudioCodePath}"" ""{Configuration.Default.Directory}\{file}""");
            Thread.Sleep(1000);
            WaitDialog.Close();
        }

        private void Compile(WaitDialogForm WaitDialog)
        {
            Tools.checkPrograms();
            if (!Configuration.Default.HasTurboC)
            {
                WaitDialog.Close();
                XtraMessageBox.Show("Necesita tener instalado Turbo C 2.0 para abrir el archivo en DOSBox.", "Programa faltante", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            if (!Configuration.Default.HasDosbox)
            {
                WaitDialog.Close();
                XtraMessageBox.Show("Necesita tener instalado DOSBox-0.74-3 para poder abrir el archivo.", "Programa faltante", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            var file = gvFiles.GetRowCellValue(gvFiles.FocusedRowHandle, "Name").ToString();
            Tools.OpenCFile(file, Configuration.Default.Directory, true);
            Tools.ExecuteCommand($@"""{Configuration.Default.DosboxPath}"" -noconsole -userconf");
            Thread.Sleep(1500);
            Tools.OpenCFile(Configuration.Default.CreateNewFile ? "NONAME.c" : file, Configuration.Default.Directory, false);
            WaitDialog.Close();
        }

        private void DOSBoxInstallerExecute(WaitDialogForm WaitDialog)
        {
            if (File.Exists($@"{Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86)}\DOSBox-0.74-3\DOSBox.exe"))
            {

                Settings settings = Archive.GetSettings();
                settings.DOSBoxPath = $@"{Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86)}\DOSBox-0.74-3\DOSBox.exe";
                Archive.SaveSettings(settings);
                Configuration.Default.HasDosbox = true;
                XtraMessageBox.Show("Ya se ha instalado DOSBox en su sistema", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            
            Process DosboxInstaller = new Process();
            DosboxInstaller.StartInfo.FileName = $@"""{Application.StartupPath}\Resource\DOSBox0.74-3.exe""";
            DosboxInstaller.Start();
            Enabled = false;
            WaitDialog.Close();
            DosboxInstaller.WaitForExit();
            Activate();
            Enabled = true;
            

            if (!File.Exists($@"{Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86)}\DOSBox-0.74-3\DOSBox.exe"))
            {
                XtraMessageBox.Show("No se ha instalado DOSBox en su sistema", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            else
            {
                XtraMessageBox.Show("Se ha instalado DOSBox correctamente", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Information);
                WaitDialogForm waitDialog = new WaitDialogForm("Ejecutando por primera vez...", "Instalandor de Windows C Assistant");
                try
                {
                    Thread.Sleep(3000);
                    Process DosboxConfig = new Process();
                    DosboxConfig.StartInfo.FileName = $@"{Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86)}\DOSBox-0.74-3\DOSBox.exe";
                    DosboxConfig.Start();
                    waitDialog.Caption = "Terminando ejecución";
                    Thread.Sleep(5000);
                    foreach (Process process in Process.GetProcesses())
                        if (process.ProcessName.ToUpper() == "DOSBOX")
                            process.Kill();

                    Settings settings = Archive.GetSettings();
                    settings.DOSBoxPath = $@"{Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86)}\DOSBox-0.74-3\DOSBox.exe";
                    Archive.SaveSettings(settings);
                    Configuration.Default.HasDosbox = true;

                    waitDialog.Close();
                }
                catch (Exception ex)
                {
                    Archive.SaveException(ex);
                    if (waitDialog.Enabled)
                        waitDialog.Close();
                    throw ex;
                }
            }
            Tools.checkPrograms();
        }
       
        private void uploadFiles()
        {
            DirectoryInfo bin = new DirectoryInfo(Configuration.Default.Directory);

            var CFiles = bin.GetFiles("*.c");
            gcFiles.DataSource = CFiles;
        }

        private void AddWindow(WaitDialogForm WaitDialog)
        {
            Archive Form = new Archive(@"C:\TC20\include\Form.H");
            Archive System = new Archive(@"C:\TC20\INCLUDE\System.h");
            Archive Window = new Archive(@"C:\TC20\BIN\Window.c");
            Thread.Sleep(300);

            WaitDialog.Caption = "Generando código de windows";
            Thread.Sleep(700);
            
            Window.Content = @"/*-----------------------------------------------------------------------------

                            Project C-Threshold

        Simulation of the <<Windows 10>> operating system in MS-DOS
            

                W   I   N   D   O   W   S               C

                                                        powered by Turbo C        

                Created by Gabriel Alejandro Ortiz Amador

                Universidad Nacional de Ingenieria [UNI]

                 >High Software Company - Minimal Tools

-------------------------------------------------------------------------------

    tested by Luis Miguel Pineda Joseph and Engel Gabriel Reyes Moreno

-------------------------------------------------------------------------------*/
#include <System.h>

void Main_PreEvent(void);
void Main_InEvent(void);
void Main_PostEvent(void);
void Desktop(void);

void main()
{
    LoadWin();
    TurnOn();
    windowhome.Visible= false;
    Desktop();
    Main_PreEvent();
}

void Main_PreEvent(void)
{
    int window_paint[2] = {0,0}, mine_paint[2] = {0,0}, uni_paint[2] = {0,0}, TurnOff[2] = {0,0}, Restart[2] = {0,0}, Sleep[2] = {0,0};
    mver();


    /*Pre-Event*/
    do
    {
        uni_paint[1] = uni_paint[0];
        mine_paint[1] = mine_paint[0];
        window_paint[1] = window_paint[0];
        TurnOff[1]=TurnOff[0];
        Restart[1]=Restart[0];
        Sleep[1]=Sleep[0];
        if (windowhome.Visible)
        {
            if(mxpos(1) > 0 && mxpos(1) < 25)
            {
                if(mypos(1) > 430 && mypos(1) < 454)
                    TurnOff[0] = true;
                else
                    TurnOff[0] = false;

                if(mypos(1) > 405 && mypos(1) < 429)
                    Restart[0] = true;
                else 
                    Restart[0] = false;
                if(mypos(1) > 380 && mypos(1) < 404)
                    Sleep[0]   = true;
                else
                    Sleep[0]   = false;
            }
            else
            {
                Sleep[0]=false;
                Restart[0]=false;
                TurnOff[0]=false;
            }
    
            if (TurnOff[0] != TurnOff[1])
            {
                mocultar();
                setfillstyle(SOLID_FILL,TurnOff[0] == true ? LIGHTGRAY : WHITE);    
                bar(0,430,25,454);
                setfillstyle(SOLID_FILL,BLACK);
                bar(11,442,11,438);
                arc(11,442,135,45, 3);
                mver();
            }
            if (Restart[0] != Restart[1])
            {
                mocultar();
                setfillstyle(SOLID_FILL,Restart[0] == true ? LIGHTGRAY : WHITE);    
                bar(0,405,25,429);
                setfillstyle(SOLID_FILL,BLACK);
                arc(11,417,0,270,3);
                line(13,417,15,416);
                line(15,416,15,418);
                line(15,418,13,417);
                mver();
            }
            if (Sleep[0] != Sleep[1])
            {
                mocultar();
                setfillstyle(SOLID_FILL,Sleep[0] == true ? LIGHTGRAY : WHITE);
                bar(0,380,25,404);
                setfillstyle(SOLID_FILL,Sleep[0] == true ? WHITE : BLACK);
                rectangle(8,389,15,394);
                putpixel(13,392,BLACK);
                putpixel(12,392,BLACK);
                putpixel(11,391,BLACK);
                putpixel(9,391,BLACK);
                mver();
            }        
        }
        
        if(mxpos(1)>54 && mypos(1)>455 && mxpos(1)<86 && mypos(1)<480)
            uni_paint[0] = true;
            else
                uni_paint[0] = false;

        if(mxpos(1)>26 && mypos(1)>455 && mxpos(1)<53 && mypos(1)<479)
            mine_paint[0] = true;

            else
                mine_paint[0] = false;

        if(mxpos(1)>0 && mypos(1)>455 && mxpos(1)<25 && mypos(1)<479)
            window_paint[0] = true;

            else
                window_paint[0] = false;
            
        if(uni_paint[0] != uni_paint[1])
        {
            mocultar();
            setfillstyle(SOLID_FILL,uni_paint[0]==false?WHITE:BLACK);
            bar(54,455,86,480);
            setfillstyle(SOLID_FILL,uni_paint[0]==false?BLACK:WHITE);
            icon(59,462,""uni"");
            mver();
        }

        if(window_paint[0] != window_paint[1])
        {
            mocultar();
            if(windowhome.Visible==true)
                setfillstyle(SOLID_FILL, BLACK);
            else
                setfillstyle(SOLID_FILL, window_paint[0]==false?WHITE:BLACK);

        bar(0,455,25,480);
            if(windowhome.Visible==true)
                if(System.Background==0)
                    setfillstyle(SOLID_FILL, WHITE);
                else
                    setfillstyle(SOLID_FILL, System.Background);
            else
                if(System.Background==0)
                    setfillstyle(SOLID_FILL, window_paint[0]==false?BLACK:WHITE);
                else
                    setfillstyle(SOLID_FILL, window_paint[0]==false?BLACK:System.Background);

        icon(5,460,""window"");
        mver();
    }
        
        if (mine_paint[0] != mine_paint[1])
        {
            mocultar();
    setfillstyle(SOLID_FILL, mine_paint[0]==false?WHITE:BLACK);
    bar(26,455,53,480);
    setfillstyle(SOLID_FILL, mine_paint[0]==false?BLACK:WHITE);
    setcolor(mine_paint[0]==false?BLACK:WHITE);
    icon(30,458,""mine"");
    mver();
}
    }
    while (mclick() == 0) ;
Main_InEvent();
}

void Main_InEvent(void)
{
    int k;

    if (windowhome.Visible == true)
    {
        k = 0;
        for (i = 0; i < 5; i++) for (j = 0; j < 3; j++)
            {
                if (mxpos(1) > 142 + (46 * j) && mypos(1) > 195 + (46 * i) && mxpos(1) < 185 + (46 * j) && mypos(1) < 238 + (46 * i))
                {
                    System.Background = k;
                    SaveWin();
                    Desktop();
                }
                k = k == 15 ? 0 : k + 1;
            }
    }

    if (mxpos(1) > 0 && mxpos(1) < 25)
    {
        if (mypos(1) > 430 && mypos(1) < 454)
        {
            mocultar();
            cleardevice();
            SystemDo(false);
        }

        if (mypos(1) > 405 && mypos(1) < 429)
        {
            mocultar();
            cleardevice();
            SystemDo(true);
            TurnOn();
            windowhome.Visible = false;
            Desktop();
            return;
        }

        if (mypos(1) > 380 && mypos(1) < 404)
        {
            mocultar();
            cleardevice();
            getch();
            TurnOn();
            windowhome.Visible = false;
            Desktop();
            mver();
        }
    }

    if (mxpos(1) > 0 && mypos(1) > 455 && mxpos(1) < 25 && mypos(1) < 479)
    {
        if (windowhome.Visible == true)
        {

            setfillstyle(SOLID_FILL, System.Background);
            bar(0, 454, 284, 174);
            windowhome.Visible = false;
        }
        else
        {
            formWindow();
            windowhome.Visible = true;
        }
    }

    else if (windowhome.Visible == true)
    {
        setfillstyle(SOLID_FILL, System.Background);
        bar(0, 454, 284, 174);
        windowhome.Visible = false;
    }

    if (mxpos(1) > 54 && mypos(1) > 455 && mxpos(1) < 86 && mypos(1) < 480)
    {
    }

    if (mxpos(1) > 26 && mypos(1) > 455 && mxpos(1) < 53 && mypos(1) < 479)
    {
    }

    while (mclick() != 0) ;
    Main_PostEvent();
}

void Main_PostEvent(void)
{
    Main_PreEvent();
}

void Desktop(void)
{
    mocultar();
    setfillstyle(SOLID_FILL, System.Background);
    bar(0, 0, 640, 480);
    setcolor(BLACK);
    setfillstyle(SOLID_FILL, WHITE);
    bar(-1, 455, 640, 480);
    TaskBar();
    setfillstyle(SOLID_FILL, BLACK);
    icon(5, 460, ""window"");
    icon(30, 458, ""mine"");
    setfillstyle(SOLID_FILL, BLACK);
    icon(59, 462, ""uni"");
    mver();
}

";
            WaitDialog.Caption = "Importando librerías";
            Thread.Sleep(500);

            System.Content = $@"#include <stdio.h>
#include <conio.h>
#include <GRAPHICS.H>
#include <time.h>
#include <stdlib.h>
#include <DOS.H>
#include <string.h>
#include <MOUSE.H>
#include <FORM.H>

#define duration 33
#define mode 1

int i, j, x, y;
int modo = DETECT, gmodo;
time_t Time;
char date[20] = {{'\0'}};
struct tm *today;
int charger[2][50] = {{  {{320,321,323,325,326,327,327,327,327,326,324,322,321,319,317,315,313,312,312,312,312,313,314,316,317,   320,321,323,325,326,327,327,327,327,326,324,322,321,319,317,315,313,312,312,312,312,313,314,316,317}},
                            {{368,368,369,370,371,373,375,377,379,381,382,383,384,384,383,382,381,379,377,375,373,371,370,369,368,   368,368,369,370,371,373,375,377,379,381,382,383,384,384,383,382,381,379,377,375,373,371,370,369,368}}}};

SystemProperties System;
FormProperties windowhome;

typedef struct
{{
  char Type[2];                     /* Tipo de archivo. establecido en ""BM"".                */
  unsigned long Size;               /* Tamano en BYTES del archivo                          */
  unsigned long Reserved;           /* Reservado. establecido en cero                       */
  unsigned long OffSet;             /* Desplazamiento hacia datos                           */
  unsigned long headsize;           /* Tama�o de la cabecera del bitmap. establecido en 40 */
  unsigned long Width;              /* Anchura en  pixeles.                                 */
  unsigned long Height;             /*  Altura en pixeles.                                  */
  unsigned int  Planes;             /* Numero de Planos. establecido en 1.                  */
  unsigned int  BitsPerPixel;       /* Numero de Bits por pixeles.                          */
  unsigned long Compression;        /* Compresion. Usually establecido en 0.                */
  unsigned long SizeImage;          /* Tamano en BYTES del bitmap.                          */
  unsigned long XPixelsPreMeter;    /* Pixeles Horizontales por metro.                      */
  unsigned long YPixelsPreMeter;    /* Pixeles Verticales por metro.                        */
  unsigned long ColorsUsed;         /* Numero de colores utilizados.                        */
  unsigned long ColorsImportant;    /* Numero de colores ""Importantes"".                     */
}} BMP;


void icon(int x, int y, char *name);
void TurnOn(void);
void WindowsSplashScreen(void);
void showImage(int x, int y, char *locate);
void LockScreen(void);
void Clock(void);
void TaskBar(void);
void formWindow(void);
void InitializeGraphics(void);
void setDateNow(void);
void SaveWin(void);
void LoadWin(void);

void TurnOn(void) 
{{
    initgraph(&modo, &gmodo, """");
    setDateNow();
    settextstyle(SMALL_FONT,HORIZ_DIR,5);
    outtextxy(425,375,""Powered by Turbo C"");
    /*showImage(220,140,""{Tools.DosBoxStartUpPath}//Resource//Dell.bmp"");*/
    delay(1000 * duration);
    cleardevice();
    for(i=0;i<5;i++)
    {{
        gotoxy(1,1);    printf(""_"");
        delay(100 * duration);
        gotoxy(1,1);    printf("" "");
        delay(100 * duration);
    }}
    cleardevice();
    WindowsSplashScreen();
    cleardevice();
    LockScreen();
    cleardevice();
    mver();
}}

void setDateNow(void)
{{
    char month[10]={{'\0'}}, year[10]={{'\0'}};

    Time = time(0);
    today = localtime(&Time);
    itoa(today->tm_mday,date,10);
    itoa(today->tm_mon+1,month,10);
    itoa(today->tm_year-100,year,10);
    strcat(date,""/"");
    strcat(date,month);
    strcat(date,""/"");
    strcat(date,year);
}}

void LockScreen(void)
{{
    char * month[] = {{""January"",""February"",""March"",""April"",""May"",""June"",""July"",""August"",""September"",""October"",""November"",""December""}};    
    int button[] = {{240,225,    400,225,    400,265,    240,265,    240,225}};
    int x, y, click, isButton, painting;

    /*showImage(0,0,""{Tools.DosBoxStartUpPath}//Resource//js.bmp"");*/
    mver();
    
    Time = time(0);
    today = localtime(&Time);
    setcolor(WHITE);
    settextstyle(2,0,10);
    outtextxy(0,400,month[today->tm_mon]);

    Clock();

    setlinestyle(SOLID_LINE,0,2);
    settextstyle(3,0,2);
    setfillstyle(SOLID_FILL,BLACK);
    setcolor(WHITE);

    isButton=false;
    painting = 1;
    do
    {{
        delay(10 * duration);
        click = mclick();
        x = mxpos(mode);
        y = mypos(mode);
        
        
        if( (x>240 && x<400) && (y>225 && y<265) )
        {{
            if(click==1)
            {{
                mocultar();
                setfillstyle(1,DARKGRAY);
                fillpoly(5,button);
                outtextxy(280,230,""Sign Up"");
                while(mclick()==1);
                isButton = true;
            }}

            if(painting == 0)
            {{
                painting = 1;
                mocultar();
                setfillstyle(1,LIGHTGRAY);
                fillpoly(5,button);
                outtextxy(280,230,""Sign Up"");
                mver();
            }}
        }}
        else
            if(painting == 1)
            {{
                painting = 0;
                mocultar();
                setfillstyle(1,BLACK);
                fillpoly(5,button);
                outtextxy(280,230,""Sign Up"");
                mver();
            }}

        
    }}
    while (isButton==false);
}}

void Clock(void)
{{
    char hour[6]={{'\0'}},minute[3]={{'\0'}};

    Time = time(0);
    today = localtime(&Time);

    itoa(today->tm_hour,hour,10);
    itoa(today->tm_min,minute,10);
    if(today->tm_min>9)
        strcat(hour,"":"");
    else
        strcat(hour,"":0"");
    strcat(hour,minute);
    settextstyle(2,0,10);

    setcolor(WHITE);
    outtextxy(0,350,hour);
    
    setcolor(BLACK);
    for(i=0;i<60;i++)
    {{
        if(mclick()!=0 || kbhit())
        {{
            outtextxy(0,500,hour);
            return;
        }}
        else delay(4500);

        if(mclick()!=0 || kbhit())
        {{
            outtextxy(0,500,hour);
            return;
        }}
        else delay(4500);
        
        if(mclick()!=0 || kbhit())
        {{
            outtextxy(0,500,hour);
            return;
        }}
        else delay(4500);
    }}
    outtextxy(0,500,hour);
    Clock();
    
}}

void WindowsSplashScreen(void)
{{
    int logo[4][10] =  {{{{288,159,   318,155,    318,182,    287,182,    288,159}},   {{321,155,   359,149,    359,182,    321,182,    321,155}},   {{288,185,   318,185,    318,212,    287,208,    288,185}},   {{321,185,   359,185,    359,217,    321,212,    321,185}}}};
                    
    
    
    setcolor(BLACK);
    setfillstyle(SOLID_FILL,LIGHTCYAN);

    for(i=0;i<4;i++)
        fillpoly(5,logo[i]);

    for(j=0;j<4;j++)
        for (i=0;i<25;i++)
        {{
            putpixel(charger[0][i+1],charger[1][i+1],WHITE);
            putpixel(charger[0][i+3],charger[1][i+3],WHITE);
            putpixel(charger[0][i+6],charger[1][i+6],WHITE);
            putpixel(charger[0][i+9],charger[1][i+9],WHITE);
            

            putpixel(charger[0][i],charger[1][i],BLACK);
            putpixel(charger[0][i+2],charger[1][i+2],BLACK);
            putpixel(charger[0][i+5],charger[1][i+5],BLACK);
            putpixel(charger[0][i+8],charger[1][i+8],BLACK);
            
            if(i>10||i<1)
                delay(20 * duration);
            else
                delay(8 * duration);
        }}
}}

void showImage(int x, int y, char *locate)
{{
    int b,a;
    BMP Obj;
    unsigned char* Datas;
    int in=0;
    unsigned char c=0;
    FILE * image;

    image = fopen(locate,""rb"");
    if(!image)
    {{
        printf(""Error : No se puede abrir el archivo ..."");
        getch();
        exit(0);
    }}

    fread(&Obj, sizeof(Obj), 1, image);
    if(Obj.BitsPerPixel!=4)  /* Este NO es un bmp de 16 colores que podamos leer*/
    {{
        fclose(image);
        printf(""Error : Formato de archivo no soportado..."");
        getch();
        exit(0);
    }}

    fseek(image,Obj.OffSet,SEEK_SET);
    Datas=(unsigned char*) calloc(Obj.Width/2+1, sizeof(unsigned char));
    for(b=Obj.Height;b>=0;b--)
    {{
        fread(Datas, sizeof(unsigned char), Obj.Width/2, image);
        c=0;
        in=0;
        for(a=0;a<=Obj.Width;a+=2)
        {{
            c = (Datas[in] | 0xF0) & 0x0F;
            putpixel(a+1+x,b+y,c);
            c = (Datas[in] | 0x00) >>4;
            putpixel(a+x,b+y,c);
            in++;
        }}
    }}
    free(Datas);
    fclose(image);
}}

void SystemDo(int Restart)
{{
    mocultar();
    setfillstyle(SOLID_FILL,BLACK);
    bar(0,454,284,174);
    bar(-1,455,640,480);
    showImage(0,0,""{Tools.DosBoxStartUpPath}//Resource//js.bmp"");

    for(j=0;j<4;j++) for (i=0;i<25;i++)
    {{
        putpixel(charger[0][i+1],charger[1][i+1],WHITE);
        putpixel(charger[0][i+3],charger[1][i+3],WHITE);
        putpixel(charger[0][i+6],charger[1][i+6],WHITE);
        putpixel(charger[0][i+9],charger[1][i+9],WHITE);
        

        putpixel(charger[0][i],charger[1][i],BLACK);
        putpixel(charger[0][i+2],charger[1][i+2],BLACK);
        putpixel(charger[0][i+5],charger[1][i+5],BLACK);
        putpixel(charger[0][i+8],charger[1][i+8],BLACK);
        
        if(i>10||i<1)
            delay(20 * duration);
        else
            delay(8 * duration);
    }}
    cleardevice();
    delay(1000 * duration);
    if(Restart == 0)
        exit(0);
}}

void TaskBar(void)
{{
    char hour[10]={{'\0'}},minute[3]={{'\0'}};

    Time = time(0);
    today = localtime(&Time);

    itoa(today->tm_hour,hour,10);
    itoa(today->tm_min,minute,10);
    if(today->tm_hour>12)
        itoa(today->tm_hour-12,hour,10);

    if(today->tm_min>10)
        strcat(hour,"":"");
    else
        strcat(hour,"":0"");

    strcat(hour,minute);

    if(today->tm_hour>12)
        strcat(hour,"" pm"");
    else
        strcat(hour,"" am"");

    settextstyle(SMALL_FONT,HORIZ_DIR,4);
    setcolor(BLACK);

    setfillstyle(SOLID_FILL,WHITE);
    bar(86,455,640,480);
    outtextxy(565,455,hour);
    outtextxy(565,465,date);

    settextstyle(SMALL_FONT,HORIZ_DIR,5);
    outtextxy(530,460,""ENG"");

    setfillstyle(SOLID_FILL,BLACK);
    fillellipse(520,472,1,1);
    arc(520,472,90,180,3);
    arc(520,472,90,180,5);
    arc(520,472,90,180,7);
    arc(520,472,90,180,9);

    line(491,461,491,472);
    line(490,462,490,471);
    line(489,463,489,470);
    bar(488,464,486,469);
    arc(491,466,315,45,4);
    arc(491,466,315,45,6);
    arc(491,466,315,45,8);
}}

void formWindow(void)
{{
    int k=0;
    setfillstyle(SOLID_FILL,WHITE);
    bar(0,454,284,174);

    setfillstyle(SOLID_FILL,BLACK);
    line(7,181,13,181);
    line(7,183,13,183);
    line(7,185,13,185);

    /* Boton de apagar*/
    bar(11,442,11,438);
    arc(11,442,135,45, 3);

    /* Boton de reiniciar */

    arc(11,417,0,270,3);
    line(13,417,15,416);
    line(15,416,15,418);
    line(15,418,13,417);

    /* Imagenes */
    rectangle(8,389,15,394);
    putpixel(13,392,BLACK);
    putpixel(12,392,BLACK);
    putpixel(11,391,BLACK);
    putpixel(9,391,BLACK);

    setfillstyle(SOLID_FILL,LIGHTGRAY);

    for (i = 0; i < 5; i++) for (j = 0; j < 3; j++)
    {{
        setfillstyle(SOLID_FILL,k);
        bar(142 + (46*j),195 + (46*i),185 + (46*j),238 + (46*i));
        k = k==15?0:k+1;
    }}
}}

void SaveWin(void)
{{
    FILE * file;
    file = fopen(""c://TC20//FICHEROS//Win32.txt"",""w"");
    fwrite(&System,sizeof(System),1,file);
    fclose(file);
}}

void LoadWin(void)
{{
    FILE * file;
    file = fopen(""c://TC20//FICHEROS//Win32.txt"",""r"");
    fread(&System,sizeof(System),1,file);
    fclose(file);
}}

void icon(int x, int y, char *name)
{{
    int windowIcon[4][14] ={{{{7,8,    7,14,   13,14,  14,15,  15,15,  15,8,   7,8}},{{7,1,   13,1,   14,0,   15,0,   15,6,   7,6,    7,1}},{{0,2,   5,2,    5,6,    0,6,    0,2}},{{0,8,   5,8,    5,13,   0,13,   0,8}}}};

    if(strcmp(name,""window"")==0)
    {{
        for(i=0;i<14;i++)
            for(j=0;j<4;j++)
                if(i%2==0)
                    windowIcon[j][i]+=x;
                else
                    windowIcon[j][i]+=y;
        
        for(i=0;i<4;i++)
        {{
            if(i<2)
                fillpoly(7,windowIcon[i]);
            else
                fillpoly(5,windowIcon[i]);
        }}
    }}
    else if (strcmp(name,""mine"")==0)
    {{
        bar(5 +x,   5 +y,   14+x,   14+y);
        bar(9 +x,   16+y,   10+x,   18+y);
        bar(16+x,   9 +y,   18+x,   10+y);
        bar(9 +x,   3 +y,   10+x,   1 +y);
        bar(3 +x,   9 +y,   1 +x,   10+y);

        line(15+x,  12+y,   15+x,   7 +y);
        line(12+x,  15+y,   7 +x,   15+y);
        line(4 +x,  7 +y,   4 +x,   12+y);
        line(7 +x,  4 +y,   12+x,   4 +y);

        putpixel(4 +x,  4 +y,   BLACK);
        putpixel(4 +x,  15+y,   BLACK);
        putpixel(15+x,  15+y,   BLACK);
        putpixel(15+x,  4 +y,   BLACK);

        setfillstyle(SOLID_FILL,WHITE==getcolor() ? BLACK : WHITE);
        bar(7 +x,   7 +y,   8 +x,   8 +y);
    }}
    else if (strcmp(name,""uni"")==0)
    {{
        bar(0+x,0+y,2+x,11+y);
        bar(3+x,9+y,12+x,11+y);
        bar(10+x,8+y,12+x,5+y);
        bar(7+x,6+y,5+x,0+y);
        bar(8+x,0+y,17+x,2+y);
        bar(15+x,0+y,17+x,11+y);
        bar(17+x,11+y,22+x,9+y);
        bar(20+x,8+y,22+x,5+y);
        bar(20+x,2+y,22+x,0+y);
    }}
}}";
            Form.Content = @"#include <stdbool.h>
enum WindowState
{
CloseState,
NormalState,
MinimizeState,
MaximizeState
};

typedef struct 
{
int Size[2];
int Location[2];
char *Text;
int BackColor;
int WindowState;
bool Visible;
} FormProperties;

typedef struct
{
int Background;
} SystemProperties;";
            WaitDialog.Caption = "Guardando archivos";
            Thread.Sleep(300);
            Window.Save();
            System.Save();
            Form.Save();
            WaitDialog.Caption = "Terminando...";
            Thread.Sleep(300);
            XtraMessageBox.Show(Tools.Message.InstalledSuccess + "\n\nEs recomendable aumentar a 100,000 ciclos DOSBOX en DOSBOX(Options)", "Instalación terminada", MessageBoxButtons.OK, MessageBoxIcon.Information);

            uploadFiles();
        }

        #endregion

        private void btnConfiguration_Click(object sender, EventArgs e)
        {
            try
            {
                frmSettings frm = new frmSettings();
                frm.ShowDialog();

                uploadFiles();
            }
            catch(Exception ex)
            {
                Archive.SaveException(ex);
                XtraMessageBox.Show("Error", "Ha ocurrido un error al momento de abrir la configuracion", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            
        }

        private void btnDosboxConfiguration_Click(object sender, EventArgs e)
        {
            frmDosboxSettings frm = new frmDosboxSettings();
            frm.ShowDialog();
        }
    }
}