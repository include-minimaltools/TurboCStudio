using System.Collections.Generic;

namespace WindowsCAssistant.Dosbox
{
    class DosboxModel
    {
        #region SDL

        public bool FullScreen { get; set; } = false;
        public bool FullDouble { get; set; } = false;
        public string FullResolution { get; set; } = "original";
        public string WindowResolution { get; set; } = "original";
        public string Output { get; set; } = "surface";
        public bool Autolock { get; set; } = true;
        public byte Sensitivity { get; set; } = 100;
        public bool WaitonError { get; set; } = true;
        public string Priority { get; set; } = "higher, normal";
        public string MapperFile { get; set; } = "mapper - 0.74.map";
        public bool UsesCanCodes { get; set; } = true;
        #endregion

        #region dosbox
        public string Language { get; set; } = string.Empty;
        public string Machine { get; set; } = "svga_s3";
        public string Captures { get; set; } = "capture";
        public byte Memsize { get; set; } = 16;
        #endregion

        #region render
        public byte FrameSkip { get; set; } = 0;
        public bool Aspect { get; set; } = false;
        public string Scaler { get; set; } = "normal2x";
        #endregion

        #region cpu
        public string Core { get; set; } = "auto";
        public string CpuType { get; set; } = "auto";
        public string Cycles { get; set; } = "auto";
        public byte CycleUp { get; set; } = 10;
        public byte CycleDown { get; set; } = 20;
        #endregion

        #region mixer
        public bool NoSound { get; set; } = false;
        public int Rate { get; set; } = 44100;
        public int BlockSize { get; set; } = 1024;
        public ushort PreBuffer { get; set; } = 25;
        #endregion

        #region midi
        public string Mpu401 { get; set; } = "intelligent";
        public string MidiDevice { get; set; } = "default";
        public string MidiConfig { get; set; } = string.Empty;
        #endregion

        #region soundblaster
        public string SbType { get; set; } = "sb16";
        public string SbBase { get; set; } = "220";
        public int Irq { get; set; } = 7;
        public int Dma { get; set; } = 1;
        public int Hdma { get; set; } = 5;
        public bool SbMixer { get; set; } = true;
        public string OplMode { get; set; } = "auto";
        public string OplEmu { get; set; } = "default";
        public int OplRate { get; set; } = 44100;
        #endregion

        #region gus
        public bool Gus = false;
        public int GusRate { get; set; } = 44100;
        public string GusBase { get; set; } = "240";
        public int GusIrq { get; set; } = 5;
        public int GusDma { get; set; } = 3;
        public string UltraDir { get; set; } = "C:\\ULTRASND";
        #endregion

        #region speaker
        public bool PcSpeaker { get; set; } = true;
        public int PcRate { get; set; } = 44100;
        public string Tandy { get; set; } = "auto";
        public int TandyRate { get; set; } = 44100;
        public bool Disney { get; set; } = true;
        #endregion

        #region Joystick
        public string JoystickType { get; set; } = "auto";
        public bool Timed { get; set; } = true;
        public bool AutoFire { get; set; } = false;
        public bool Swap34 { get; set; } = false;
        public bool ButtonWrap { get; set; } = false;
        #endregion

        #region Serial
        public string Serial1 { get; set; } = "dummy";
        public string Serial2 { get; set; } = "dummy";
        public string Serial3 { get; set; } = "disabled";
        public string Serial4 { get; set; } = "disabled";
        #endregion

        #region Dos
        public bool Xms { get; set; } = true;
        public bool Ems { get; set; } = true;
        public bool Umb { get; set; } = true;
        public string KeyboardLayout { get; set; } = "auto";
        #endregion

        #region Ipx
        public bool Ipx { get; set; } = false;
        #endregion

        #region Autoexec
        public string AutoExecute { get; set; } = "mount c c:/\nc:\ncd tc20\\bin\ntc";
        #endregion

        public List<string> getModel()
        {
            return new List<string>
            {
                "\n[sdl]",
                "fullscreen=" + FullScreen.ToString(),
                "fulldouble=" + FullDouble.ToString(),
                "fullresolution=" + FullResolution,
                "windowresolution=" + WindowResolution,
                "output=" + Output,
                "autolock=" + Autolock.ToString(),
                "sensitivity=" + Sensitivity.ToString(),
                "waitonerror=" + WaitonError.ToString(),
                "priority=" + Priority,
                "mapperfile=" + MapperFile,
                "usescancodes=" +  UsesCanCodes.ToString(),

                "\n[dosbox]",
                "language=" + Language,
                "machine=" + Machine,
                "captures=" + Captures,
                "memsize=" + Memsize.ToString(),

                "\n[render]",
                "frameskip=" + FrameSkip.ToString(),
                "aspect=" + Aspect.ToString(),
                "scaler=" + Scaler,

                "\n[cpu]",
                "core=" + Core,
                "cputype=" + CpuType,
                "cycles=" + Cycles,
                "cycleup=" + CycleUp.ToString(),
                "cycledown=" + CycleDown.ToString(),

                "\n[mixer]",
                "nosound=" + NoSound.ToString(),
                "rate=" + Rate.ToString(),
                "blocksize=" + BlockSize.ToString(),
                "prebuffer=" + PreBuffer.ToString(),

                "\n[midi]",
                "mpu401=" + Mpu401,
                "mididevice=" + MidiDevice,
                "midiconfig=" + MidiConfig,

                "\n[sblaster]",
                "sbtype=" + SbType,
                "sbbase=" + SbBase,
                "irq=" + Irq.ToString(),
                "dma=" + Dma.ToString(),
                "hdma=" + Hdma.ToString(),
                "sbmixer=" + SbMixer.ToString(),
                "oplmode=" + OplMode,
                "oplemu=" + OplEmu,
                "oplrate=" + OplRate.ToString(),

                "\n[gus]",
                "gus=" + Gus.ToString(),
                "gusrate=" + GusRate.ToString(),
                "gusbase=" + GusBase.ToString(),
                "gusirq=" + GusIrq.ToString(),
                "gusdma=" + GusDma.ToString(),
                "ultradir=" + UltraDir,

                "\n[speaker]",
                "pcspeaker=" + PcSpeaker.ToString(),
                "pcrate=" + PcRate.ToString(),
                "tandy=" + Tandy,
                "tandyrate=" + TandyRate.ToString(),
                "disney=" + Disney.ToString(),

                "\n[joystick]",
                "joysticktype=" + JoystickType,
                "timed=" + Timed.ToString(),
                "autofire=" + AutoFire.ToString(),
                "swap34=" + Swap34.ToString(),
                "buttonwrap=" + ButtonWrap.ToString(),

                "\n[serial]",
                "serial1=" + Serial1,
                "serial2=" + Serial2,
                "serial3=" + Serial3,
                "serial4=" + Serial4,

                "\n[dos]",
                "xms=" + Xms.ToString(),
                "ems=" + Ems.ToString(),
                "umb=" + Umb.ToString(),
                "keyboardlayout=" + KeyboardLayout,

                "\n[ipx]",
                "ipx=" + Ipx.ToString(),

                "\n[autoexec]",
                AutoExecute
            };
        }
    }
}
