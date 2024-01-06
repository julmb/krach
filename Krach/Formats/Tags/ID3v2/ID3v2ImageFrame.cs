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
using System.Text;
using SkiaSharp;

namespace Krach.Formats.Tags.ID3v2
{
	public enum ID3v2ImageType : byte
	{
		Other,
		FileIcon,
		OtherFileIcon,
		FrontCover,
		BackCover,
		LeafletPage,
		Media,
		LeadArtist,
		Artist,
		Conductor,
		Band,
		Composer,
		Lyricist,
		RecordingLocation,
		DuringRecording,
		DuringPerformance,
		MovieScreenCapture,
		ABrightColouredFish,
		Illustration,
		BandLogotype,
		PublisherLogotype
	}
	public class ID3v2ImageFrame : ID3v2EncodedFrame
	{
		readonly string mimeType;
		readonly ID3v2ImageType imageType;
		readonly string description;
		readonly byte[] imageData;
		readonly bool hasChanged = false;

		public string MimeType { get { return mimeType; } }
		public ID3v2ImageType ImageType { get { return imageType; } }
		public string Description { get { return description; } }
		public IEnumerable<byte> ImageData { get { return imageData; } }
		public override int DataLength { get { return base.DataLength + TextToData(Encoding.ASCII, mimeType + '\0').Length + 1 + TextToData(Encoding, description + '\0').Length + imageData.Length; } }
		public override bool HasChanged { get { return base.HasChanged || hasChanged; } }

		public ID3v2ImageFrame(IEnumerable<byte> imageData)
			: base("APIC", 0)
		{
			if (imageData == null) throw new ArgumentNullException("imageData");

			this.mimeType = GetMimeType(imageData);
			this.imageType = ID3v2ImageType.FrontCover;
			this.description = string.Empty;
			this.imageData = imageData.ToArray();
		}
		public ID3v2ImageFrame(string mimeType, ID3v2ImageType imageType, string description, IEnumerable<byte> imageData)
			: base("APIC", 1)
		{
			if (mimeType == null) throw new ArgumentNullException("mimeType");
			if (description == null) throw new ArgumentNullException("description");
			if (imageData == null) throw new ArgumentNullException("imageData");

			this.mimeType = mimeType;
			this.imageType = imageType;
			this.description = description;
			this.imageData = imageData.ToArray();
		}
		public ID3v2ImageFrame(BinaryReader reader)
			: base(reader)
		{
			long headerStartPosition = reader.BaseStream.Position;

			this.mimeType = ReadText(reader, Encoding.ASCII);
			this.imageType = (ID3v2ImageType)reader.ReadByte();
			this.description = ReadText(reader, Encoding);

			long headerEndPosition = reader.BaseStream.Position;

			long imageDataLength = ParsedDataLength - base.DataLength - (headerEndPosition - headerStartPosition);

			if (imageDataLength < 0) throw new InvalidDataException(string.Format("Invalid image data length '{0}'.", imageDataLength));

			this.imageData = reader.ReadBytes((int)imageDataLength);

			string dataMimeType = GetMimeType(this.imageData);

			if (this.mimeType != dataMimeType)
			{
				this.mimeType = dataMimeType;
				this.hasChanged = true;
			}
		}

		public override void Write(BinaryWriter writer)
		{
			base.Write(writer);

			WriteText(writer, Encoding.ASCII, mimeType + '\0');
			writer.Write((byte)imageType);
			WriteText(writer, Encoding, description + '\0');
			writer.Write(imageData);
		}

		static string GetMimeType(IEnumerable<byte> imageData)
		{
			SKCodec codec = SKCodec.Create(new MemoryStream(imageData.ToArray()));

			switch (codec.EncodedFormat)
			{
				case SKEncodedImageFormat.Bmp: return "image/bmp";
				case SKEncodedImageFormat.Jpeg: return "image/jpeg";
				case SKEncodedImageFormat.Png: return "image/png";
				default: throw new InvalidDataException(string.Format("Unknown image format '{0}'.", codec.EncodedFormat));
			}
		}
	}
}
