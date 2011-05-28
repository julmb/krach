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

namespace Krach.Formats.Mpeg
{
	public abstract class MpegAudioFrame
	{
		readonly BitField header;
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

		public BitField Header { get { return header; } }
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

		public MpegAudioFrame(MpegAudioFrame referenceFrame) : this(referenceFrame.header)
		{
			if (referenceFrame == null) throw new ArgumentNullException("referenceFrame");
		}
		public MpegAudioFrame(BinaryReader reader) : this(BitField.FromBytes(reader.ReadBytes(4)))
		{
			if (reader == null) throw new ArgumentNullException("reader");

			// TODO: Test for correctness of the checksum
			this.checksum = hasErrorProtection ? reader.ReadUInt16() : (ushort)0;
		}
		MpegAudioFrame(BitField header)
		{
			this.header = header;
			if (header.Length != 32) throw new ArgumentException(string.Format("Incorrect header length '{0}', expected '32'.", header.Bits.Count()));

			this.sync = header[0, 11];
			if (sync.Value != 0x07FF) throw new ArgumentException(string.Format("Incorrect sync '{0}', expected '11111111111'.", sync));
			this.version = (MpegAudioVersion)header[11, 13].Value;
			if (version == MpegAudioVersion.Reserved) throw new ArgumentException(string.Format("Incorrect version '{0}'.", version));
			this.layer = (MpegAudioLayer)header[13, 15].Value;
			if (layer == MpegAudioLayer.Reserved) throw new ArgumentException(string.Format("Incorrect layer '{0}'.", layer));
			this.hasErrorProtection = !header[15];
			this.dataRate = MpegAudioSpecification.GetBitRate(version, layer, header[16, 20].Value) * 1000 / 8;
			this.sampleRate = MpegAudioSpecification.GetSamplingRate(version, header[20, 22].Value);
			this.hasPadding = header[22];
			this.isPrivate = header[23];
			this.channelMode = (MpegAudioChannelMode)header[24, 26].Value;
			switch (layer)
			{
				case MpegAudioLayer.LayerI:
				case MpegAudioLayer.LayerII:
					this.joinBands = (MpegAudioJoinBands)header[26, 28].Value;
					this.joinMode = MpegAudioJoinMode.IntensityStereo;
					break;
				case MpegAudioLayer.LayerIII:
					this.joinBands = MpegAudioJoinBands.Dynamic;
					this.joinMode = (MpegAudioJoinMode)header[26, 28].Value;
					break;
				default: throw new InvalidOperationException();
			}
			this.isCopyrighted = header[28];
			this.isOriginal = header[29];
			this.emphasis = (MpegAudioEmphasis)header[30, 32].Value;
		}

		public virtual void Write(BinaryWriter writer)
		{
			writer.Write(header.Bytes.ToArray());

			if (hasErrorProtection) writer.Write(checksum);
		}

		public static bool AreCompatible(MpegAudioFrame frame1, MpegAudioFrame frame2)
		{
			if (frame1 == null) throw new ArgumentNullException("frame1");
			if (frame2 == null) throw new ArgumentNullException("frame2");

			return frame1.version == frame2.version && frame1.layer == frame2.layer && frame1.sampleRate == frame2.sampleRate;
		}
	}
}
