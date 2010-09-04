// Copyright © Julian Brunner 2010

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
using Krach.Audio;

namespace Krach.Formats.Riff
{
	public class RiffWaveChunk : RiffChunk
	{
		readonly string riffID;
		readonly RiffFormatChunk formatChunk;
		readonly RiffDataChunk dataChunk;

		public string RiffID { get { return riffID; } }
		public RiffFormatChunk FormatChunk { get { return formatChunk; } }
		public RiffDataChunk DataChunk { get { return dataChunk; } }
		public uint BlockCount { get { return dataChunk.Size / formatChunk.BlockSize; } }

		public RiffWaveChunk(BinaryReader reader)
			: base(reader)
		{
			if (ID != "RIFF") throw new ArgumentException(string.Format("Wrong chunk ID '{0}', should be 'RIFF'.", ID));

			this.riffID = new string(reader.ReadChars(4));

			if (riffID != "WAVE") throw new ArgumentException(string.Format("Wrong RIFF ID '{0}', should be 'WAVE'.", riffID));

			this.formatChunk = new RiffFormatChunk(reader);
			this.dataChunk = new RiffDataChunk(reader);

			uint paddingSize = dataChunk.Size % 2;
			if (Size != 4 + 8 + formatChunk.Size + 8 + dataChunk.Size + paddingSize) throw new ArgumentException(string.Format("Incorrect chunk size '{0}', should be '{1}'.", Size, 4 + 8 + formatChunk.Size + 8 + dataChunk.Size + paddingSize));
		}

		public PcmAudio ToPcmAudio()
		{
			PcmBlock[] blocks = new PcmBlock[BlockCount];

			int half = 1 << formatChunk.SampleSize - 1;
			int full = 1 << formatChunk.SampleSize;

			for (int block = 0; block < BlockCount; block++)
			{
				double[] samples = new double[formatChunk.ChannelCount];

				for (int channel = 0; channel < formatChunk.ChannelCount; channel++)
				{
					int sample = 0;

					for (int part = 0; part < (formatChunk.SampleSize / 8); part++)
						sample += dataChunk.Data[block * formatChunk.BlockSize + channel * (formatChunk.SampleSize / 8) + part] << part * 8;

					if (sample >= half) sample -= full;

					samples[channel] = (double)sample / (double)half;
				}

				blocks[block] = new PcmBlock(samples);
			}

			return new PcmAudio(blocks);
		}
	}
}