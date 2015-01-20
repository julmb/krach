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
	public abstract class MpegAudioFrame : IParseItem
	{
		readonly MpegAudioVersion version;
		readonly MpegAudioLayer layer;
		readonly bool hasErrorProtection;
		readonly int bitRateID;
		readonly int sampleRateID;
		readonly bool hasPadding;
		readonly bool privateBit;
		readonly MpegAudioChannelMode channelMode;
		readonly int joinID;
		readonly bool isCopyrighted;
		readonly bool isOriginal;
		readonly MpegAudioEmphasis emphasis;
		readonly ushort checksum;

		public MpegAudioVersion Version { get { return version; } }
		public MpegAudioLayer Layer { get { return layer; } }
		public bool HasErrorProtection { get { return hasErrorProtection; } }
		public int BitRateID { get { return bitRateID; } }
		public int SampleRateID { get { return sampleRateID; } }
		public bool HasPadding { get { return hasPadding; } }
		public bool PrivateBit { get { return privateBit; } }
		public MpegAudioChannelMode ChannelMode { get { return channelMode; } }
		public int JoinID { get { return joinID; } }
		public bool IsCopyrighted { get { return isCopyrighted; } }
		public bool IsOriginal { get { return isOriginal; } }
		public MpegAudioEmphasis Emphasis { get { return emphasis; } }
		public ushort Checksum { get { return checksum; } }

		public MpegAudioJoinBands JoinBands { get { return MpegAudioSpecification.GetJoinBands(layer, joinID); } }
		public MpegAudioJoinMode JoinMode { get { return MpegAudioSpecification.GetJoinMode(layer, joinID); } }
		public int SampleCount { get { return MpegAudioSpecification.GetSampleCount(version, layer); } }
		public int DataRate { get { return MpegAudioSpecification.GetBitRate(version, layer, bitRateID) * 1000 / 8; } }
		public int SampleRate { get { return MpegAudioSpecification.GetSamplingRate(version, sampleRateID); } }
		public int SlotLength { get { return MpegAudioSpecification.GetSlotLength(layer); } }
		public int SideInformationLength { get { return MpegAudioSpecification.GetSideInformationLength(version, channelMode); } }
		public double AudioLength { get { return (double)SampleCount / (double)SampleRate; } }
		public int HeaderLength { get { return 4; } }
		public int ChecksumLength { get { return hasErrorProtection ? 2 : 0; } }
		public int DataLength { get { return GetTotalLength(version, layer, bitRateID, sampleRateID) + (hasPadding ? SlotLength : 0) - HeaderLength - ChecksumLength; } }
		public int TotalLength { get { return HeaderLength + ChecksumLength + DataLength; } }
		public MpegAudioFrameType Type { get { return new MpegAudioFrameType(version, layer, sampleRateID, channelMode == MpegAudioChannelMode.Mono ? 1 : 2); } }
		public virtual bool HasChanged { get { return false; } }
		
		protected MpegAudioFrame
		(
			MpegAudioVersion version,
			MpegAudioLayer layer,
			bool hasErrorProtection,
			int bitRateID,
			int sampleRateID,
			bool hasPadding,
			bool isPrivate,
			MpegAudioChannelMode channelMode,
			int joinID,
			bool isCopyrighted,
			bool isOriginal,
			MpegAudioEmphasis emphasis,
			ushort checksum
		)
		{
			if (version == MpegAudioVersion.Reserved) throw new ArgumentException(string.Format("Incorrect version '{0}'.", version));
			if (layer == MpegAudioLayer.Reserved) throw new ArgumentException(string.Format("Incorrect layer '{0}'.", layer));
			if (bitRateID <= 0 || bitRateID >= 15) throw new ArgumentException(string.Format("Incorrect bit rate ID '{0}'.", bitRateID));
			if (sampleRateID < 0 || sampleRateID >= 3) throw new ArgumentException(string.Format("Incorrect sample rate ID '{0}'.", sampleRateID));

			this.version = version;
			this.layer = layer;
			this.hasErrorProtection = hasErrorProtection;
			this.bitRateID = bitRateID;
			this.sampleRateID = sampleRateID;
			this.hasPadding = hasPadding;
			this.privateBit = isPrivate;
			this.channelMode = channelMode;
			this.joinID = joinID;
			this.isCopyrighted = isCopyrighted;
			this.isOriginal = isOriginal;
			this.emphasis = emphasis;

			this.checksum = checksum;
		}
		protected MpegAudioFrame(BinaryReader reader)
		{
			if (reader == null) throw new ArgumentNullException("reader");

			BitField header = BitField.FromBytes(reader.ReadBytes(4));
			if (header.Length != 32) throw new InvalidDataException(string.Format("Incorrect header length '{0}', expected '32'.", header.Bits.Count()));

			if (header[0, 11].Value != 0x07FF) throw new InvalidDataException(string.Format("Incorrect sync '{0}', expected '11111111111'.", header[0, 11]));
			this.version = (MpegAudioVersion)header[11, 13].Value;
			if (version == MpegAudioVersion.Reserved) throw new InvalidDataException(string.Format("Incorrect version '{0}'.", version));
			this.layer = (MpegAudioLayer)header[13, 15].Value;
			if (layer == MpegAudioLayer.Reserved) throw new InvalidDataException(string.Format("Incorrect layer '{0}'.", layer));
			this.hasErrorProtection = !header[15];
			this.bitRateID = header[16, 20].Value;
			if (bitRateID == 0 || bitRateID == 15) throw new InvalidDataException(string.Format("Incorrect bit rate ID '{0}'.", bitRateID));
			this.sampleRateID = header[20, 22].Value;
			if (sampleRateID == 3) throw new InvalidDataException(string.Format("Incorrect sample rate ID '{0}'.", sampleRateID));
			this.hasPadding = header[22];
			this.privateBit = header[23];
			this.channelMode = (MpegAudioChannelMode)header[24, 26].Value;
			this.joinID = header[26, 28].Value;
			this.isCopyrighted = header[28];
			this.isOriginal = header[29];
			this.emphasis = (MpegAudioEmphasis)header[30, 32].Value;

			this.checksum = hasErrorProtection ? reader.ReadUInt16() : (ushort)0;
		}

		public virtual void Write(BinaryWriter writer)
		{
			BitField header = BitField.FromBits
			(
				Enumerables.Concatenate
				(
					BitField.FromValue(0x07FF, 11).Bits,
					BitField.FromValue((int)version, 2).Bits,
					BitField.FromValue((int)layer, 2).Bits,
					Enumerables.Create(!hasErrorProtection),
					BitField.FromValue(bitRateID, 4).Bits,
					BitField.FromValue(sampleRateID, 2).Bits,
					Enumerables.Create(hasPadding),
					Enumerables.Create(privateBit),
					BitField.FromValue((int)channelMode, 2).Bits,
					BitField.FromValue((int)joinID, 2).Bits,
					Enumerables.Create(isCopyrighted),
					Enumerables.Create(isOriginal),
					BitField.FromValue((int)emphasis, 2).Bits
				)
			);

			writer.Write(header.Bytes.ToArray());

			if (hasErrorProtection) writer.Write(checksum);
		}

		public static int GetTotalLength(MpegAudioVersion version, MpegAudioLayer layer, int bitRateID, int sampleRateID)
		{
			int sampleCount = MpegAudioSpecification.GetSampleCount(version, layer);
			int dataRate = MpegAudioSpecification.GetBitRate(version, layer, bitRateID) * 1000 / 8;
			int sampleRate = MpegAudioSpecification.GetSamplingRate(version, sampleRateID);

			return sampleCount * dataRate / sampleRate;
		}
	}
}
