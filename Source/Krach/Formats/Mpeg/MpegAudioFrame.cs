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
using System.IO;
using System.Linq;
using Krach.Extensions;

namespace Krach.Formats.Mpeg
{
	public abstract class MpegAudioFrame
	{
		readonly BitField sync;
		readonly MpegAudioVersion version;
		readonly MpegAudioLayer layer;
		readonly bool hasErrorProtection;
		readonly int dataRate;
		readonly int sampleRate;
		readonly bool hasPadding;
		readonly bool isPrivate;
		readonly MpegAudioChannelMode channelMode;
		readonly MpegAudioJoinBands joinBands;
		readonly MpegAudioJoinMode joinMode;
		readonly bool isCopyrighted;
		readonly bool isOriginal;
		readonly MpegAudioEmphasis emphasis;
		readonly ushort checksum;

		public BitField Sync { get { return sync; } }
		public MpegAudioVersion Version { get { return version; } }
		public MpegAudioLayer Layer { get { return layer; } }
		public bool HasErrorProtection { get { return hasErrorProtection; } }
		public int DataRate { get { return dataRate; } }
		public int SampleRate { get { return sampleRate; } }
		public bool HasPadding { get { return hasPadding; } }
		public bool IsPrivate { get { return isPrivate; } }
		public MpegAudioChannelMode ChannelMode { get { return channelMode; } }
		public MpegAudioJoinBands JoinBands { get { return joinBands; } }
		public MpegAudioJoinMode JoinMode { get { return joinMode; } }
		public bool IsCopyrighted { get { return isCopyrighted; } }
		public bool IsOriginal { get { return isOriginal; } }
		public MpegAudioEmphasis Emphasis { get { return emphasis; } }
		public ushort Checksum { get { return checksum; } }
		public int HeaderLength { get { return 4; } }
		public int ChecksumLength { get { return hasErrorProtection ? 2 : 0; } }
		public int SideInformationLength { get { return MpegAudioSpecification.GetSideInformationLength(version, channelMode); } }
		public int DataLength { get { return MpegAudioSpecification.GetSampleCount(version, layer) * dataRate / sampleRate + (hasPadding ? MpegAudioSpecification.GetSlotLength(layer) : 0) - HeaderLength - ChecksumLength - SideInformationLength; } }
		public int TotalLength { get { return HeaderLength + ChecksumLength + SideInformationLength + DataLength; } }

		public MpegAudioFrame(BinaryReader reader)
		{
			if (reader == null) throw new ArgumentNullException("reader");

			BitField header = BitField.FromBytes(reader.ReadBytes(4));

			this.sync = header.GetRange(0, 11);
			if (sync.Bits.Any(bit => !bit)) throw new ArgumentException(string.Format("Incorrect sync '{0}', expected '11111111111'.", sync));
			this.version = header.GetRange(11, 13).Value.ToEnumeration<MpegAudioVersion>();
			if (version == MpegAudioVersion.Reserved) throw new ArgumentException(string.Format("Incorrect version '{0}'.", version));
			this.layer = header.GetRange(13, 15).Value.ToEnumeration<MpegAudioLayer>();
			if (layer == MpegAudioLayer.Reserved) throw new ArgumentException(string.Format("Incorrect layer '{0}'.", layer));
			this.hasErrorProtection = !header.GetBit(15);
			this.dataRate = MpegAudioSpecification.GetBitRate(version, layer, header.GetRange(16, 20).Value) * 1000 / 8;
			this.sampleRate = MpegAudioSpecification.GetSamplingRate(version, header.GetRange(20, 22).Value);
			this.hasPadding = header.GetBit(22);
			this.isPrivate = header.GetBit(23);
			this.channelMode = header.GetRange(24, 26).Value.ToEnumeration<MpegAudioChannelMode>();
			switch (layer)
			{
				case MpegAudioLayer.LayerI:
				case MpegAudioLayer.LayerII:
					this.joinBands = header.GetRange(26, 28).Value.ToEnumeration<MpegAudioJoinBands>();
					this.joinMode = MpegAudioJoinMode.IntensityStereo;
					break;
				case MpegAudioLayer.LayerIII:
					this.joinBands = MpegAudioJoinBands.Dynamic;
					this.joinMode = header.GetRange(26, 28).Value.ToEnumeration<MpegAudioJoinMode>();
					break;
				default: throw new InvalidOperationException();
			}
			this.isCopyrighted = header.GetBit(28);
			this.isOriginal = header.GetBit(29);
			this.emphasis = header.GetRange(30, 32).Value.ToEnumeration<MpegAudioEmphasis>();

			// TODO: Test for correctness of the checksum
			this.checksum = hasErrorProtection ? reader.ReadUInt16() : (ushort)0;
		}

		public static bool AreCompatible(MpegAudioFrame frame1, MpegAudioFrame frame2)
		{
			if (frame1 == null) throw new ArgumentNullException("frame1");
			if (frame2 == null) throw new ArgumentNullException("frame2");

			return frame1.version == frame2.version && frame1.layer == frame2.layer && frame1.sampleRate == frame2.sampleRate;
		}
	}
}
