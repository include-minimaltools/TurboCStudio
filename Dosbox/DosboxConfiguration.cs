using Microsoft.Win32;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Management;

namespace WindowsCAssistant.Dosbox
{
    class DosboxConfiguration
    {
        #region Lists_Options
        public List<string> Resolutions { get; } = new List<string> { "original", "1024x768" };
        public List<string> Outputs = new List<string> { "surface", "overlay", "opengl", "openglnb", "ddraw" };
        public List<string> Priorities = new List<string> { "lowest", "lower", "normal", "higher", "highest", "pause" };
        public List<string> Machines = new List<string> { "hercules", "cga", "tandy", "pcjr", "ega", "vgaonly", "svga_s3", "svga_et3000", "svga_et4000", "svga_paradise", "vesa_nolfb", "vesa_oldvbe" };
        public List<string> Scales = new List<string> { "none", "normal2x", "normal3x", "advmame2x", "advmame3x", "advinterp2x", "advinterp3x", "hq2x", "hq3x", "_2xsai", "super2xsai", "supereagle", "tv2x", "tv3x", "rgb2x", "rgb3x", "scan2x", "scan3x" };
        public List<string> Cores = new List<string> { "auto", "dynamic", "normal", "simple" };
        public List<string> CpuTypes = new List<string> { "auto", "386", "386_slow", "486_slow", "pentium_slow", "386_prefetch" };
        public List<string> Cycles = new List<string> { "auto", "max", "fixed" };
        public List<int> BlockSizes = new List<int> { 1024, 2048, 4096, 8192, 512, 256 };
        public List<string> MpuTypes = new List<string> { "intelligent", "uart", "none" };
        public List<string> MidiTypes = new List<string> { "default", "win32", "alsa", "oss", "coreaudio", "coremidi", "none" };
        public List<string> SbTypes = new List<string> { "sb1", "sb2", "sbpro1", "sbpro2", "sb16", "gb", "none" };
        public List<string> Bases = new List<string> { "220", "240", "260", "280", "2a0", "2c0", "2e0", "300" };
        public List<int> Irqs = new List<int> { 7, 5, 3, 9, 10, 11, 12 };
        public List<int> DmaHdas = new List<int> { 1, 5, 0, 3, 6, 7 };
        public List<string> Opls = new List<string> { "auto", "cms", "opl2", "dualopl2", "opl3", "none" };
        public List<string> OplMus = new List<string> { "default", "compat", "fast" };
        public List<int> Rates = new List<int> { 44100, 48000, 32000, 220500, 16000, 11025, 8000, 49716 };
        public List<string> Tandys = new List<string> { "auto", "on", "off" };
        public List<string> Serials = new List<string> { "dummy", "disabled", "modem", "nullmodem", "directserial" };
        public List<string> Joysticks = new List<string> { "auto", "_2axis", "_4axis", "_4axis_2", "fcs", "ch", "none" };
        #endregion

        #region Enum_Options
        public enum ResolutionOption
        {
            original, _1024x768
        };

        public enum OutputOption
        {
            surface, overlay, opengl, openglnb, ddraw
        };

        public enum PriorityOption
        {
            lowest, lower, normal, higher, highest, pause
        };

        public enum MachineOption
        {
            hercules, cga, tandy, pcjr, ega, vgaonly, svga_s3, svga_et3000, svga_et4000, svga_paradise, vesa_nolfb, vesa_oldvbe
        }

        public enum ScalerOption
        {
            none, normal2x, normal3x, advmame2x, advmame3x, advinterp2x, advinterp3x, hq2x, hq3x, _2xsai, super2xsai, supereagle, tv2x, tv3x, rgb2x, rgb3x, scan2x, scan3x
        }

        public enum CoreOption
        {
            auto, dynamic, normal, simple
        }

        public enum CpuTypeOption
        {
            auto, _386, _386_slow, _486_slow, pentium_slow, _386_prefetch
        }

        public enum CyclesOption
        {
            auto, max, personalized
        }

        public enum BlockSizeOption
        {
            _1024, _2048, _4096, _8192, _512, _256
        }

        public enum MpuTypeOption
        {
            intelligent, uart, none
        }

        public enum MidiTypeOption
        {
            _default, win32, alsa, oss, coreaudio, coremidi, none
        }

        public enum SbTypeOption
        {
            sb1, sb2, sbpro1, sbpro2, sb16, gb, none
        }

        public enum BaseOptions
        {
            _220, _240, _260, _280, _2a0, _2c0, _2e0, _300
        }

        public enum IrqOption
        {
            _7, _5, _3, _9, _10, _11, _12
        }

        public enum DmaHdmaOption
        {
            _1, _5, _0, _3, _6, _7
        }

        public enum OplMode
        {
            auto, cms, opl2, dualopl2, opl3, none
        }

        public enum OplMu
        {
            _default, compat, fast
        }

        public enum RateOption
        {
            _44100, _48000, _32000, _220500, _16000, _11025, _8000, _49716
        }

        public enum TandyOption
        {
            auto, on, off
        }

        public enum SerialOptions
        {
            dummy, disabled, modem, nullmodem, directserial
        }

        public enum JoystickOptions
        {
            auto, _2axis, _4axis, _4axis_2, fcs, ch, none
        }
        #endregion
    }
}
