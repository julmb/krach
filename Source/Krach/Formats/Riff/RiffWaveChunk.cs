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
using System.Text;
using Krach.Audio;
using Krach.Extensions;
using System.Collections.Generic;

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

		public RiffWaveChunk(RiffFormatChunk formatChunk, RiffDataChunk dataChunk)
			: base("RIFF", 4 + 8 + formatChunk.Size + 8 + dataChunk.Size + dataChunk.Size % 2)
		{
			if (formatChunk == null) throw new ArgumentNullException("formatChunk");
			if (dataChunk == null) throw new ArgumentNullException("dataChunk");

			this.riffID = "WAVE";
			this.formatChunk = formatChunk;
			this.dataChunk = dataChunk;
		}
		public RiffWaveChunk(BinaryReader reader)
			: base(reader)
		{
			if (ID != "RIFF") throw new InvalidDataException(string.Format("Wrong chunk ID '{0}', should be 'RIFF'.", ID));

			this.riffID = Encoding.ASCII.GetString(reader.ReadBytes(4));

			if (riffID != "WAVE") throw new InvalidDataException(string.Format("Wrong RIFF ID '{0}', should be 'WAVE'.", riffID));

			this.formatChunk = new RiffFormatChunk(reader);
			this.dataChunk = new RiffDataChunk(reader);

			if (Size != 4 + 8 + formatChunk.Size + 8 + dataChunk.Size + dataChunk.Size % 2) throw new InvalidDataException(string.Format("Incorrect chunk size '{0}', should be '{1}'.", Size, 4 + 8 + formatChunk.Size + 8 + dataChunk.Size + dataChunk.Size % 2));
		}

		public override void Write(BinaryWriter writer)
		{
			base.Write(writer);

			writer.Write(riffID.ToCharArray());

			formatChunk.Write(writer);
			dataChunk.Write(writer);
		}

		public static PcmAudio ToPcmAudio(RiffWaveChunk waveChunk)
		{
			int half = 1 << waveChunk.formatChunk.SampleSize - 1;
			int full = 1 << waveChunk.formatChunk.SampleSize;

			IEnumerable<double>[] blocks = new IEnumerable<double>[waveChunk.dataChunk.Size / waveChunk.formatChunk.BlockSize];

			for (int blockIndex = 0; blockIndex < blocks.Length; blockIndex++)
			{
				double[] samples = new double[waveChunk.formatChunk.ChannelCount];

				for (int sampleIndex = 0; sampleIndex < samples.Length; sampleIndex++)
				{
					int sample = 0;

					for (int part = 0; part < (waveChunk.formatChunk.SampleSize / 8); part++)
						sample += waveChunk.dataChunk.Data[blockIndex * waveChunk.formatChunk.BlockSize + sampleIndex * (waveChunk.formatChunk.SampleSize / 8) + part] << part * 8;

					if (sample >= half) sample -= full;

					samples[sampleIndex] = (double)sample / (double)half;
				}

				blocks[blockIndex] = samples;
			}

			return PcmAudio.FromBlocks(blocks, (double)blocks.Length / (double)waveChunk.formatChunk.SampleRate);
		}
		public static RiffWaveChunk FromPcmAudio(PcmAudio pcmAudio)
		{
			IEnumerable<double>[] blocks = pcmAudio.Blocks.ToArray();

			RiffFormatChunk formatChunk = new RiffFormatChunk((ushort)pcmAudio.Channels.Count(), (ushort)(blocks.Length / pcmAudio.Length), 16);

			byte[] data = new byte[formatChunk.BlockSize * blocks.Length];
			int position = 0;

			for (int blockIndex = 0; blockIndex < blocks.Length; blockIndex++)
			{
				double[] samples = blocks[blockIndex].ToArray();

				for (int sampleIndex = 0; sampleIndex < samples.Length; sampleIndex++)
				{
					if (samples[sampleIndex] < -1 || samples[sampleIndex] >= +1) throw new InvalidDataException(string.Format("Sample at index {0} was out of range ({1}).", sampleIndex, samples[sampleIndex]));

					foreach (byte part in BitConverter.GetBytes((short)(samples[sampleIndex] * 0x8000))) data[position++] = part;
				}
			}

			RiffDataChunk dataChunk = new RiffDataChunk(data);

			return new RiffWaveChunk(formatChunk, dataChunk);
		}
	}
}