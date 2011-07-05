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
	public class MpegAudioFrameType : IEquatable<MpegAudioFrameType>
	{
		readonly MpegAudioVersion version;
		readonly MpegAudioLayer layer;
		readonly int sampleRateID;
		readonly int channelCount;
		
		public MpegAudioVersion Version { get { return version; } }
		public MpegAudioLayer Layer { get { return layer; } }
		public int SampleRateID { get { return sampleRateID; } }
		public int ChannelCount { get { return channelCount; } }
		
		public static IEnumerable<MpegAudioFrameType> AllTypes
		{
			get
			{
				foreach (MpegAudioVersion version in Enumerations.GetValues<MpegAudioVersion>().Except(Enumerables.Create(MpegAudioVersion.Reserved)))
					foreach (MpegAudioLayer layer in Enumerations.GetValues<MpegAudioLayer>().Except(Enumerables.Create(MpegAudioLayer.Reserved)))
						for (int sampleRateID = 0; sampleRateID < 3; sampleRateID++)
							for (int channelCount = 1; channelCount <= 2; channelCount++)
								yield return new MpegAudioFrameType(version, layer, sampleRateID, channelCount);
			}
		}
		
		public MpegAudioFrameType(MpegAudioVersion version, MpegAudioLayer layer, int sampleRateID, int channelCount)
		{
			if (version == MpegAudioVersion.Reserved) throw new ArgumentOutOfRangeException("version");
			if (layer == MpegAudioLayer.Reserved) throw new ArgumentOutOfRangeException("layer");
			if (sampleRateID < 0 || sampleRateID >= 3) throw new ArgumentOutOfRangeException("sampleRateID");
			if (channelCount < 1 || channelCount > 2) throw new ArgumentOutOfRangeException("channelCount");
			
			this.version = version;
			this.layer = layer;
			this.sampleRateID = sampleRateID;
			this.channelCount = channelCount;
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
			if (object.ReferenceEquals(type1, type2)) return true;
			if (object.ReferenceEquals(type1, null) || object.ReferenceEquals(type2, null)) return false;
			
			return
				type1.version == type2.version &&
				type1.layer == type2.layer &&
				type1.sampleRateID == type2.sampleRateID &&
				type1.channelCount == type2.channelCount;
		}
		public static bool operator !=(MpegAudioFrameType type1, MpegAudioFrameType type2)
		{
			if (object.ReferenceEquals(type1, type2)) return false;
			if (object.ReferenceEquals(type1, null) || object.ReferenceEquals(type2, null)) return true;
			
			return
				type1.version != type2.version ||
				type1.layer != type2.layer ||
				type1.sampleRateID != type2.sampleRateID ||
				type1.channelCount != type2.channelCount;
		}
	}
}
