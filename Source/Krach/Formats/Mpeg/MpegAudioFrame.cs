// Copyright © Julian Brunner 2010 - 2011

// This file is part of Krach.
//
// Krach is free software: you can redistribute it and/or modify it under the
// terms of the GNU Lesser General Public License as published by the Free
// Software Foundation, either version 3 of the License, or (at your option) any
// later version.
//
// Krach is distributed in the hope that it will be useful, but WITHOUT ANY
// WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR
// A PARTICULAR PURPOSE. See the GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License along with
// Krach. If not, see <http://www.gnu.org/licenses/>.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Krach.Extensions;

namespace Krach.Formats.Mpeg
{
	public enum Version { Mpeg25 = 0x0, Reserved = 0x1, Mpeg2 = 0x2, Mpeg1 = 0x3 }
	public enum Layer { Reserved = 0x0, LayerIII = 0x1, LayerII = 0x2, LayerI = 0x3 }
	public enum ChannelMode { Stereo = 0x0, JointStereo = 0x1, DualChannel = 0x2, Mono = 0x3 }
	public enum JoinBands { Bands4_31 = 0x0, Bands8_31 = 0x1, Bands12_31 = 0x2, Bands16_31 = 0x3, Dynamic = 0x4 }
	public enum JoinMode { None = 0x0, IntensityStereo = 0x1, MSStereo = 0x2, IntensityAndMSStereo = 0x3 }
	public enum Emphasis { None = 0x0, MS50_15 = 0x1, Reserved = 0x2, CcitJ_17 = 0x3 }
	public class MpegAudioFrame
	{
		static readonly int[] version1Layer1Bitrates = new[] { 0, 32, 64, 96, 128, 160, 192, 224, 256, 288, 320, 352, 384, 416, 448, 0 };
		static readonly int[] version1Layer2Bitrates = new[] { 0, 32, 48, 56, 64, 80, 96, 112, 128, 160, 192, 224, 256, 320, 384, 0 };
		static readonly int[] version1Layer3Bitrates = new[] { 0, 32, 40, 48, 56, 64, 80, 96, 112, 128, 160, 192, 224, 256, 320, 0 };
		static readonly int[] version2Layer1Bitrates = new[] { 0, 32, 48, 56, 64, 80, 96, 112, 128, 144, 160, 176, 192, 224, 256, 0 };
		static readonly int[] version2Layer2Bitrates = new[] { 0, 8, 16, 24, 32, 40, 48, 56, 64, 80, 96, 112, 128, 144, 160, 0 };
		static readonly int[] version2Layer3Bitrates = new[] { 0, 8, 16, 24, 32, 40, 48, 56, 64, 80, 96, 112, 128, 144, 160, 0 };
		static readonly int[] version1SamplingRates = new[] { 44100, 48000, 32000, 0 };
		static readonly int[] version2SamplingRates = new[] { 22050, 24000, 16000, 0 };
		static readonly int[] version25SamplingRates = new[] { 11025, 12000, 8000, 0 };

		readonly BitField sync;
		readonly Version version;
		readonly Layer layer;
		readonly bool errorProtection;
		readonly int dataRate;
		readonly int sampleRate;
		readonly bool padding;
		readonly bool privateBit;
		readonly ChannelMode channelMode;
		readonly JoinBands joinBands;
		readonly JoinMode joinMode;
		readonly bool copyright;
		readonly bool original;
		readonly Emphasis emphasis;
		readonly ushort checksum;
		readonly byte[] data;

		public BitField Sync { get { return sync; } }
		public Version Version { get { return version; } }
		public Layer Layer { get { return layer; } }
		public bool ErrorProtection { get { return errorProtection; } }
		public int DataRate { get { return dataRate; } }
		public int SampleRate { get { return sampleRate; } }
		public bool Padding { get { return padding; } }
		public bool PrivateBit { get { return privateBit; } }
		public ChannelMode ChannelMode { get { return channelMode; } }
		public JoinBands JoinBands { get { return joinBands; } }
		public JoinMode JoinMode { get { return joinMode; } }
		public bool Copyright { get { return copyright; } }
		public bool Original { get { return original; } }
		public Emphasis Emphasis { get { return emphasis; } }
		public ushort Checksum { get { return checksum; } }
		public IEnumerable<byte> Data { get { return data; } }
		public int Length { get { return 4 + (errorProtection ? 2 : 0) + data.Length; } }

