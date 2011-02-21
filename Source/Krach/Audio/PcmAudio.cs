// Copyright © Julian Brunner 2009 - 2011

// This file is part of Stream Visualizer (streamvis).
// 
// Stream Visualizer is free software: you can redistribute it and/or modify it
// under the terms of the GNU General Public License as published by the Free
// Software Foundation, either version 3 of the License, or (at your option) any
// later version.
// 
// Stream Visualizer is distributed in the hope that it will be useful, but
// WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or
// FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more
// details.
// 
// You should have received a copy of the GNU General Public License along with
// Stream Visualizer. If not, see <http://www.gnu.org/licenses/>.

using System;
using System.Collections.Generic;
using System.Linq;

namespace Krach.Audio
{
	public class PcmAudio
	{
		readonly IEnumerable<PcmBlock> blocks;
		readonly double length;
		readonly int channelCount;

		public IEnumerable<PcmBlock> Blocks { get { return blocks; } }
		public double Length { get { return length; } }
		public int ChannelCount { get { return channelCount; } }

		public PcmAudio(IEnumerable<PcmBlock> blocks, double length)
		{
			if (blocks == null) throw new ArgumentNullException("blocks");
			if (!blocks.Any()) throw new ArgumentException("Parameter blocks cannot be empty.");
			if (length <= 0) throw new ArgumentOutOfRangeException("length");

			this.blocks = blocks;
			this.length = length;
			this.channelCount = blocks.First().Samples.Count();

			if (!blocks.All(block => block.Samples.Count() == channelCount)) throw new ArgumentException("All blocks must have the same channel count.");
		}
	}
}