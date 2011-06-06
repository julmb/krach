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
using System.Linq;
using Krach.Extensions;
using System.Collections.Generic;

namespace Krach.Formats.Mpeg
{
	public struct MpegAudioFrameType : IEquatable<MpegAudioFrameType>
	{
		readonly MpegAudioVersion version;
		readonly MpegAudioLayer layer;
		readonly int sampleRateID;
		
		public MpegAudioVersion Version { get { return version; } }
		public MpegAudioLayer Layer { get { return layer; } }
		public int SampleRateID { get { return sampleRateID; } }
		
		public static IEnumerable<MpegAudioFrameType> AllTypes
		{
			get
			{
				foreach (MpegAudioVersion version in Enumerations.GetValues<MpegAudioVersion>().Except(MpegAudioVersion.Reserved))
					foreach (MpegAudioLayer layer in Enumerations.GetValues<MpegAudioLayer>().Except(MpegAudioLayer.Reserved))
						for (int sampleRateID = 0; sampleRateID < 3; sampleRateID++)
							yield return new MpegAudioFrameType(version, layer, sampleRateID);
			}
		}
		
		public MpegAudioFrameType(MpegAudioVersion version, MpegAudioLayer layer, int sampleRateID)
		{
			if (version == MpegAudioVersion.Reserved) throw new ArgumentOutOfRangeException("version");
			if (layer == MpegAudioLayer.Reserved) throw new ArgumentOutOfRangeException("layer");
			if (sampleRateID < 0 || sampleRateID >= 3) throw new ArgumentOutOfRangeException("sampleRateID");
			
			this.version = version;
			this.layer = layer;
			this.sampleRateID = sampleRateID;
		}
		
		public override bool Equals(object obj)
		{
			return obj is MpegAudioFrameType && this == (MpegAudioFrameType)obj;
		}
		public override int GetHashCode()
		{
			return version.GetHashCode() ^ layer.GetHashCode() ^ sampleRateID.GetHashCode();
		}
		public override string ToString()
		{
			return string.Format("Version: {0}, Layer {1}, SampleRateID: {2}", version, layer, sampleRateID);
		}
		public bool Equals(MpegAudioFrameType other)
		{
			return this == other;
		}

		public static bool operator ==(MpegAudioFrameType type1, MpegAudioFrameType type2)
		{
			return type1.version == type2.version && type1.layer == type2.layer && type1.sampleRateID == type2.sampleRateID;
		}
		public static bool operator !=(MpegAudioFrameType type1, MpegAudioFrameType type2)
		{
			return type1.version != type2.version || type1.layer != type2.layer || type1.sampleRateID != type2.sampleRateID;
		}
	}
}
