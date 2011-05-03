//using System;
//using System.Collections.Generic;
//using System.IO;
//using System.Linq;
//using System.Text;

//namespace Krach.Formats.Mpeg
//{
//    public enum Version { Mpeg25 = 0x0, Reserved = 0x1, Mpeg2 = 0x2, Mpeg1 = 0x3 }
//    public enum Layer { Reserved = 0x0, LayerIII = 0x1, LayerII = 0x2, LayerI = 0x3 }
//    public enum ChannelMode { Stereo = 0x0, JointStereo = 0x1, DualChannel = 0x2, Mono = 0x3 }
//    public enum JoinBands { Bands4_31 = 0x0, Bands8_31 = 0x1, Bands12_31 = 0x2, Bands16_31 = 0x3, Dynamic = 0x4 }
//    public enum JoinMode { None = 0x0, IntensityStereo = 0x1, MSStereo = 0x2, IntensityAndMSStereo = 0x3 }
//    public enum Emphasis { None = 0x0, MS50_15 = 0x1, Reserved = 0x2, CcitJ_17 = 0x3 }
//    public class MpegFrame
//    {

//        #region Bit Rate Table
//        static readonly int[, ,] bitRates = new int[,,]
//        {
//            // Version.Mpeg25
//            {
//                // Layer.Undefined
//                { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
//                // Layer.LayerIII
//                { 0, 8, 16, 24, 32, 40, 48, 56, 64, 80, 96, 112, 128, 144, 160, 0 },
//                // Layer.LayerII
//                { 0, 8, 16, 24, 32, 40, 48, 56, 64, 80, 96, 112, 128, 144, 160, 0 },
//                // Layer.LayerI
//                { 0, 32, 48, 56, 64, 80, 96, 112, 128, 144, 160, 176, 192, 224, 256, 0 },
//            },

//            // Version.Reserved
//            {
//                // Layer.Undefined
//                { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
//                // Layer.LayerIII
//                { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
//                // Layer.LayerII
//                { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
//                // Layer.LayerI
//                { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }
//            },

//            // Version.Mpeg2
//            {
//                // Layer.Undefined
//                { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
//                // Layer.LayerIII
//                { 0, 8, 16, 24, 32, 40, 48, 56, 64, 80, 96, 112, 128, 144, 160, 0 },
//                // Layer.LayerII
//                { 0, 8, 16, 24, 32, 40, 48, 56, 64, 80, 96, 112, 128, 144, 160, 0 },
//                // Layer.LayerI
//                { 0, 32, 48, 56, 64, 80, 96, 112, 128, 144, 160, 176, 192, 224, 256, 0 },
//            },

//            // Version.Mpeg1
//            {
//                // Layer.Undefined
//                { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
//                // Layer.LayerIII
//                { 0, 32, 40, 48, 56, 64, 80, 96, 112, 128, 160, 192, 224, 256, 320, 0 },
//                // Layer.LayerII
//                { 0, 32, 48, 56, 64, 80, 96, 112, 128, 160, 192, 224, 256, 320, 384, 0 },
//                // Layer.LayerI
//                { 0, 32, 64, 96, 128, 160, 192, 224, 256, 288, 320, 352, 384, 416, 448, 0 }
//            }
//        };
//        #endregion
//        #region Sampling Rate Table
//        static readonly int[,] samplingRates = new int[,]
//        {
//            // Version.Mpeg25
//            { 11025, 12000, 8000, 0 },
//            // Version.Reserved
//            { 0, 0, 0, 0 },
//            // Version.Mpeg2
//            { 22500, 24000, 16000, 0 },
//            // Version.Mpeg1
//            { 44100, 48000, 32000, 0 }
//        };
//        #endregion

//        readonly byte[] sync;
//        readonly Version version;
//        readonly Layer layer;
//        readonly bool errorProtection;
//        readonly int dataRate;
//        readonly int sampleRate;
//        readonly bool padding;
//        readonly bool privateBit;
//        readonly ChannelMode channelMode;
//        readonly JoinBands joinBands;
//        readonly JoinMode joinMode;
//        readonly bool copyright;
//        readonly bool original;
//        readonly Emphasis emphasis;

//        readonly byte[] header;
//        readonly byte[] checksum;
//        readonly byte[] data;

