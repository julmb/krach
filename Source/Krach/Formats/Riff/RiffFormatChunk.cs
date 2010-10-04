// Copyright © Julian Brunner 2010

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

namespace Krach.Formats.Riff
{
	public class RiffFormatChunk : RiffChunk
	{
		readonly ushort format;
		readonly ushort channelCount;
		readonly uint sampleRate;
		readonly uint dataRate;
		readonly ushort blockSize;
		readonly ushort sampleSize;

		public ushort Format { get { return format; } }
		public ushort ChannelCount { get { return channelCount; } }
		public uint SampleRate { get { return sampleRate; } }
		public uint DataRate { get { return dataRate; } }
		public ushort BlockSize { get { return blockSize; } }
		public ushort SampleSize { get { return sampleSize; } }

		public RiffFormatChunk(ushort channelCount, uint sampleRate, ushort sampleSize)
			: base("fmt ", 16)
		{
			this.format = 1;
			this.channelCount = channelCount;
			this.sampleRate = sampleRate;
			this.dataRate = sampleRate * blockSize;
			this.blockSize = (ushort)(channelCount * sampleSize / 8);
			this.sampleSize = sampleSize;
		}
		public RiffFormatChunk(BinaryReader reader)
			: base(reader)
		{
			if (ID != "fmt ") throw new ArgumentException(string.Format("Wrong chunk ID '{0}', should be 'fmt '.", ID));
			if (Size != 16) throw new ArgumentException(string.Format("Incorrect chunk size '{0}', should be '16'.", Size));

			this.format = reader.ReadUInt16();
			this.channelCount = reader.ReadUInt16();
			this.sampleRate = reader.ReadUInt32();
			this.dataRate = reader.ReadUInt32();
			this.blockSize = reader.ReadUInt16();
			this.sampleSize = reader.ReadUInt16();

			if (format != 1) throw new ArgumentException(string.Format("Unsupported format code '{0}'.", format));
			if (blockSize != channelCount * sampleSize / 8) throw new ArgumentException(string.Format("Incorrect block size '{0}'.", blockSize));
			if (dataRate != sampleRate * blockSize) throw new ArgumentException(string.Format("Incorrect data rate '{0}'.", dataRate));
		}

		public override void Write(BinaryWriter writer)
		{
			base.Write(writer);

			writer.Write(format);
			writer.Write(channelCount);
			writer.Write(sampleRate);
			writer.Write(dataRate);
			writer.Write(blockSize);
			writer.Write(sampleSize);
		}
	}
}
