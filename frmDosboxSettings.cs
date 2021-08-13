using DevExpress.XtraEditors;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using WindowsCAssistant.Dosbox;
using WindowsCAssistant.Model;

namespace WindowsCAssistant
{
    public partial class frmDosboxSettings : XtraForm
    {
        Settings Settings = Archive.GetSettings();
        DosboxModel DosboxModel = Archive.GetSettings().DosboxConfig;
        public frmDosboxSettings()
        {
            InitializeComponent();
        }

        private void frmDosboxSettings_Load(object sender, EventArgs e)
        {
            try
            {
                LoadData();
            }
            catch(Exception ex)
            {
                Archive.SaveException(ex);
                XtraMessageBox.Show("Ha ocurrido un error", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Close();
            }
        }
        
        private void btnCerrar_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            try
            {
                Save();
                XtraMessageBox.Show("Se ha guardado correctamente", "Guardado", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Close();
            }
            catch (Exception ex)
            {
                Archive.SaveException(ex);
                XtraMessageBox.Show("Ha ocurrido un error", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Close();
            }
        }

        private void btnRestore_Click(object sender, EventArgs e)
        {
            try
            {
                DosboxModel = new DosboxModel();
                Archive.SaveDosboxConfig(DosboxModel);
                Settings.DosboxConfig = DosboxModel;
                Archive.SaveSettings(Settings);

                XtraMessageBox.Show("Se han restaurado los valores correctamente", "Guardado", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Close();
            }
            catch (Exception ex)
            {
                Archive.SaveException(ex);
                XtraMessageBox.Show("Ha ocurrido un error", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Close();
            }
        }

        private void Cycles_EditValueChanged(object sender, EventArgs e)
        {
            Number.Enabled = Cycles.EditValue.ToString() == "fixed" ?  true : false;
            Number.Text = string.Empty;
        }

        private void Save()
        {
            DosboxModel.FullScreen = FullScreen.Checked;
            DosboxModel.FullDouble = FullDouble.Checked;

            if (FullResolutionX.Text.Contains("original") || FullResolutionY.Text.Contains("original"))
                DosboxModel.FullResolution = "original";
            else
                DosboxModel.FullResolution = FullResolutionX.EditValue + "x" + FullResolutionY.EditValue;


            if (WindowResolutionX.Text.Contains("original") || WindowResolutionY.Text.Contains("original"))
                DosboxModel.WindowResolution = "original";
            else
                DosboxModel.WindowResolution = WindowResolutionX.EditValue + "x" + WindowResolutionY.EditValue;


            DosboxModel.Output = Output.EditValue.ToString();
            DosboxModel.Autolock = AutoLock.Checked;
            DosboxModel.Sensitivity = byte.Parse(Sensitivity.EditValue.ToString());
            DosboxModel.WaitonError = WaitOnError.Checked;
            DosboxModel.Priority = PriorityFirst.EditValue.ToString() + "," + PrioritySecond.EditValue.ToString();
            DosboxModel.MapperFile = MapperFile.EditValue.ToString();
            DosboxModel.UsesCanCodes = UsesCanCodes.Checked;

            DosboxModel.Language = Language.EditValue.ToString();
            DosboxModel.Machine = Machine.EditValue.ToString();
            DosboxModel.Captures = Captures.EditValue.ToString();
            DosboxModel.Memsize = byte.Parse(Memsize.EditValue.ToString());

            DosboxModel.FrameSkip = byte.Parse(FrameSkip.EditValue.ToString());
            DosboxModel.Aspect = Aspect.Checked;
            DosboxModel.Scaler = Scaler.EditValue.ToString();

            DosboxModel.Core = Core.EditValue.ToString();
            DosboxModel.CpuType = CpuType.EditValue.ToString();

            if (Cycles.EditValue.ToString() == "max" || Cycles.EditValue.ToString() == "auto")
                DosboxModel.Cycles = Cycles.EditValue.ToString();
            else
                DosboxModel.Cycles = Number.EditValue.ToString();

            DosboxModel.CycleUp = byte.Parse(CycleUp.EditValue.ToString());
            DosboxModel.CycleDown = byte.Parse(CycleDown.EditValue.ToString());

            DosboxModel.NoSound = NoSound.Checked;
            DosboxModel.Rate = int.Parse(Rate.EditValue.ToString());
            DosboxModel.BlockSize = int.Parse(BlockSize.EditValue.ToString());
            DosboxModel.PreBuffer = ushort.Parse(PreBuffer.EditValue.ToString());

            DosboxModel.Mpu401 = Mpu401.EditValue.ToString();
            DosboxModel.MidiDevice = MidiDevice.EditValue.ToString();
            DosboxModel.MidiConfig = MidiConfig.EditValue.ToString();

            DosboxModel.SbType = SbType.EditValue.ToString();
            DosboxModel.SbBase = SbBase.EditValue.ToString();
            DosboxModel.Irq = int.Parse(Irq.EditValue.ToString());
            DosboxModel.Dma = int.Parse(Dma.EditValue.ToString());
            DosboxModel.Hdma = int.Parse(Hdma.EditValue.ToString());
            DosboxModel.SbMixer = SbMixer.Checked;
            DosboxModel.OplMode = OplMode.EditValue.ToString();
            DosboxModel.OplEmu = OplMu.EditValue.ToString();
            DosboxModel.OplRate = int.Parse(OplRate.EditValue.ToString());

            DosboxModel.Gus = Gus.Checked;
            DosboxModel.GusRate = int.Parse(GusRate.EditValue.ToString());
            DosboxModel.GusBase = GusBase.EditValue.ToString();
            DosboxModel.GusIrq = int.Parse(GusIrq.EditValue.ToString());
            DosboxModel.GusDma = int.Parse(GusDma.EditValue.ToString());
            DosboxModel.UltraDir = UltraDir.EditValue.ToString();

            DosboxModel.PcSpeaker = PcSpeaker.Checked;
            DosboxModel.PcRate = int.Parse(PcRate.EditValue.ToString());
            DosboxModel.Tandy = Tandy.EditValue.ToString();
            DosboxModel.TandyRate = int.Parse(TandyRate.EditValue.ToString());
            DosboxModel.Disney = Disney.Checked;

            DosboxModel.JoystickType = JoystickType.EditValue.ToString();
            DosboxModel.Timed = Timed.Checked;
            DosboxModel.AutoFire = AutoFire.Checked;
            DosboxModel.Swap34 = Swap34.Checked;
            DosboxModel.ButtonWrap = ButtonWrap.Checked;

            DosboxModel.Serial1 = Serial1.EditValue.ToString();
            DosboxModel.Serial2 = Serial2.EditValue.ToString();
            DosboxModel.Serial3 = Serial3.EditValue.ToString();
            DosboxModel.Serial4 = Serial4.EditValue.ToString();

            DosboxModel.Xms = Xms.Checked;
            DosboxModel.Ems = Ems.Checked;
            DosboxModel.Umb = Umb.Checked;
            DosboxModel.KeyboardLayout = KeyboardLayout.EditValue.ToString();

            DosboxModel.Ipx = Ipx.Checked;

            DosboxModel.AutoExecute = AutoExec.Text;

            Archive.SaveDosboxConfig(DosboxModel);
            Settings.DosboxConfig = DosboxModel;
            Archive.SaveSettings(Settings);
        }

        private void LoadData()
        {
            FullScreen.Checked = DosboxModel.FullScreen;
            FullDouble.Checked = DosboxModel.FullDouble;

            if (DosboxModel.FullResolution.Contains("x"))
            {
                FullResolutionX.Text = DosboxModel.FullResolution.Split('x')[0];
                FullResolutionY.Text = DosboxModel.FullResolution.Split('x')[1];
            }
            else
            {
                FullResolutionX.Text = "original";
                FullResolutionY.Text = "original";
            }

            if (DosboxModel.WindowResolution.Contains("x"))
            {
                WindowResolutionX.Text = DosboxModel.WindowResolution.Split('x')[0];
                WindowResolutionY.Text = DosboxModel.WindowResolution.Split('x')[1];
            }
            else
            {
                WindowResolutionX.Text = "original";
                WindowResolutionY.Text = "original";
            }

            Output.EditValue = DosboxModel.Output;
            AutoLock.Checked = DosboxModel.Autolock;
            Sensitivity.EditValue = DosboxModel.Sensitivity.ToString();
            WaitOnError.Checked = DosboxModel.WaitonError;
            PriorityFirst.EditValue = DosboxModel.Priority.Split(',')[0];
            PrioritySecond.EditValue = DosboxModel.Priority.Split(',')[1];
            MapperFile.EditValue = DosboxModel.MapperFile;
            UsesCanCodes.Checked = DosboxModel.UsesCanCodes;

            Language.EditValue = DosboxModel.Language;
            Machine.EditValue = DosboxModel.Machine;
            Captures.EditValue = DosboxModel.Captures;
            Memsize.EditValue = DosboxModel.Memsize.ToString();

            FrameSkip.EditValue = DosboxModel.FrameSkip.ToString();
            Aspect.Checked = DosboxModel.Aspect;
            Scaler.EditValue = DosboxModel.Scaler;

            Core.EditValue = DosboxModel.Core;
            CpuType.EditValue = DosboxModel.CpuType;

            if (DosboxModel.Cycles.Contains("auto") || DosboxModel.Cycles.Contains("max"))
            {
                Cycles.EditValue = DosboxModel.Cycles;
                Number.Enabled = false;
            }
            else
            {
                Cycles.EditValue = DosboxModel.Cycles.Split(' ')[0];
                Number.EditValue = DosboxModel.Cycles.Split(' ')[1];
            }

            CycleUp.EditValue = DosboxModel.CycleUp.ToString();
            CycleDown.EditValue = DosboxModel.CycleDown.ToString();

            NoSound.Checked = DosboxModel.NoSound;
            Rate.EditValue = DosboxModel.Rate.ToString();
            BlockSize.EditValue = DosboxModel.BlockSize.ToString();
            PreBuffer.EditValue = DosboxModel.PreBuffer.ToString();

            Mpu401.EditValue = DosboxModel.Mpu401;
            MidiDevice.EditValue = DosboxModel.MidiDevice;
            MidiConfig.EditValue = DosboxModel.MidiConfig;

            SbType.EditValue = DosboxModel.SbType;
            SbBase.EditValue = DosboxModel.SbBase;
            Irq.EditValue = DosboxModel.Irq.ToString();
            Dma.EditValue = DosboxModel.Dma.ToString();
            Hdma.EditValue = DosboxModel.Hdma.ToString();
            SbMixer.Checked = DosboxModel.SbMixer;
            OplMode.EditValue = DosboxModel.OplMode;
            OplMu.EditValue = DosboxModel.OplEmu;
            OplRate.EditValue = DosboxModel.OplRate.ToString();

            Gus.Checked = DosboxModel.Gus;
            GusRate.EditValue = DosboxModel.GusRate.ToString();
            GusBase.EditValue = DosboxModel.GusBase;
            GusIrq.EditValue = DosboxModel.GusIrq.ToString();
            GusDma.EditValue = DosboxModel.GusDma.ToString();
            UltraDir.EditValue = DosboxModel.UltraDir;

            PcSpeaker.Checked = DosboxModel.PcSpeaker;
            PcRate.EditValue = DosboxModel.PcRate.ToString();
            Tandy.EditValue = DosboxModel.Tandy;
            TandyRate.EditValue = DosboxModel.TandyRate.ToString();
            Disney.Checked = DosboxModel.Disney;

            JoystickType.EditValue = DosboxModel.JoystickType;
            Timed.Checked = DosboxModel.Timed;
            AutoFire.Checked = DosboxModel.AutoFire;
            Swap34.Checked = DosboxModel.Swap34;
            ButtonWrap.Checked = DosboxModel.ButtonWrap;

            Serial1.EditValue = DosboxModel.Serial1;
            Serial2.EditValue = DosboxModel.Serial2;
            Serial3.EditValue = DosboxModel.Serial3;
            Serial4.EditValue = DosboxModel.Serial4;

            Xms.Checked = DosboxModel.Xms;
            Ems.Checked = DosboxModel.Ems;
            Umb.Checked = DosboxModel.Umb;
            KeyboardLayout.EditValue = DosboxModel.KeyboardLayout;

            Ipx.Checked = DosboxModel.Ipx;

            AutoExec.Text = DosboxModel.AutoExecute;
        }
    }
}