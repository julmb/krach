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
using Krach.Extensions;
using Krach.Formats.Mpeg;
using Krach.Formats.Tags.ID3v2;
using Krach.Formats;
using Krach.Formats.Tags.ID3v1;
using Krach.Formats.Mpeg.MetaData;

namespace Krach.Formats.Mpeg
{
	public class MpegAudioFile : IParseItem
	{
		readonly ID3v2Tag id3v2Tag;
		readonly IEnumerable<MpegAudioFrame> mpegAudioFrames;

		public ID3v2Tag ID3v2Tag { get { return id3v2Tag; } }
		public IEnumerable<MpegAudioFrame> MpegAudioFrames { get { return mpegAudioFrames; } }
		public virtual bool HasChanged { get { return id3v2Tag.HasChanged || mpegAudioFrames.Any(frame => frame.HasChanged); } }

		public MpegAudioFile(ID3v2Tag id3v2Tag, IEnumerable<MpegAudioFrame> mpegAudioFrames)
		{
			if (id3v2Tag == null) throw new ArgumentNullException("id3v2Tag");
			if (mpegAudioFrames == null) throw new ArgumentNullException("mpegAudioFrames");

			this.id3v2Tag = id3v2Tag;
			this.mpegAudioFrames = mpegAudioFrames;
		}
		public MpegAudioFile(BinaryReader reader)
		{
			this.id3v2Tag = new ID3v2Tag(reader);
			this.mpegAudioFrames = ReadFrames(reader).ToArray();

			if (reader.BaseStream.Position < reader.BaseStream.Length) throw new InvalidDataException("Found data after MPEG frames.");
		}

		public void Write(BinaryWriter writer)
		{
			id3v2Tag.Write(writer);
			foreach (MpegAudioFrame mpegAudioFrame in mpegAudioFrames) mpegAudioFrame.Write(writer);
		}

		static IEnumerable<MpegAudioFrame> ReadFrames(BinaryReader reader)
		{
			MpegAudioFrame referenceFrame = new MpegAudioDataFrame(reader);

			yield return referenceFrame;

			while (reader.BaseStream.Position < reader.BaseStream.Length)
			{
				MpegAudioFrame frame = new MpegAudioDataFrame(reader);

				if (frame.Type != referenceFrame.Type) throw new InvalidDataException("Got a frame of different type than the reference frame.");

				yield return frame;
			}
		}
	}
}
