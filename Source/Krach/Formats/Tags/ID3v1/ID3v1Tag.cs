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

namespace Krach.Formats.Tags.ID3v1
{
	public class ID3v1Tag
	{
		readonly string[] genreNames = new string[]
		{
			"Blues",
			"Classic Rock",
			"Country",
			"Dance",
			"Disco",
			"Funk",
			"Grunge",
			"Hip-Hop",
			"Jazz",
			"Metal",
			"New Age",
			"Oldies",
			"Other",
			"Pop",
			"R&B",
			"Rap",
			"Reggae",
			"Rock",
			"Techno",
			"Industrial",
			"Alternative",
			"Ska",
			"Death Metal",
			"Pranks",
			"Soundtrack",
			"Euro-Techno",
			"Ambient",
			"Trip-Hop",
			"Vocal",
			"Jazz+Funk",
			"Fusion",
			"Trance",
			"Classical",
			"Instrumental",
			"Acid",
			"House",
			"Game",
			"Sound Clip",
			"Gospel",
			"Noise",
			"AlternRock",
			"Bass",
			"Soul",
			"Punk",
			"Space",
			"Meditative",
			"Instrumental Pop",
			"Instrumental Rock",
			"Ethnic",
			"Gothic",
			"Darkwave",
			"Techno-Industrial",
			"Electronic",
			"Pop-Folk",
			"Eurodance",
			"Dream",
			"Southern Rock",
			"Comedy",
			"Cult",
			"Gangsta",
			"Top 40",
			"Christian Rap",
			"Pop/Funk",
			"Jungle",
			"Native American",
			"Cabaret",
			"New Wave",
			"Psychadelic",
			"Rave",
			"Showtunes",
			"Trailer",
			"Lo-Fi",
			"Tribal",
			"Acid Punk",
			"Acid Jazz",
			"Polka",
			"Retro",
			"Musical",
			"Rock & Roll",
			"Hard Rock",
			"Folk",
			"Folk-Rock",
			"National Folk",
			"Swing",
			"Fast Fusion",
			"Bebob",
			"Latin",
			"Revival",
			"Celtic",
			"Bluegrass",
			"Avantgarde",
			"Gothic Rock",
			"Progressive Rock",
			"Psychedelic Rock",
			"Symphonic Rock",
			"Slow Rock",
			"Big Band",
			"Chorus",
			"Easy Listening",
			"Acoustic",
			"Humour",
			"Speech",
			"Chanson",
			"Opera",
			"Chamber Music",
			"Sonata",
			"Symphony",
			"Booty Brass",
			"Primus",
			"Porn Groove",
			"Satire",
			"Slow Jam",
			"Club",
			"Tango",
			"Samba",
			"Folklore",
			"Ballad",
			"Poweer Ballad",
			"Rhytmic Soul",
			"Freestyle",
			"Duet",
			"Punk Rock",
			"Drum Solo",
			"A Capela",
			"Euro-House",
			"Dance Hall"		 
		};

		readonly string title;
		readonly string artist;
		readonly string album;
		readonly string year;
		readonly string comment;
		readonly byte genreID;

		public string Title { get { return title; } }
		public string Artist { get { return artist; } }
		public string Album { get { return album; } }
		public string Year { get { return year; } }
		public string Comment { get { return comment; } }
		public string Genre { get { return genreID == 0xFF ? string.Empty : genreNames[genreID]; } }

		public ID3v1Tag(BinaryReader reader)
		{
			string identifier = Encoding.ASCII.GetString(reader.ReadBytes(3));
			if (identifier != "TAG") throw new ArgumentException(string.Format("Wrong identifier '{0}', should be 'TAG'.", identifier));

			// TODO: Remove trailing zeroes from strings
			this.title = Encoding.ASCII.GetString(reader.ReadBytes(30));
			this.artist = Encoding.ASCII.GetString(reader.ReadBytes(30));
			this.album = Encoding.ASCII.GetString(reader.ReadBytes(30));
			this.year = Encoding.ASCII.GetString(reader.ReadBytes(4));
			this.comment = Encoding.ASCII.GetString(reader.ReadBytes(30));
			this.genreID = reader.ReadByte();
			if (genreID != 0xFF && genreID > 125) throw new ArgumentException(string.Format("Unknown genre id '{0}'.", genreID));
		}

		public virtual void Write(BinaryWriter writer)
		{
			writer.Write(Encoding.ASCII.GetBytes("TAG"));

			writer.Write(Encoding.ASCII.GetBytes(title));
			writer.Write(Encoding.ASCII.GetBytes(artist));
			writer.Write(Encoding.ASCII.GetBytes(album));
			writer.Write(Encoding.ASCII.GetBytes(year));
			writer.Write(Encoding.ASCII.GetBytes(comment));
			writer.Write(genreID);
		}
	}
}
