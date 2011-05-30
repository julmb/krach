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

namespace Krach.Formats.Mpeg
{
	public enum MpegAudioVersion { Mpeg25 = 0x0, Reserved = 0x1, Mpeg2 = 0x2, Mpeg1 = 0x3 }
	public enum MpegAudioLayer { Reserved = 0x0, LayerIII = 0x1, LayerII = 0x2, LayerI = 0x3 }
	public enum MpegAudioChannelMode { Stereo = 0x0, JointStereo = 0x1, DualChannel = 0x2, Mono = 0x3 }
	public enum MpegAudioJoinBands { Bands4_31 = 0x0, Bands8_31 = 0x1, Bands12_31 = 0x2, Bands16_31 = 0x3, Dynamic = 0x4 }
	public enum MpegAudioJoinMode { None = 0x0, IntensityStereo = 0x1, MSStereo = 0x2, IntensityAndMSStereo = 0x3 }
	public enum MpegAudioEmphasis { None = 0x0, MS50_15 = 0x1, Reserved = 0x2, CcitJ_17 = 0x3 }
	public static class MpegAudioSpecification
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

		public static int GetBitRate(MpegAudioVersion version, MpegAudioLayer layer, int value)
		{
			if (value <= 0 || value >= 15) throw new ArgumentOutOfRangeException("value");

			switch (version)
			{
				case MpegAudioVersion.Mpeg1:
					switch (layer)
					{
						case MpegAudioLayer.LayerI: return version1Layer1Bitrates[value];
						case MpegAudioLayer.LayerII: return version1Layer2Bitrates[value];
						case MpegAudioLayer.LayerIII: return version1Layer3Bitrates[value];
						default: throw new ArgumentOutOfRangeException("layer");
					}
				case MpegAudioVersion.Mpeg2:
				case MpegAudioVersion.Mpeg25:
					switch (layer)
					{
						case MpegAudioLayer.LayerI: return version2Layer1Bitrates[value];
						case MpegAudioLayer.LayerII: return version2Layer2Bitrates[value];
						case MpegAudioLayer.LayerIII: return version2Layer3Bitrates[value];
						default: throw new ArgumentOutOfRangeException("layer");
					}
				default: throw new ArgumentOutOfRangeException("version");
			}
		}
		public static int GetSamplingRate(MpegAudioVersion version, int value)
		{
			if (value < 0 || value >= 3) throw new ArgumentOutOfRangeException("value");

			switch (version)
			{
				case MpegAudioVersion.Mpeg1: return version1SamplingRates[value];
				case MpegAudioVersion.Mpeg2: return version2SamplingRates[value];
				case MpegAudioVersion.Mpeg25: return version25SamplingRates[value];
				default: throw new ArgumentOutOfRangeException("version");
			}
		}
		public static int GetSampleCount(MpegAudioVersion version, MpegAudioLayer layer)
		{
			switch (layer)
			{
				case MpegAudioLayer.LayerI: return 0x180;
				case MpegAudioLayer.LayerII: return 0x480;
				case MpegAudioLayer.LayerIII:
					switch (version)
					{
						case MpegAudioVersion.Mpeg1: return 0x480;
						case MpegAudioVersion.Mpeg2: return 0x240;
						case MpegAudioVersion.Mpeg25: return 0x240;
						default: throw new InvalidOperationException();
					}
				default: throw new InvalidOperationException();
			}
		}
		public static int GetSlotLength(MpegAudioLayer layer)
		{
			switch (layer)
			{
				case MpegAudioLayer.LayerI: return 4;
				case MpegAudioLayer.LayerII: return 1;
				case MpegAudioLayer.LayerIII: return 1;
				default: throw new InvalidOperationException();
			}
		}
		public static int GetSideInformationLength(MpegAudioVersion version, MpegAudioChannelMode channelMode)
		{
			switch (version)
			{
				case MpegAudioVersion.Mpeg1:
					switch (channelMode)
					{
						case MpegAudioChannelMode.Stereo:
						case MpegAudioChannelMode.JointStereo:
						case MpegAudioChannelMode.DualChannel:
							return 32;
						case MpegAudioChannelMode.Mono:
							return 17;
						default: throw new ArgumentException("channelMode");
					}
				case MpegAudioVersion.Mpeg2:
				case MpegAudioVersion.Mpeg25:
					switch (channelMode)
					{
						case MpegAudioChannelMode.Stereo:
						case MpegAudioChannelMode.JointStereo:
						case MpegAudioChannelMode.DualChannel:
							return 17;
						case MpegAudioChannelMode.Mono:
							return 9;
						default: throw new ArgumentException("channelMode");
					}
				default: throw new ArgumentException("version");
			}
		}
		public static MpegAudioJoinBands GetJoinBands(MpegAudioLayer layer, int joinID)
		{
			switch (layer)
			{
				case MpegAudioLayer.LayerI:
				case MpegAudioLayer.LayerII:
					return (MpegAudioJoinBands)joinID;
				case MpegAudioLayer.LayerIII:
					return MpegAudioJoinBands.Dynamic;
				default: throw new InvalidOperationException();
			}
		}
		public static MpegAudioJoinMode GetJoinMode(MpegAudioLayer layer, int joinID)
		{
			switch (layer)
			{
				case MpegAudioLayer.LayerI:
				case MpegAudioLayer.LayerII:
					return MpegAudioJoinMode.IntensityStereo;
				case MpegAudioLayer.LayerIII:
					return (MpegAudioJoinMode)joinID;
				default: throw new InvalidOperationException();
			}
		}
	}
}
