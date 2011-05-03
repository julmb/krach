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
using System.Linq;
using System.Text;
using System.IO;
using Krach.Extensions;
using Krach.Formats.Tags.Id3v2;

namespace Krach.Formats.Mpeg
{
	public class MpegAudioFile
	{
		readonly Id3v2Tag tag;
		readonly List<MpegAudioFrame> frames = new List<MpegAudioFrame>();

		public Id3v2Tag Tag { get { return tag; } }
		public IEnumerable<MpegAudioFrame> Frames { get { return frames; } }

		public MpegAudioFile(BinaryReader reader)
		{
			if (new string(reader.PeekChars(3)) == "ID3") tag = new Id3v2Tag(reader);

			while (reader.BaseStream.Position < reader.BaseStream.Length) frames.Add(new MpegAudioFrame(reader));
		}
	}
}
