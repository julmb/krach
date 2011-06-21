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
using System.Text;
using Krach.Extensions;
using System.Collections.Generic;
using System.Linq;

namespace Krach.Formats.Tags.ID3v2
{
	public class ID3v2EncodedFrame : ID3v2Frame
	{
		readonly byte encodingID;

		public byte EncodingID { get { return encodingID; } }
		public Encoding Encoding
		{
			get
			{
				switch (encodingID)
				{
					case 0: return Encoding.GetEncoding("ISO-8859-1");
					case 1: return Encoding.GetEncoding("UTF-16");
					default: throw new InvalidOperationException();
				}
			}
		}

		public ID3v2EncodedFrame(BinaryReader reader)
			: base(reader)
		{
			this.encodingID = reader.ReadByte();
			if (encodingID < 0 || encodingID > 1) throw new InvalidDataException(string.Format("Invalid encoding identifier: {0}.", encodingID));
		}

		public override void Write(BinaryWriter writer)
		{
			base.Write(writer);

			writer.Write(encodingID);
		}

		protected string ReadString(BinaryReader reader, Encoding encoding, int length)
		{
			using (MemoryStream memoryStream = new MemoryStream(reader.ReadBytes(length)))
			using (StreamReader streamReader = new StreamReader(memoryStream, encoding))
				return streamReader.ReadToEnd();
		}
		protected string ReadString(BinaryReader reader, Encoding encoding)
		{
			using (MemoryStream memoryStream = new MemoryStream(reader.ReadToZero()))
			using (StreamReader streamReader = new StreamReader(memoryStream, encoding))
			{
				reader.ReadBytes(encoding.GetByteCount("\0"));

				return streamReader.ReadToEnd();
			}
		}
		protected void WriteString(BinaryWriter writer, Encoding encoding, string text)
		{
			using (MemoryStream memoryStream = new MemoryStream())
			{
				using (StreamWriter streamWriter = new StreamWriter(memoryStream, encoding)) streamWriter.Write(text);

				writer.Write(memoryStream.ToArray());
			}
		}
	}
}