		public MpegAudioFrame(BinaryReader reader)
		{
			BitField header = BitField.FromBytes(reader.ReadBytes(4));

			this.sync = header.GetRange(0, 11);
			if (sync.Bits.Any(bit => !bit)) throw new ArgumentException(string.Format("Incorrect sync '{0}', expected '11111111111'.", sync));
			this.version = header.GetRange(11, 13).Value.ToEnumeration<Version>();
			if (version == Version.Reserved) throw new ArgumentException(string.Format("Incorrect version '{0}'.", version));
			this.layer = header.GetRange(13, 15).Value.ToEnumeration<Layer>();
			if (layer == Layer.Reserved) throw new ArgumentException(string.Format("Incorrect layer '{0}'.", layer));
			this.errorProtection = !header.GetBit(15);
			this.dataRate = GetBitRate(version, layer, header.GetRange(16, 20).Value) * 1000 / 8;
			this.sampleRate = GetSamplingRate(version, header.GetRange(20, 22).Value);
			this.padding = header.GetBit(22);
			this.privateBit = header.GetBit(23);
			this.channelMode = header.GetRange(24, 26).Value.ToEnumeration<ChannelMode>();
			switch (layer)
			{
				case Layer.LayerI:
				case Layer.LayerII:
					this.joinBands = header.GetRange(26, 28).Value.ToEnumeration<JoinBands>();
					this.joinMode = JoinMode.IntensityStereo;
					break;
				case Layer.LayerIII:
					this.joinBands = JoinBands.Dynamic;
					this.joinMode = header.GetRange(26, 28).Value.ToEnumeration<JoinMode>();
					break;
				default: throw new InvalidOperationException();
			}
			this.copyright = header.GetBit(28);
			this.original = header.GetBit(29);
			this.emphasis = header.GetRange(30, 32).Value.ToEnumeration<Emphasis>();

			// TODO: Test for correctness of the checksum
			this.checksum = errorProtection ? reader.ReadUInt16() : (ushort)0;

			// TODO: This is not always correct
			int headerLength = 4;
			int checksumLength = errorProtection ? 2 : 0;
			int dataLength = GetSampleCount(version, layer) * dataRate / sampleRate;
			int paddingLength = padding ? GetSlotLength(layer) : 0;
			this.data = reader.ReadBytes(dataLength + paddingLength - headerLength - checksumLength);
		}

		public override string ToString()
		{
			StringBuilder result = new StringBuilder();

			result.AppendLine("Sync: " + sync);
			result.AppendLine("Version: " + version);
			result.AppendLine("Layer: " + layer);
			result.AppendLine("Error protection: " + errorProtection);
			result.AppendLine("Data rate (B/s): " + dataRate);
			result.AppendLine("Sample rate (Hz): " + sampleRate);
			result.AppendLine("Padding: " + padding);
			result.AppendLine("Private bit: " + privateBit);
			result.AppendLine("Channel mode: " + channelMode);
			result.AppendLine("Join bands: " + joinBands);
			result.AppendLine("Join mode: " + joinMode);
			result.AppendLine("Copyright: " + copyright);
			result.AppendLine("Original: " + original);
			result.AppendLine("Emphasis: " + emphasis);
			if (errorProtection) result.AppendLine("Checksum: " + checksum.ToString("X4"));
			result.AppendLine("Data length: " + data.Length);

			return result.ToString();
		}

		static int GetBitRate(Version version, Layer layer, int value)
		{
			if (value <= 0 || value >= 15) throw new ArgumentOutOfRangeException("value");

			switch (version)
			{
				case Version.Mpeg1:
					switch (layer)
					{
						case Layer.LayerI: return version1Layer1Bitrates[value];
						case Layer.LayerII: return version1Layer2Bitrates[value];
						case Layer.LayerIII: return version1Layer3Bitrates[value];
						default: throw new ArgumentOutOfRangeException("layer");
					}
				case Version.Mpeg2:
				case Version.Mpeg25:
					switch (layer)
					{
						case Layer.LayerI: return version2Layer1Bitrates[value];
						case Layer.LayerII: return version2Layer2Bitrates[value];
						case Layer.LayerIII: return version2Layer3Bitrates[value];
						default: throw new ArgumentOutOfRangeException("layer");
					}
				default: throw new ArgumentOutOfRangeException("version");
			}
		}
		static int GetSamplingRate(Version version, int value)
		{
			if (value < 0 || value >= 3) throw new ArgumentOutOfRangeException("value");

			switch (version)
			{
				case Version.Mpeg1: return version1SamplingRates[value];
				case Version.Mpeg2: return version2SamplingRates[value];
				case Version.Mpeg25: return version25SamplingRates[value];
				default: throw new ArgumentOutOfRangeException("version");
			}
		}
		static int GetSampleCount(Version version, Layer layer)
		{
			switch (layer)
			{
				case Layer.LayerI: return 0x180;
				case Layer.LayerII: return 0x480;
				case Layer.LayerIII:
					switch (version)
					{
						case Version.Mpeg1: return 0x480;
						case Version.Mpeg2: return 0x240;
						case Version.Mpeg25: return 0x240;
						default: throw new InvalidOperationException();
					}
				default: throw new InvalidOperationException();
			}
		}
		static int GetSlotLength(Layer layer)
		{
			switch (layer)
			{
				case Layer.LayerI: return 4;
				case Layer.LayerII: return 1;
				case Layer.LayerIII: return 1;
				default: throw new InvalidOperationException();
			}
		}
	}
}
