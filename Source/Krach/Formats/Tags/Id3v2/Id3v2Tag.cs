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
using Krach.Extensions;
using System.Text;

namespace Krach.Formats.Tags.Id3v2
{
	public class Id3v2Tag
	{
		readonly string identifier;
		readonly byte majorVersion;
		readonly byte minorVersion;
		readonly bool unsynchronisation;
		readonly bool extendedHeader;
		readonly bool experimental;
		readonly int length;
		readonly List<Id3v2Frame> frames = new List<Id3v2Frame>();

		public string Identifier { get { return identifier; } }
		public byte MajorVersion { get { return majorVersion; } }
		public byte MinorVersion { get { return minorVersion; } }
		public bool Unsynchronization { get { return unsynchronisation; } }
		public bool ExtendedHeader { get { return extendedHeader; } }
		public bool Experimental { get { return experimental; } }
		public int Length { get { return length + 10; } }
		public IEnumerable<Id3v2Frame> Frames { get { return frames; } }

		public Id3v2Tag(BinaryReader reader)
		{
			this.identifier = Encoding.ASCII.GetString(reader.ReadBytes(3));
			if (identifier != "ID3") throw new ArgumentException(string.Format("Wrong identifier '{0}', should be 'ID3'.", identifier));

			this.majorVersion = reader.ReadByte();
			this.minorVersion = reader.ReadByte();

			// TODO:
			if (majorVersion != 3) throw new NotImplementedException();

			BitField flags = BitField.FromBytes(reader.ReadBytes(1));
			if (flags.GetRange(3, 8).Value != 0) throw new ArgumentException(string.Format("Found non-used but set flags '{0}'", flags));

			this.unsynchronisation = flags.GetBit(0);
			this.extendedHeader = flags.GetBit(1);
			this.experimental = flags.GetBit(2);

			// TODO:
			if (unsynchronisation || extendedHeader || experimental) throw new NotImplementedException();

			BitField lengthData = BitField.FromBytes(reader.ReadBytes(4));
			if (lengthData.GetBit(0) || lengthData.GetBit(8) || lengthData.GetBit(16) || lengthData.GetBit(24)) throw new ArgumentException(string.Format("Found wrongly set bits in the length field '{0}'.", lengthData));
			lengthData = new BitField(Enumerables.Concatenate(lengthData.Bits.GetRange(1, 8), lengthData.Bits.GetRange(9, 16), lengthData.Bits.GetRange(17, 24), lengthData.Bits.GetRange(25, 32)));
			this.length = lengthData.Value;

			long endPosition = reader.BaseStream.Position + length;

			while (reader.BaseStream.Position < endPosition)
			{
				// Padding has started
				if (reader.PeekChar() == 0) break;

				string frameIdentifier = Encoding.ASCII.GetString(reader.Peek(4));;

				Id3v2Frame frame;

				if (frameIdentifier[0] == 'T')
				{
					if (frameIdentifier == "TXXX") frame = new Id3v2UserTextFrame(reader);
					else frame = new Id3v2TextFrame(reader);
				}
				else frame = new Id3v2GenericFrame(reader);

				frames.Add(frame);
			}

			// Move to the end of the tag (skip potential padding)
			reader.BaseStream.Position = endPosition;
		}
	}
}
