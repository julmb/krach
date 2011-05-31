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

using System.Collections.Generic;
using System.IO;
using Krach.Extensions;

namespace Krach.Formats.Tags.ID3v2
{
	public class ID3v2ImageFrame : ID3v2Frame
	{
		readonly byte encodingID;
		readonly string mimeType;
		readonly byte pictureType;
		readonly string description;
		readonly byte[] imageData;

		public byte EncodingID { get { return encodingID; } }
		public string MimeType { get { return mimeType; } }
		public byte PictureType { get { return pictureType; } }
		public string Description { get { return description; } }
		public IEnumerable<byte> ImageData { get { return imageData; } }

		public ID3v2ImageFrame(BinaryReader reader)
			: base(reader)
		{
			long headerStartPosition = reader.BaseStream.Position;

			this.encodingID = reader.ReadByte();
			this.mimeType = GetEncoding(encodingID).GetString(reader.ReadToNextZero());
			this.pictureType = reader.ReadByte();
			this.description = GetEncoding(encodingID).GetString(reader.ReadToNextZero());

			long headerEndPosition = reader.BaseStream.Position;

			long imageDataLength = DataLength - (headerEndPosition - headerStartPosition);

			if (imageDataLength < 0) throw new InvalidDataException(string.Format("Invalid image data length '{0}'.", imageDataLength));

			this.imageData = reader.ReadBytes((int)imageDataLength);
		}

		public override void Write(BinaryWriter writer)
		{
			base.Write(writer);

			writer.Write(encodingID);

			writer.Write(GetEncoding(encodingID).GetBytes(mimeType));
			writer.Write((byte)0);

			writer.Write(pictureType);

			writer.Write(GetEncoding(encodingID).GetBytes(description));
			writer.Write((byte)0);

			writer.Write(imageData);
		}
	}
}
