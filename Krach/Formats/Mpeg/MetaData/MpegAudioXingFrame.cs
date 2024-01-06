﻿// Copyright © Julian Brunner 2010 - 2011

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
using Krach.Basics;
using Krach.Extensions;
using Krach.Maps;
using Krach.Maps.Abstract;
using Krach.Maps.Scalar;

namespace Krach.Formats.Mpeg.MetaData
{
	[Flags]
	public enum MpegAudioXingFields { FrameCount = 0x00000001, AudioDataLength = 0x00000002, TableOfContents = 0x00000004, QualityIndicator = 0x0000008 }
	public class MpegAudioXingFrame : MpegAudioMetaDataFrame
	{
		readonly MpegAudioXingFields fields;
		readonly int frameCount;
		readonly int audioDataLength;
		readonly byte[] tableOfContents;
		readonly int qualityIndicator;
		
		public override int MetaDataOffset { get { return SideInformationLength - ChecksumLength; } }
		public override string MetaDataIdentifier { get { return "Xing"; } }
		public MpegAudioXingFields Fields { get { return fields; } }
		public int FrameCount { get { return frameCount; } }
		public int AudioDataLength { get { return audioDataLength; } }
		public IEnumerable<byte> TableOfContents { get { return tableOfContents; } }
		public int QualityIndicator { get { return qualityIndicator; } }
		public int FrameCountLength { get { return ((fields & MpegAudioXingFields.FrameCount) != 0) ? 4 : 0; } }
		public int AudioDataLengthLength { get { return ((fields & MpegAudioXingFields.AudioDataLength) != 0) ? 4 : 0; } }
		public int TableOfContentsLength { get { return ((fields & MpegAudioXingFields.TableOfContents) != 0) ? 100 : 0; } }
		public int QualityIndicatorLength { get { return ((fields & MpegAudioXingFields.QualityIndicator) != 0) ? 4 : 0; } }
		public int XingContentLength { get { return 4 + FrameCountLength + AudioDataLengthLength + TableOfContentsLength + QualityIndicatorLength; } }
		public int PaddingLength { get { return MetaDataContentLength - XingContentLength; } }

		public MpegAudioXingFrame(IEnumerable<MpegAudioDataFrame> mpegAudioDataFrames)
			: base
			(
				mpegAudioDataFrames.First().Version,
				mpegAudioDataFrames.First().Layer,
				mpegAudioDataFrames.First().HasErrorProtection,
				FindLowestBitRateID(mpegAudioDataFrames.First()),
				mpegAudioDataFrames.First().SampleRateID,
				false,
				mpegAudioDataFrames.First().PrivateBit,
				mpegAudioDataFrames.First().ChannelMode,
				mpegAudioDataFrames.First().JoinID,
				mpegAudioDataFrames.First().IsCopyrighted,
				mpegAudioDataFrames.First().IsOriginal,
				mpegAudioDataFrames.First().Emphasis,
				0x0000
			)
		{
			if (mpegAudioDataFrames == null) throw new ArgumentNullException("mpegAudioDataFrames");

			this.fields = MpegAudioXingFields.FrameCount | MpegAudioXingFields.AudioDataLength | MpegAudioXingFields.TableOfContents;

			this.frameCount = mpegAudioDataFrames.Count();
			this.audioDataLength = mpegAudioDataFrames.Sum(frame => frame.TotalLength);
			this.tableOfContents = GenerateTableOfContents(mpegAudioDataFrames).ToArray();
		}
		public MpegAudioXingFrame(BinaryReader reader)
			: base(reader)
		{
			this.fields = (MpegAudioXingFields)Binary.SwitchEndianness(reader.ReadUInt32());
			if (((int)fields & 0xFFFFFFF0) != 0) throw new InvalidDataException(string.Format("Unexpected field flags '{0}' in Xing tag.", fields));

			if ((fields & MpegAudioXingFields.FrameCount) != 0) this.frameCount = Binary.SwitchEndianness(reader.ReadInt32());
			if ((fields & MpegAudioXingFields.AudioDataLength) != 0) this.audioDataLength = Binary.SwitchEndianness(reader.ReadInt32());
			if ((fields & MpegAudioXingFields.TableOfContents) != 0) this.tableOfContents = reader.ReadBytes(100);
			if ((fields & MpegAudioXingFields.QualityIndicator) != 0) this.qualityIndicator = Binary.SwitchEndianness(reader.ReadInt32());

			if (PaddingLength < 0) throw new InvalidDataException(string.Format("Invalid padding length '{0}'.", PaddingLength));

			if (reader.ReadBytes(PaddingLength).Length != PaddingLength) throw new InvalidDataException("Invalid padding length");
		}

		public override void Write(BinaryWriter writer)
		{
			base.Write(writer);

			writer.Write(Binary.SwitchEndianness((int)fields));

			if ((fields & MpegAudioXingFields.FrameCount) != 0) writer.Write(Binary.SwitchEndianness(frameCount));
			if ((fields & MpegAudioXingFields.AudioDataLength) != 0) writer.Write(Binary.SwitchEndianness(audioDataLength));
			if ((fields & MpegAudioXingFields.TableOfContents) != 0) writer.Write(tableOfContents);
			if ((fields & MpegAudioXingFields.QualityIndicator) != 0) writer.Write(Binary.SwitchEndianness(qualityIndicator));

			writer.Write(new byte[PaddingLength]);
		}

		IEnumerable<byte> GenerateTableOfContents(IEnumerable<MpegAudioDataFrame> mpegAudioDataFrames)
		{
			double totalDataLength = mpegAudioDataFrames.Sum(frame => frame.TotalLength);
			double totalTimeLength = mpegAudioDataFrames.Sum(frame => frame.AudioLength);

			IMap<double, double> toAbsoluteTime = new RangeMap(new OrderedRange<double>(0, 99), new OrderedRange<double>(0, totalTimeLength), Mappers.Linear);
			IMap<double, double> toRelativeData = new RangeMap(new OrderedRange<double>(0, totalDataLength), new OrderedRange<double>(0, 255), Mappers.Linear);

			for (int index = 0; index < 100; index++) yield return (byte)toRelativeData.Map(GetDataPosition(mpegAudioDataFrames, toAbsoluteTime.Map(index))).Round();
		}
		double GetDataPosition(IEnumerable<MpegAudioDataFrame> mpegAudioDataFrames, double timePosition)
		{
			double currentDataPosition = 0;
			double currentTimePosition = 0;

			foreach (MpegAudioDataFrame frame in mpegAudioDataFrames)
			{
				if (currentTimePosition >= timePosition) return currentDataPosition;

				currentDataPosition += frame.TotalLength;
				currentTimePosition += frame.AudioLength;
			}

			return currentDataPosition;
		}

		static int FindLowestBitRateID(MpegAudioFrame referenceFrame)
		{
			int requiredLength = 4 + 32 + 4 + 4 + 4 + 4 + 100 + 4;

			for (int bitRateID = 1; bitRateID < 15; bitRateID++)
				if (MpegAudioFrame.GetTotalLength(referenceFrame.Version, referenceFrame.Layer, bitRateID, referenceFrame.SampleRateID) >= requiredLength)
					return bitRateID;

			throw new InvalidOperationException("Found no bit rate ID for the requested frame size.");
		}
	}
}
