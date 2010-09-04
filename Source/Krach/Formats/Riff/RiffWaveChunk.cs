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

namespace Krach.Formats.Riff
{
	class RiffWaveChunk
	{
		readonly byte[] data;
		readonly ushort channelCount;
		readonly ushort sampleSize;
		readonly uint sampleRate;

		public byte[] Data { get { return data; } }
		public ushort ChannelCount { get { return channelCount; } }
		public ushort SampleSize { get { return sampleSize; } }
		public uint SampleRate { get { return sampleRate; } }
		public int BlockSize { get { return channelCount * (sampleSize / 8); } }
		public int BlockCount { get { return data.Length / (sampleSize / 8) / channelCount; } }

		public RiffWaveChunk(BinaryReader reader)
		{
			if (reader == null) throw new ArgumentNullException("reader");

			// RIFF chunk ID
			if (new string(reader.ReadChars(4)) != "RIFF") throw new ArgumentException();
			// RIFF chunk size
			uint chunkSize = reader.ReadUInt32();
			// Wave ID
			if (new string(reader.ReadChars(4)) != "WAVE") throw new ArgumentException();

			// Format chunk ID
			if (new string(reader.ReadChars(4)) != "fmt ") throw new ArgumentException();
			// Format chunk size (only accept basic PCM formats)
			if (reader.ReadUInt32() != 16) throw new ArgumentException();
			// Format code (only accept basic PCM formats)
			if (reader.ReadUInt16() != 1) throw new ArgumentException();
			// Channel count
			this.channelCount = reader.ReadUInt16();
			// Sample rate (samples / second)
			this.sampleRate = reader.ReadUInt32();
			// Data rate (bytes / second)
			uint dataRate = reader.ReadUInt32();
			// Data block size
			ushort blockSize = reader.ReadUInt16();
			// Sample size (bits)
			this.sampleSize = reader.ReadUInt16();

			if (blockSize != channelCount * sampleSize / 8) throw new ArgumentException();
			if (dataRate != sampleRate * blockSize) throw new ArgumentException();

			// Data chunk ID
			if (new string(reader.ReadChars(4)) != "data") throw new ArgumentException();
			// Data chunk size
			uint dataChunkSize = reader.ReadUInt32();
			// Data
			data = reader.ReadBytes((int)dataChunkSize);

			uint paddingSize = dataChunkSize % 2;
			if (4 + 8 + 16 + 8 + dataChunkSize + paddingSize != chunkSize) throw new ArgumentException();
		}

		//public PcmAudio ToPcmAudio()
		//{
		//    PcmBlock[] blocks = new PcmBlock[BlockCount];

		//    int half = 1 << SampleSize - 1;
		//    int full = 1 << SampleSize;

		//    for (int block = 0; block < BlockCount; block++)
		//    {
		//        double[] samples = new double[ChannelCount];

		//        for (int channel = 0; channel < ChannelCount; channel++)
		//        {
		//            int sample = 0;

		//            for (int part = 0; part < (SampleSize / 8); part++)
		//                sample += data[block * BlockSize + channel * (SampleSize / 8) + part] << part * 8;

		//            if (sample >= half) sample -= full;

		//            samples[channel] = (double)sample / (double)half;
		//        }

		//        blocks[block] = new PcmBlock(samples);
		//    }

		//    return new PcmAudio(blocks);
		//}
	}
}