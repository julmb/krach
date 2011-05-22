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
using System.Text;
using Krach.Extensions;

namespace Krach.Formats.Mpeg
{
	[Flags]
	public enum MpegAudioXingFields { FrameCount = 0x0001, AudioLength = 0x0002, TableOfContents = 0x0004, QualityIndicator = 0x0008 }
	public class MpegAudioXingFrame : MpegAudioFrame
	{
		readonly byte[] sideInformation;
		readonly string identifier;
		readonly MpegAudioXingFields fields;
		readonly int frameCount;
		readonly int audioLength;
		readonly byte[] tableOfContents;
		readonly int qualityIndicator;
		readonly byte[] padding;

		public IEnumerable<byte> SideInformation { get { return sideInformation; } }
		public string Identifier { get { return identifier; } }
		public MpegAudioXingFields Fields { get { return fields; } }
		public int FrameCount { get { return frameCount; } }
		public int AudioLength { get { return audioLength; } }
		public IEnumerable<byte> TableOfContents { get { return tableOfContents; } }
		public int QualityIndicator { get { return qualityIndicator; } }
		public IEnumerable<byte> Padding { get { return padding; } }
		public int FrameCountLength { get { return ((fields & MpegAudioXingFields.FrameCount) != 0) ? 4 : 0; } }
		public int AudioLengthLength { get { return ((fields & MpegAudioXingFields.AudioLength) != 0) ? 4 : 0; } }
		public int TableOfContentsLength { get { return ((fields & MpegAudioXingFields.TableOfContents) != 0) ? 100 : 0; } }
		public int QualityIndicatorLength { get { return ((fields & MpegAudioXingFields.QualityIndicator) != 0) ? 4 : 0; } }

		public MpegAudioXingFrame(BinaryReader reader)
			: base(reader)
		{
			this.sideInformation = reader.ReadBytes(SideInformationLength);

			this.identifier = Encoding.ASCII.GetString(reader.ReadBytes(4));
			if (identifier != "Xing") throw new ArgumentException(string.Format("Wrong identifier '{0}', should be 'Xing'.", identifier));

			this.fields = (MpegAudioXingFields)Binary.SwitchEndianness(reader.ReadUInt32());
			if (((int)fields & 0xFFFFFFF0) != 0) throw new ArgumentException(string.Format("Unexpected field flags '{0}' in Xing tag.", fields));

			if ((fields & MpegAudioXingFields.FrameCount) != 0) this.frameCount = Binary.SwitchEndianness(reader.ReadInt32());
			if ((fields & MpegAudioXingFields.AudioLength) != 0) this.audioLength = Binary.SwitchEndianness(reader.ReadInt32());
			if ((fields & MpegAudioXingFields.TableOfContents) != 0) this.tableOfContents = reader.ReadBytes(100);
			if ((fields & MpegAudioXingFields.QualityIndicator) != 0) this.qualityIndicator = Binary.SwitchEndianness(reader.ReadInt32());

			this.padding = reader.ReadBytes(DataLength - (4 + 4 + FrameCountLength + AudioLengthLength + TableOfContentsLength + QualityIndicatorLength));
		}

		public override void Write(BinaryWriter writer)
		{
			base.Write(writer);

			writer.Write(sideInformation);

			writer.Write(Encoding.ASCII.GetBytes("Xing"));

			writer.Write(Binary.SwitchEndianness((int)fields));

			if ((fields & MpegAudioXingFields.FrameCount) != 0) writer.Write(Binary.SwitchEndianness(frameCount));
			if ((fields & MpegAudioXingFields.AudioLength) != 0) writer.Write(Binary.SwitchEndianness(audioLength));
			if ((fields & MpegAudioXingFields.TableOfContents) != 0) writer.Write(tableOfContents);
			if ((fields & MpegAudioXingFields.QualityIndicator) != 0) writer.Write(Binary.SwitchEndianness(qualityIndicator));

			writer.Write(padding);
		}
	}
}