//        public IEnumerable<byte> Sync { get { return sync; } }
//        public Version Version { get { return version; } }
//        public Layer Layer { get { return layer; } }
//        public bool ErrorProtection { get { return errorProtection; } }
//        public int DataRate { get { return dataRate; } }
//        public int SampleRate { get { return sampleRate; } }
//        public bool Padding { get { return padding; } }
//        public bool PrivateBit { get { return privateBit; } }
//        public ChannelMode ChannelMode { get { return channelMode; } }
//        public JoinBands JoinBands { get { return joinBands; } }
//        public JoinMode JoinMode { get { return joinMode; } }
//        public bool Copyright { get { return copyright; } }
//        public bool Original { get { return original; } }
//        public Emphasis Emphasis { get { return emphasis; } }
//        public IEnumerable<byte> Checksum { get { return checksum; } }
//        public IEnumerable<byte> Data { get { return data; } }
//        public int Length { get { return 4 + checksum.Length + data.Length; } }
//        public int SampleCount
//        {
//            get
//            {
//                switch (layer)
//                {
//                    case Layer.LayerI: return 0x180;
//                    case Layer.LayerII:
//                    case Layer.LayerIII: return 0x480;
//                    case Layer.Reserved:
//                    default: throw new InvalidOperationException();
//                }
//            }
//        }
//        public double SampleLength { get { return (double)dataRate / (double)sampleRate; } }
//        public int SlotLength
//        {
//            get
//            {
//                switch (layer)
//                {
//                    case Layer.LayerI: return 4;
//                    case Layer.LayerII:
//                    case Layer.LayerIII: return 1;
//                    case Layer.Reserved:
//                    default: throw new InvalidOperationException();
//                }
//            }
//        }

//        public MpegFrame(BinaryReader reader)
//        {
//            try
//            {
//                header = reader.ReadBytes(4);

//                #region Read Header

//                byte[] headerBits = header.GetBits(0, 32);

//                sync = headerBits.Range(0, 11);
//                if (sync.Any(bit => bit == 0)) throw new InvalidFrameException();
//                version = headerBits.GetData(11, 13).ToEnumeration<Version>();
//                layer = headerBits.GetData(13, 15).ToEnumeration<Layer>();
//                errorProtection = headerBits.GetData(15, 16) == 0;
//                dataRate = bitRates[(int)version, (int)layer, headerBits.GetData(16, 20)] * 1000 / 8;
//                sampleRate = samplingRates[(int)version, headerBits.GetData(20, 22)];
//                padding = headerBits.GetData(22, 23) == 1;
//                privateBit = headerBits.GetData(23, 24) == 1;
//                channelMode = headerBits.GetData(24, 26).ToEnumeration<ChannelMode>();
//                // TODO: Is this actually correct?
//                switch (layer)
//                {
//                    case Layer.LayerI:
//                        joinBands = headerBits.GetData(26, 28).ToEnumeration<JoinBands>();
//                        joinMode = JoinMode.IntensityStereo;
//                        break;
//                    case Layer.LayerII:
//                        joinBands = headerBits.GetData(26, 28).ToEnumeration<JoinBands>();
//                        joinMode = JoinMode.IntensityStereo;
//                        break;
//                    case Layer.LayerIII:
//                        joinBands = JoinBands.Dynamic;
//                        joinMode = headerBits.GetData(26, 28).ToEnumeration<JoinMode>();
//                        break;
//                    default: throw new InvalidOperationException();
//                }
//                copyright = headerBits.GetData(28, 29) == 1;
//                original = headerBits.GetData(29, 30) == 1;
//                emphasis = headerBits.GetData(30, 32).ToEnumeration<Emphasis>();
//                #endregion

//                // TODO: Test for correctness of the checksum
//                checksum = errorProtection ? source.Read(2) : new byte[0];

//                data = source.Read((int)(SampleCount * SampleLength) + (padding ? SlotLength : 0) - checksum.Length - header.Length);
//            }
//            catch (EndOfStreamException) { throw new InvalidFrameException(); }
//        }

//        public override string ToString()
//        {
//            StringBuilder result = new StringBuilder();

//            result.AppendLine("Sync: " + sync.Aggregate(string.Empty, (seed, current) => seed + current));
//            result.AppendLine("Version: " + version);
//            result.AppendLine("Layer: " + layer);
//            result.AppendLine("Error protection: " + errorProtection);
//            result.AppendLine("Data rate (B/s): " + dataRate);
//            result.AppendLine("Sample rate (Hz): " + sampleRate);
//            result.AppendLine("Padding: " + padding);
//            result.AppendLine("Private bit: " + privateBit);
//            result.AppendLine("Channel mode: " + channelMode);
//            result.AppendLine("Join bands: " + joinBands);
//            result.AppendLine("Join mode: " + joinMode);
//            result.AppendLine("Copyright: " + copyright);
//            result.AppendLine("Original: " + original);
//            result.AppendLine("Emphasis: " + emphasis);
//            if (errorProtection) result.AppendLine("Checksum: " + checksum.GetBits().BitsToString());
//            result.AppendLine("Data length: " + data.Length);

//            return result.ToString();
//        }
//    }
//}
