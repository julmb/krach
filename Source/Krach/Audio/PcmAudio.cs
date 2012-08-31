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
using Krach.Extensions;

namespace Krach.Audio
{
	public class PcmAudio
	{
		readonly IEnumerable<IEnumerable<double>> blocks;
		readonly IEnumerable<IEnumerable<double>> channels;
		readonly double length;

		public IEnumerable<IEnumerable<double>> Blocks { get { return blocks; } }
		public IEnumerable<IEnumerable<double>> Channels { get { return channels; } }
		public double Length { get { return length; } }

		PcmAudio(IEnumerable<IEnumerable<double>> blocks, IEnumerable<IEnumerable<double>> channels, double length)
		{
			if (blocks == null) throw new ArgumentNullException("blocks");
			if (!blocks.Any()) throw new ArgumentException("Parameter blocks cannot be empty.");
			if (channels == null) throw new ArgumentNullException("channels");
			if (!channels.Any()) throw new ArgumentException("Parameter 'channels' did not contains any items.");
			if (length <= 0) throw new ArgumentOutOfRangeException("length");

			this.blocks = blocks;
			this.channels = channels;
			this.length = length;
		}

		public static PcmAudio FromBlocks(IEnumerable<IEnumerable<double>> blocks, double length)
		{
			if (blocks == null) throw new ArgumentNullException("blocks");
			if (!blocks.Any()) throw new ArgumentException("Parameter blocks cannot be empty.");
			if (length <= 0) throw new ArgumentOutOfRangeException("length");
			if (blocks.Select(block => block.Count()).Distinct().Count() > 1) throw new ArgumentException("All blocks must have the same channel count.");

			return new PcmAudio(blocks, blocks.Flip().ToArray(), length);
		}
		public static PcmAudio FromChannels(IEnumerable<IEnumerable<double>> channels, double length)
		{
			if (channels == null) throw new ArgumentNullException("channels");
			if (!channels.Any()) throw new ArgumentException("Parameter 'channels' did not contains any items.");
			if (length <= 0) throw new ArgumentOutOfRangeException("length");
			if (channels.Select(channel => channel.Count()).Distinct().Count() > 1) throw new ArgumentException("All channels must have the same block count.");

			return new PcmAudio(channels.Flip().ToArray(), channels, length);
		}
	}
}